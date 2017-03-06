using UnityEngine;
using System.Collections;

public class ZipTo : MonoBehaviour {
	[SerializeField] bool setPointOnStart = false;
	public Vector3 point;
	public float time = 0.2f;

	void Start()
	{
		if (setPointOnStart)
		{
			point = transform.position;
		}
	}

	void FixedUpdate () {
		transform.position = Vector3.Lerp(transform.position, point, Time.deltaTime / time);
	}

	public void EnableWithDelay(float delay)
	{
		Invoke("Enable", delay);
	}
	void Enable()
	{
		enabled = true;
	}
}
