using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class npcOwn : MonoBehaviour
{
    public Score score;

    private void Start()
    {
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
    }
    public void Interact()
    {
        score.AddScore(200);
        Destroy(gameObject);
    }

}
