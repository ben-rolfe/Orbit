using UnityEngine;
using System.Collections;

public class TestInt : MonoBehaviour {
	enum compareOptions { EqualTo, NotEqualTo, LessThan, LessThanOrEqualTo, GreaterThan, GreaterThanOrEqualTo, BitTrue, BitFalse }

	public string removeIf;
	[SerializeField]
	compareOptions removeCompare;
	[SerializeField]
	int removeValue;

	public string unlessIf;
	[SerializeField]
	compareOptions unlessCompare;
	[SerializeField]
	int unlessValue;

	public void PrepareTest()
	{
		//Test to make sure this isn't called on the object after it's destroyed (weird, but it was happening)
		if (this != null)
		{
			if (!gameObject.activeInHierarchy)
			{
				gameObject.SetActive(true);
			}
		}
	}

	public void DoTest()
	{
		bool remove = false;
		switch (removeCompare)
		{
			case compareOptions.EqualTo:
				remove = (GameController.GetInt(removeIf) == removeValue);
				break;
			case compareOptions.NotEqualTo:
				remove = (GameController.GetInt(removeIf) != removeValue);
				break;
			case compareOptions.LessThan:
				remove = (GameController.GetInt(removeIf) < removeValue);
				break;
			case compareOptions.LessThanOrEqualTo:
				remove = (GameController.GetInt(removeIf) <= removeValue);
				break;
			case compareOptions.GreaterThan:
				remove = (GameController.GetInt(removeIf) > removeValue);
				break;
			case compareOptions.GreaterThanOrEqualTo:
				remove = (GameController.GetInt(removeIf) >= removeValue);
				break;
			case compareOptions.BitTrue:
				remove = ((GameController.GetInt(removeIf) & (1 << removeValue)) != 0);
				break;
			case compareOptions.BitFalse:
				remove = ((GameController.GetInt(removeIf) & (1 << removeValue)) == 0);
				break;
		}
		if (remove && unlessIf != "") //Has an unless clause. Check it.
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
		if (remove)
		{
			//The following test seems crazy, but we were getting errors when the TestInt component was destroyed, but this was still somehow being called on it.
			if (this != null)
			{
				gameObject.SetActive(false);
			}
		}
	}

}
