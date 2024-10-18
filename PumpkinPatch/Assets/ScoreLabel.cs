using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreLabel : MonoBehaviour
{
    public Match3.Match3 match3;
    public TMP_Text[] uiTexts;

    private void Update()
    {
        uiTexts[0].text = "Score: " + (match3.AllMatches.Count / 3).ToString();
    }
}
