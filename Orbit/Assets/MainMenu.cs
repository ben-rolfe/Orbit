using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	void Start () {
		GameController.singleton.SetOverlay("Main Menu");
	}
}
