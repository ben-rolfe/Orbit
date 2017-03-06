using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour {
	public Transform speaker;
	public bool inFront;
	//	public Vector3 offset;
	//	private Vector3 flippedOffset;
	private Vector3 position;
//	private Vector3 scale;
//	private Vector3 flippedScale;
	private Text textBox;
	private float yScale;


	void Awake()
	{
//		yScale = transform.localScale.y;
		textBox = GetComponentInChildren<Text>();
//		flippedOffset = new Vector3(-offset.x, offset.y, offset.z);
/*		if (inFront)
		{
			scale = new Vector3(-1f, yScale, 1f);
			flippedScale = new Vector3(1f, yScale, 1f);
		}
		else
		{
			scale = new Vector3(1f, yScale, 1f);
			flippedScale = new Vector3(-1f, yScale, 1f);
		}*/
	}

	
	// Update is called once per frame
	void Update () {

//		Vector3 scale = transform.localScale; //speaker.localScale?
		Vector3 scale = Vector3.one; //speaker.localScale?

		if (GetComponentInParent<Canvas>() != null) //Avoid errors if canvas not responding.
		{
			if (GetComponentInParent<Canvas>().renderMode == RenderMode.WorldSpace) //If in world space, check if the speech bubble is near the edge of the screen, and override the flippedness accordingly.
			{
				Vector3 bubbleOffset = Camera.main.transform.position - transform.position;
				Debug.Log(bubbleOffset);
				if (bubbleOffset.x > 0.1f) //Offset slightly, to stop the avatar's speech bubble from jumping about when they move.
				{
					scale.x = -1f;
				}
				if (bubbleOffset.y < 0f)
				{
					scale.y = -1f;
				}
				transform.localScale = scale;
			}
			else
			{
				if (transform.parent.localScale.x < 0)
				{
					scale.x = -1f;
				}
				if (transform.parent.localScale.y < 0)
				{
					scale.y = -1f;
				}
				Debug.Log(scale);
				transform.localScale = Vector3.one;
			}
			textBox.transform.localScale = scale;
			transform.position = speaker.position + Vector3.back * 2;
		}

	}

	public string text
	{
		get
		{
			return textBox.text;
		}
		set
		{
			textBox.text = value;
		}
	}
}
