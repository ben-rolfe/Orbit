using UnityEngine;
using System.Collections;

public class VRHelpTrigger : MonoBehaviour {
	[SerializeField] string[] directions;
	bool triggered = false;

	void OnTriggerEnter2D(Collider2D col)
	{
		if (!triggered && (col.tag == "Player" || col.tag == "Follower"))
		{
			triggered = true;
			GameController.Directions(directions);
		}
	}
}
