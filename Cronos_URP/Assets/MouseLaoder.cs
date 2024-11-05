using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MouseLaoder : MonoBehaviour
{
	public bool isMousOn;
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
		
	}

}
