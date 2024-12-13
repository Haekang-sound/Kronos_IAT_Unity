using UnityEditor;
using UnityEngine;

public class MapScaler : EditorWindow
{
	[MenuItem("Tools/MapScaler")]
	public static void ShowWindow()
	{
		GetWindow<MapScaler>("Scale Lights");
	}

	private void OnGUI()
	{
		if (GUILayout.Button("Scale All Light Multiply 0.1"))
		{
			ScaleAllMinimum();
		}

		if (GUILayout.Button("Scale All Light Multiply 10.0"))
		{
			ScaleAllMaximum();
		}

		if (GUILayout.Button("Scale All Map Multiply 0.1"))
		{
			ScaleMapMinimum();
		}

		if (GUILayout.Button("Scale All Map Multiply 10.0"))
		{
			ScaleMapMaximum();
		}
	}

	private void ScaleMapMinimum()
	{
		GameObject map = GameObject.Find("MapName");

		Undo.RecordObject(map, "Scale Map Properties"); // Undo 기능을 위해 기록합니다.

		map.transform.localScale *= 0.1f;

		EditorUtility.SetDirty(map); // 변경 사항을 마킹합니다.

		Debug.Log("Scaled all map' local Scale by 0.1.");
	}

	private void ScaleMapMaximum()
	{
		GameObject map = GameObject.Find("MapName");

		Undo.RecordObject(map, "Scale Map Properties"); // Undo 기능을 위해 기록합니다.

		map.transform.localScale *= 10f;

		EditorUtility.SetDirty(map); // 변경 사항을 마킹합니다.

		Debug.Log("Scaled all map' local Scale by 10.");
	}


	private void ScaleAllMinimum()
	{
		// 현재 씬의 모든 Light 컴포넌트를 찾습니다.
		Light[] lights = FindObjectsOfType<Light>();

		foreach (Light light in lights)
		{
			if (light.type == LightType.Rectangle || light.type == LightType.Disc)
			{
				Undo.RecordObject(light, "Scale Light Properties"); // Undo 기능을 위해 기록합니다.

				light.areaSize *= 0.1f; // range 값을 0.1로 곱합니다.

				EditorUtility.SetDirty(light); // 변경 사항을 마킹합니다.
			}

			if (light.type == LightType.Point)
			{
				Undo.RecordObject(light, "Scale Light Properties"); // Undo 기능을 위해 기록합니다.

				light.range *= 0.1f;

				EditorUtility.SetDirty(light); // 변경 사항을 마킹합니다.
			}

			if (light.type == LightType.Spot)
			{
				Undo.RecordObject(light, "Scale Light Properties"); // Undo 기능을 위해 기록합니다.

				light.range *= 0.1f;

				EditorUtility.SetDirty(light); // 변경 사항을 마킹합니다.
			}
		}

		Debug.Log("Scaled all lights' width and height by 0.1.");
	}

	private void ScaleAllMaximum()
	{
		// 현재 씬의 모든 Light 컴포넌트를 찾습니다.
		Light[] lights = FindObjectsOfType<Light>();

		foreach (Light light in lights)
		{
			if (light.type == LightType.Rectangle || light.type == LightType.Disc)
			{
				Undo.RecordObject(light, "Scale Light Properties"); // Undo 기능을 위해 기록합니다.

				light.areaSize *= 10f; // range 값을 0.1로 곱합니다.

				EditorUtility.SetDirty(light); // 변경 사항을 마킹합니다.
			}

			if (light.type == LightType.Point)
			{
				Undo.RecordObject(light, "Scale Light Properties"); // Undo 기능을 위해 기록합니다.

				light.range *= 10f;

				EditorUtility.SetDirty(light); // 변경 사항을 마킹합니다.
			}

			if (light.type == LightType.Spot)
			{
				Undo.RecordObject(light, "Scale Light Properties"); // Undo 기능을 위해 기록합니다.

				light.range *= 10f;

				EditorUtility.SetDirty(light); // 변경 사항을 마킹합니다.
			}
		}

		Debug.Log("Scaled all lights' width and height by 10.");
	}

}