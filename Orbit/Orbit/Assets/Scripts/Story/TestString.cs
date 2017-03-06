using UnityEngine;
using System.Collections;

public class TestString : MonoBehaviour {
	enum compareOptions { EqualTo, NotEqualTo, StartsWith, EndsWith }

	[SerializeField]
	string removeIf;
	[SerializeField]
	compareOptions removeCompare;
	[SerializeField]
	string removeValue;
/*
	[SerializeField]
	string unlessIf;
	[SerializeField]
	compareOptions unlessCompare;
	[SerializeField]
	string unlessValue;
*/
	public void PrepareTest()
	{
		if (!gameObject.activeInHierarchy)
		{
			gameObject.SetActive(true);
		}
	}

	public void DoTest()
	{
		bool remove = false;
		switch (removeCompare)
		{
			case compareOptions.EqualTo:
				remove = (GameController.GetString(removeIf) == removeValue);
				break;
			case compareOptions.NotEqualTo:
				remove = (GameController.GetString(removeIf) != removeValue);
				break;
			case compareOptions.StartsWith:
				remove = (GameController.GetString(removeIf).StartsWith(removeValue));
				break;
			case compareOptions.EndsWith:
				remove = (GameController.GetString(removeIf).EndsWith(removeValue));
				break;
		}
/*		if (remove && unlessIf != "") //Has an unless clause. Check it.
		{
			switch (unlessCompare)
			{
				case compareOptions.EqualTo:
					if (GameController.GetInt(unlessIf) == unlessValue)
					{
						remove = false;
					}
					break;
				case compareOptions.NotEqualTo:
					if (GameController.GetInt(unlessIf) != unlessValue)
					{
						remove = false;
					}
					break;
				case compareOptions.LessThan:
					if (GameController.GetInt(unlessIf) < unlessValue)
					{
						remove = false;
					}
					break;
				case compareOptions.LessThanOrEqualTo:
					if (GameController.GetInt(unlessIf) <= unlessValue)
					{
						remove = false;
					}
					break;
				case compareOptions.GreaterThan:
					if (GameController.GetInt(unlessIf) > unlessValue)
					{
						remove = false;
					}
					break;
				case compareOptions.GreaterThanOrEqualTo:
					if (GameController.GetInt(unlessIf) >= unlessValue)
					{
						remove = false;
					}
					break;
			}
		}
*/
		if (remove)
		{
			gameObject.SetActive(false);
		}
	}
}
