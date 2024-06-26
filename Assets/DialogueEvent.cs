using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DialogueEvent : StoryEvent
{
	public string filename;
	private StreamReader scan;
	private List<List<string>> rets;
	private int idx;

	public DialogueEvent(int turn, string filename) : base(turn)
	{
		this.filename = filename;
		this.rets = new List<List<string>>();
		this.idx = 0;
		readDialogue();
	}

	public void readDialogue()
	{
		scan = new StreamReader(filename);
		while (scan.Peek() != -1) {
			List<string> ret = new List<string>();
			string test = scan.ReadLine();
			int input;
			while (int.TryParse(test[0] + "", out input))
            {
				ret.Add(test);
				test = scan.ReadLine();
            }
			ret.Add(test);
			ret.Add(scan.ReadLine());
			rets.Add(ret);
		}
		scan.Close();
	}
	/*
	 * For lines beginning with integers, the integer represents
	 * the position of the picture to change
	 * (0 = background, 1 = near left, 2 = near right, 3 = far left, 4 = far right, 5 = center)
	 * After this integer is the filename of the picture to change to
	 * 
	 * After those lines is one line describing the speaker
	 * Then one line with the dialogue. Use wrap-text to split it up
	 */
	public List<string> nextDialogueComponent()
{
	if (idx >= rets.Count)
	{
		return null;
	}
	List<string> ret = rets[idx];
	idx++;
	return ret;
}

}
