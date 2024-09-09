using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerZoom : MonoBehaviour
{
	static CamerZoom instance;
	public static CamerZoom GetInstance() { return instance; }

	[SerializeField] [Range(0f, 10f)] private float defaultDistance = 6f;
	[SerializeField] [Range(0f, 10f)] private float minimumDistance = 1f;
	[SerializeField] [Range(0f, 10f)] private float maximumDistance = 100f;

	[SerializeField] [Range(0f, 10f)] private float smoothing = 4f;
	[SerializeField] [Range(0f, 10f)] private float zoomSensitivity = 1f;

	private CinemachineFramingTransposer framingTransposer;
	private CinemachineInputProvider inputProvider;

	[SerializeField] private float currentTargetDistance;

	private void Awake()
	{
		instance = this;
		framingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
		inputProvider = GetComponent<CinemachineInputProvider>();

		framingTransposer.m_CameraDistance = defaultDistance;
	}

	private void Update()
	{
		//Zoom();
	}

	private void Zoom()
	{
		float zoomValue = inputProvider.GetAxisValue(2) * zoomSensitivity;

		currentTargetDistance = Mathf.Clamp(currentTargetDistance + zoomValue, minimumDistance, maximumDistance);

		float currentDistance = framingTransposer.m_CameraDistance;
		if(currentDistance == currentTargetDistance)
		{
			return;
		}

		float lerpedZoomValue = Mathf.Lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime);

		framingTransposer.m_CameraDistance = lerpedZoomValue;
	}

	public void Zoomer(float zValue, float time)
	{
		//원하는 시간만큼간다
		float currentDistance = framingTransposer.m_CameraDistance;
		float lerpedZoomValue = Mathf.Lerp(currentDistance, zValue, time * Time.deltaTime);
		framingTransposer.m_CameraDistance = lerpedZoomValue; 
	}
	public void SetDefault()
	{
		// 원하는 시간에 돌아온다.
		framingTransposer.m_CameraDistance = defaultDistance;
	}

}
