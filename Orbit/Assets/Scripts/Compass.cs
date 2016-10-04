using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Compass : MonoBehaviour {
	public float targetX = 0;
	[SerializeField]
	Sprite[] sprites;
	Image image;

	void Start()
	{
		image = GetComponent<Image>();
	}

	void Update()
	{
		if (targetX!=0)
		{
//			Debug.Log(Camera.main.transform.position.x + ":" + targetX);
			if (Camera.main.transform.position.x < targetX - 1)
			{
				image.sprite = sprites[2];
			}
			else if (Camera.main.transform.position.x > targetX + 1)
			{
				image.sprite = sprites[0];
			}
			else
			{
				image.sprite = sprites[1];
			}
		}
		else
		{
			image.sprite = sprites[1];
		}
	}
}
