using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseONOFF : MonoBehaviour
{
	// Start is called before the first frame update


	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			SetCursorInactive();
		}
	}

	void SetCursorInactive()
	{
		Cursor.visible = !Cursor.visible; // ???²J ?????? ???
		if (Cursor.visible)
		{
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
}
