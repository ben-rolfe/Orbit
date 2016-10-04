using UnityEngine;
using System.Collections;

public class FactoryItem : MonoBehaviour {
	SpriteRenderer sr;
	public string part;
	int _variant;
	[SerializeField] SpriteRenderer mark;
	[SerializeField] Sprite[] variants;
	SoundController sound;

	void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		sound = Camera.main.GetComponent<SoundController>();
		GetComponent<Draggable>().Grab = Grab;
		GetComponent<Draggable>().Drop = Drop;
		if ((part == "mouth" || part == "chest" || part == "crotch") && GameController.GetInt("factory_level") <3) //For first few levels, colour code the private body parts
		{
			GetComponent<SpriteRenderer>().color = Color.red;
		}
	}
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.collider.name == "Floor")
		{
			FindObjectOfType<FactoryBooth>().BreakDown((part == "paint") ? "paint" : "part");
		}
		else if (col.collider.GetComponent("FactoryConveyor") != null)
		{
			sr.sortingOrder = col.collider.GetComponent<SpriteRenderer>().sortingOrder + 1;
		}
		sound.PlaySound("FactoryThunk", "ItemLand");
	}

	public int variant
	{
		get
		{
			return _variant;
		}
		set
		{
			_variant = Mathf.Clamp(value, 0, variants.Length - 1);
			mark.sprite = variants[_variant];
		}
	}

	void Grab(Draggable obj)
	{
		if (part == "chest" || part == "crotch" || part == "mouth")
		{
			FindObjectOfType<FactoryBooth>().BreakDown("private");
		}
		else
		{
			sound.PlaySound("FactoryBwoop", "ItemGrabDrop");
		}
		transform.SetParent(null);
	}
	void Drop(Draggable obj)
	{
		sound.PlaySound("FactoryPew", "ItemGrabDrop");
	}

}
