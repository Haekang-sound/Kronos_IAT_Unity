using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureBox : MonoBehaviour
{
	// Start is called before the first frame update
	bool isDone ;
	bool isBroke;

	public float TPValue;

	SoundManager sm;

	void Start()
	{
		GetComponent<Fracture>().OnDeath += BoxTP;
		sm = SoundManager.Instance;
	}

	void BoxTP()
	{
		if (!isDone)
		{
			EffectManager.Instance.CreateAbsorbFX(transform, TPValue);
			isDone = true;

		}
	}

	public void BoxSound()
	{
		if (!isBroke)
		{
			sm.PlaySFX("Effect_BoxBreak_1_Sound_SE", transform);
			isBroke = true;
		}
    }

}
