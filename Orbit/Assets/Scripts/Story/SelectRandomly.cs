using UnityEngine;
using System.Collections;

public class SelectRandomly : MonoBehaviour {
	[SerializeField] GameObject[] options;

	// Use this for initialization
	void Awake () {
		if (options.Length > 1)
		{
			int choice = Random.Range(0, options.Length);
			for (int i = 0; i < options.Length; i++)
			{
				if (i != choice)
				{
					options[i].SetActive(false);
				}
			}
		}
	}
}
