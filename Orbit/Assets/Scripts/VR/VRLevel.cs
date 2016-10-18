using UnityEngine;
using System.Collections;

public class VRLevel : MonoBehaviour
{
	[SerializeField]
	string[] introDirections;
	[SerializeField]
	string[] outroDirections;
	VRItem[] items;

	void Start()
	{
		items = FindObjectsOfType<VRItem>();
		GameController.Directions(introDirections); //Would really prefer this to happen on the ship.
	}

	public void TriggerListen()
	{
		int activeItems = items.Length;
		foreach (VRItem item in items)
		{
			if (item.isActiveAndEnabled)
			{
				item.TriggerListen();
			}
			else
			{
				activeItems--;

			}
		}
		//If some items were active, check if all items are inactive, now, and if so, play outro.
		Debug.Log(activeItems);
		if (activeItems > 0)
		{
			
			activeItems = items.Length;
			foreach (VRItem item in items)
			{
				if (!item.isActiveAndEnabled)
				{
					activeItems--;
				}
			}
			Debug.Log(activeItems);
			if (activeItems == 0)
			{
				GameController.Directions(outroDirections); //Would really prefer this to happen on the ship.
															//If the VR game was triggered from the ship, the player should return to the deck it was triggered from.
															//Otherwise, the player should return to the teleporter room.
				GameController.CompleteLevel("VR", GameController.GetInt("vr_level"));
				string returnScene = GameController.GetString("prevScene");
				if (!returnScene.StartsWith("Ship"))
				{
					returnScene = "Teleporter>ShipBottom";
				}
				GameController.Directions(new string[] { "@" + returnScene }); //Return to previous scene
			}

		}
	}
}
