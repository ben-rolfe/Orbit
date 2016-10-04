using UnityEngine;
using System.Collections;

public class SorterScroll : MonoBehaviour {
	public Sprite[] types;
	[SerializeField] float speed;

	void Start () {
		transform.Translate(new Vector3(-2.345f, 2f, 0f));
	}
	
	void FixedUpdate () {
		transform.Translate(Vector3.down * speed * Time.fixedDeltaTime);
	}
}
