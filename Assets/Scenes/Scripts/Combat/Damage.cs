using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Damage : MonoBehaviour
{
    
    public AttributeManager playerAtm;
    public AttributeManager enemyAtm;
    public List<HP> centaurHP = new();
    public HP playerHP;
    public List<GameObject> centaur = new();
    public MobCounter mobcounter;
    public GameObject portal;

    
    /// <summary>
    /// Score
    /// </summary>
    public Score score;
    private List<bool> scoreAdded = new List<bool>();

    private void Start()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        centaurHP.Clear();
        centaur.Clear();
        scoreAdded.Clear();

        foreach(var e in enemies)
        {
            centaurHP.Add(e.GetComponent<HP>());
            centaur.Add(e);
            scoreAdded.Add(false);
        }

    }
    void Update()
    {
        //Player Attack enemies 
        if (Input.GetKeyDown(KeyCode.T))
        {
            playerAtm.DealDamage(enemyAtm.gameObject);
            
        }


        //Enemies attack player
        if (Input.GetKeyDown(KeyCode.U))
        {
            enemyAtm.DealDamage(playerAtm.gameObject);

        }


        
        if (playerHP.healthBar.value == 0.0f)
        {
            Debug.Log("You Lose");
            score.DoneGame();
        }

        
        //Foreach monster
        for (int i = 0; i < centaur.Count; i++)
        {
            if (centaurHP[i].healthBar.value == 0.0f)
            {
                
                //Debug.Log("Monsters left ( before remove ) : " + MobCounter.monsters.Count);
                MobCounter.monsters.Remove(centaur[i]);
                Destroy(centaur[i]);
                mobcounter.mobCounter.text = "Enemies Left: " + MobCounter.monsters.Count;
                //Debug.Log("Monsters left ( after remove ): " + MobCounter.monsters.Count);
                mobcounter.UpdateMonsterList();

                if (!scoreAdded[i])
                {
                    score.AddScore(100);
                    scoreAdded[i] = true;
                }
            }
        }
        
        centaur.RemoveAll(enemy => enemy == null); // Remove destroyed enemies from the list
        centaurHP.RemoveAll(enemy => enemy == null);

        /*
        if (MobCounter.monsters.Count == 0 )
        {
            Vector3 position = new Vector3(132.94f, 1.96f, 310.33f);
            Quaternion rotation = Quaternion.identity; // No rotation

            GameObject instantiatedObject = Instantiate(portal, position, rotation);
            Debug.Log();
            Debug.Log("Instantiate Portal!");
        }
        */
    }
    

}
