using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DefaultColliderData
{
	[SerializeField] public float Hieght { get; set; } = 1.8f;
	[SerializeField] public float CenterY { get; set; } = 0.9f;
	[SerializeField] public float Radius { get; set; } = 0.2f;
}
