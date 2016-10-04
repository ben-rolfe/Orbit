using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour {
	public Transform speaker;
	public bool inFront;
	public Vector3 offset;
	private Vector3 flippedOffset;
	private Vector3 scale;
	private Vector3 flippedScale;
	private Text textBox;
	private float yScale;

	void Awake()
	{
		yScale = transform.localScale.y;
		textBox = GetComponentInChildren<Text>();
		flippedOffset = new Vector3(-offset.x, offset.y, offset.z);
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
		if (speaker.localScale.x > 0)
		{
			transform.position = speaker.position + offset;
			textBox.transform.localScale = transform.localScale = scale;
		}
		else
		{
			transform.position = speaker.position + flippedOffset;
			textBox.transform.localScale = transform.localScale = flippedScale;
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
