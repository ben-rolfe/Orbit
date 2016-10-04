using UnityEngine;
using System.Collections;

public class FactoryConveyor : MonoBehaviour {
	Animator anim;
	SurfaceEffector2D eff;
	[SerializeField] float baseSpeed = 1f;
	float _speed = 0f;
	[SerializeField] bool right = true;

	void Start()
	{
		baseSpeed = (new float[12] { 0.7f, 0.7f, 0.7f, 0.8f, 0.9f, 1f, 1.25f, 1.5f, 1f, 1f, 1f, 1f })[PlayerPrefs.GetInt("factory_level")];
		anim = GetComponent<Animator>();
		eff = GetComponent<SurfaceEffector2D>();
		speed = baseSpeed;
		//TODO: Slow when draggable part on final conveyor.
	}

	public float speed
	{
		get
		{
			return _speed;
		}
		set
		{
			anim.speed = value * 1.35f;
			eff.speed = (right) ? value : -value;
		}
	}

	public static void SetSpeed(float value)
	{
		foreach(FactoryConveyor conveyor in FindObjectsOfType<FactoryConveyor>())
		{
			conveyor.speed = value;
		}
	}

}
