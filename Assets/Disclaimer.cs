using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Disclaimer : SequenceMember
{

    public float timer;
    public void constructor(Camera cam)
    {
        cam.transform.position = new Vector3(0, 0, GridMap.CAMERA_LAYER);
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = (float)17.5;

        transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
            = "This is a fanmade project, not in affiliation\nwith Intelligent Systems or Cartoon Network." +
            "\nThis project is not for profit, so please\ndon't sue me!";
        timer = 4;

    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public override bool completed()
    {
        if (timer <= 0)
        {
            transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = null;
            return true;
        }
        return false;
    }
    public override void LEFT_MOUSE(float mouseX, float mouseY)
    {
        Z();
    }
    public override void RIGHT_MOUSE(float mouseX, float mouseY)
    {
        //nothing
    }

    public override void Z()
    {
        timer = 0;
    }
    public override void X()
    {
        //nothing
    }
    public override void C()
    {
        //nothing
    }
    public override void UP()
    {
        //nothing
    }
    public override void LEFT()
    {
        //nothing
    }
    public override void DOWN()
    {
        //nothing
    }
    public override void RIGHT()
    {
        //nothing
    }
    public override void ENTER()
    {
        timer = 0;
    }
    public override void ESCAPE()
    {
        //TODO nothing
    }
}
