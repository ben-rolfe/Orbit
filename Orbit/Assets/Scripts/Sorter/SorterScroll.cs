using UnityEngine;
using System.Collections;

public class SorterScroll : MonoBehaviour {
	public Sprite[] types;
	[SerializeField] float speed;
	AudioSource track;

	void Start () {
		track = GetComponent<AudioSource>();
//		transform.Translate(new Vector3(-2.345f, 2f, 0f));
	}
	
	void Update()
	{
/*		if (Input.GetKeyDown("up"))
		{
			track.time += 1f;
		}
		else if (Input.GetKeyDown("down"))
		{
			track.time -= 1f;
		}*/
		transform.position = new Vector3(-2.345f, 2f - track.time, 0f);
	}

//	void FixedUpdate () {
//		transform.Translate(Vector3.down * speed * Time.fixedDeltaTime);
//	}
}
