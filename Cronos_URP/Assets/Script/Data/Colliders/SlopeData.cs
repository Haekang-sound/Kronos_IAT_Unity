using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SlopeData
{
	[SerializeField][field : Range(0f, 1f)] public float StepHeightPercentage { get; set; } = 0.25f;

}
 