using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpringbackSlider : MonoBehaviour {
	[SerializeField]
	string pcKeyLeft;
	[SerializeField]
	string pcKeyRight;
	[SerializeField]
	string pcKeyJump;
	public ShipAvatar avatar;
	public void Reset(BaseEventData data)
	{
		GetComponent<Slider>().value = 0f;
	}
	public void Jump()
	{
		avatar.Jump();
	}

#if UNITY_EDITOR || !(UNITY_IOS || UNITY_ANDROID || UNITY_WP8)
	void Update()
	{
		if (Input.GetKeyDown(pcKeyJump))
		{
			Jump();
		}


		if (Input.GetKeyUp(pcKeyLeft) || Input.GetKeyUp(pcKeyRight))
		{
			GetComponent<Slider>().value = 0f;
		}
		else
		{
			GetComponent<Slider>().value = (Input.GetKey(pcKeyRight) ? 1 : 0) - (Input.GetKey(pcKeyLeft) ? 1 : 0);
		}
	}
#endif


}
