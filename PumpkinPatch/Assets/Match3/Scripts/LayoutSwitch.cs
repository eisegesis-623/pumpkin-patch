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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) //Candyland
        {
            candy.SetActive(true);
            pumpkin.SetActive(false);
            frank.SetActive(false);
            grave.SetActive(false);
            vampire.SetActive(false);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2)) //Pumpkin Patch
        {
            candy.SetActive(false);
            pumpkin.SetActive(true);
            frank.SetActive(false);
            grave.SetActive(false);
            vampire.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) //Frankenstein
        {
            candy.SetActive(false);
            pumpkin.SetActive(false);
            frank.SetActive(true);
            grave.SetActive(false);
            vampire.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) //Graveyard
        {
            candy.SetActive(false);
            pumpkin.SetActive(false);
            frank.SetActive(false);
            grave.SetActive(true);
            vampire.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) //Vampire
        {
            candy.SetActive(false);
            pumpkin.SetActive(false);
            frank.SetActive(false);
            grave.SetActive(false);
            vampire.SetActive(true);
        }
    }
}
