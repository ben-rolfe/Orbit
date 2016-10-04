using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {
	Rigidbody2D rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public void FixedUpdate()
	{
		float posDiff = GameController.singleton.shipAvatar.transform.position.x - transform.position.x;
		Vector3 vel = rb.velocity;
		if (posDiff > 0.5f)
		{
			vel.x = (posDiff - 0.5f) * 5f;
		}
		else if (posDiff < -0.5f)
		{
			vel.x = (posDiff + 0.5f) * 5f;
		}
		else
		{
			vel.x = 0f;
		}
		rb.velocity = vel;
	}
}
