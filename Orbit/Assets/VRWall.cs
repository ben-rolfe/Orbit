using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRWall : MonoBehaviour {
	public int activeBricks;
	private int bricksShown;
	[SerializeField]
	private GameObject[] bricks;
	private AudioSource audio;

	void Awake () {
		audio = GetComponent<AudioSource>();
	}

	public void ShowBrick()
	{
		Debug.Log("Show Brick Called");

		if (bricksShown < activeBricks)
		{
			audio.Play();
			if (bricksShown < bricks.Length)
			{
				bricks[bricksShown].SetActive(true);
			}
			bricksShown++;
			Invoke("ShowBrick", .3f);
		}
		else
		{
			//All bricks shown. Call for the next queued line
			GameController.singleton.NextLine(); //Call for next queued line, after clip has finished playing
		}
	}
	public void HideBrick()
	{
		bricksShown--;
		audio.Play();
		if (bricksShown > -1)
		{
			bricks[bricksShown].SetActive(false);
		}
		GameController.singleton.Invoke("NextLine", 1f);
	}
}
