using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{

    public TMP_Text textBox;
    public string[] goodMessages;
    public string[] badMessages;
    public string[] funMessages;
    
    /* Little Guy Index Guide:
    0 - Default 
    1 - Happy 1
    2 - Happy 2
    3 - Angry 1
    4 - Angry 2 */
    public Sprite[] littleGuyPoses;
    public Image littleGuy;

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
        if (randomValue != 1 && randomValue != 0)
        {
            randomValue = Random.Range(0, goodMessages.Length);
            textBox.text = goodMessages[randomValue];
        }
        else
        {
            randomValue = Random.Range(0, funMessages.Length);
            textBox.text = funMessages[randomValue];
        }

        randomValue = Random.Range(0, 3);
        littleGuy.sprite = littleGuyPoses[randomValue];
        littleGuy.GetComponent<Animation>().Play();
    }

    public void FailMessage()
    {
        randomValue = Random.Range(0, badMessages.Length);
        textBox.text = badMessages[randomValue];

        randomValue = Random.Range(3, 5);
        littleGuy.sprite = littleGuyPoses[randomValue];
        littleGuy.GetComponent<Animation>().Play();
    }
}
