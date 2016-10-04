using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpringbackSlider : MonoBehaviour {
	public ShipAvatar avatar;
	public void Reset(BaseEventData data)
	{
		GetComponent<Slider>().value = 0f;
	}
	public void Jump()
	{
		avatar.Jump();
	}
}
