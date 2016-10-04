using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipButtonZone : MonoBehaviour {
	[SerializeField] Button[] buttons;

	void Awake () {
		SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player")
		{
			SetActive(true);
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == "Player")
		{
			SetActive(false);
		}
	}

	void SetActive(bool active)
	{
		foreach (Button button in buttons)
		{
			button.gameObject.SetActive(active);
		}
	}
}
