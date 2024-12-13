using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 파괴가능한 박스를 위한 클래스
/// 
/// ohk v1
/// </summary>
public class FractureBox : MonoBehaviour
{
	// Start is called before the first frame update
	private bool _isDone ;
	private bool _isBroke;

	public float TPValue;

	private SoundManager _sm;

	void Start()
	{
		GetComponent<Fracture>().OnDeath += BoxTP;
		_sm = SoundManager.Instance;
	}

	void BoxTP()
	{
		if (!_isDone)
		{
			EffectManager.Instance.CreateAbsorbFX(transform, TPValue);
			_isDone = true;

		}
	}

	public void BoxSound()
	{
		if (!_isBroke)
		{
			_sm.PlaySFX("Effect_BoxBreak_1_Sound_SE", transform);
			_isBroke = true;
		}
    }

}
