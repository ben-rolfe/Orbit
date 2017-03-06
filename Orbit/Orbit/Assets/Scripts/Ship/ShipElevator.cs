using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipElevator : MonoBehaviour {
	Animator anim;
	[SerializeField] string connectedScene;
	Transform car;
	AudioSource audioSource;

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
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
		//Find any followers who aren't in the car, and move them inside.
		foreach (GameObject follower in GameObject.FindGameObjectsWithTag("Follower"))
		{
			if (follower.transform.parent != car)
			{
				follower.transform.SetParent(car);
			}
		}
		StartMove(true);
	}
	
	void StartMove(bool ride)
	{
		anim.SetBool("ride", ride);
		foreach(ShipAgent passenger in GetComponentsInChildren<ShipAgent>())
		{
			passenger.EnablePhysics(false);
			//If any passengers are outside the car, move them inside
			if (Mathf.Abs(passenger.transform.localPosition.x) > 0.5f)
			{
				passenger.transform.localPosition = new Vector3(Mathf.Sign(passenger.transform.localPosition.x) * 0.5f, passenger.transform.localPosition.y, passenger.transform.localPosition.z);
			}
		}
		if (ride)
		{
			audioSource.Play();
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
