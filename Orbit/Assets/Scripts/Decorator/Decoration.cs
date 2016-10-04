using UnityEngine;
using System.Collections;

public class Decoration : MonoBehaviour {
	public Sprite[] items;
	public static int numItems = 5; //Including an empty item at 0;
	public static int firstWallItem = 4;

	// Use this for initialization
	void Start ()
	{
		Redecorate();
	}
	
	public void SetItem(int item)
	{
		Debug.Log("set item");
		GameController.SetInt(name, item);
		Redecorate();
	}

	public int Redecorate()
	{
		int item = GameController.GetInt(name);
//		Debug.Log(item);
		if (item < items.Length)
		{
			GetComponent<SpriteRenderer>().sprite = items[item];
		}
		return item;
	}
	public static void RedecorateAll()
	{
		bool[] usedItems = new bool[numItems];
		foreach (Decoration decoration in FindObjectsOfType<Decoration>())
		{
			int usedItem = decoration.Redecorate();
			if (usedItem < numItems)
			{
				usedItems[usedItem] = true; //TODO: use this to build menu of items - alternatively, allow multiple instances of items? (and store by pos, instead
			}
		}
	}
}
