using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {
	bool _dragging = false;
	int returnLayer;
	ZipTo zip;
	public delegate void DragDelegate(Draggable obj);
	public DragDelegate Drop = delegate (Draggable obj) { /* Do nothing by default */ };
	public DragDelegate Grab = delegate (Draggable obj) { /* Do nothing by default */ };
	Rigidbody2D rb;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		returnLayer = gameObject.layer;
		zip = GetComponent<ZipTo>();
	}

	void FixedUpdate ()
	{
		if (dragging)
		{
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - Camera.main.transform.position);
		}
	}

	void OnMouseDown()
	{
		dragging = true;
	}

	void OnMouseUp()
	{
		dragging = false;
	}

	public bool dragging
	{
		get
		{
			return _dragging;
		}
		set
		{
			if (_dragging != value)
			{
				_dragging = value;
				rb.isKinematic = value;
				rb.velocity = Vector2.zero;
				if (_dragging)
				{
					Grab(this);
					if (zip != null)
					{
						zip.enabled = false;
					}
					foreach (SpriteRenderer sr in gameObject.GetComponentsInChildren<SpriteRenderer>())
					{
						sr.sortingLayerName = "Foreground";
					}
					returnLayer = gameObject.layer;
					gameObject.layer = LayerMask.NameToLayer("Dragging");
				}
				else
				{
					if (zip != null)
					{
						foreach (SpriteRenderer sr in gameObject.GetComponentsInChildren<SpriteRenderer>())
						{
							sr.sortingLayerName = "Midground";
						}
						zip.EnableWithDelay(0.1f);
						//zip.enabled = true;
					}
					else
					{
						foreach (SpriteRenderer sr in gameObject.GetComponentsInChildren<SpriteRenderer>())
						{
							sr.sortingLayerName = "Default";
						}
					}
					//					gameObject.GetComponent<SpriteRenderer>().sortingOrder = 5;
					gameObject.layer = returnLayer;
					Drop(this);
				}
			}
		}
	}
}
