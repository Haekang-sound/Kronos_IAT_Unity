using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ȭ�� ĸ�ĸ� ���� Ŭ����
/// (���� ������� ����)
/// 
/// ohk    v1
/// </summary>
public class ScreenCapter : MonoBehaviour
{
	public int screenShotCount = 0;

	private float _fixedTime = 0f;

	void Update()
	{
		_fixedTime += Time.deltaTime;

		// F12 Ű�� ������ �� ��ũ���� ���
		if (_fixedTime > 3f)
		{
			TakeScreenshot();
			_fixedTime = 0f;
		}

	}

	public void TakeScreenshot()
	{
		// ��ũ���� ���� �̸� ����
		string screenshotFilename = string.Format("Screenshot_{0}.png", screenShotCount);
		
		// ��ũ���� ���
		ScreenCapture.CaptureScreenshot(screenshotFilename);
		screenShotCount++;
	}
}
