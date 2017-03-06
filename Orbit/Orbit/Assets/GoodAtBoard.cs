using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoodAtBoard : MonoBehaviour {

	void Start () {
		Refresh();
	}

	public void Refresh()
	{
		GetComponent<Text>().text = "I am good at " + GameController.GetString("goodat").Trim();
	}
}
