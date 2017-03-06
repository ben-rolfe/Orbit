using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VRLevelLoader : MonoBehaviour {
	void Start () {
		SceneManager.LoadScene("VR" + GameController.GetInt("vr_level"), LoadSceneMode.Additive);
		GameController.singleton.SetOverlay("VR Overlay");
	}	
}
