using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour {
	public Transform speaker;
	public bool inFront;
	//	public Vector3 offset;
	//	private Vector3 flippedOffset;
	private Vector3 position;
	private Vector3 scale;
	private Vector3 flippedScale;
	private Text textBox;
	private float yScale;


	void Awake()
	{
		yScale = transform.localScale.y;
		textBox = GetComponentInChildren<Text>();
//		flippedOffset = new Vector3(-offset.x, offset.y, offset.z);
		if (inFront)
		{
			scale = new Vector3(-1f, yScale, 1f);
			flippedScale = new Vector3(1f, yScale, 1f);
		}
		else
		{
			scale = new Vector3(1f, yScale, 1f);
			flippedScale = new Vector3(-1f, yScale, 1f);
		}
	}

	
	// Update is called once per frame
	void Update () {

		bool xFlipped = (speaker.localScale.x > 0);
		if (GetComponentInParent<Canvas>().renderMode == RenderMode.WorldSpace) //If in world space, check that the speech bubble is near the edge of the screen, and override the flippedness accordingly. Unfortunately, can't check this on awake - it doesn't find the canvas.
		{
			Vector3 bubbleOffset = Camera.main.transform.position - transform.position;
			if (bubbleOffset.x < -(Camera.main.orthographicSize - 0.5f))
			{
				xFlipped = false;
			}
			else if (bubbleOffset.x > (Camera.main.orthographicSize - 0.5f))
			{
				xFlipped = true;
			}
		}

		if (xFlipped)
		{
			textBox.transform.localScale = transform.localScale = scale;
		}
		else
		{
			textBox.transform.localScale = transform.localScale = flippedScale;
		}
		transform.position = speaker.position + Vector3.back * 2; //TODO: Don't let go off screen;
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
