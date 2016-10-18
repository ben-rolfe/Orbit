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
	float runSpeed = 3f;
	[SerializeField]
	float jumpPower = 250f;

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

	}
	void FixedUpdate()
	{
		//horizontalMovement = Input.GetAxis("Horizontal");
		//Using isKinematic to disable physics
		if (!rb.isKinematic && movement != null)
		{
			float horizontalMovement = movement.value;
//			horizontalMovement = Input.GetAxis("Horizontal");
//			Debug.Log(horizontalMovement);
			//TODO: simplify this when my brain is working.
			if (horizontalMovement < 0f)
			{
				if (rb.velocity.x > horizontalMovement * runSpeed)
				{
					rb.AddForce(Vector3.right * horizontalMovement * Mathf.Abs(rb.velocity.x - horizontalMovement * runSpeed) * 100f);
				}
			}
			else
			{
				if (rb.velocity.x < horizontalMovement * runSpeed)
				{
					rb.AddForce(Vector3.right * horizontalMovement * Mathf.Abs(rb.velocity.x - horizontalMovement * runSpeed) * 100f);
				}
			}
		}

		if (Input.GetAxis("Vertical") > 0)
		{
			Jump();
		}
	}

	public void Jump()
	{
		//		Debug.Log("jump");
		//		if (!rb.isKinematic && col.IsTouchingLayers(LayerMask.GetMask("Platforms")) && Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("Platforms")) != null) //Using isKinematic to disable physics
		if (!rb.isKinematic && col.IsTouchingLayers(LayerMask.GetMask("Platforms")) && rb.velocity.y <= 0) //y check is to avoid double boost
		{
			rb.AddForce(Vector2.up * jumpPower);
		}
	}
}
