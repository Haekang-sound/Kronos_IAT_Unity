// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Sonity.Internal {

    public abstract class SoundManagerEditorBase : Editor {

        public SoundManagerBase mTarget;
        public SoundManagerBase[] mTargets;

        public GUIStyle guiStyleBoldCenter = new GUIStyle();

        public GUIStyle[] statisticsStyles = new GUIStyle[5];
        public string[] statisticsStrings = new string[5];

        public Color defaultGuiStyleTextColor;
        public Color defaultGuiColor;

        public SerializedProperty internals;
        public SerializedProperty settings;
        public SerializedProperty statistics;

        public SerializedProperty debugSoundEventsInSceneViewEnabled;
        public SerializedProperty debugSoundEventsInGameViewEnabled;
        public SerializedProperty debugSoundEventsFontSize;
        public SerializedProperty debugSoundEventsVolumeToAlpha;
        public SerializedProperty debugSoundEventsLifetimeToAlpha;
        public SerializedProperty debugSoundEventsLifetimeColorLength;
        public SerializedProperty debugSoundEventsColorStart;
        public SerializedProperty debugSoundEventsColorEnd;
        public SerializedProperty debugSoundEventsColorOutline;

        public SerializedProperty settingExpand;
        public SerializedProperty dontDestoyOnLoad;
        public SerializedProperty debugWarnings;
        public SerializedProperty debugInPlayMode;
        public SerializedProperty guiWarnings;
        public SerializedProperty disablePlayingSounds;

        public SerializedProperty soundTimeScale;
        public SerializedProperty globalPause;
        public SerializedProperty globalVolumeRatio;
        public SerializedProperty globalVolumeDecibel;
        public SerializedProperty globalSoundTag;
        public SerializedProperty distanceScale;
        public SerializedProperty overrideListenerDistance;
        public SerializedProperty overrideListenerDistanceAmount;
        public SerializedProperty speedOfSoundEnabled;
        public SerializedProperty speedOfSoundScale;

        public SerializedProperty voicePreload;
        public SerializedProperty voiceLimit;
        public SerializedProperty voiceEffectLimit;
        public SerializedProperty voiceStopTime;

        public SerializedProperty statisticsGlobalExpand;
        public SerializedProperty statisticsInstancesExpand;
        public SerializedProperty statisticsSorting;

        public SerializedProperty statisticsShowActive;
        public SerializedProperty statisticsShowDisabled;
        public SerializedProperty statisticsShowVoices;
        public SerializedProperty statisticsShowPlays;
        public SerializedProperty statisticsShowVolume;

        private void OnEnable() {
            for (int i = 0; i < statisticsStyles.Length; i++) {
                if (statisticsStyles[i] == null) {
                    statisticsStyles[i] = new GUIStyle();
                }
            }
            FindProperties();
        }

        private void FindProperties() {

            internals = serializedObject.FindProperty(nameof(SoundManagerBase.Internals).ToLowerInvariant());
            settings = internals.FindPropertyRelative(nameof(SoundManagerInternals.settings));
            statistics = internals.FindPropertyRelative(nameof(SoundManagerInternals.statistics));

            debugSoundEventsInSceneViewEnabled = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.debugSoundEventsInSceneViewEnabled));
            debugSoundEventsInGameViewEnabled = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.debugSoundEventsInGameViewEnabled));
            debugSoundEventsFontSize = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.debugSoundEventsFontSize));
            debugSoundEventsVolumeToAlpha = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.debugSoundEventsVolumeToAlpha));
            debugSoundEventsLifetimeToAlpha = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.debugSoundEventsLifetimeToAlpha));
            debugSoundEventsLifetimeColorLength = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.debugSoundEventsLifetimeColorLength));
            debugSoundEventsColorStart = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.debugSoundEventsColorStart));
            debugSoundEventsColorEnd = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.debugSoundEventsColorEnd));
            debugSoundEventsColorOutline = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.debugSoundEventsColorOutline));

            settingExpand = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.settingExpand));
            dontDestoyOnLoad = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.dontDestroyOnLoad));
            debugWarnings = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.debugWarnings));
            debugInPlayMode = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.debugInPlayMode));
            guiWarnings = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.guiWarnings));
            disablePlayingSounds = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.disablePlayingSounds));

            soundTimeScale = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.soundTimeScale));
            globalPause = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.globalPause));
            globalVolumeRatio = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.globalVolumeRatio));
            globalVolumeDecibel = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.globalVolumeDecibel));
            globalSoundTag = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.globalSoundTag));
            distanceScale = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.distanceScale));
            overrideListenerDistance = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.overrideListenerDistance));
            overrideListenerDistanceAmount = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.overrideListenerDistanceAmount));
            speedOfSoundEnabled = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.speedOfSoundEnabled));
            speedOfSoundScale = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.speedOfSoundScale));

            voicePreload = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.voicePreload));
            voiceLimit = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.voiceLimit));
            voiceEffectLimit = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.voiceEffectLimit));
            voiceStopTime = settings.FindPropertyRelative(nameof(SoundManagerInternalsSettings.voiceStopTime));

            statisticsGlobalExpand = statistics.FindPropertyRelative(nameof(SoundManagerInternalsStatistics.statisticsGlobalExpand));
            statisticsInstancesExpand = statistics.FindPropertyRelative(nameof(SoundManagerInternalsStatistics.statisticsInstancesExpand));
            statisticsSorting = statistics.FindPropertyRelative(nameof(SoundManagerInternalsStatistics.statisticsSorting));
            statisticsShowActive = statistics.FindPropertyRelative(nameof(SoundManagerInternalsStatistics.statisticsShowActive));
            statisticsShowDisabled = statistics.FindPropertyRelative(nameof(SoundManagerInternalsStatistics.statisticsShowDisabled));
            statisticsShowVoices = statistics.FindPropertyRelative(nameof(SoundManagerInternalsStatistics.statisticsShowVoices));
            statisticsShowPlays = statistics.FindPropertyRelative(nameof(SoundManagerInternalsStatistics.statisticsShowPlays));
            statisticsShowVolume = statistics.FindPropertyRelative(nameof(SoundManagerInternalsStatistics.statisticsShowVolume));
        }

        private void BeginChange() {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
        }

        private void EndChange() {
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void StartBackgroundColor(Color color) {
            GUI.color = color;
            EditorGUILayout.BeginVertical("Button");
            GUI.color = defaultGuiColor;
        }

        private void StopBackgroundColor() {
            EditorGUILayout.EndVertical();
        }

        public override void OnInspectorGUI() {

            mTarget = (SoundManagerBase)target;

            mTargets = new SoundManagerBase[targets.Length];
            for (int i = 0; i < targets.Length; i++) {
                mTargets[i] = (SoundManagerBase)targets[i];
            }

            defaultGuiColor = GUI.color;
            if (EditorGUIUtility.isProSkin) {
                defaultGuiStyleTextColor = EditorColorProSkin.GetDarkSkinTextColor();
            } else {
                defaultGuiStyleTextColor = new GUIStyle().normal.textColor;
            }

            guiStyleBoldCenter.fontStyle = FontStyle.Bold;
            guiStyleBoldCenter.alignment = TextAnchor.MiddleCenter;
            if (EditorGUIUtility.isProSkin) {
                guiStyleBoldCenter.normal.textColor = EditorColorProSkin.GetDarkSkinTextColor();
            }

            EditorGUI.indentLevel = 0;

            EditorGuiFunction.DrawObjectNameBox((UnityEngine.Object)mTarget, NameOf.SoundManager, EditorTextSoundManager.soundManagerTooltip, false);
            EditorTrial.InfoText();
            EditorGUILayout.Separator();

            DrawSettings();

            EditorGUILayout.Separator();
            DrawStatistics();

            // Transparent background so the offset will be right
            StartBackgroundColor(new Color(0f, 0f, 0f, 0f));
            EditorGUILayout.BeginHorizontal();
            // For offsetting the buttons to the right
            EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
            BeginChange();
            if (GUILayout.Button(new GUIContent(EditorTextSoundManager.resetAllLabel, EditorTextSoundManager.resetAllTooltip))) {
                for (int i = 0; i < mTargets.Length; i++) {
                    Undo.RecordObject(mTargets[i], "Reset All");
                    mTargets[i].Internals.settings = new SoundManagerInternalsSettings();
                    mTargets[i].Internals.statistics = new SoundManagerInternalsStatistics();
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
            EndChange();
            EditorGUILayout.EndHorizontal();
            StopBackgroundColor();
        }

        private void DrawSettings() {
            StartBackgroundColor(EditorColor.GetSettings(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            EditorGUI.indentLevel = 1;
            BeginChange();
            EditorGuiFunction.DrawFoldout(settingExpand, "Settings");
            EndChange();

            if (settingExpand.boolValue) {

                // Free Trial Button
                if (EditorTrial.isTrial) {
                    // Enable Sound in Builds
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.Toggle(new GUIContent(EditorTextSoundManager.enableSoundInBuildsLabel, EditorTextSoundManager.enableSoundInBuildsTooltip), false);
                    EditorGUI.EndDisabledGroup();
                }

                // Disable Playing Sounds
                BeginChange();
                EditorGUILayout.PropertyField(disablePlayingSounds, new GUIContent(EditorTextSoundManager.disablePlayingSoundsLabel, EditorTextSoundManager.disablePlayingSoundsTooltip));
                EndChange();

                if (ShouldDebug.GuiWarnings()) {
                    if (disablePlayingSounds.boolValue) {
                        EditorGUILayout.Separator();
                        EditorGUILayout.HelpBox(EditorTextSoundManager.disablePlayingSoundsWarning, MessageType.Warning);
                        EditorGUILayout.Separator();
                    }
                }

                // Global Pause
                BeginChange();
                bool tempGlobalPauseValue = EditorGUILayout.Toggle(new GUIContent(EditorTextSoundManager.globalPauseLabel, EditorTextSoundManager.globalPauseTooltip), globalPause.boolValue);
                if (globalPause.boolValue != tempGlobalPauseValue) {
                    globalPause.boolValue = tempGlobalPauseValue;
                    mTarget.Internals.InternalSetGlobalPauseUnpause(tempGlobalPauseValue);
                }
                EndChange();

                // Global Volume
                BeginChange();
                EditorGUILayout.Slider(globalVolumeDecibel, VolumeScale.lowestVolumeDecibel, 0f, new GUIContent(EditorTextSoundManager.globalVolumeLabel, EditorTextSoundManager.globalVolumeTooltip));
                if (globalVolumeDecibel.floatValue <= VolumeScale.lowestVolumeDecibel) {
                    globalVolumeDecibel.floatValue = Mathf.NegativeInfinity;
                }
                if (globalVolumeRatio.floatValue != VolumeScale.ConvertDecibelToRatio(globalVolumeDecibel.floatValue)) {
                    globalVolumeRatio.floatValue = VolumeScale.ConvertDecibelToRatio(globalVolumeDecibel.floatValue);
                }
                if (Application.isPlaying) {
                    if (AudioListener.volume != globalVolumeRatio.floatValue) {
                        AudioListener.volume = globalVolumeRatio.floatValue;
                    }
                }
                EndChange();

                // Sound Time Scale
                BeginChange();
                EditorGUILayout.PropertyField(soundTimeScale, new GUIContent(EditorTextSoundManager.soundTimeScaleLabel, EditorTextSoundManager.soundTimeScaleTooltip));
                EndChange();

                // Global SoundTag
                BeginChange();
                EditorGUILayout.PropertyField(globalSoundTag, new GUIContent(EditorTextSoundManager.globalSoundTagLabel, EditorTextSoundManager.globalSoundTagTooltip));
                EndChange();

                // Distance Scale
                BeginChange();
                float distanceScaleValue = distanceScale.floatValue;
                distanceScaleValue = EditorGUILayout.FloatField(new GUIContent(EditorTextSoundManager.distanceScaleLabel, EditorTextSoundManager.distanceScaleTooltip), distanceScaleValue);
                if (distanceScaleValue > 0f) {
                    distanceScale.floatValue = distanceScaleValue;
                }
                EndChange();

                // Audio Listener Distance Use
                BeginChange();
                EditorGUILayout.PropertyField(overrideListenerDistance, new GUIContent(EditorTextSoundManager.overrideListenerDistanceLabel, EditorTextSoundManager.overrideListenerDistanceTooltip));
                EndChange();

                if (overrideListenerDistance.boolValue) {
                    EditorGUI.indentLevel++;
                    BeginChange();
                    overrideListenerDistanceAmount.floatValue = EditorGUILayout.Slider(new GUIContent(EditorTextSoundManager.overrideListenerDistanceAmountLabel, EditorTextSoundManager.overrideListenerDistanceAmountTooltip), overrideListenerDistanceAmount.floatValue, 0f, 100f);
                    EndChange();
                    EditorGUI.indentLevel--;
                }

                // Speed of Sound
                BeginChange();
                EditorGUILayout.PropertyField(speedOfSoundEnabled, new GUIContent(EditorTextSoundManager.speedOfSoundEnabledLabel, EditorTextSoundManager.speedOfSoundEnabledTooltip));
                EndChange();

                if (speedOfSoundEnabled.boolValue) {
                    EditorGUI.indentLevel++;
                    BeginChange();
                    EditorGUILayout.PropertyField(speedOfSoundScale, new GUIContent(EditorTextSoundManager.speedOfSoundScaleLabel, EditorTextSoundManager.speedOfSoundScaleTooltip));
                    if (speedOfSoundScale.floatValue <= 0f) {
                        if (speedOfSoundScale.floatValue < 0f) {
                            speedOfSoundScale.floatValue = 0f;
                        }
                        if (ShouldDebug.GuiWarnings()) {
                            EditorGUILayout.Separator();
                            EditorGUILayout.HelpBox(EditorTextSoundManager.speedOfSoundScaleWarning, MessageType.Warning);
                            EditorGUILayout.Separator();
                        }
                    }
                    EndChange();
                    EditorGUI.indentLevel--;
                }

                // DontDestroyOnLoad
                BeginChange();
                EditorGUILayout.PropertyField(dontDestoyOnLoad, new GUIContent(EditorTextSoundManager.dontDestoyOnLoadLabel, EditorTextSoundManager.dontDestoyOnLoadTooltip));
                EndChange();

                // Debug Warnings
                BeginChange();
                EditorGUILayout.PropertyField(debugWarnings, new GUIContent(EditorTextSoundManager.debugWarningsLabel, EditorTextSoundManager.debugWarningsTooltip));
                EndChange();
                if (debugWarnings.boolValue) {
                    EditorGUI.indentLevel++;
                    BeginChange();
                    EditorGUILayout.PropertyField(debugInPlayMode, new GUIContent(EditorTextSoundManager.debugInPlayModeLabel, EditorTextSoundManager.debugInPlayModeTooltip));
                    EndChange();
                    EditorGUI.indentLevel--;
                }

                // Gui Warnings
                BeginChange();
                EditorGUILayout.PropertyField(guiWarnings, new GUIContent(EditorTextSoundManager.guiWarningsLabel, EditorTextSoundManager.guiWarningsTooltip));
                EndChange();

                EditorGUILayout.Separator();

                EditorGUI.indentLevel = 1;
                EditorGUILayout.LabelField(new GUIContent("Performance"), EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                // Voice Limit
                BeginChange();
                EditorGUILayout.PropertyField(voiceLimit, new GUIContent(EditorTextSoundManager.voiceLimitLabel, EditorTextSoundManager.voiceLimitTooltip));
                if (voiceLimit.intValue < 1) {
                    voiceLimit.intValue = 1;
                }
                // Sets the voice limit to the voice preload
                if (voicePreload.intValue > voiceLimit.intValue) {
                    voicePreload.intValue = voiceLimit.intValue;
                }
                EndChange();

                // Source Preload On Awake
                BeginChange();
                EditorGUILayout.PropertyField(voicePreload, new GUIContent(EditorTextSoundManager.voicePreloadLabel, EditorTextSoundManager.voicePreloadTooltip));
                if (voicePreload.intValue < 0) {
                    voicePreload.intValue = 0;
                }
                // Sets the voice limit to the voice preload
                if (voiceLimit.intValue < voicePreload.intValue) {
                    voiceLimit.intValue = voicePreload.intValue;
                }
                EndChange();

                // Project Audio Settings Info
                int oldIndentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;
                EditorGUILayout.BeginHorizontal();
                // For offsetting the buttons to the right
                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUILayout.LabelField(new GUIContent(
                    "Project Audio Settings:" + "\n" +
                    AudioSettings.GetConfiguration().numRealVoices.ToString() + " " + 
                    EditorTextSoundManager.audioSettingsRealVoicesLabel + "\n" + 
                    AudioSettings.GetConfiguration().numVirtualVoices.ToString() + " " + 
                    EditorTextSoundManager.audioSettingsVirtualVoicesLabel
                    , 
                    EditorTextSoundManager.audioSettingsRealAndVirtualVoicesTooltip), 
                    EditorStyles.helpBox);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel = oldIndentLevel;

                // Apply voice limit to audio settings
                EditorGUILayout.BeginHorizontal();
                // For offsetting the buttons to the right
                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextSoundManager.applyVoiceLimitToAudioSettingsLabel, EditorTextSoundManager.applyVoiceLimitToAudioSettingsTooltip))) {
                    // Load the AudioManager asset
                    UnityEngine.Object audioManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/AudioManager.asset")[0];
                    SerializedObject serializedManager = new SerializedObject(audioManager);
                    // Set RealVoiceCount
                    SerializedProperty m_RealVoiceCount = serializedManager.FindProperty("m_RealVoiceCount");
                    m_RealVoiceCount.intValue = voiceLimit.intValue;
                    // Apply properties
                    serializedManager.ApplyModifiedProperties();
                }
                EndChange();
                EditorGUILayout.EndHorizontal();

                // If Real Voices are lower than Voice Limit
                if (ShouldDebug.GuiWarnings()) {
                    if (AudioSettings.GetConfiguration().numRealVoices < voiceLimit.intValue) {
                        EditorGUILayout.Separator();
                        EditorGUILayout.HelpBox(EditorTextSoundManager.audioSettingsRealVoicesWarning, MessageType.Warning);
                        EditorGUILayout.Separator();
                    }

                    // If Virtual Voices are lower than Real Voices
                    if (AudioSettings.GetConfiguration().numVirtualVoices < AudioSettings.GetConfiguration().numRealVoices) {
                        EditorGUILayout.Separator();
                        EditorGUILayout.HelpBox(EditorTextSoundManager.audioSettingsVirtualVoicesWarning, MessageType.Warning);
                        EditorGUILayout.Separator();
                    }
                }

                // Voice Stop Time
                BeginChange();
                EditorGUILayout.PropertyField(voiceStopTime, new GUIContent(EditorTextSoundManager.voiceStopTimeLabel, EditorTextSoundManager.voiceStopTimeTooltip));
                if (voiceStopTime.floatValue < 0f) {
                    voiceStopTime.floatValue = 0f;
                }
                EndChange();

                BeginChange();
                EditorGUILayout.PropertyField(voiceEffectLimit, new GUIContent(EditorTextSoundManager.voiceEffectLimitLabel, EditorTextSoundManager.voiceEffectLimitTooltip));
                if (voiceEffectLimit.intValue < 0) {
                    voiceEffectLimit.intValue = 0;
                }
                EndChange();
                EditorGUILayout.Separator();

                // Debug SoundEvents Live
                EditorGUI.indentLevel = 1;
                EditorGUILayout.LabelField(new GUIContent(EditorTextSoundManager.voiceDebugSoundEventsLiveLabel, EditorTextSoundManager.voiceDebugSoundEventsLiveTooltip), EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                // Debug SoundEvents in Scene View
                BeginChange();
                EditorGUILayout.PropertyField(debugSoundEventsInSceneViewEnabled, new GUIContent(EditorTextSoundManager.debugSoundEventsInSceneViewEnabledLabel, EditorTextSoundManager.debugSoundEventsInSceneViewEnabledTooltip));
                EndChange();

                // Debug SoundEvents in Game View
                BeginChange();
                EditorGUILayout.PropertyField(debugSoundEventsInGameViewEnabled, new GUIContent(EditorTextSoundManager.debugSoundEventsInGameViewEnabledLabel, EditorTextSoundManager.debugSoundEventsInGameViewEnabledTooltip));
                EndChange();

                if (debugSoundEventsInSceneViewEnabled.boolValue || debugSoundEventsInGameViewEnabled.boolValue) {
                    EditorGUILayout.LabelField(new GUIContent("Style"));
                    EditorGUI.indentLevel++;
                    BeginChange();
                    EditorGUILayout.IntSlider(debugSoundEventsFontSize, 1, 100, new GUIContent(EditorTextSoundManager.debugSoundEventsFontSizeLabel, EditorTextSoundManager.debugSoundEventsFontSizeTooltip));
                    EndChange();
                    BeginChange();
                    debugSoundEventsVolumeToAlpha.floatValue = EditorGUILayout.Slider(new GUIContent(EditorTextSoundManager.debugSoundEventsVolumeToAlphaLabel, EditorTextSoundManager.debugSoundEventsVolumeToAlphaTooltip), debugSoundEventsVolumeToAlpha.floatValue, 0f, 1f);
                    EndChange();
                    BeginChange();
                    debugSoundEventsLifetimeToAlpha.floatValue = EditorGUILayout.Slider(new GUIContent(EditorTextSoundManager.debugSoundEventsLifetimeToAlphaLabel, EditorTextSoundManager.debugSoundEventsLifetimeToAlphaTooltip), debugSoundEventsLifetimeToAlpha.floatValue, 0f, 1f);
                    EndChange();
                    BeginChange();
                    debugSoundEventsLifetimeColorLength.floatValue = EditorGUILayout.Slider(new GUIContent(EditorTextSoundManager.debugSoundEventsLifetimeFadeLengthLabel, EditorTextSoundManager.debugSoundEventsLifetimeFadeLengthTooltip), debugSoundEventsLifetimeColorLength.floatValue, 0f, 10f);
                    EndChange();
                    BeginChange();
                    EditorGUILayout.PropertyField(debugSoundEventsColorStart, new GUIContent(EditorTextSoundManager.debugSoundEventsColorStartLabel, EditorTextSoundManager.debugSoundEventsColorStartTooltip));
                    EndChange();
                    BeginChange();
                    EditorGUILayout.PropertyField(debugSoundEventsColorEnd, new GUIContent(EditorTextSoundManager.debugSoundEventsColorFadeToLabel, EditorTextSoundManager.debugSoundEventsColorEndTooltip));
                    EndChange();
                    BeginChange();
                    EditorGUILayout.PropertyField(debugSoundEventsColorOutline, new GUIContent(EditorTextSoundManager.debugSoundEventsColorOutlineLabel, EditorTextSoundManager.debugSoundEventsColorOutlineTooltip));
                    EndChange();
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                // For offsetting the buttons to the right
                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextSoundManager.resetSettingsLabel, EditorTextSoundManager.resetSettingsTooltip))) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Reset Settings");
                        mTargets[i].Internals.settings = new SoundManagerInternalsSettings();
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                }
                EndChange();
                EditorGUILayout.EndHorizontal();
            }
            StopBackgroundColor();
        }

        private Dictionary<SoundEventBase, SoundEventInstanceDictionaryValue>.Enumerator managedUpdateTempInstanceDictionaryValueEnumerator;
        private Dictionary<int, SoundEventInstance>.Enumerator managedUpdateTempInstanceEnumerator;

        private class StatisticsListClass {
            public SoundEventBase soundEvent;
            public int soundEventInstancesActive;
            public int soundEventInstancesDisabled;
            public int numberOfUsedVoices;
            public float averageVolume;
        }

        private List<StatisticsListClass> statisticsList = new List<StatisticsListClass>();

        private void DrawStatistics() {

            StartBackgroundColor(EditorColor.GetStatistics(EditorColorProSkin.GetCustomEditorBackgroundAlpha()));

            EditorGUI.indentLevel = 1;
            BeginChange();
            EditorGuiFunction.DrawFoldout(statisticsGlobalExpand, EditorTextSoundManager.statisticsExpandLabel, EditorTextSoundManager.statisticsExpandTooltip);
            EndChange();

            if (statisticsGlobalExpand.boolValue) {

                if (Application.isPlaying) {
                    // Repaint so its updated all the time
                    Repaint();
                }

                // SoundEvent Statistics
                int usedSoundEvents = 0;
                int disabledSoundEvents = 0;

                foreach (var soundEventDictionaryValue in mTarget.Internals.soundEventDictionary.Values) {
                    foreach (var transformDictionaryValue in soundEventDictionaryValue.instanceDictionary) {
                        usedSoundEvents++;
                    }
                    disabledSoundEvents += soundEventDictionaryValue.unusedInstanceStack.Count;
                }

                // SoundEvents
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent(EditorTextSoundManager.statisticsSoundEventsLabel, EditorTextSoundManager.statisticsSoundEventsTooltip), EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                // Created
                EditorGUILayout.LabelField(new GUIContent((usedSoundEvents + disabledSoundEvents) + "\t " + EditorTextSoundManager.statisticsSoundEventsCreatedLabel, EditorTextSoundManager.statisticsSoundEventsCreatedTooltip));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                // Active
                EditorGUILayout.LabelField(new GUIContent(usedSoundEvents + "\t " + EditorTextSoundManager.statisticsSoundEventsActiveLabel, EditorTextSoundManager.statisticsSoundEventsActiveTooltip));
                // Disabled
                EditorGUILayout.LabelField(new GUIContent(disabledSoundEvents + "\t " + EditorTextSoundManager.statisticsSoundEventsDisabledLabel, EditorTextSoundManager.statisticsSoundEventsDisabledTooltip));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;

                EditorGUILayout.Separator();
                // Source Pool
                int numberOfActiveSources = 0;
                for (int i = 0; i < mTarget.Internals.voicePool.voicePool.Length; i++) {
                    if (mTarget.Internals.voicePool.voicePool[i].isAssigned) {
                        numberOfActiveSources++;
                    }
                }

                // Max Simultaneous
                if (mTarget.Internals.statistics.statisticsMaxSimultaneousVoices < numberOfActiveSources) {
                    mTarget.Internals.statistics.statisticsMaxSimultaneousVoices = numberOfActiveSources;
                }

                // Voices
                EditorGUILayout.LabelField(new GUIContent(EditorTextSoundManager.statisticsVoicesLabel, EditorTextSoundManager.statisticsVoicesTooltip), EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                // Played
                EditorGUILayout.LabelField(new GUIContent(mTarget.Internals.statistics.statisticsVoicesPlayed + "\t " + EditorTextSoundManager.statisticsVoicesPlayedLabel, EditorTextSoundManager.statisticsVoicesPlayedTooltip));
                // Max Simultaneous
                EditorGUILayout.LabelField(new GUIContent(mTarget.Internals.statistics.statisticsMaxSimultaneousVoices + "\t " + EditorTextSoundManager.statisticsMaxSimultaneousVoicesLabel, EditorTextSoundManager.statisticsMaxSimultaneousVoicesTooltip));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                // Stolen
                EditorGUILayout.LabelField(new GUIContent(mTarget.Internals.voicePool.statisticsVoicesStolen + "\t " + EditorTextSoundManager.statisticsVoicesStolenLabel, EditorTextSoundManager.statisticsVoicesStolenTooltip));
                // Created
                EditorGUILayout.LabelField(new GUIContent(mTarget.Internals.voicePool.voicePool.Length + "\t " + EditorTextSoundManager.statisticsVoicesCreatedLabel, EditorTextSoundManager.statisticsVoicesCreatedTooltip));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                // Active
                EditorGUILayout.LabelField(new GUIContent(numberOfActiveSources + "\t " + EditorTextSoundManager.statisticsVoicesActiveLabel, EditorTextSoundManager.statisticsVoicesActiveTooltip));
                // Inactive
                EditorGUILayout.LabelField(new GUIContent(mTarget.Internals.voicePool.voicePool.Length - numberOfActiveSources + "\t " + EditorTextSoundManager.statisticsVoicesInactiveLabel, EditorTextSoundManager.statisticsVoicesInactiveTooltip));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                int pausedSources = 0;
                for (int i = 0; i < mTarget.Internals.voicePool.voicePool.Length; i++) {
                    if (mTarget.Internals.voicePool.voicePool[i].GetState() == VoiceState.Pause) {
                        pausedSources++;
                    }
                }
                // Paused
                EditorGUILayout.LabelField(new GUIContent(pausedSources + "\t " + EditorTextSoundManager.statisticsVoicesPausedLabel, EditorTextSoundManager.statisticsVoicesPausedTooltip));
                int stoppedSources = 0;
                for (int i = 0; i < mTarget.Internals.voicePool.voicePool.Length; i++) {
                    if (mTarget.Internals.voicePool.voicePool[i].GetState() == VoiceState.Stop) {
                        stoppedSources++;
                    }
                }
                // Stopped
                EditorGUILayout.LabelField(new GUIContent(stoppedSources + "\t " + EditorTextSoundManager.statisticsVoicesStoppedLabel, EditorTextSoundManager.statisticsVoicesStoppedTooltip));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;

                EditorGUILayout.Separator();
                // Voice Effect Statistics
                EditorGUILayout.LabelField(new GUIContent(EditorTextSoundManager.statisticsVoiceEffectsLabel, EditorTextSoundManager.statisticsVoiceEffectsTooltip), EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                int numberOfActiveVoiceEffects = 0;

                for (int i = 0; i < mTarget.Internals.voiceEffectPool.voiceEffectPool.Length; i++) {
                    if (mTarget.Internals.voiceEffectPool.voiceEffectPool[i] != null) {
                        if (mTarget.Internals.voiceEffectPool.voiceEffectPool[i].cachedVoiceEffect.GetEnabled()) {
                            numberOfActiveVoiceEffects++;
                        }
                    }
                }
                EditorGUILayout.BeginHorizontal();
                // Active
                EditorGUILayout.LabelField(new GUIContent(numberOfActiveVoiceEffects + "\t " + EditorTextSoundManager.statisticsVoiceEffectsActiveLabel, EditorTextSoundManager.statisticsVoiceEffectsActiveTooltip));
                // Available
                EditorGUILayout.LabelField(new GUIContent(mTarget.Internals.settings.voiceEffectLimit - numberOfActiveVoiceEffects + "\t " + EditorTextSoundManager.statisticsVoiceEffectsAvailableLabel, EditorTextSoundManager.statisticsVoiceEffectsAvailableTooltip));
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
                EditorGUILayout.Separator();
            }

            EditorGUILayout.Separator();

            BeginChange();
            EditorGuiFunction.DrawFoldout(statisticsInstancesExpand, EditorTextSoundManager.statisticsSoundEventInstancesLabel, EditorTextSoundManager.statisticsSoundEventInstancesTooltip);
            EndChange();

            if (statisticsInstancesExpand.boolValue) {

                // Statistics Instances
                BeginChange();
                EditorGUILayout.PropertyField(statisticsSorting, new GUIContent(EditorTextSoundManager.statisticsSortingLabel, EditorTextSoundManager.statisticsSortingTooltip));
                EndChange();

                // Statistics Dropdown Menu
                EditorGUILayout.BeginHorizontal();
                // For offsetting the buttons to the right
                EditorGUILayout.LabelField(new GUIContent(""), GUILayout.Width(EditorGUIUtility.labelWidth));
                BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextSoundManager.statisticsShowLabel, EditorTextSoundManager.statisticsShowTooltip))) {
                    GenericMenu menu = new GenericMenu();

                    // Tooltips dont work for menu
                    menu.AddItem(new GUIContent("Show Active"), statisticsShowActive.boolValue, StatisticsMenuCallback, StatisticsButtonType.ShowActive);
                    menu.AddItem(new GUIContent("Show Disabled"), statisticsShowDisabled.boolValue, StatisticsMenuCallback, StatisticsButtonType.ShowDisabled);
                    menu.AddItem(new GUIContent("Show Voices"), statisticsShowVoices.boolValue, StatisticsMenuCallback, StatisticsButtonType.ShowVoices);
                    menu.AddItem(new GUIContent("Show Plays"), statisticsShowPlays.boolValue, StatisticsMenuCallback, StatisticsButtonType.ShowPlays);
                    menu.AddItem(new GUIContent("Show Volume"), statisticsShowVolume.boolValue, StatisticsMenuCallback, StatisticsButtonType.ShowVolume);

                    menu.ShowAsContext();
                }
                EndChange();
                BeginChange();
                if (GUILayout.Button(new GUIContent(EditorTextSoundManager.statisticsResetLabel, EditorTextSoundManager.statisticsResetTooltip))) {
                    for (int i = 0; i < mTargets.Length; i++) {
                        Undo.RecordObject(mTargets[i], "Reset Statistics");
                        mTargets[i].Internals.statistics = new SoundManagerInternalsStatistics();
                        EditorUtility.SetDirty(mTargets[i]);
                    }
                }
                EndChange();
                EditorGUILayout.EndHorizontal();

                if (!Application.isPlaying) {
                    EditorGUILayout.LabelField(new GUIContent("Available in Playmode"), EditorStyles.boldLabel);
                } else {
                    // Only when application is playing

                    // Iterating over all the SoundEvents
                    statisticsList.Clear();
                    managedUpdateTempInstanceDictionaryValueEnumerator = mTarget.Internals.soundEventDictionary.GetEnumerator();
                    while (managedUpdateTempInstanceDictionaryValueEnumerator.MoveNext()) {
                        StatisticsListClass newStatisticsListClass = new StatisticsListClass();
                        newStatisticsListClass.soundEvent = managedUpdateTempInstanceDictionaryValueEnumerator.Current.Value.statisticsSoundEvent;
                        newStatisticsListClass.soundEventInstancesActive = managedUpdateTempInstanceDictionaryValueEnumerator.Current.Value.instanceDictionary.Count;
                        newStatisticsListClass.soundEventInstancesDisabled = managedUpdateTempInstanceDictionaryValueEnumerator.Current.Value.unusedInstanceStack.Count;
                        managedUpdateTempInstanceEnumerator = managedUpdateTempInstanceDictionaryValueEnumerator.Current.Value.instanceDictionary.GetEnumerator();
                        int numberOfInstances = 0;
                        float tempVolume = 0f;
                        while (managedUpdateTempInstanceEnumerator.MoveNext()) {
                            numberOfInstances++;
                            // Number of voices
                            newStatisticsListClass.numberOfUsedVoices += managedUpdateTempInstanceEnumerator.Current.Value.StatisticsGetNumberOfUsedVoices();
                            // Average Volume
                            tempVolume += managedUpdateTempInstanceEnumerator.Current.Value.StatisticsGetAverageVolumeRatio();
                        }
                        // Avoid divide by zero
                        if (numberOfInstances > 0) {
                            newStatisticsListClass.averageVolume = tempVolume / numberOfInstances;
                        } else {
                            newStatisticsListClass.averageVolume = tempVolume;
                        }

                        statisticsList.Add(newStatisticsListClass);
                    }

                    // All but time should be sorted by name
                    if ((SoundManagerStatisticsSorting)statisticsSorting.enumValueIndex != SoundManagerStatisticsSorting.Time) {
                        // Sort by name
                        statisticsList = statisticsList.OrderBy(parameter => parameter.soundEvent.name).ToList();
                    }

                    if ((SoundManagerStatisticsSorting)statisticsSorting.enumValueIndex == SoundManagerStatisticsSorting.Voices) {
                        // Sort by voices
                        statisticsList = statisticsList.OrderByDescending(parameter => parameter.numberOfUsedVoices).ToList();
                    } else if ((SoundManagerStatisticsSorting)statisticsSorting.enumValueIndex == SoundManagerStatisticsSorting.Plays) {
                        // Sort by number of plays
                        statisticsList = statisticsList.OrderByDescending(parameter => SoundManagerBase.Instance.Internals.InternalStatisticsNumberOfPlays(parameter.soundEvent, false)).ToList();
                    } else if ((SoundManagerStatisticsSorting)statisticsSorting.enumValueIndex == SoundManagerStatisticsSorting.Volume) {
                        // Sort by volume
                        statisticsList = statisticsList.OrderByDescending(parameter => parameter.averageVolume).ToList();
                    }

                    for (int i = 0; i < statisticsList.Count; i++) {

                        // SoundEvent
                        EditorGUILayout.LabelField(new GUIContent(statisticsList[i].soundEvent.name), EditorStyles.boldLabel);

                        int numberOfFields = 0;
                        if (statisticsShowActive.boolValue) {
                            statisticsStrings[numberOfFields] = statisticsList[i].soundEventInstancesActive + "\t " + "Active";
                            // Active
                            if (statisticsList[i].soundEventInstancesActive > 0) {
                                // Green
                                statisticsStyles[numberOfFields].normal.textColor = EditorColorProSkin.GetTextGreen();
                            } else {
                                statisticsStyles[numberOfFields].normal.textColor = defaultGuiStyleTextColor;
                            }
                            numberOfFields++;
                        }
                        if (statisticsShowDisabled.boolValue) {
                            statisticsStrings[numberOfFields] = statisticsList[i].soundEventInstancesDisabled + "\t " + "Disabled";
                            // Disabled
                            if (statisticsList[i].soundEventInstancesDisabled > 0) {
                                // Red
                                statisticsStyles[numberOfFields].normal.textColor = EditorColorProSkin.GetTextRed();
                            } else {
                                statisticsStyles[numberOfFields].normal.textColor = defaultGuiStyleTextColor;
                            }
                            numberOfFields++;
                        }
                        if (statisticsShowVoices.boolValue) {
                            statisticsStrings[numberOfFields] = statisticsList[i].numberOfUsedVoices + "\t " + "Voices";
                            // Voices
                            if (statisticsList[i].numberOfUsedVoices > 0) {
                                // Green
                                statisticsStyles[numberOfFields].normal.textColor = EditorColorProSkin.GetTextGreen();
                            } else {
                                statisticsStyles[numberOfFields].normal.textColor = defaultGuiStyleTextColor;
                            }
                            numberOfFields++;
                        }
                        if (statisticsShowPlays.boolValue) {
                            // Number of Plays
                            statisticsStrings[numberOfFields] = SoundManagerBase.Instance.Internals.InternalStatisticsNumberOfPlays(statisticsList[i].soundEvent, false) + "\t " + "Plays";
                            statisticsStyles[numberOfFields].normal.textColor = defaultGuiStyleTextColor;
                            numberOfFields++;
                        }
                        if (statisticsShowVolume.boolValue) {
                            // Average Volume
                            float tempVolume = VolumeScale.ConvertRatioToDecibel(statisticsList[i].averageVolume);
                            if (tempVolume < -140f) {
                                tempVolume = Mathf.NegativeInfinity;
                            }
                            statisticsStrings[numberOfFields] = tempVolume.ToString("0.0") + "\t " + "dB Average";
                            statisticsStyles[numberOfFields].normal.textColor = defaultGuiStyleTextColor;
                            numberOfFields++;
                        }

                        int currentField = 0;

                        if (numberOfFields > 0) {
                            EditorGUI.indentLevel++;
                            if (numberOfFields >= 2) {
                                EditorGUILayout.BeginHorizontal();
                            }
                            if (numberOfFields >= 1) {
                                EditorGUILayout.LabelField(new GUIContent(statisticsStrings[currentField]), statisticsStyles[currentField]);
                                currentField++;
                            }
                            if (numberOfFields >= 2) {
                                EditorGUILayout.LabelField(new GUIContent(statisticsStrings[currentField]), statisticsStyles[currentField]);
                                currentField++;
                            }
                            if (numberOfFields >= 2) {
                                EditorGUILayout.EndHorizontal();
                            }
                            if (numberOfFields >= 4) {
                                EditorGUILayout.BeginHorizontal();
                            }
                            if (numberOfFields >= 3) {
                                EditorGUILayout.LabelField(new GUIContent(statisticsStrings[currentField]), statisticsStyles[currentField]);
                                currentField++;
                            }
                            if (numberOfFields >= 4) {
                                EditorGUILayout.LabelField(new GUIContent(statisticsStrings[currentField]), statisticsStyles[currentField]);
                                currentField++;
                            }
                            if (numberOfFields >= 4) {
                                EditorGUILayout.EndHorizontal();
                            }
                            if (numberOfFields >= 5) {
                                EditorGUILayout.LabelField(new GUIContent(statisticsStrings[currentField]), statisticsStyles[currentField]);
                                currentField++;
                            }
                            EditorGUI.indentLevel--;
                        }
                        EditorGUILayout.Separator();
                    }
                }
            }

            StopBackgroundColor();
        }

        private enum StatisticsButtonType {
            ShowActive,
            ShowDisabled,
            ShowVoices,
            ShowPlays,
            ShowVolume,
        }

        private void StatisticsMenuCallback(object obj) {

            StatisticsButtonType buttonType;

            try {
                buttonType = (StatisticsButtonType)obj;
            } catch {
                return;
            }

            bool tempToggle = false;

            for (int i = 0; i < mTargets.Length; i++) {
                if (mTargets[i] != null) {
                    Undo.RecordObject(mTargets[i], "Statistics Setting");
                    // Show Active
                    if (buttonType == StatisticsButtonType.ShowActive) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.statistics.statisticsShowActive;
                        }
                        mTargets[i].Internals.statistics.statisticsShowActive = tempToggle;
                    }
                    // Show Disabled
                    if (buttonType == StatisticsButtonType.ShowDisabled) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.statistics.statisticsShowDisabled;
                        }
                        mTargets[i].Internals.statistics.statisticsShowDisabled = tempToggle;
                    }
                    // Show Voices
                    if (buttonType == StatisticsButtonType.ShowVoices) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.statistics.statisticsShowVoices;
                        }
                        mTargets[i].Internals.statistics.statisticsShowVoices = tempToggle;
                    }
                    // Show Plays
                    if (buttonType == StatisticsButtonType.ShowPlays) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.statistics.statisticsShowPlays;
                        }
                        mTargets[i].Internals.statistics.statisticsShowPlays = tempToggle;
                    }
                    // Show Volume
                    if (buttonType == StatisticsButtonType.ShowVolume) {
                        if (i == 0) {
                            tempToggle = !mTargets[0].Internals.statistics.statisticsShowVolume;
                        }
                        mTargets[i].Internals.statistics.statisticsShowVolume = tempToggle;
                    }
                    EditorUtility.SetDirty(mTargets[i]);
                }
            }
        }
    }
}
#endif