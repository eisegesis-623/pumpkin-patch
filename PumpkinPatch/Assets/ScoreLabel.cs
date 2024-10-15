using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreLabel : MonoBehaviour
{
    public Match3.Match3 match3;

    private void Update()
    {
        GetComponent<TextMeshProUGUI>().text = (match3.AllMatches.Count / 3).ToString();
    }
}
