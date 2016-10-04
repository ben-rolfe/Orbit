using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipElevator : MonoBehaviour {
	Animator anim;
	[SerializeField] string connectedScene;
	Transform car;


	void Awake()
	{
		car = transform.Find("Elevator Car");
		//TODO: REMOVE - FOR TESTING
		//		PlayerPrefs.SetString("prevScene", "Ship Middle");
		//		PlayerPrefs.SetString("scene", "Ship Bottom");

		if (GameController.GetString("prevScene") == connectedScene)
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			player.transform.SetParent(car);
			player.transform.localPosition = new Vector3(0.3f, -1f, 0f);
			GameObject follower = GameObject.FindGameObjectWithTag("Follower");
			follower.transform.SetParent(car);
			follower.transform.localPosition = new Vector3(-0.3f, -1f, 0f);

		}
	}

	void Start () {
		anim = GetComponent<Animator>();
		StartMove(false);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player" || col.tag == "Follower")
		{
			col.transform.SetParent(car);
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == "Player" || col.tag == "Follower")
		{
			col.transform.SetParent(null);
		}
	}

	public void Ride()
	{
		StartMove(true);
	}
	
	void StartMove(bool ride)
	{
		anim.SetBool("ride", ride);
		foreach(ShipAgent passenger in GetComponentsInChildren<ShipAgent>())
		{
			passenger.EnablePhysics(false);
		}
	}

	void EndMove()
	{
		if (anim.GetBool("ride")) //Left scene - load new scene
		{
			GameController.singleton.LoadScene(connectedScene);
		}
		else //arrived on scene, enable passengers
		{
			foreach (ShipAgent passenger in GetComponentsInChildren<ShipAgent>())
			{
				passenger.EnablePhysics(true);
			}
		}
	}
}
