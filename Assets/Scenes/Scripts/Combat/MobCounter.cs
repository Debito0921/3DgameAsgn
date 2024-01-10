using Save;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MobCounter : MonoBehaviour
{
    
    public static List<GameObject> monsters { get; set; } = new();
    public TextMeshProUGUI mobCounter;
    public GameObject[] arrayMonster;
    public GameObject portal;
    private bool isUpdateMobCount = false;
    private bool isPortal = false; 
    // Start is called before the first frame update
    void Start()
    { 
        //UpdateMonsterList();
        monsters.Clear();
        StartCoroutine(InitCount());
        //arrayMonster = GameObject.FindGameObjectsWithTag("Enemy");
        //monsters.AddRange(arrayMonster);
        //Debug.Log("ArrayMonster length = " + arrayMonster.Length);
        //mobCounter.text = "Enemies Left : " + arrayMonster.Length;

    }

    private void Update()
    {
        if (arrayMonster.Length == 0 && isUpdateMobCount && isPortal == false)
        {
            InstantiatePortal();
        }
    }
    
    public void UpdateMonsterList()
    {
        arrayMonster = GameObject.FindGameObjectsWithTag("Enemy");
        //arrayMonster = SaveGame.Instance.Enemies.Select(x => x.gameObject).ToArray();
        

        // Remove missing or destroyed GameObjects from monsters list
        monsters.RemoveAll(obj => !ArrayContainsGameObject(arrayMonster, obj));
        monsters.AddRange(arrayMonster);
        Debug.Log("ArrayMonster length = " + arrayMonster.Length);
        mobCounter.text = "Enemies Left : " + arrayMonster.Length;
    }

    bool ArrayContainsGameObject(GameObject[] array, GameObject gameObject)
    {
        foreach (GameObject obj in array)
        {
            if (object.ReferenceEquals(obj, gameObject))
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator InitCount()
    {
        yield return new WaitForEndOfFrame();
        UpdateMonsterList();
        isUpdateMobCount = true;
    }

    public void InstantiatePortal()
    {
        Vector3 position = new Vector3(132.94f, 1.96f, 310.33f);
        Quaternion rotation = Quaternion.identity; // No rotation

        GameObject instantiatedObject = Instantiate(portal, position, rotation);
        isPortal = true;
        Debug.Log("Instantiate Portal!");
    }
}
