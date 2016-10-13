using UnityEngine;
using System.Collections;

public class ClassifierAvatar : MonoBehaviour {

	void Start () {
		GameController.singleton.SetOverlay("Classifier Overlay");
		//		Invoke("Setup", 1000);
		GameController.singleton.GetComponentInChildren<Classifier>().Setup();
	}



}
