using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCapter : MonoBehaviour
{
	public int screenshotCount = 0;

	float fixedTime = 0f;

	void Update()
	{
		fixedTime += Time.deltaTime;
		// F12 Å°¸¦ ´­·¶À» ¶§ ½ºÅ©¸°¼¦ Âï±â
		if (fixedTime > 3f)
		{
			TakeScreenshot();
			fixedTime = 0f;
		}

	}

	public void TakeScreenshot()
	{
		// ½ºÅ©¸°¼¦ ÆÄÀÏ ÀÌ¸§ ¼³Á¤
		string screenshotFilename = string.Format("Screenshot_{0}.png", screenshotCount);
		// ½ºÅ©¸°¼¦ Âï±â
		ScreenCapture.CaptureScreenshot(screenshotFilename);
		screenshotCount++;
		Debug.Log("½ºÅ©¸°¼¦ ÀúÀåµÊ: " + screenshotFilename);
	}
}
