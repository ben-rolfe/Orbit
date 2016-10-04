using UnityEngine;
using System.Collections;

public class ClassifierAvatar : MonoBehaviour {

	void Start () {
		GameController.singleton.SetOverlay("Classifier Overlay");
		FindObjectOfType<Classifier>().Setup();
	}
	
}
