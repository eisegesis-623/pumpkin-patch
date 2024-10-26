using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutSwitch : MonoBehaviour
{
    public GameObject candy;
    public GameObject pumpkin;
    public GameObject frank;
    public GameObject grave;
    public GameObject vampire;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    void LoadNextLevel(int nextIndex)
    {
        List<GameObject> list = new List<GameObject>() { candy,pumpkin,frank,grave,vampire};

        foreach (GameObject i in list)
        {
            if (list[nextIndex] == i)
            {
                i.SetActive(true);
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
