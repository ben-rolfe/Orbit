using UnityEngine;
using System.Collections;
//This class passes in-scene button events to the GameController. In-scene buttons cannot be directly linked to the GameController as the GameController persists across scenes.

public class ButtonHandler : MonoBehaviour {

	public void LoadScene(string sceneName)
	{
		GameController.singleton.LoadScene(sceneName);
	}
	public void LoadTeleporter(int character)
	{
		GameController.singleton.LoadTeleporter(character);
	}
	public void LoadFactory()
	{
		GameController.singleton.LoadFactory();
	}
	public void LoadClassifier()
	{
		GameController.singleton.LoadClassifier();
	}
	public void LoadUnscrambler()
	{
		GameController.singleton.LoadUnscrambler();
	}
	public void LoadVR()
	{
		GameController.singleton.LoadVR();
	}

}
