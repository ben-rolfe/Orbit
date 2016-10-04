using UnityEngine;
using System.Collections;

public class ShipAgent : MonoBehaviour {
	Rigidbody2D rb;
	BoxCollider2D col;
	Animator anim;

	void Awake () {
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<BoxCollider2D>();
		anim = GetComponentInChildren<Animator>();
	}

	void Update()
	{
		if (rb.velocity.x < 0)
		{
			transform.localScale = new Vector3(-transform.localScale.y, transform.localScale.y, transform.localScale.y);
		}
		else if (rb.velocity.x > 0) //Note: if vel.x == 0, keep current orientation.
		{
			transform.localScale = Vector3.one * transform.localScale.y;
		}
		anim.SetLayerWeight(1, Mathf.Abs(rb.velocity.x));
	}

	public void EnablePhysics(bool enabled)
	{
		rb.isKinematic = !enabled;
		col.enabled = enabled;
	}
}
