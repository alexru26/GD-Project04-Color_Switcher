using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{

    [Header("Switch")]
    private bool red = true;

    [Header("References")]

    public Material active;
    public Material inactive;

    private GameObject[] red_list;
    private GameObject[] blue_list;

    void Start()
    {
        red_list = GameObject.FindGameObjectsWithTag("red");
        blue_list = GameObject.FindGameObjectsWithTag("blue");
    }

    void Update()
    {
        input();
    }

    void input()
    {
        if(Input.GetButtonDown("Switch"))
        {
            if(red)
            {
                red = false;
                GameObject.Find("LeftEye").GetComponent<SpriteRenderer>().color = new Color(29f/255f, 127f/255f, 226f/255f, 1);
                GameObject.Find("RightEye").GetComponent<SpriteRenderer>().color = new Color(29f/255f, 127f/255f, 226f/255f, 1);
                foreach(GameObject go in red_list)
                {
                    go.GetComponent<Collider2D>().enabled = false;
                    go.GetComponent<Renderer>().material = inactive;
                }
                foreach(GameObject go in blue_list)
                {
                    go.GetComponent<Collider2D>().enabled = true;
                    go.GetComponent<Renderer>().material = active;
                }
            }
            else
            {
                red = true;
                GameObject.Find("LeftEye").GetComponent<SpriteRenderer>().color = new Color(150f/255f, 10f/255f, 10f/255f, 1);
                GameObject.Find("RightEye").GetComponent<SpriteRenderer>().color = new Color(150f/255f, 10f/255f, 10f/255f, 1);
                foreach(GameObject go in red_list)
                {
                    go.GetComponent<Collider2D>().enabled = true;
                    go.GetComponent<Renderer>().material = active;
                }
                foreach(GameObject go in blue_list)
                {
                    go.GetComponent<Collider2D>().enabled = false;
                    go.GetComponent<Renderer>().material = inactive;
                }
            }
        }
    }
}
