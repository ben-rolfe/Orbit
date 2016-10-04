using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SorterGameController : MonoBehaviour {

	void Start () {
		SceneManager.LoadScene("sorter_level_" + GameController.GetInt("classifier_level"), LoadSceneMode.Additive);
		GameController.singleton.SetOverlay("Sorter Overlay");
	}
	
}
