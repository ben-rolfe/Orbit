using UnityEngine;
using System.Collections;

public class DecoratorInventoryItem : MonoBehaviour {
	[SerializeField] int item = 0;
	DecoratorMouse mouse;
	public void Start()
	{
		mouse = FindObjectOfType<DecoratorMouse>();
	}
	void OnMouseDown()
	{
		mouse.GrabItem(item);
	}

	void OnMouseUp()
	{
		mouse.PlaceItem();
	}
}
