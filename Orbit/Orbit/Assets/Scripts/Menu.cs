using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	[SerializeField]
	Text continueButtonText;
	[SerializeField]
	Text versionText;
	[SerializeField]
	Text loadMenuText;
	[SerializeField]
	Button[] loadButtons;
	[SerializeField]
	Button[] gameExistsButtons;
	bool overwriteMode = false;
	int cheatTapCount = 0;


	void Start()
	{
		Setup();
	}

	public void Setup()
	{
		versionText.text = "v" + Application.version;
		overwriteMode = false;
		foreach (Button button in gameExistsButtons)
		{
			button.gameObject.SetActive(GameController.slot != "");
		}
		continueButtonText.text = "Continue\n(" + GameController.GetString("child_name") + ")";
	}

	public void NewGame()
	{
		overwriteMode = false;
		//Loop through the slots looking for an empty one.
		for (int i = 0; i < 12; i++)
		{
			GameController.slot = "game" + i;
			if (!GameController.GetBool("exists"))
			{
				overwriteMode = true;
				break;
			}
		}
		if (overwriteMode)
		{
			StartGame();
		}
		else
		{
			OpenSavedGameMenu(true);
		}
	}
	public void StartGame()
	{
		GameController.singleton.SetOverlay("None");
		FindObjectOfType<ParticleSystem>().Play();
		Invoke((overwriteMode) ? "StartNewGame" : "StartExistingGame", 3f);
	}
	public void StartGame(string slot)
	{
		GameController.slot = slot;
		StartGame();
	}


	public void OpenSavedGameMenu(bool overwrite)
	{
		string currentSlot = GameController.slot;
		overwriteMode = overwrite;
		loadMenuText.text = (overwrite) ? "Choose save slot. THIS WILL OVERWRITE THE GAME IN THAT SLOT!" : "Choose game";
		for (int i = 0; i < 12; i++)
		{
			GameController.slot = "game" + i;
			if (GameController.GetBool("exists"))
			{
				loadButtons[i].enabled = true;
				loadButtons[i].GetComponentInChildren<Text>().text = GameController.GetString("child_name");
			}
			else
			{
				loadButtons[i].enabled = false;
				loadButtons[i].GetComponentInChildren<Text>().text = "[No Save]";
			}
		}
		GameController.slot = currentSlot;
		GameController.singleton.SetOverlay("Load Menu");
	}
	
	public void OpenCheatMenu()
	{
		if (++cheatTapCount > 9)
		{
			GameController.singleton.SetOverlay("Cheat Menu");
			cheatTapCount = 0;
		}
	}

	public void CheatToCheckPoint(int checkpoint)
	{
		GameController.SetInt("Checkpoint", checkpoint);
		GameController.singleton.LoadTeleporter(0);
	}

	void StartExistingGame()
	{
		Debug.Log("Scene: " + GameController.GetString("scene"));
		if (GameController.GetString("scene") == "")
		{
			//If scene hasn't been set, yet, the player hasn't made it through the teleporter, so we send them there.
			GameController.SetString("scene", "Teleporter");
		}
		else if (!GameController.GetString("scene").StartsWith("Ship"))
		{
			//If scene isn't on ship (if it's a minigame, or the main menu) send them to the bottom of the ship instead. Not ideal, but avoids them getting stuck somewhere (esp. main menu).
			GameController.SetString("scene", "ShipBottom");
		}
		GameController.singleton.LoadScene(GameController.GetString("scene"));
	}
	void StartNewGame()
	{
		//Start a new game!
		GameController.SetBool("exists", true);
		GameController.SetInt("Checkpoint", 0);
		GameController.SetColor("walls_paint", Teleporter.HexToColor("8888AA"));
		GameController.SetString("goodat", "exploring space ships");
		for (int i = 0; i < 6; i++)
		{
			string charactor = (i == 0) ? "child" : "adult" + i.ToString();
			GameController.SetString(charactor + "_name", "");
			GameController.SetBool(charactor + "_isMale", false);
			GameController.SetColor(charactor + "_hair", Teleporter.HexToColor("331100"));
			GameController.SetColor(charactor + "_eyes", Teleporter.HexToColor("330000"));
			GameController.SetColor(charactor + "_skin", Teleporter.HexToColor("995533"));
			GameController.SetColor(charactor + "_top", Teleporter.HexToColor("660033"));
			GameController.SetColor(charactor + "_legs", Teleporter.HexToColor("9999FF"));
			GameController.SetInt(charactor + "_voice", 1);
			GameController.SetInt(charactor + "_hair", 1);
			GameController.SetInt(charactor + "_brows", 0);
			GameController.SetInt(charactor + "_eyes", 0);
			GameController.SetInt(charactor + "_nose", 0);
			GameController.SetInt(charactor + "_mouth", 0);
			GameController.SetInt(charactor + "_face", 0);
			GameController.SetInt(charactor + "_top", 3);
			GameController.SetInt(charactor + "_legs", 0);
			GameController.SetInt(charactor + "_shoes", 0);
			GameController.SetInt(charactor + "_feature", 0);
		}
		GameController.singleton.LoadTeleporter(0);
	}


}

