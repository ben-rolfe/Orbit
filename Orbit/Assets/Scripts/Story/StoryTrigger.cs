using UnityEngine;
using System.Collections;

public class StoryTrigger : MonoBehaviour {
	[SerializeField] bool entryTrigger;
	[SerializeField] int fromCheckpoint;
	[SerializeField] int toCheckpoint;
	[SerializeField] string[] directions;

	void OnTriggerEnter2D(Collider2D col)
	{
		if (entryTrigger && col.tag == "Player")
		{
			Trigger();
		}
	}


	public void Trigger()
	{
		if (GameController.GetInt("Checkpoint") >= fromCheckpoint && GameController.GetInt("Checkpoint") <= toCheckpoint)
		{
			Debug.Log("Trigger fired");
			GameController.Directions(directions);
		}
	}

	//Separate method required, since buttons only allow a void return type. TriggerTest is called from SpeakingCharacter.Clicked
	public bool TriggerTest()
	{
		if (GameController.GetInt("Checkpoint") >= fromCheckpoint && GameController.GetInt("Checkpoint") <= toCheckpoint)
		{
			Debug.Log("Trigger fired");
			GameController.Directions(directions);
			return true;
		}
		return false;
	}

}
