using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

namespace Save
{
    #region Structures
    [System.Serializable]
    public struct MyVector3
    {
        public float x;
        public float y;
        public float z;

        public MyVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public MyVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }

    [System.Serializable]
    public struct PlayerSave
    {
        public MyVector3 position;
        public int hp;

        public PlayerSave(MyVector3 position, int hp )
        {
            this.position = position;
            this.hp = hp;
        }
    }

    [System.Serializable]
    public struct EnemySave
    {
        public MyVector3 position;
        public int hp;

        public EnemySave(MyVector3 position, int hp)
        {
            this.position = position;
            this.hp = hp;
        }
    }

    [System.Serializable]
    public struct SaveData
    {
        public PlayerSave playerSave;
        public List<EnemySave> enemySaves;
        public int score;
        /// <summary>
        /// If true mean the powerup is still exist, false means is consumed by the player
        /// </summary>
        public bool[] powerUps;
    }
    #endregion

    public class SaveGame : Singleton<SaveGame>
    {
        private static bool RequestLoad = false;
        private static SaveData SaveData;

        [Header("Instances")]
        [SerializeField] private AttributeManager player;
        [SerializeField] private List<AttributeManager> enemies;
        public List<AttributeManager> Enemies
        {
            get { return enemies; }
        }

        [SerializeField] private Score score;
        [SerializeField] private GameObject[] powerUps;

        [SerializeField] private SaveData testSave;
        [SerializeField] private SaveData testLoadSave;

        [SerializeField] private Transitions gameLoader;


        protected override void AwakeSingleton()
        {
            if (RequestLoad)
            {
                RequestLoad = false;

                // Load game
                player.transform.position = SaveData.playerSave.position.ToVector3();
                player.health = SaveData.playerSave.hp;

                // TODO: Enemy
                int enemyCount = Mathf.Min(SaveData.enemySaves.Count, enemies.Count);
                for (int i = 0; i < enemyCount; i++)
                {
                    AttributeManager enemy = enemies[i];
                    if (enemy != null)
                    {
                        EnemySave enemySave = SaveData.enemySaves[i];
                        enemy.transform.position = enemySave.position.ToVector3();
                        enemy.health = enemySave.hp;
                    }
                }

                //Destroy enemies if its not saved 
                for (int i = enemyCount; i < enemies.Count; i++)
                {
                    AttributeManager enemy = enemies[i];
                    if (enemy != null)
                    {
                        Destroy(enemy.gameObject);
                    }
                }

                enemies.RemoveAll(enemy => enemy == null);

                score.score = SaveData.score;
                for(int i = 0; i < powerUps.Length; i++)
                {
                    if (!SaveData.powerUps[i])
                    {
                        Destroy(powerUps[i]);
                    }
                }
            }
        }

        public void Save()
        {
            SaveData save = new();
            save.playerSave = new(new(player.transform.position), player.health);
            save.enemySaves = new List<EnemySave>();
            foreach(AttributeManager am in enemies)
            {
                if(am != null)
                {
                    save.enemySaves.Add(new EnemySave(new(am.transform.position), am.health));
                }
            }
            save.score = score.score;

            save.powerUps = new bool[powerUps.Length];
            for (int i = 0; i < powerUps.Length; i++)
            {
                if (powerUps[i] != null)
                {
                    save.powerUps[i] = true;
                }
                else
                {
                    save.powerUps[i] = false;
                }
            }

            SaveData = save;
            testSave = save;

            string path = Application.persistentDataPath + "/Save";

            using (FileStream file = System.IO.File.Create(path + ".save"))
            {
                new BinaryFormatter().Serialize(file, save);
            }

            string json = JsonUtility.ToJson(save);
            System.IO.File.WriteAllText(path + ".json", json);

            Debug.Log("Save To: " + path + ".save");
        }

        public void Load()
        {
            string path = Application.persistentDataPath + "/Save.save";
            if (System.IO.File.Exists(path))
            {
                using (FileStream file = System.IO.File.Open(path, FileMode.Open))
                {
                    object data = new BinaryFormatter().Deserialize(file);
                    testLoadSave = (SaveData)data;
                    SaveData = testLoadSave;
                    RequestLoad = true;
                    //SceneManager.LoadScene(1);

                    gameLoader.LoadNextLevel(1);

                }
            }
            else
            {
                Debug.LogWarning("File Save Not Found");
            }
        }
    }
}
