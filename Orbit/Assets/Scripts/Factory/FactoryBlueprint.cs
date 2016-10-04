using UnityEngine;
using System.Collections;

public class FactoryBlueprint : MonoBehaviour {
	public Vector3 zipPoint = Vector3.zero;
	Robot _robot;
	[SerializeField] SpriteRenderer[] paintIndicators;
	[SerializeField] Sprite[] paintSymbols;
	[SerializeField] SpriteRenderer glow;
	[SerializeField] Robot[] plans;
	FactoryBooth booth;

	SoundController sound;

	BoxCollider2D bc;

	void Start()
	{
		booth = FindObjectOfType<FactoryBooth>();
		bc = GetComponent<BoxCollider2D>();
		sound = Camera.main.GetComponent<SoundController>();
		zipPoint = transform.position + new Vector3(0.1f, -0.65f, 0f);
		robot = null;
	}

	public Robot robot
	{
		get
		{
			return _robot;
		}
		set
		{
			_robot = value;
			if (_robot == null) //TODO: If robot set to null, get next one from queue. If queue is done, clean up and unready blueprint.
			{
				if (booth.dispensed < booth.robots)
				{
					int plan = Random.Range(0, 6);
					//Custom blueprints on some levels
					switch (PlayerPrefs.GetInt("factory_level"))
					{
						case 0:
							plan = 6; //Tutorial blueprint
							break;
						case 1:
							plan = 7; //Forcer blueprint
							break;
						case 8:
							plan = 8; //Searcher blueprint
							break;
						case 9:
						case 10:
						case 11:
							plan = 9; //Constructor blueprint
							break;
					}
					robot = Instantiate(plans[plan], zipPoint, Quaternion.identity) as Robot;
					booth.dispensed++;
				}
				else
				{
					bc.enabled = false;
					glow.color = Color.clear;
					for (int i = 0; i < 3; i++)
					{
						paintIndicators[i].sprite = null;
					}
				}
			}
			else //If robot set, ready blueprint
			{
				robot.transform.SetParent(transform);
				robot.Ready();
				bc.enabled = true;
				glow.color = new Color(1f, .6f, 0f);

				//Add robot parts to dispenser
				FactoryDispenser dispenser = FindObjectOfType<FactoryDispenser>();
				dispenser.AddCrate("arm", robot.leftArmVariant);
				dispenser.AddCrate("arm", robot.rightArmVariant);
				dispenser.AddCrate("head", robot.headVariant);

				for (int i = 0; i < 3; i++)
				{
					if (robot.paint[i] > -1)
					{
						Debug.Log(paintIndicators[i].sprite);
						Debug.Log(i);
						Debug.Log(robot.paint[i]);
						Debug.Log(paintSymbols[robot.paint[i]]);
						paintIndicators[i].sprite = paintSymbols[robot.paint[i]];
						dispenser.AddCrate("paint", robot.paint[i]);
					}
				}

				dispenser.AddCrate("mouth", 0);
				dispenser.AddCrate("chest", 0);

				dispenser.AddCrate("leg", robot.legVariant);
				if (robot.legVariant < 3) // 2-legged robot; Add another leg.
				{
					dispenser.AddCrate("leg", robot.legVariant);
				}

				dispenser.AddCrate("crotch", 0);

				dispenser.AddCrate("stomach", robot.torsoVariant);

				//Order of parts shuffled (except in tutorial)
				if (GameController.GetInt("factory_level") > 0)
				{
					dispenser.Shuffle();
				}

			}

		}
	}
	void OnTriggerEnter2D (Collider2D col)
	{
		FactoryItem item = col.GetComponent<FactoryItem>();
		if (item != null)
		{
			switch(item.part)
			{
				case "arm":
					if (robot.variant["arm_l"] == item.variant)
					{
						robot.Undercoat("arm_l");
						Destroy(item.gameObject);
					}
					else if (robot.variant["arm_r"] == item.variant)
					{
						robot.Undercoat("arm_r");
						Destroy(item.gameObject);
					}
					break;
				case "leg":
					if (robot.variant.ContainsKey("leg_m") && robot.variant["leg_m"] == item.variant)
					{
						robot.Undercoat("leg_m");
						Destroy(item.gameObject);
					}
					else if (robot.variant.ContainsKey("leg_l") && robot.variant["leg_l"] == item.variant)
					{
						robot.Undercoat("leg_l");
						Destroy(item.gameObject);
					}
					else if (robot.variant.ContainsKey("leg_r") && robot.variant["leg_r"] == item.variant)
					{
						robot.Undercoat("leg_r");
						Destroy(item.gameObject);
					}
					break;
				default:
					if (robot.variant.ContainsKey(item.part) && robot.variant[item.part] == item.variant)
					{
						robot.Undercoat(item.part);
						Destroy(item.gameObject);
					}
					break;
			}
		}
	}
	public void Complete()
	{
		sound.PlaySound("FactoryProgress", "Achievement");
		if (GameController.GetInt("factory_level") == 0)
		{
			GameController.Directions(new string[] { "factory_queue" });
		}
		bc.enabled = false;
		glow.color = Color.green;
	}
/*	public void MoveToQueue (Draggable robot)
	{
		Debug.Log("YESS!!!!");
	}*/

}
