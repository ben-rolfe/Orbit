using UnityEngine;
using System.Collections;

//TODO: Clean up. Lots of overlap between this class and factory blueprint

public class FactoryQueue : MonoBehaviour {
	public Vector3 zipPoint = Vector3.zero;
	Robot _robot;
	[SerializeField]
	SpriteRenderer[] paintIndicators;
	[SerializeField]
	Sprite[] paintSymbols;
	[SerializeField]
	SpriteRenderer glow;

	BoxCollider2D bc;

	void Start()
	{
		bc = GetComponent<BoxCollider2D>();
		zipPoint = transform.position + new Vector3(0.1f, -0.65f, 0f);
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
			if (_robot == null) //TODO: If robot set to null, clean up and unready queue
			{
				bc.enabled = true;
				glow.color = Color.clear;
				for (int i = 0; i < 3; i++)
				{
					paintIndicators[i].sprite = null;
				}
			}
			else //If robot set, ready queue
			{
				FactoryBlueprint blueprint = robot.GetComponentInParent<FactoryBlueprint>();
				if (blueprint != null)
				{
					blueprint.robot = null;
					robot.GetComponent<ZipTo>().point = zipPoint;
					robot.transform.SetParent(transform);
					bc.enabled = false;
					glow.color = new Color(0f, .4f, 1f);
					//				FactoryBooth booth = FindObjectOfType<FactoryBooth>(); // Find the booth so we 

					Destroy(robot.GetComponent<BoxCollider2D>());

					//Set paint indicators
					for (int i = 0; i < 3; i++)
					{
						if (robot.paint[i] > 0)
						{
							paintIndicators[i].sprite = paintSymbols[robot.paint[i]];
						}
					}
				}
			}
		}
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		Robot droppedRobot = col.GetComponent<Robot>();
		if (droppedRobot != null)
		{
			robot = droppedRobot;
		}
	}
}
