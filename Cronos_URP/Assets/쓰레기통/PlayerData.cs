

using System.Numerics;
using UnityEngine;

public class PlayerData
{
	// 저장할 데이터를 하나씩 늘이면 될듯
	public string saveScene { get; set; }
	public float CP { get; set; }
	public float TP { get; set; }
	public UnityEngine.Vector3 RespawnPos { get; set; } = new UnityEngine.Vector3(0f,0f,0f);

}

