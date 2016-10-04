using UnityEngine;
using System.Collections;

public class RobotAppendage: MonoBehaviour {
	public string part = "";
	int _variant = 1;
	[SerializeField] Sprite[] variants;

	public int variant
	{
		get
		{
			return _variant;
		}
		set
		{
			_variant = Mathf.Clamp(value, 0, variants.Length - 1);
			GetComponent<SpriteRenderer>().sprite = variants[_variant];
		}
	}

}
