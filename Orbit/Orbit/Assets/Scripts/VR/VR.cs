using UnityEngine;
using System.Collections;

public class VR : MonoBehaviour {
	public ShipAvatar adult;
	public ShipAvatar child;

	public void SetTogetherness()
	{
		adult.transform.position = child.transform.position + Vector3.up * 0.3f; //Adding a little upness because of different collider offsets between child and adult.
	}
	public void SetListening()
	{
		FindObjectOfType<VRLevel>().TriggerListen();
	}
	public void SetUnderstanding(bool value)
	{
		for (int i=0; i<adult.transform.childCount; i++)
		{
			Transform child = adult.transform.GetChild(i);
			if (child.name == "platform")
			{
				child.gameObject.SetActive(value);
				break;
			}
		}
	}
	public void SetBelieving(bool value)
	{
		child.gameObject.layer = LayerMask.NameToLayer((value) ? "BraveAgents" : "Agents");
	}
	public void SetCourage(bool value)
	{
		adult.gameObject.layer = LayerMask.NameToLayer((value) ? "BraveAgents" : "Agents");
	}
}
