using UnityEngine;
using System.Collections;

public class SorterMessage : MonoBehaviour {
	public bool safe;
	public SpriteRenderer flag;
	int mode = 0;
	const float speed = 1f;

	[SerializeField] SpriteRenderer messageType;
	// Use this for initialization
	void Start () {
		Sprite[] types = GetComponentInParent<SorterScroll>().types;
		messageType.sprite = types[Random.Range(0, types.Length)];
		transform.localScale = Vector3.one * 0.9f;
	}

	void FixedUpdate()
	{
		switch (mode)
		{
			case 0:
				if (transform.position.y < -1.43f)
				{
					Sort(false);
				}
				break;
			case 1:
				transform.Translate(Vector3.right * speed * Time.fixedDeltaTime);
				if (transform.position.x > 1.3f)
				{
					ReadMessage();
				}
				else if (transform.position.x > 0.8f) //Start angling up after this point.
				{
					transform.Translate(Vector3.up * speed / 3f * Time.fixedDeltaTime);
				}
				break;
			case 3:
				transform.Translate(Vector3.right * speed * Time.fixedDeltaTime);
				if (transform.position.x > 0.75f)
				{
					mode = 4;
					Vector3 pos = transform.position;
					pos.x = 0.75f;
					transform.position = pos;
				}
				break;
			case 4:
				transform.Translate(Vector3.up * speed * Time.fixedDeltaTime);
				if (transform.position.y > 0.88f)
				{
					if (safe == flag.enabled) //Item has been mislabeled - send it left
					{
						mode = 5;
					}
					else //Item correctly labeled - send it right
					{
						mode = 7;
					}
					Vector3 pos = transform.position;
					pos.y = 0.88f;
					transform.position = pos;
				}
				break;
			case 5:
				transform.Translate(Vector3.left * speed * Time.fixedDeltaTime);
				if (transform.position.x < 0.21f)
				{
					// For now, at least, changing the left channel to be a hole, like the right, instead of a stack.
					// mode = 6; 
					mode = 10;
					Vector3 pos = transform.position;
					pos.x = 0.21f;
					transform.position = pos;
				}
				break;
			case 6:
				transform.Translate(Vector3.up * speed * Time.fixedDeltaTime);
				if (transform.position.y > 1.8f) //TODO: Stack multiple held messages
				{
					mode = 0;
					Vector3 pos = transform.position;
					pos.y = 1.8f;
					transform.position = pos;
					TrashMessage();
				}
				break;
			case 7:
				transform.Translate(Vector3.right * speed * Time.fixedDeltaTime);
				if (transform.position.x > 1.94f)
				{
					if (safe) //Safe item was hit. No big deal - we send it to Sammy
					{
						mode = 8;
					}
					else //Unsafe item was hit. Good work - we send it to the garbage chute
					{
						mode = 9;
					}
					Vector3 pos = transform.position;
					pos.x = 1.94f;
					transform.position = pos;
				}
				break;
			case 8:
				transform.Translate(Vector3.down * speed * Time.fixedDeltaTime);
				if (transform.position.y < -0.25f)
				{
					ReadMessage();
				}
				break;
			case 9:
				transform.Translate(Vector3.right * speed * Time.fixedDeltaTime);
				if (transform.position.x > 2.7f)
				{
					mode = 10;
					Vector3 pos = transform.position;
					pos.x = 2.7f;
					transform.position = pos;
				}
				break;
			case 10:
				transform.Translate(Vector3.up * speed * Time.fixedDeltaTime);
				if (transform.position.y > 2.25f)
				{
					TrashMessage();
				}
				break;


		}
	}

	public bool Sort(bool hit)
	{
		transform.SetParent(null);
		Vector3 pos = transform.position;
		if (hit)
		{
			mode = 3;
			pos.y = -0.6f;
		}
		else
		{
			mode = 1;
			pos.y = -1.43f;
		}
		transform.position = pos;
		transform.localScale = Vector3.one * 0.8f;
		//Was it hit if unsafe, or left if safe?
		return hit != safe;
	}

	void ReadMessage()
	{
		//TODO: Score.
		FindObjectOfType<SorterGameController>().score += (safe) ? 1 : -1;
		TrashMessage();
	}
	void TrashMessage()
	{
		Destroy(gameObject);
	}
}
