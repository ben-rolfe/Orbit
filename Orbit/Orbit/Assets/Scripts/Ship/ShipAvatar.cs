using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipAvatar : MonoBehaviour
{
	Rigidbody2D rb;
	CapsuleCollider2D col;
	[SerializeField]
	Slider movement;
	[SerializeField]
	bool vr;
	float runSpeed = 4f;
	float jumpPower = 230f;
	LayerMask jumpableLayers;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<CapsuleCollider2D>();
	}
	void Start()
	{
		jumpableLayers = LayerMask.GetMask("Platforms", "Obstacles", "Machinery");
		Debug.Log("Avatar Start");
		if (vr)
		{
			runSpeed = 3f; // Reduce the run speed so that the player can't jump across the whole screen
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
			rb.AddForce(Vector3.right * (horizontalMovement * runSpeed - rb.velocity.x) * 10f * rb.mass);
		}
	}

	public void Jump()
	{
		if (!rb.isKinematic && rb.velocity.y <= 0 && Physics2D.OverlapCircle((Vector2)transform.position + col.offset + Vector2.down * col.size.y / 2f, 0.1f, jumpableLayers) != null) //y check is to avoid double boost
		{
			rb.AddForce(Vector2.up * jumpPower * rb.mass);
		}
	}
}
