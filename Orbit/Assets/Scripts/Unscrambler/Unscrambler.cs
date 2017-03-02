using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Unscrambler : MonoBehaviour {
	[SerializeField] UnscramblerFrame framePrefab;
	[SerializeField] UnscramblerSocket socketPrefab;
	[SerializeField] Camera cameraPrefab;
	[SerializeField] Sprite[] diagnosticSprites;

	int[] solutions;
	int[] clips;
	UnscramblerSocket[] sockets;
	UnscramblerFrame[] frames;

	int watchingSocket;
	List<string> helpDirections;

	public void Setup()
	{
		UnscramblerAvatar unscramblerAvatar = FindObjectOfType<UnscramblerAvatar>();

		switch (GameController.GetInt("unscrambler_level"))
		{
			case 0:
				solutions = new int[] { 0, 1, 2 };
				clips = new int[] { 0, 10, 2, 11, 1, 12 };
				break;
			case 1:
				solutions = new int[] { 3, 4, 5 };
				clips = new int[] { 13, 11, 12, 4, 3, 10, 5 };
				break;
			default:
				solutions = new int[] { 6, 7, 8, 9 };
				clips = new int[] { 6, 15, 14, 7, 8, 11, 9, 12 };
				break;

		}

		watchingSocket = -1;

		sockets = new UnscramblerSocket[solutions.Length];
		for (int i = 0; i < solutions.Length; i++)
		{
			sockets[i] = Instantiate<UnscramblerSocket>(socketPrefab);
			sockets[i].transform.position = new Vector3(1.35f * i - 2f, -1.5f, 1f);
		}

		frames = new UnscramblerFrame[clips.Length];
		for (int i = 0; i < clips.Length; i++)
		{
			frames[i] = Instantiate<UnscramblerFrame>(framePrefab);
			frames[i].clip = clips[i];
			Debug.Log(GameObject.Find("clip" + clips[i]));
			Debug.Log(GameObject.Find("clip" + clips[i]).GetComponent<Animator>());
			//			frames[i].anim = GameObject.Find("clip" + clips[i]).GetComponent<Animator>();
			//			frames[i].animLength = frames[i].anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
			frames[i].anim = unscramblerAvatar.anims[clips[i]];
			frames[i].animLength = frames[i].anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
			
			Camera clipCamera = Instantiate<Camera>(cameraPrefab);
			Debug.Log(GameObject.Find("clip" + clips[i]).transform.localPosition + Vector3.back * 10f);
			clipCamera.transform.position = frames[i].anim.transform.position + Vector3.back * 10f;
			RenderTexture frameTexture = new RenderTexture(480, 320, 24);
			clipCamera.targetTexture = frameTexture;

			Material frameMaterial = new Material(Shader.Find("Unlit/Texture"));
			frameMaterial.mainTexture = frameTexture;
			frames[i].GetComponentInChildren<MeshRenderer>().material = frameMaterial;

			if (i < sockets.Length)
			{
				frames[i].currentSocket = sockets[i];
			}
			else
			{
				frames[i].transform.position = new Vector3(2 * ((i - sockets.Length) % 2) - 0.5f - ((i - sockets.Length) / 2), 1.2f * (1 - ((i - sockets.Length) / 2)), 1f);
				frames[i].Jiggle();
			}
		}

		GameController.Directions(new string[] { "unscrambler_help_intro" });
//		GameController.Directions(new string[] { "unscrambler_help_intro", "unscrambler_help_original|0" });
	}

	public void Watch () {
		//Can't do this in setup, because the animators aren't ready, and return null :/
/*		for (int i = 0; i < clips.Length; i++)
		{
			frames[i].anim = GameObject.Find("clip" + clips[i]).GetComponent<Animator>();
			frames[i].animLength = frames[i].anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
		}
		*/
		bool ready = true;
		foreach (UnscramblerSocket socket in sockets)
		{
			if (socket.socketed == null)
			{
				ready = false;
			}
		}
		if (ready)
		{
			watchingSocket = -1;
			GameController.singleton.SetOverlay("None");
			Camera.main.orthographicSize = 0.45f;
			MoveCamera();

			float delay = 0f;
			for (int i = 0; i < sockets.Length; i++)
			{
				//Revoke dragging priveleges.
				sockets[i].GetComponent<BoxCollider2D>().enabled = false;
				sockets[i].socketed.GetComponent<BoxCollider2D>().enabled = false;

				//Set diagnostic sprite
				sockets[i].socketed.diagnostic.sprite = diagnosticSprites[3];

				delay += sockets[i].socketed.animLength;
				Invoke("MoveCamera", delay);
			}

		}
	}
	public void MoveCamera()
	{
		watchingSocket++;
		if (watchingSocket < sockets.Length)
		{
			UnscramblerSocket socket = sockets[watchingSocket];
			//Go to start of clip
			socket.socketed.anim.Play("clip" + socket.socketed.clip, 0, 0f);
			//Center camera on clip
			Camera.main.transform.position = socket.transform.position + new Vector3(-0.04f, 0f, -10f);
		}
		else
		{
			Camera.main.orthographicSize = 2f;
			Camera.main.transform.position = Vector3.back * 10f;
			bool finished = true;
			helpDirections = new List<string>();

			for (int i = 0; i < sockets.Length; i++)
			{

				int diagnosis = (solutions[i] == sockets[i].socketed.clip) ? 0 : ((solutions.Contains<int>(sockets[i].socketed.clip)) ? 1 : 2);
				switch (diagnosis)
				{
					case 0:
						helpDirections.Add("unscrambler_help_original|" + i);
						break;
					case 1:
						helpDirections.Add("unscrambler_help_moved|" + i);
						break;
					case 2:
						helpDirections.Add("unscrambler_help_edited|" + i);
						helpDirections.Add("unscrambler_help_clips");
						break;
				}
				//Set diagnostic sprite
				sockets[i].socketed.diagnostic.sprite = diagnosticSprites[diagnosis];
				if (diagnosis != 0)
				{
					//If socketed frame is incorrect, reinstate dragging privelege
					sockets[i].GetComponent<BoxCollider2D>().enabled = true;
					sockets[i].socketed.GetComponent<BoxCollider2D>().enabled = true;
					finished = false;
				}

			}

			if (finished)
			{
				GameController.CompleteLevel("unscrambler", GameController.GetInt("unscrambler_level"));
				GameController.singleton.LoadScene("ShipMiddle");
			}
//			else if (GameController.GetInt("unscrambler_level") == 0)
			else
			{
				GameController.singleton.SetOverlay("Unscrambler Overlay");
				GameController.Directions(helpDirections.ToArray());
			}
		}

	}
}
