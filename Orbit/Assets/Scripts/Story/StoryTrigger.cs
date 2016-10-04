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

}
