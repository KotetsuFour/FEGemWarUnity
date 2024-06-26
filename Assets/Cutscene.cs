using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cutscene : SequenceMember
{
    private DialogueEvent dialogue;
    private bool storyComplete;

    public GameObject background;
    public GameObject dialogueBox;
    public GameObject speakerPortrait;
    private List<GameObject> instantiatedSpeakerPortraits;
    public GameObject instructions;

    public void constructor(DialogueEvent dialogue, Camera cam)
    {
        this.dialogue = dialogue;

        cam.transform.position = new Vector3(0, 0, GridMap.CAMERA_LAYER);
        Camera camCam = cam.GetComponent<Camera>();
        camCam.orthographicSize = (float)17.5;

        background = Instantiate(background);
        dialogueBox = Instantiate(dialogueBox);
        instructions = Instantiate(instructions);
        instantiatedSpeakerPortraits = new List<GameObject>();
        for (int q = 0; q < 5; q++)
        {
            instantiatedSpeakerPortraits.Add(Instantiate(speakerPortrait));
        }
        instantiatedSpeakerPortraits[0].transform.Translate(new Vector3(-16, 0, 0));
        instantiatedSpeakerPortraits[1].transform.Translate(new Vector3(16, 0, 0));
        instantiatedSpeakerPortraits[2].transform.Translate(new Vector3(-32, 0, 0));
        instantiatedSpeakerPortraits[3].transform.Translate(new Vector3(32, 0, 0));
        //        instantiatedSpeakerPortraits[4].transform.Translate(new Vector3(0, 0, 0));
        foreach (GameObject g in instantiatedSpeakerPortraits)
        {
            g.GetComponent<SpriteRenderer>().sprite = null;
        }

        nextSaying();
    }
    private bool nextSaying()
    {
        List<string> component = dialogue.nextDialogueComponent();
        if (component == null)
        {
            return false;
        }
        int idx = 0;
        while (char.IsNumber(component[idx][0]))
        {
            int command = component[idx][0] - '0';
            string pic = component[idx].Substring(2);
            Debug.Log(command);
            Debug.Log(pic);
            if (command == 0)
            {
                if (pic.Equals("clear"))
                {
                    background.GetComponent<SpriteRenderer>().sprite = null;
                }
                else
                {
                    background.GetComponent<SpriteRenderer>().sprite = ImageDictionary.getImage(pic);
                }
            } else if (command == 8)
            {
                bool alive = false;
                foreach (Unit u in CampaignData.members)
                {
                    if (u.unitName.Equals(pic) && u.isAlive())
                    {
                        alive = true;
                        break;
                    }
                }
                if (!alive)
                {
                    while (command != 7)
                    {
                        idx++;
                        if (idx == component.Count)
                        {
                            idx = 0;
                            component = dialogue.nextDialogueComponent();
                        }
                        command = component[idx][0] - '0';
                    }
                }
            } else if (command == 7)
            {
                //nothing. Just know that the conditional text is done
            }
            else //Only 1-5
            {
                if (pic.Equals("clear"))
                {
                    instantiatedSpeakerPortraits[command - 1].GetComponent<SpriteRenderer>().sprite = null;
                }
                else
                {
                    instantiatedSpeakerPortraits[command - 1].GetComponent<SpriteRenderer>().sprite = ImageDictionary.getImage(pic);
                    instantiatedSpeakerPortraits[command - 1].GetComponent<SpriteRenderer>().size = new Vector3(1, 1);
                }
            }
            idx++;
        }
        TextMeshProUGUI speakerName = dialogueBox.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI dialogueText = dialogueBox.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
        speakerName.text = component[idx].Equals("-") || component[idx].Equals("_") ? "" : component[idx];
        dialogueText.text = component[idx + 1].Equals("-") || component[idx + 1].Equals("_") ? "" : component[idx + 1];
        return true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void finish()
    {
        background.SetActive(false);
        dialogueBox.SetActive(false);
        foreach (GameObject ob in instantiatedSpeakerPortraits)
        {
            Destroy(ob);
        }
        instantiatedSpeakerPortraits.Clear();
        Destroy(instructions);

        storyComplete = true;
    }

    public override bool completed()
    {
        return storyComplete;
    }
    public override void LEFT_MOUSE(float mouseX, float mouseY)
    {
//        Z();
    }
    public override void RIGHT_MOUSE(float mouseX, float mouseY)
    {
        //nothing
    }

    public override void Z()
    {
        if (!nextSaying())
        {
            finish();
        }
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
        finish();
    }
    public override void ESCAPE()
    {
        //TODO maybe something
    }

}
