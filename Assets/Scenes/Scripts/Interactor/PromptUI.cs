using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PromptUI : MonoBehaviour
{
    public Transform cam;
    [SerializeField] private GameObject UIPanel;
    [SerializeField] private TextMeshProUGUI _promptText;

    // Update is called once per frame
    void LateUpdate()
    {
       
    }
    public bool isDisplay = false;
    public void SetUp(string promptText)
    {
        _promptText.text = promptText;
        UIPanel.SetActive(true);
        isDisplay = true;
        Debug.Log("prompting !!!");
    }

    public void Close()
    {
        UIPanel.SetActive(false);
        isDisplay = false;

    }
}
