using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SorterGameController : MonoBehaviour {

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
	

		switch(GameController.GetInt("classifier_level"))
		{
			case 0:
				Invoke("Complete", 65f); //track0
				GameController.Directions(new string[] { "sorter_help_flagged", "~5000", "sorter_help_hit", "~8000", "sorter_help_unflagged" });
				break;
			case 1:
			case 5:
				Invoke("Complete", 74f); //track3
				break;
			case 2:
			case 6:
				Invoke("Complete", 93f); //track2
				break;
			case 3:
			case 7:
				Invoke("Complete", 103f); //track1
				break;
			case 4:
			case 8:
				Invoke("Complete", 60f); //track4
				break;
		}
	}

	void Reclassify()
	{
		GameController.Directions(new string[] { "sorter_help_reclassify", "@Classifier" });
	}

	void Complete()
	{
		Debug.Log("Completed Sorter");
		GameController.CompleteLevel("classifier", GameController.GetInt("classifier_level"));
		GameController.singleton.LoadScene("ShipMiddle", "classifier");
	}

}
