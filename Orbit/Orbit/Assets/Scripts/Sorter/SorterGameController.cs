using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SorterGameController : MonoBehaviour {

	[SerializeField]
	SpriteRenderer faceRenderer;
	[SerializeField]
	Sprite[] faces;
	int _score = 4;
	bool paused = false;

	void Start () {
		SceneManager.LoadScene("sorter_level_" + GameController.GetInt("classifier_level"), LoadSceneMode.Additive);
		GameController.singleton.SetOverlay("Sorter Overlay");

		//Test classifier answers. If any incorrect, call a reclassify after 30s.
		for (int i=0; i < 8; i++)
		{
			if (GameController.GetBool("classifier_answer_" + i) != GameController.GetBool("classifier_response_" + i))
			{
				Invoke("Reclassify", 30f);
				break;
			}
		}

		Debug.Log("LEVEL " + GameController.GetInt("classifier_level"));
		switch(GameController.GetInt("classifier_level"))
		{
			case 0:
				Invoke("Pause", 1f);
				Invoke("UnPause", 6.5f);
				Invoke("Pause", 13f);
				Invoke("UnPause", 14f);
				Invoke("Pause", 19.75f);
				Invoke("UnPause", 26.25f);
				Invoke("Complete", 73f); //track4, plus time for pauses, above
				GameController.Directions(new string[] { "sorter_help_flagged", "~5000", "sorter_help_hit", "~6000", "sorter_help_unflagged" });
				break;
			case 1:
			case 4:
			case 7:
				Invoke("Complete", 93f); //track2
				break;
			case 3:
			case 6:
			case 8:
				Invoke("Complete", 74f); //track3
				break;
			case 2:
			case 5:
				Invoke("Complete", 60f); //track4
				break;
		}
	}

	void Pause()
	{
		paused = true;
		FindObjectOfType<SorterScroll>().GetComponent<AudioSource>().Pause();
	}
	void UnPause()
	{
		paused = false;
		FindObjectOfType<SorterScroll>().GetComponent<AudioSource>().UnPause();
	}

	void Reclassify()
	{
		Pause();
		GameController.Directions(new string[] { "sorter_help_reclassify", "@Classifier" });
	}

	void Complete()
	{
		if (!paused)
		{
			GameController.CompleteLevel("classifier", GameController.GetInt("classifier_level"));
			GameController.singleton.LoadScene("ShipMiddle", "Sorter");
		}
	}

	public int score
	{
		get
		{
			return _score;
		}
		set
		{
			_score = Mathf.Clamp(value, 0, faces.Length - 1);
			if (_score == 0)
			{
				if (!paused)
				{
					Pause();
					GameController.Directions(new string[] { "sorter_help_miss", "@Sorter" });
				}
			}
			else
			{
				faceRenderer.sprite = faces[_score];
			}
		}
	}

}
