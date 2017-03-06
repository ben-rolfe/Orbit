using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour {
	string charactor = "adult1";
//	string charactor = "child";
	string shapeMenu = "hair";
	string colorMenu;
	bool muffleVoice = true; // Muffle the voice line when the teleporter is first opened.
	bool isMale = false;
	bool isAdult;
	[SerializeField] GameObject[] panels;
	[SerializeField] Button maleButton;
	[SerializeField] Button femaleButton;
	[SerializeField] InputField nameText;


	public void Setup()
	{
		if (GameController.GetInt("teleporter_level") == 0)
		{
			charactor = "child";
		}
		else
		{
			charactor = "adult" + GameController.GetInt("teleporter_level");
		}
		SetCharactor(charactor);
	}

	public void SetCharactor(string charactor)
	{
		this.charactor = charactor;
		FindObjectOfType<TeleporterAvatar>().gameObject.name = charactor;
		isAdult = charactor != "child";
		nameText.text = GameController.GetString(charactor + "_name");
		muffleVoice = true;
		SetIsMale(GameController.GetBool(charactor + "_isMale"));
	}

	public void SetSubMenu()
	{
		SetSubMenu(shapeMenu);
	}
	public void SetSubMenu(string subMenu)
	{
		foreach (GameObject panel in panels)
		{
			panel.SetActive(false);
		}
		shapeMenu = subMenu;
		colorMenu = "";
		switch (subMenu)
		{
			case "face":
			case "mouth":
			case "nose":
				colorMenu = "skin";
				break;
			case "hair":
			case "feature":
				colorMenu = "hair";
				break;
			case "eyes":
			case "brows":
				colorMenu = "eyes";
				break;
			case "top":
			case "legs":
				colorMenu = "cloth";
				break;

		}
		string panelName = subMenu;
		if ((subMenu == "face" && isAdult) || subMenu == "hair" || subMenu == "top" || subMenu == "legs" || subMenu == "feature" || subMenu == "voice") //Some menu panels are gendered
		{
			panelName = ((isMale) ? "male" : "female") + "_" + panelName;
		}
		if (subMenu == "face" || (subMenu == "hair" && isMale) || subMenu == "top" || subMenu == "legs") //Some menu panels are age dependent
		{
			panelName = ((isAdult) ? "adult" : "child") + "_" + panelName;
		}

		foreach (GameObject panel in panels)
		{
			if (panel.name == panelName || panel.name == "color_" + colorMenu)
			panel.SetActive(true);
		}

		Debug.Log(subMenu + " selected");
	}
	public void SetIsMale(bool isMale)
	{
		this.isMale = isMale;
		GameController.SetBool(charactor + "_isMale", isMale);

		//Set normal color states of gender buttons
		ColorBlock maleColors = maleButton.colors;
		ColorBlock femaleColors = femaleButton.colors;
		maleColors.normalColor = (isMale) ? Color.white : new Color(1f, 1f, 1f, 0.4f);
		femaleColors.normalColor = (isMale) ? new Color(1f, 1f, 1f, 0.4f) : Color.white;
		maleButton.colors = maleColors;
		femaleButton.colors = femaleColors;

		//Adults can't select shoes, and have voice options
		if (isAdult)
		{
			SetShape(isMale ? 2 : 5, "shoes");
			SetSubMenu("voice");
			//Highlight and play current voice
			int voiceNum = GameController.GetInt(charactor + "_voice");
			if (voiceNum > 3)
			{
				voiceNum -= 3;
			}
			if (voiceNum < 1) //Shouldn't happen, but just in case the value wasn't set or has been destroyed
			{
				voiceNum = 1;
			}
			Transform voicePanel;
			if (isMale)
			{
				GameController.SetInt(charactor + "_voice", 3 + voiceNum);
				voicePanel = transform.FindChild("male_voice");
			}
			else
			{
				GameController.SetInt(charactor + "_voice", voiceNum);
				voicePanel = transform.FindChild("female_voice");
			}
			if (voicePanel != null)
			{
				if (voicePanel.childCount >= voiceNum)
				{
					if (muffleVoice)
					{
						muffleVoice = false;
					}
					else
					{
						voicePanel.GetChild(voiceNum - 1).GetComponent<Button>().Select();
						voicePanel.GetChild(voiceNum - 1).GetComponent<AudioSource>().Play();
					}
				}
			}
		}
		else
		{
			SetSubMenu();
		}

		CustomPerson.RedressAll();
	}


	public void SetColor(string hexCode)
	{
		SetColor(hexCode, colorMenu);
	}
	public void SetColor(string hexCode, string colorMenu)
	{
		GameController.SetColor(charactor + "_" + ((colorMenu == "cloth") ? shapeMenu : colorMenu), HexToColor(hexCode));
		CustomPerson.RedressAll();
	}
	public void SetShape(int shapeNumber)
	{
		SetShape(shapeNumber, shapeMenu);
	}
	public void SetShape(int shapeNumber, string shapeMenu)
	{
		GameController.SetInt(charactor + "_" + shapeMenu, shapeNumber);
		Debug.Log("Set " + charactor + "_" + shapeMenu + " = " + shapeNumber);
		CustomPerson.RedressAll();
	}

	public static Color HexToColor(string hexCode)
	{
		return new Color(int.Parse(hexCode.Substring(0, 2), System.Globalization.NumberStyles.HexNumber) / 255f, int.Parse(hexCode.Substring(2, 2), System.Globalization.NumberStyles.HexNumber) / 255f, int.Parse(hexCode.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) / 255f);
	}

	public void SaveCharactor()
	{
		if (nameText.text == "")
		{
			ColorBlock colors = nameText.colors;
			colors.normalColor = Color.red;
			nameText.colors = colors;
		}
		else
		{
			GameController.SetString(charactor + "_name", nameText.text);
			GameController.singleton.LoadScene("ShipBottom");
		}
	}
}
