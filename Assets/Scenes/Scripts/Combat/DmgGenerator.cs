using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DmgGenerator : MonoBehaviour
{
    public static DmgGenerator current;
    public GameObject prefab;
    private void Awake()
    {
        current = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            CreatePopUp(Vector3.one, Random.Range(0, 1000).ToString(), Color.black);
        }
    }

    public void CreatePopUp(Vector3 position, string text, Color color)
    {
        var popup = Instantiate(prefab, position, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;


        Destroy(popup, 1f);
    }
}
