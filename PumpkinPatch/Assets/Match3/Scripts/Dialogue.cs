using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public Match3.Match3 match3;

    public TMP_Text textBox;
    public string[] goodMessages;
    public string[] badMessages;
    public string[] funMessages;

    int randomValue;
    
    // Start is called before the first frame update
    void Start()
    {
        randomValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) 
        { 
            MatchMessage();
        }
    }

    public void MatchMessage()
    {
        randomValue = Random.Range(0, 10);
        if (randomValue != 0)
        {
            randomValue = Random.Range(0, goodMessages.Length);
            textBox.text = goodMessages[randomValue];
        }
        else
        {
            randomValue = Random.Range(0, funMessages.Length);
            textBox.text = funMessages[randomValue];
        }
        
    }

    public void FailMessage()
    {
        randomValue = Random.Range(0, badMessages.Length);
        textBox.text = badMessages[randomValue];
    }
}
