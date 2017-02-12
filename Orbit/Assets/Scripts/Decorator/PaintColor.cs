using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintColor : MonoBehaviour {
	[SerializeField]
	string objectName;

	void Start()
	{
		Paint();
	}

	public void Paint () {
		GetComponent<SpriteRenderer>().color = GameController.GetColor(objectName + "_paint");
	}
}
