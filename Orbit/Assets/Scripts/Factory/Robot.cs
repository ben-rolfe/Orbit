using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Robot : MonoBehaviour {
	public static Color[] paintColor = new Color[8] {
		new Color(1f, 1f, 1f),
		new Color(1f, 0f, 0f),
		new Color(0f, 1f, 1f),
		new Color(0f, 0f, 1f),
		new Color(.4f, .2f, 0f),
		new Color(.8f, 0f, 1f),
		new Color(1f, 1f, 0f),
		new Color(.2f, 0f, 1f)
	};

	public int headVariant = -1;
	public int torsoVariant = -1;
	public int leftArmVariant = -1;
	public int rightArmVariant = -1;
	public int legVariant = -1;

	public Dictionary<string, int> variant = new Dictionary<string, int>();
	public int[] paint = { -1, -1, -1 };

	BoxCollider2D bc;

	void Start()
	{
		Ready();
	}

	public void Ready()
	{
		if (bc == null)
		{
			bc = GetComponent<BoxCollider2D>();

			//Populate variant dictionary and randomise any appendages that aren't specified
			variant["head"] = headVariant = (headVariant > -1) ? headVariant : Random.Range(0, 12);
			variant["stomach"] = torsoVariant;
			variant["arm_l"] = leftArmVariant = (leftArmVariant > -1) ? leftArmVariant : Random.Range(0, 7);
			variant["arm_r"] = rightArmVariant = (rightArmVariant > -1) ? rightArmVariant : Random.Range(0, 7);
			if (legVariant < 0)
			{
				legVariant = Random.Range(0, 6);
			}
			//Remove redundant legs
			if (legVariant < 3)
			{
				variant["leg_l"] = legVariant;
				variant["leg_r"] = legVariant;
				foreach (Transform t in transform)
				{
					if (t.name == "Mid Leg")
					{
						t.gameObject.SetActive(false);
					}
				}
			}
			else
			{
				variant["leg_m"] = legVariant;
				foreach (Transform t in transform)
				{
					if (t.name == "Left Leg" || t.name == "Right Leg")
					{
						t.gameObject.SetActive(false);
					}
				}
			}

			//Set appendage sprites and add crates to queue
			foreach (RobotAppendage appendage in GetComponentsInChildren<RobotAppendage>())
			{
				appendage.variant = variant[appendage.part];
				//After setting limbs, change them to generic part names.
				/*			if (appendage.part == "arm_l" || appendage.part == "arm_r")
							{
								appendage.part = "arm";
							}
							else if (appendage.part == "leg_l" || appendage.part == "leg_m" || appendage.part == "leg_r")
							{
								appendage.part = "leg";
							}*/
			}

			//Paint the robot unless it is a blueprint
			FactoryBlueprint blueprint = GetComponentInParent<FactoryBlueprint>();
			if (blueprint == null)
			{
				Paint();
			}
			else
			{
				bc.enabled = false;
				for (int i = 0; i < 3; i++)
				{
					if (paint[i] < 0 && (PlayerPrefs.GetInt("FactoryLevel") / 3) >= i) // If paint is not already set, randomise it, if high enough level
					{
						paint[i] = Random.Range(0, 7);
					}
				}

			}
		}
	}
	public void Undercoat(string part)
	{
		foreach (RobotAppendage appendage in GetComponentsInChildren<RobotAppendage>())
		{
			if (appendage.part == part)
			{
				appendage.GetComponent<SpriteRenderer>().color = paintColor[0];
			}
		}
		variant[part] = -2;
		bool ready = true;
		foreach (KeyValuePair<string, int> check in variant)
		{
			if (check.Value > -1)
			{
				ready = false;
				break;
			}
		}
		if (ready)
		{
			bc.enabled = true;
			GetComponentInParent<FactoryBlueprint>().Complete();
		}
	}
	public void Paint ()
	{
		foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
		{
			switch(sr.tag)
			{
				case "Paint 1":
					sr.color = paintColor[(paint[0] > -1) ? paint[0] + 1 : 0]; //If paint[0] is not set, use default colour
					break;
				case "Paint 2":
					sr.color = paintColor[(paint[1] > -1) ? paint[1] + 1 : ((paint[0] > -1) ? paint[0] + 1 : 0)]; //If paint[1] is not set, use paint[0] for these parts. If paint[0] is also not set, use default colour
					break;
				case "Paint 3":
					sr.color = paintColor[(paint[2] > -1) ? paint[2] + 1 : 0]; //If paint[2] is not set, use default colour
					break;
				default:
					sr.color = paintColor[0]; //Use default colour
					break;
			}
		}
	}
}
