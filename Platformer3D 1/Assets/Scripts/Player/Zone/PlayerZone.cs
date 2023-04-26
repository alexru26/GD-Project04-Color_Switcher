using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class PlayerZone : MonoBehaviour
{

    public float zoneTime;
    public float zoneCooldown;
    private bool canZone = true;

    private GameObject[] objects;

    public PostProcessVolume pf;
    private Vignette v;
    private LensDistortion ld;

    void Start()
    {
        objects = GameObject.FindGameObjectsWithTag("fakeWall");

        pf.profile.TryGetSettings(out v);
        pf.profile.TryGetSettings(out ld);
    }

    void Update() 
    {
        input();
    }

    void input()
    {
        if(Input.GetButton("Zone") && canZone)
        {
            zone();
            Invoke(nameof(resetZone), zoneTime);
            Invoke(nameof(resetCanZone), zoneCooldown);
        }
    }

    void zone()
    {
        for(int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<BoxCollider>().enabled = false;
        }

        v.intensity.value = Mathf.Lerp(0f, 0.4f, 1f);
        ld.intensity.value = Mathf.Lerp(0f, 20f, 5f);

        canZone = false;
    }

    void resetZone()
    {
        for(int i = 0; i < objects.Length; i++)
        {
            objects[i].GetComponent<BoxCollider>().enabled = true;
        }

        v.intensity.value = Mathf.Lerp(0.4f, 0f, 1f);
        ld.intensity.value = Mathf.Lerp(20f, 0f, 5f);
    }

    void resetCanZone()
    {
        canZone = true;
    }

}
