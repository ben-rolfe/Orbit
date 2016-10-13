using UnityEngine;
using System.Collections;

public class SpeakingCharacter : MonoBehaviour {
	public SpeechBubble speechBubble;
	AudioSource audioSource;
	Animator anim;
	int checkpoint = 0;
	[SerializeField] string[] chatLines;

	void Start()
	{
		anim = GetComponent<Animator>();
	}

	public void Speak(ScriptLine line)
	{
		if (speechBubble == null)
		{
			//If speech bubble doesn't exist, yet, create a new one and parent it to the storyteller canvas
			GameObject bubbleObj = Instantiate(Resources.Load("Prefabs/Speech Bubble")) as GameObject;
			bubbleObj.name = name + " speech";
			bubbleObj.transform.SetParent(GameObject.Find("World Canvas").transform);
			speechBubble = bubbleObj.GetComponent<SpeechBubble>();
			speechBubble.speaker = transform;
		}
		if (audioSource == null)
		{
			audioSource = speechBubble.GetComponent<AudioSource>();
		}
		speechBubble.gameObject.SetActive(true);
		speechBubble.text = (line.maleText != null && GameController.isMale) ? line.maleText : line.text;
		string clipName = line.lineID;
		if (clipName.Contains("|"))
		{
			clipName = clipName.Substring(0, clipName.IndexOf('|'));
		}

		if (name.StartsWith("adult"))
		{
			Debug.Log("adult speaking: " + name);
			clipName += "_a" + GameController.GetInt(name + "_voice");
		}
		if (line.maleText != null && GameController.isMale)
		{
			clipName += "_m";
		}
		AudioClip clip = Resources.Load<AudioClip>("Audio/en/" + clipName);
		if (clip != null)
		{
			if (anim != null)
			{
				anim.SetBool("speaking", true);
			}
			audioSource.PlayOneShot(clip);
			Invoke("Unspeak", clip.length); //Call for next queued line, after clip has finished playing
		}
		else
		{
			Debug.LogWarning("No audio clip: " + clipName);
			Unspeak();
		}

		//		speechBubble
	}
	void Unspeak()
	{
		if (anim != null)
		{
			anim.SetBool("speaking", false);
		}
		speechBubble.gameObject.SetActive(false);
		GameController.singleton.NextLine(); //Call for next queued line, after clip has finished playing
	}

	public void Clicked()
	{
		if (checkpoint < GameController.GetInt("Checkpoint"))
		{
			checkpoint = GameController.GetInt("Checkpoint");
			foreach (StoryTrigger trigger in GetComponents<StoryTrigger>())
			{
				trigger.Trigger();
			}
		}
		else if (chatLines.Length > 0) //have already spoken line for this checkpoint - pull a random chat line.
		{
			GameController.Directions(new string[] { chatLines[Random.Range(0, chatLines.Length)] });
		}
	}

}
