using UnityEngine;
using System.Collections;

public class DecoratorPosition : MonoBehaviour {
	DecoratorMouse mouse;
	Decoration dec;

	void Awake()
	{
		Debug.Log("test");
		Debug.Log(GetComponent<SpriteRenderer>().sprite);
		dec = GetComponent<Decoration>();
		//Set the "no object" image to be the position's tile.
		//Must occur before the Decoration's first Redecorate call, which happens during Start.
		dec.items[0] = GetComponent<SpriteRenderer>().sprite;
	}

	void Start()
	{
		mouse = FindObjectOfType<DecoratorMouse>();
	}

	void OnMouseDown()
	{
		mouse.GrabItem(GameController.GetInt(name));
		dec.SetItem(0);
	}

	void OnMouseUp()
	{
		mouse.PlaceItem();
	}
}
