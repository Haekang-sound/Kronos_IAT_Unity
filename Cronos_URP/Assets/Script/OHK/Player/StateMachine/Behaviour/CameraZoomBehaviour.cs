using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카메라 줌을 
/// 애니메이션 단위로 관리하기 위한 클래스
/// 
/// ohk    v1
/// </summary>
public class CameraZoomBehaviour : StateMachineBehaviour
{
	public float time = 0f;
	[SerializeField] private AnimationCurve curve;

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		float value = curve.Evaluate(stateInfo.normalizedTime);

		if (time > 0f)
		{
			CameraZoom.GetInstance().Zoomer(value, time);
		}
		else
		{
			CameraZoom.GetInstance().ZoomerCurve(value);
		}
	}
}
