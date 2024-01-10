using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class HelpNPC : MonoBehaviour
{


    private float interactRange = 2f;
    private string promptMsg = "Press [E] to rescue"; 
    public PromptUI PromptUI;

    bool interactable = false;
    /// <summary>
    /// Score
    /// </summary>
    public Score score;
    //private List<bool> scoreNPCadded = new List<bool>();
    // Start is called before the first frame update
    void Start()
    {

        //scoreNPCadded.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out npcOwn npcOwn))
                {
                    npcOwn.Interact();
                    FindObjectOfType<Audios>().PlaySound("SaveNPC");
                    PromptUI.Close();

                }
            }
            interactable = true;
        } else
        {
            interactable = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("NPC"))
            PromptUI.SetUp(promptMsg);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
            PromptUI.Close();
    }
}
