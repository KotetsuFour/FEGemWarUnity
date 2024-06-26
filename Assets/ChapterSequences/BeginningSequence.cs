using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginningSequence : MonoBehaviour
{
    public MainMenu menuLogic;

    public GameObject cam;

    public Unit unit;
    public Cutscene cutScene;

    public int sequenceNum;
    public SequenceMember seqMem;

    public List<Unit> playerList;

    void Start()
    {
        Cutscene firstScene = Instantiate(cutScene);
        firstScene.constructor(new DialogueEvent(0, "Assets/Dialogue/opening_dialogue.txt"), cam.GetComponent<Camera>());
        seqMem = firstScene;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            seqMem.LEFT_MOUSE(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            seqMem.RIGHT_MOUSE(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetKeyDown("up") || Input.GetKeyDown("w"))
        {
            seqMem.UP();
        }
        if (Input.GetKeyDown("left") || Input.GetKeyDown("a"))
        {
            seqMem.LEFT();
        }
        if (Input.GetKeyDown("down") || Input.GetKeyDown("s"))
        {
            seqMem.DOWN();

        }
        if (Input.GetKeyDown("right") || Input.GetKeyDown("d"))
        {
            seqMem.RIGHT();

        }
        if (Input.GetKeyDown("z") || Input.GetKeyDown("p"))
        {
            seqMem.Z();

        }
        if (Input.GetKeyDown("x") || Input.GetKeyDown("o"))
        {
            seqMem.X();
        }
        if (Input.GetKeyDown("c") || Input.GetKeyDown("i"))
        {
            seqMem.C();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            seqMem.ENTER();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            seqMem.ESCAPE();
        }
        if (seqMem.completed())
        {
            sequenceNum++;
            if (sequenceNum == 1)
            {
                MainMenu menu = Instantiate(menuLogic);
                menu.activate(cam.GetComponent<Camera>());
                seqMem = menu;
            }
        }
    }
}
