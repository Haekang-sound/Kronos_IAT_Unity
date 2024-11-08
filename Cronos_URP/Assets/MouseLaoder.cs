using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class MouseLaoder : MonoBehaviour
{
	public bool isMousOn;
	public RenderObjects SkillRenderObj;
	// Start is called before the first frame update
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
