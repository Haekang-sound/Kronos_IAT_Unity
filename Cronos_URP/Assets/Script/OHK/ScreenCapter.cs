using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 화면 캡쳐를 위한 클래스
/// (현재 사용하지 않음)
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

		// F12 키를 눌렀을 때 스크린샷 찍기
		if (_fixedTime > 3f)
		{
			TakeScreenshot();
			_fixedTime = 0f;
		}

	}

	public void TakeScreenshot()
	{
		// 스크린샷 파일 이름 설정
		string screenshotFilename = string.Format("Screenshot_{0}.png", screenShotCount);
		
		// 스크린샷 찍기
		ScreenCapture.CaptureScreenshot(screenshotFilename);
		screenShotCount++;
	}
}
