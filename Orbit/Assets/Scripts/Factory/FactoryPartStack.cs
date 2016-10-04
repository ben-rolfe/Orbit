using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FactoryPartStack : MonoBehaviour {
	public List<FactoryItem>[] stacks = new List<FactoryItem>[6];
	void Start()
	{
		for (int i = 0; i < stacks.Length; i++)
		{
			stacks[i] = new List<FactoryItem>();
		}
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		FactoryItem item = col.GetComponent<FactoryItem>();
		if (item != null)
		{
			if (item.part == "chest" || item.part == "crotch" || item.part == "mouth")
			{
				int stack = -1;
				//Look for non-empty stacks requiring the part
				for (int i = 0; i < 6; i++)
				{
					if (stacks[i].Count > 0)
					{
						bool alreadyInStack = false;
						foreach (FactoryItem stackedItem in stacks[i])
						{
							if (stackedItem.part == item.part)
							{
								alreadyInStack = true;
								break;
							}
						}
						if (!alreadyInStack)
						{
							stack = i;
							break;
						}
					}
					else if (stack < 0) // As a backup, in case no non-empty stacks require this part, store the first empty stack
					{
						stack = i;
					}

				}
				stacks[stack].Add(item);
				ZipTo zip = item.gameObject.AddComponent<ZipTo>();
				zip.point = new Vector3(transform.position.x - stack * 0.35f, transform.position.y - 2.5f + (stacks[stack].Count + stack % 2) * 0.55f, transform.position.z);
				Destroy(item.GetComponent<BoxCollider2D>());
				item.GetComponent<Rigidbody2D>().isKinematic = true;
			}
		}
	}
}
