using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomPerson : MonoBehaviour {
	//	public int adultNumber; //trusted adult 1-5, or 0 for child
	void Awake()
	{
		if (name == "adult")
		{
			int checkpoint = GameController.GetInt("Checkpoint");
			if (checkpoint< 209)
			{
				name = "adult1";
			}
			else if (checkpoint< 319)
			{
				name = "adult2";
			}
			else if (checkpoint< 409)
			{
				name = "adult3";
			}
			else if (checkpoint< 539)
			{
				name = "adult4";
			}
			else
			{
				name = "adult5";
			}
		}
	}

	void Start () {
		Redress();
	}
	
	public static void RedressAll()
	{
		foreach (CustomPerson customPerson in FindObjectsOfType<CustomPerson>())
		{
			customPerson.Redress();
		}
	}

	void Redress()
	{
		foreach (CustomSprite customSprite in GetComponentsInChildren<CustomSprite>())
		{
			customSprite.Resprite(name);
		}
		foreach (CustomColor customColor in GetComponentsInChildren<CustomColor>())
		{
			customColor.Recolor(name);
		}
		foreach (CustomShow customShow in GetComponentsInChildren<CustomShow>())
		{
			customShow.Reshow(name);
		}
	}
}
