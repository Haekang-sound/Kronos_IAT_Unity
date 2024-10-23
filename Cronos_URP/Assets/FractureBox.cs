using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureBox : MonoBehaviour
{
	// Start is called before the first frame update
	bool isDone ;

	public float TPValue;

	void Start()
	{
		GetComponent<Fracture>().OnDeath += BoxTP;
	}

	void BoxTP()
	{
		if (!isDone)
		{
			EffectManager.Instance.CreateAbsorbFX(transform, TPValue);
			isDone = true;

		}
	}

}
