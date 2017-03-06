using UnityEngine;
using System.Collections;

public class UnscramblerAvatar : MonoBehaviour {
	public Animator[] anims;
	// Use this for initialization
	void Start () {
		GameController.singleton.SetOverlay("Unscrambler Overlay");
		GameController.singleton.GetComponentInChildren<Unscrambler>().Setup();
	}
	
}
