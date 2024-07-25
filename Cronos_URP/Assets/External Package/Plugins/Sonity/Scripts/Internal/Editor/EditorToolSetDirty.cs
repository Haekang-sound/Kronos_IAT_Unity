// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEditor;

namespace Sonity.Internal {

    public static class ToolsSetDirty {

        [MenuItem("Tools/Sonity 🔊/Set Selected Assets as Dirty (Force Reserialize for Resave)")]
        private static void SetDirty() {
            foreach (UnityEngine.Object objects in Selection.objects) {
                EditorUtility.SetDirty(objects);
            }
        }
    }
}
#endif