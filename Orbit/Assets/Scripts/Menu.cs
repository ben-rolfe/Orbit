using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	ParticleSystem ps;
	void Start()
	{
		ps = FindObjectOfType<ParticleSystem>();
		ps.Stop();
	}

	public void StartGame(string slot)
	{
		GameController.slot = slot;
		GameController.singleton.SetOverlay("None");
		ps.Play();
		Invoke("StartGameGo", 3f);
	}
	void StartGameGo()
	{
		if (GameController.GetString("scene") != "")
		{
			SceneManager.LoadScene(GameController.GetString("scene"), LoadSceneMode.Single);
		}
		else
		{
			//Start a new game!
			GameController.SetInt("Checkpoint", 0);
			for (int i = 0; i < 6; i++)
			{
				string charactor = (i == 0) ? "child" : "adult" + i.ToString();
				GameController.SetBool(charactor + "_isMale", false);
				GameController.SetColor(charactor + "_hair", Teleporter.HexToColor("331100"));
				GameController.SetColor(charactor + "_eyes", Teleporter.HexToColor("330000"));
				GameController.SetColor(charactor + "_skin", Teleporter.HexToColor("995533"));
				GameController.SetColor(charactor + "_top", Teleporter.HexToColor("660033"));
				GameController.SetColor(charactor + "_legs", Teleporter.HexToColor("9999FF"));
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

	public void ContinueGame()
	{
		if (GameController.slot == "")
		{
			GameController.slot = "game1";
		}
		StartGame(GameController.slot);
	}
	public void NewGame()
	{
		if (GameController.slot == "")
		{
			GameController.slot = "game1";
		}
		GameController.SetString("scene", "");
		StartGame(GameController.slot);
	}


}

