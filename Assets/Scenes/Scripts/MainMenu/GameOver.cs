using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public void Setup (int score)
    {
        gameObject.SetActive(true);
        pointsText.text = score.ToString();
    }
}
