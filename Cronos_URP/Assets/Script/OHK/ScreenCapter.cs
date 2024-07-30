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
		// F12 Ű�� ������ �� ��ũ���� ���
		if (fixedTime > 3f)
		{
			TakeScreenshot();
			fixedTime = 0f;
		}

	}

	public void TakeScreenshot()
	{
		// ��ũ���� ���� �̸� ����
		string screenshotFilename = string.Format("Screenshot_{0}.png", screenshotCount);
		// ��ũ���� ���
		ScreenCapture.CaptureScreenshot(screenshotFilename);
		screenshotCount++;
		Debug.Log("��ũ���� �����: " + screenshotFilename);
	}
}
