﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Decorator : MonoBehaviour {
	[SerializeField] Button loungeButton;
	[SerializeField] Button bedroomButton;

	public void GoToBedroom()
	{
		bedroomButton.interactable = false;
		loungeButton.interactable = true;
		Camera.main.transform.position = new Vector3(3f, 0f, -10f);
	}
	public void GoToLounge()
	{
		bedroomButton.interactable = true;
		loungeButton.interactable = false;
		Camera.main.transform.position = new Vector3(9.4f, 0f, -10f);
	}

}