using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutSwitch : MonoBehaviour
{
    public List<GameObject> levelLayouts;
    [HideInInspector]
    public int currentLevelIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void LoadNextLevel(int nextIndex)
    {
        if (nextIndex > 4)
        {
            Debug.Log("You won the game!");
            //nextIndex = 0;
        }
        else if (nextIndex < 0)
        {
            nextIndex = 4;
        }
        currentLevelIndex = nextIndex;
        foreach (GameObject i in levelLayouts)
        {
            if (levelLayouts[nextIndex] == i)
            {
                i.SetActive(true);
                Debug.Log("Succesfully loaded "+ i.name);
            }
            else
            {
                i.SetActive(false);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) //Candyland
        {
            LoadNextLevel(0);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2)) //Pumpkin Patch
        {
            LoadNextLevel(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) //Frankenstein
        {
            LoadNextLevel(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) //Graveyard
        {
            LoadNextLevel(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) //Vampire
        {
            LoadNextLevel(4);
        }
    }
}
