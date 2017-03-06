using UnityEngine;
using System.Collections;

public class UnscramblerFrame : MonoBehaviour {
	bool dragging = false;
	UnscramblerSocket _currentSocket;
	public int clip;
	public Animator anim;
	public float animLength;
	public SpriteRenderer diagnostic;

	void Start () {
	}

	void OnMouseDown()
	{
		currentSocket = null;
		dragging = true;
		transform.rotation = Quaternion.identity;
	}
	void OnMouseUp()
	{
		dragging = false;

		bool socketed = false;
		Collider2D[] cols = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition - Camera.main.transform.position));
		foreach (Collider2D col in cols)
		{
			UnscramblerSocket socket = col.GetComponent<UnscramblerSocket>();
			if (socket != null)
			{
				currentSocket = socket;
				socketed = true;
				break;
			}
		}
		if (!socketed)
		{
			Jiggle();
		}

	}

	public UnscramblerSocket currentSocket
	{
		get
		{
			return _currentSocket;
		}
		set
		{
			if (value != null)
			{
				if (value.socketed != null)
				{
					value.socketed.transform.Translate(0f, 1f, 0f);
					value.socketed.Jiggle();
					value.socketed.currentSocket = null;
				}
				value.socketed = this;
				transform.position = value.transform.position;
				//TODO: Layering
			}
			else if (_currentSocket != null)
			{
				//If removing a frame from a socket, let the socket know.
				_currentSocket.socketed = null;
			}
			_currentSocket = value;
		}
	}

	public void Jiggle()
	{
		//TODO: Layering
		transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(5f, 10f) * ((Random.Range(0, 2) == 0) ? 1 : -1)); //random rotation between 5-10 degrees in random direction
	}

	void Update()
	{
		if (dragging)
		{
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - Camera.main.transform.position) + Vector3.back;
		}
	}

}
