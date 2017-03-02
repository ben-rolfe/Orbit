using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Compass : MonoBehaviour {
	public float targetX = 0;
	[SerializeField]
	Sprite[] sprites;
	Image image;
	int currentSprite = -1;
	int nextSprite = 1;

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
				nextSprite = 2;
			}
			else if (Camera.main.transform.position.x > targetX + 1)
			{
				nextSprite = 0;
			}
			else
			{
				nextSprite = 1;
			}
		}
		else
		{
			nextSprite = 1;
		}
		if (currentSprite != nextSprite)
		{
			currentSprite = nextSprite;
			image.sprite = sprites[currentSprite];
			image.rectTransform.localScale = Vector3.one * 2f;
			Invoke("RestoreScale", 0.3f);
		}
	}
	void RestoreScale()
	{
		image.rectTransform.localScale = Vector3.one;
	}
}
