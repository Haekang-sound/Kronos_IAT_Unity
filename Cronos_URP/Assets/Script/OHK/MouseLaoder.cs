using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// 마우스커서 표시를 위한 클래스
/// 
/// ohk    v1
/// </summary>
public class MouseLaoder : MonoBehaviour
{
	public bool isMousOn;
	public RenderObjects SkillRenderObj;
	
	void Start()
    {
		if (isMousOn)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		if(SkillRenderObj != null)	SkillRenderObj.SetActive(false);

	}

}
