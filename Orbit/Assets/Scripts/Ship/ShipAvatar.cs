using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipAvatar : MonoBehaviour
{
	Rigidbody2D rb;
	BoxCollider2D col;
	[SerializeField]
	Slider movement;
	[SerializeField] bool vr;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<BoxCollider2D>();
	}
	void Start()
	{
		Debug.Log("Avatar Start");
		if (vr)
		{
			VR vrController = FindObjectOfType<VR>();
			if (name == "child")
			{
				vrController.child = this;
				movement = GameController.singleton.moveSliderChildVR;
			}
			else {
				vrController.adult = this;
				movement = GameController.singleton.moveSliderAdultVR;
			}
		}
		else
		//If there is a ship avatar in the scene, and it's not VR, then switch the overlay to the ship overlay.
		{
			movement = GameController.singleton.moveSlider;
			GameController.overlay = "Ship Overlay";
			GameController.SetAvatar(this);
		}
		movement.GetComponent<SpringbackSlider>().avatar = this;

	}

	void Update()
	{
		if (!vr)
		{
			Vector3 pos = Camera.main.transform.position;
			pos.x = transform.position.x;
			Camera.main.transform.position = pos;
		}

		if (Input.GetAxis("Vertical") > 0)
		{
			Jump();
		}

	}
	void FixedUpdate()
	{
		//TODO: Remove this code for mobile platforms (to disable movement opn mobile, we just remove controls)
		//Using isKinematic to disable physics
		if (!rb.isKinematic && movement != null)
		{
			Vector3 vel = rb.velocity;
			vel.x = ((movement.value != 0) ? movement.value : Input.GetAxis("Horizontal")) * 5f;
			rb.velocity = vel;
		}
	}

	public void Jump()
	{
//		Debug.Log("jump");
//		if (!rb.isKinematic && col.IsTouchingLayers(LayerMask.GetMask("Platforms")) && Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("Platforms")) != null) //Using isKinematic to disable physics
		if (!rb.isKinematic && col.IsTouchingLayers(LayerMask.GetMask("Platforms")))
		{
				rb.velocity = new Vector2(rb.velocity.x, 5f);
		}
	}
}
