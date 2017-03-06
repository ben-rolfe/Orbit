using UnityEngine;
using System.Collections;

public class CustomColor : MonoBehaviour {
	[SerializeField] string part;

	public void Recolor(string charName)
	{
		GetComponent<SpriteRenderer>().color = GameController.GetColor(charName + "_" + part);
	}
}
