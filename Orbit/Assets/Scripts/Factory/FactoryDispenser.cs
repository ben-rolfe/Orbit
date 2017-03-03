using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FactoryDispenser : MonoBehaviour {
	[SerializeField] FactoryItem[] prefabArray;
	List<GameObject> crates = new List<GameObject>();
	Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
	[SerializeField] ParticleSystem[] guides;
	int guideCount = 0;
	int guide = 0;

	bool tutFirstArm = true;

	void Awake()
	{
		foreach (FactoryItem prefab in prefabArray)
		{
			prefabs.Add(prefab.part, prefab.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		float delay = (new float[12] { 8f, 4f, 4f, 3.5f, 3.5f, 3f, 2.5f, 2f, 3f, 3f, 3f, 3f })[GameController.GetInt("factory_level")];
		InvokeRepeating("Dispense", delay, delay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddCrate(string part, int variant)
	{
		GameObject crate = GameObject.Instantiate(prefabs[part]);
		crate.GetComponent<FactoryItem>().variant = variant;
		crate.transform.position = transform.position + Vector3.right * Random.Range(-1, 2); //spawn at dispenser position +/- 1 horizontal unit
		crate.SetActive(false);
		crates.Add(crate);
	}

	public void Dispense()
	{
		if (crates.Count > 0)
		{
			if (GameController.GetInt("factory_level") == 0) //Tutorial lines
			{
				string line = "factory_" + crates[0].GetComponent<FactoryItem>().part;
				if (line == "factory_arm" && tutFirstArm)
				{
					line += "1";
					tutFirstArm = false;
				}
				if (line == "factory_arm1" || line == "factory_paint")
				{
					// Sorry! Doing this because CancelInvoke to stop an invokerepeating also kills the invokerepeating that drops crates.
					Invoke("EmitGuideArrow", 0.3f);
					Invoke("EmitGuideArrow", 0.6f);
					Invoke("EmitGuideArrow", 0.9f);
					Invoke("EmitGuideArrow", 1.2f);
					Invoke("EmitGuideArrow", 1.5f);
					Invoke("EmitGuideArrow", 1.8f);
					Invoke("EmitGuideArrow", 2.1f);
					Invoke("EmitGuideArrow", 2.4f);
					Invoke("EmitGuideArrow", 2.7f);
					Invoke("EmitGuideArrow", 3f);
				}

				GameController.Directions(new string[] { line });
				if (line == "factory_head") //Paint coming next, begin private part intro
				{
					GameController.Directions(new string[] { "factory_private_01" });
				}
				if (line == "factory_paint") //Private Parts coming next, complete private part intro
				{
					GameController.Directions(new string[] { "factory_private_02", "factory_private_03" });
				}

			}
			crates[0].SetActive(true);
			crates.RemoveAt(0);
		}
	}

	public void Shuffle()
	{
		crates.Shuffle();
	}

	public void EmitGuideArrow()
	{
		if (guide < guides.Length)
		{
			guides[guide].Emit(1);
			if (++guideCount > 9)
			{
				guideCount = 0;
				guide++;
			}
		}
	}
}

