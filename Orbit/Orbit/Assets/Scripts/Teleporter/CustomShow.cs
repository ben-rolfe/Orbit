using UnityEngine;
using System.Collections;

public class CustomShow : MonoBehaviour {
	[SerializeField] bool showIfMale;
	[SerializeField] bool showIfFemale;
	[SerializeField] bool showIfAdult;
	[SerializeField] bool showIfChild;
	[SerializeField] bool hideIfWheelchair;
	[SerializeField] bool hideIfNotWheelchair;

	public void Reshow (string charName) {
		bool show = true;

		if (charName == "child")
		{
			if (!showIfChild)
			{
				show = false;
			}
		}
		else if (!showIfAdult)
		{
			show = false;
		}
		if (GameController.GetBool(charName + "_isMale"))
		{
			if (!showIfMale)
			{
				show = false;
			}
		}
		else if (!showIfFemale)
		{
			show = false;
		}
		if (hideIfWheelchair && GameController.GetInt(charName + "_legs") == 6)
		{
			show = false;
		}
		if (hideIfNotWheelchair && GameController.GetInt(charName + "_legs") != 6)
		{
			show = false;
		}
		//		GetComponent<SpriteRenderer>().enabled = show;
		foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
		{
			sr.enabled = show;
		}
	}
}
