// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

using UnityEngine;

namespace ExampleSonity {

    [AddComponentMenu("")]
    public class ExampleHelperGuiText : MonoBehaviour {

        [Header("Use /n for newline")]
        public string textString = "";
        private int fontsize = 50;
        private GUIStyle guiStyle;
        public Color textColor = Color.white;

        private void Start() {
            guiStyle = new GUIStyle();
            guiStyle.fontSize = fontsize;
            guiStyle.alignment = TextAnchor.LowerCenter;
            guiStyle.normal.textColor = textColor;
            // So newline works
            textString = textString.Replace("/n", "\n");
        }

        void OnGUI() {
            GUI.Label(new Rect(Screen.width / 2, Screen.height, 0, 0), textString, guiStyle);
        }
    }
}