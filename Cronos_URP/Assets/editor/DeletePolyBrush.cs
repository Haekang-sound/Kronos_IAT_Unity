using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEditor.Formats.Fbx.Exporter;
using System.Runtime.Remoting;
using System.IO;
using Autodesk.Fbx;
using System.Diagnostics;
using System;
using UnityEngine.Polybrush;
using Polybrush;

public class DeletePoltBrush : MonoBehaviour
{


    [MenuItem("Tools/Delete Poly Brush")]
    private static void DeletePolyBrush()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            Component[] components = obj.GetComponents<Component>();
            foreach (Component comp in components)
            {
                // Polybrush 관련 네임스페이스를 가진 컴포넌트만 삭제
                if (comp != null && comp.GetType().Namespace != null && comp.GetType().Namespace.Contains("Polybrush"))
                {
                    Undo.DestroyObjectImmediate(comp);
                }
            }
        }


        UnityEngine.Debug.Log("Delete Completly");
    }
}
