using UnityEngine;
using System.Collections;

public class UnscramblerAvatar : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameController.singleton.SetOverlay("Unscrambler Overlay");
		FindObjectOfType<Unscrambler>().Setup();
	}
	
}
