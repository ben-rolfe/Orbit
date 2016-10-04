using UnityEngine;
using System.Collections;

//Slowly Rotate Earth

public class EarthRotation : MonoBehaviour {
	void Update () {
		transform.localEulerAngles = new Vector3(0f, -Time.realtimeSinceStartup, 0f);
	}
}
