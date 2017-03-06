using UnityEngine;
using System.Collections;

public class CustomSprite : MonoBehaviour {
	[SerializeField] string part;
	[SerializeField] Sprite[] sprites;

	public void Resprite(string charName)
	{
		GetComponent<SpriteRenderer>().sprite = sprites[GameController.GetInt(charName + "_" + part)];
//		Debug.Log("Resprite " + charName + "_" + part + ": " + GameController.GetInt(charName + "_" + part));
	}
}
