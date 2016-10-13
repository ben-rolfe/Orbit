using UnityEngine;
using System.Collections;

public class TeleporterAvatar : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("Check1");
		GameController.singleton.SetOverlay("Teleporter Overlay");
		FindObjectOfType<Teleporter>().Setup();
	}
}
