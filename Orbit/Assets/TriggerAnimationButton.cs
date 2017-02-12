using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimationButton : MonoBehaviour {
	[SerializeField]
	Animator anim;
	bool pressed = false;

	void Start()
	{
		anim.speed = 0f;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (!pressed && (col.tag == "Player" || col.tag == "Follower"))
		{
			anim.speed = 1f;
			transform.GetChild(0).position += Vector3.down * 0.05f;
			pressed = true;
		}

	}
	void OnTriggerExit2D(Collider2D col)
	{
		if (pressed && (col.tag == "Player" || col.tag == "Follower"))
		{
			anim.speed = 0f;
			transform.GetChild(0).position += Vector3.up * 0.05f;
			pressed = false;
		}

	}

}
