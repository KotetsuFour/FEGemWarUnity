using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveScreen : SequenceMember
{
    private int menuIdx;
    private bool done;
    private Camera cam;
    public void Start()
    {
        menuIdx = 0;
        transform.GetChild(0).GetChild(menuIdx).GetComponent<TextMeshProUGUI>().color = Color.cyan;
    }
    public void constructor(Camera cam)
    {
        this.cam = cam;
        cam.transform.position = new Vector3(0, 0, GridMap.CAMERA_LAYER);
        cam.orthographicSize = (float)17.5;
    }
    public override bool completed()
    {
        return done;
    }
    public override void LEFT_MOUSE(float mouseX, float mouseY)
    {
        /*
        CanvasScaler canvas = transform.GetChild(0).GetComponent<CanvasScaler>();
        float xPos = mouseX * canvas.referenceResolution.x / Screen.width;
        float yPos = mouseY * canvas.referenceResolution.y / Screen.height;
        for (int q = 0; q < transform.GetChild(0).childCount; q++)
        {
            RectTransform rect = canvas.transform.GetChild(q).GetComponent<RectTransform>();
            if (rect.rect.xMin <= xPos && rect.rect.xMax >= xPos
                && rect.rect.yMin <= yPos && rect.rect.yMax >= yPos)
            {
                transform.GetChild(0).GetChild(menuIdx).GetComponent<TextMeshProUGUI>().color = Color.white;
                menuIdx = q;
                transform.GetChild(0).GetChild(menuIdx).GetComponent<TextMeshProUGUI>().color = Color.cyan;
                Z();
                return;
            }
        }
        */
    }
    public override void RIGHT_MOUSE(float mouseX, float mouseY)
    {
        //nothing
    }
    public override void Z()
    {
//        if (menuIdx < 3) //We'll treat "Continue without saving" like saving to a fourth file
        {
            SaveMechanism.saveGame(menuIdx + 1);
        }

        gameObject.SetActive(false);
        done = true;
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
        transform.GetChild(0).GetChild(menuIdx).GetComponent<TextMeshProUGUI>().color = Color.white;
        menuIdx--;
        if (menuIdx < 0)
        {
            menuIdx = transform.GetChild(0).childCount - 1;
        }
        transform.GetChild(0).GetChild(menuIdx).GetComponent<TextMeshProUGUI>().color = Color.cyan;
    }
    public override void LEFT()
    {
        //nothing
    }
    public override void DOWN()
    {
        transform.GetChild(0).GetChild(menuIdx).GetComponent<TextMeshProUGUI>().color = Color.white;
        menuIdx = (menuIdx + 1) % transform.GetChild(0).childCount;
        transform.GetChild(0).GetChild(menuIdx).GetComponent<TextMeshProUGUI>().color = Color.cyan;
    }
    public override void RIGHT()
    {
        //nothing
    }
    public override void ENTER()
    {
        //nothing
    }
    public override void ESCAPE()
    {
        //nothing
    }
}
