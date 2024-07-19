using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamControler : MonoBehaviour
{
	CinemachineVirtualCamera PlayerCam;
	CinemachineBrain cinemachineBrain; 
	private void OnEnable()
	{
// 		CinemachineBrain cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
// 		if (cinemachineBrain != null)
// 		{
// 			cinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
// 		}


	}

	void FixedUpdate()
	{

	}
}
