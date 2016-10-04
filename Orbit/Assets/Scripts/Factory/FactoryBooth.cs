using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FactoryBooth : MonoBehaviour {
	//TODO Load level
	int _robots = 0;
	int _completed = 0;
	public int dispensed = 0;
	[SerializeField] SpriteRenderer[] indicators;
	SoundController sound;

	bool busy;
	FactoryQueue[] qs;
	FactoryPartStack parts;

	void Start ()
	{
		GameController.overlay = "Factory Overlay";
		sound = Camera.main.GetComponent<SoundController>();
		sound.PlaySound("FactoryBackground", "Background", true);

		robots = (new int[12] { 1, 1, 2, 2, 3, 4, 5, 6, 3, 2, 2, 3 })[PlayerPrefs.GetInt("factory_level")]; //Number of robots per level is complex
		completed = 0;
		dispensed = 0;
		qs = GetComponentsInChildren<FactoryQueue>();
		parts = FindObjectOfType<FactoryPartStack>().GetComponent<FactoryPartStack>();
		InvokeRepeating("Check", 1f, 1f);

		switch (GameController.GetInt("factory_level"))
		{
			case 0:
				GameController.Directions(new string[] { "factory_01", "factory_02" });
				break;
			case 2:
				GameController.Directions(new string[] { "factory_multirobots" });
				break;
		}
	}

	public int robots
	{
		get
		{
			return _robots;
		}
		set
		{
			_robots = Mathf.Clamp(value, 0, 8);
			for (int i = 0; i < indicators.Length; i++)
			{
				indicators[i].color = new Color(1f, 1f, 1f, (i < _robots) ? 0.2f : 0f);
			}
		}
	}
	public int completed
	{
		get
		{
			return _completed;
		}
		set
		{
			_completed = Mathf.Clamp(value, 0, robots);
			for (int i = 0; i < robots; i++)
			{
				indicators[i].color = new Color(1f, 1f, 1f, (i < _completed) ? 1f : 0.2f);
			}
		}
	}

	void Check()
	{
		if (!busy)
		{
			
			foreach(FactoryQueue q in qs)
			{
				if (q.robot != null)
				{
					//Is there a full stack of private parts?
					foreach (List<FactoryItem> stack in parts.stacks)
					{
						if (stack.Count == 3)
						{
							//Are the right paints stacked?
							FactoryItem[] paints = FindObjectOfType<FactoryPaintStack>().GetComponentsInChildren<FactoryItem>();
							bool allPaints = true;
							List<int> usedPaints = new List<int>();
							for (int i = 0; i < 3; i++)
							{
								if (q.robot.paint[i] > -1)
								{
									bool paintFound = false;
									for (int j = 0; j < paints.Length; j++)
									{
										if (q.robot.paint[i] == paints[j].variant && !usedPaints.Contains(j))
										{
											usedPaints.Add(j);
											paintFound = true;
											break;
										}
									}
									if (!paintFound)
									{
										allPaints = false;
										break;
									}
								}
							}
							if (allPaints)
							{
								//All requirements met, let's paint a robot!
								if (GameController.GetInt("factory_level") == 0)
								{
									GameController.Directions(new string[] { "factory_firstrobot" });
								}
								busy = true;
								foreach (FactoryItem part in stack)
								{
									part.GetComponent<ZipTo>().point = transform.position;
									Destroy(part, 0.5f);
								}
								stack.Clear();
								foreach (int paint in usedPaints)
								{
									paints[paint].transform.SetParent(null);
									ZipTo zip = paints[paint].gameObject.AddComponent<ZipTo>();
									zip.point = transform.position;
									Destroy(paints[paint].GetComponent<BoxCollider2D>());
									paints[paint].GetComponent<Rigidbody2D>().isKinematic = true;
									Destroy(paints[paint], 0.5f);
								}
								q.robot.GetComponent<ZipTo>().point = transform.position;
								q.robot.transform.SetParent(transform);
								q.robot = null;
								sound.PlaySound("FactoryPaint", "Achievement");
								Invoke("Finish", 1f);
								break;
							}
							Debug.Log("ready");
							if (busy)
							{
								break;
							}
						}
					}
					if (busy)
					{
						break;
					}
				}
			}
		}
	}
	void Finish()
	{
		sound.PlaySound("FactoryComplete", "Achievement");
		Robot robot = GetComponentInChildren<Robot>();
		robot.Paint();
		robot.GetComponent<ZipTo>().point = new Vector3(transform.position.x + 3f - completed / 3 - (completed % 3) * 0.25f, transform.position.y + 0.5f - (completed % 3) * 0.5f, transform.position.z);
		robot.transform.SetParent(null);
		completed++;
		busy = false;
		Invoke("Complete", 0.5f);
	}
	void Complete()
	{
		if (completed == robots)
		{
			GameController.CompleteLevel("factory", GameController.GetInt("factory_level"));
			GameController.Directions(new string[] { "@ShipBottom" });
		}
	}
	public void BreakDown(string cause)
	{
		sound.PlaySound("FactoryBreakdown", "Background", false);
		GameController.Directions(new string[] { "factory_fail" + cause, "@ShipBottom>Factory" });
	}
}
