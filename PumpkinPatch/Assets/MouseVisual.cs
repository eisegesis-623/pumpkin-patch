using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseVisual : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse X") < 0)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Code for action on mouse moving left
            //print("Mouse moved left");
        }
        if (Input.GetAxis("Mouse X") > 0)
        {
            //Code for action on mouse moving right
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //print("Mouse moved right");
        }
    }
}
