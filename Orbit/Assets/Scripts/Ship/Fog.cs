using UnityEngine;
using System.Collections;

public class Fog : MonoBehaviour {
	[SerializeField] float driftSpeed = 1f;
	float initialX = 0f;
	void Start()
	{
		driftSpeed = Mathf.Abs(driftSpeed); // Drift is always in same direction
		initialX = transform.position.x;
	}

	void Update () {
		transform.Translate(Vector3.left * driftSpeed * Time.deltaTime);
		if (transform.position.x < initialX)
		{
			transform.Translate(Vector3.right * transform.localScale.y); //As long as the x scale is a full multiple of the y, this should work.
		} 
	}
}
