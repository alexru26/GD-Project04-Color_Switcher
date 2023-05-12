using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{

    [Header("Switch")]
    private bool red = true;

    [Header("References")]
    public Collider2D red_col;
    public Collider2D blue_col;

    public Renderer red_rend;
    public Renderer blue_rend;

    public Material active;
    public Material inactive;

    void Start()
    {
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
                red_col.enabled = false;
                red_rend.material = inactive;
                blue_col.enabled = true;
                blue_rend.material = active;
            }
            else
            {
                red = true;
                GameObject.Find("LeftEye").GetComponent<SpriteRenderer>().color = new Color(150f/255f, 10f/255f, 10f/255f, 1);
                GameObject.Find("RightEye").GetComponent<SpriteRenderer>().color = new Color(150f/255f, 10f/255f, 10f/255f, 1);
                red_col.enabled = true;
                red_rend.material = active;
                blue_col.enabled = false;
                blue_rend.material = inactive;
            }
        }
    }
}
