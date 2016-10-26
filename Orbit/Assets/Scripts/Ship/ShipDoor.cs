using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShipDoor : MonoBehaviour {
	BoxCollider2D sensor;
	BoxCollider2D barrier;
	List<Collider2D> cols = new List<Collider2D>();
	Animator anim;
	[SerializeField]
	int lockedFrom = 0;
	[SerializeField]
	int lockedTo = 0;
	[SerializeField]
	Button knockButton;


	void Start()
	{
		anim = GetComponent<Animator>();
		//Find the barrier (the BoxCollider2D that isn't a trigger)
		foreach (BoxCollider2D col in GetComponents<BoxCollider2D>())
		{
			if (!col.isTrigger)
			{
				barrier = col;
				break;
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		cols.Add(col);
		if (!locked)
		{
			Open();
		}
	}
	void OnTriggerExit2D(Collider2D col)
	{
		cols.Remove(col);
		if (!locked)
		{
			Close();
		}
	}
	public void Open()
	{
		if (knockButton != null)
		{
			knockButton.gameObject.SetActive(false);
		}
		anim.SetBool("open", true);
		barrier.size = new Vector2(1f, 1.5f);
		barrier.offset = new Vector2(0f, 1.25f);
	}
	public void Close()
	{
		if(cols.Count==0) //Only close door if last collider is clear
		{
			anim.SetBool("open", false);
			barrier.size = new Vector2(1f, 4f);
			barrier.offset = Vector2.zero;
		}
	}

	bool locked
	{
		get
		{
			int checkpoint = GameController.GetInt("Checkpoint");
			return checkpoint >= lockedFrom && checkpoint <= lockedTo;
		}
	}

}
