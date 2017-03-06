using UnityEngine;
using System.Collections;

public class VRItem : MonoBehaviour {

	[SerializeField] string[] directions;

	bool inRange = false;

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.name == "child")
		{
			inRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.name == "child")
		{
			inRange = false;
		}
	}

	public void TriggerListen()
	{
		if (inRange)
		{
			GameController.Directions(directions);
			gameObject.SetActive(false);
		}
	}

}
