using UnityEngine;
using System.Collections;

public class SpawnZone : MonoBehaviour {
	[SerializeField] string connectedScene;

	void Awake () {
		if (GameController.GetString("prevScene") == connectedScene)
		{
			//TODO: Also bring follower.
			GameObject.FindGameObjectWithTag("Player").transform.position = transform.position;
			GameObject.FindGameObjectWithTag("Follower").transform.position = transform.position + Vector3.right;
		}
	}

}
