using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SorterButton : MonoBehaviour {
	[SerializeField] string pcKey; //TODO: Key link and display for PC.
	Button button;
	Image img;
	Sprite unpressedSprite;
	[SerializeField] ParticleSystem stars;

	void Start()
	{
		button = GetComponent<Button>();
		img = GetComponent<Image>();
		unpressedSprite = img.sprite;
		pcKey = pcKey.ToLower();
#if UNITY_EDITOR || !(UNITY_IOS || UNITY_ANDROID || UNITY_WP8)
		GetComponentInChildren<Text>().text = pcKey.ToUpper();
#else
		GetComponentInChildren<Text>().enabled = false;
#endif
	}

	public void Hit () {
		foreach (Collider2D col in Physics2D.OverlapCircleAll(transform.position, 0.1f)) {
			if (col.GetComponent<SorterMessage>().Sort(true))
			{
				stars.Emit(5);
			}
			col.enabled = false;
		}
	}

#if UNITY_EDITOR || !(UNITY_IOS || UNITY_ANDROID || UNITY_WP8)
	void Update()
	{
		if (Input.GetKeyDown(pcKey))
		{
			Debug.Log(pcKey);
			img.sprite = button.spriteState.pressedSprite;
			button.onClick.Invoke();
		}
		if (Input.GetKeyUp(pcKey))
		{
			img.sprite = unpressedSprite;
		}
	}
#endif
}
