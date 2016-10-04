using UnityEngine;
using System.Collections;

public class DecoratorMouse : MonoBehaviour {
	Decoration dec;
	int item;
	void Awake()
	{
		dec = GetComponent<Decoration>();
	}
	void Update () {
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - Camera.main.transform.position) + Vector3.back;
	}
	public void GrabItem(int item)
	{
		this.item = item;
		dec.SetItem(item);
	}
	public void PlaceItem()
	{
		Collider2D[] cols = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition - Camera.main.transform.position));
		if (cols.Length > 0)
		{
			DecoratorPosition pos = cols[0].GetComponent<DecoratorPosition>();
			if (pos != null)
			{
				Decoration posDec = pos.GetComponent<Decoration>();
				if (posDec.items[item] != null && GameController.GetInt(posDec.name) == 0) //If the position allows this item, and there is nothing in the position already
				{
					posDec.SetItem(item);
				}
			}
		}
		//Clear the item from the mouse
		GrabItem(0);
	}
}
