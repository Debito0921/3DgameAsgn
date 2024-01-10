using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI dialogue;
    public GameObject canvas;
    public float displayDelay = 10f;

    public float typingSpeed = 0.1f;
    private string originalText;
    private string currentText;
    private float timer;
    private bool isCanvasActive;
    // Start is called before the first frame update
    void Start()
    {
        originalText = dialogue.text;
        currentText = string.Empty;
        timer = 0f;
        canvas.SetActive(true);
        isCanvasActive = true;

        StartCoroutine(DisableCanvas());
    }

    // Update is called once per frame
    void Update()
    {
        if (isCanvasActive)
        {
            timer += Time.deltaTime;

            if (timer >= typingSpeed)
            {
                timer = 0f;

                // Check if the current text is fully typed
                if (currentText.Length < originalText.Length)
                {
                    // Add the next character to the current text
                    currentText = originalText.Substring(0, currentText.Length + 1);

                    // Update the TextMeshProUGUI text
                    dialogue.text = currentText;
                }
            }
        }
    }

    IEnumerator DisableCanvas()
    {
        yield return new WaitForSeconds(displayDelay);
        canvas.SetActive(false);
        isCanvasActive = false;
    }
}
