using UnityEngine;
using System.Collections;

public class FactoryPaintStack : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D col)
	{
		FactoryItem item = col.GetComponent<FactoryItem>();
		if (item != null)
		{
			if (item.part == "paint")
			{
				item.transform.SetParent(transform);
				item.transform.localPosition = new Vector3(0f, item.transform.localPosition.y, 0f);
			}
		}
	}
}
