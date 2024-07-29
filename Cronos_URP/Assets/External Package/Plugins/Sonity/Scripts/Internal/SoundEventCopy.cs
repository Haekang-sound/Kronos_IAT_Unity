// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

namespace Sonity.Internal {

    public static class SoundEventCopy {

        public static void CopyTo(SoundEventBase to, SoundEventBase from, 
            bool copySoundContainers = false, bool copyTimeline = false, bool copyIntensity = false, 
            bool copyTriggerOnSoundEvents = false, bool copySoundTagSoundEvents = false
            ) {

            if (copySoundContainers) {
                to.internals.soundContainers = from.internals.soundContainers;
            }

#if UNITY_EDITOR
            to.internals.data.previewAudioMixerGroup = from.internals.data.previewAudioMixerGroup;
#endif
            to.internals.data.expandSoundContainers = from.internals.data.expandSoundContainers;
            to.internals.data.expandTimeline = from.internals.data.expandTimeline;
            to.internals.data.expandPreview = from.internals.data.expandPreview;

            to.internals.data.expandSettings = from.internals.data.expandSettings;
            to.internals.data.expandTriggerOnPlay = from.internals.data.expandTriggerOnPlay;
            to.internals.data.expandTriggerOnStop = from.internals.data.expandTriggerOnStop;
            to.internals.data.expandTriggerOnTail = from.internals.data.expandTriggerOnTail;
            to.internals.data.expandAllSoundTag = from.internals.data.expandAllSoundTag;

            if (copyTimeline) {
                to.internals.data.timelineSoundContainerData = from.internals.data.timelineSoundContainerData;
            }

//#if UNITY_EDITOR
//            to.internals.data.foundReferences = from.internals.data.foundReferences;
//#endif

            to.internals.data.disableEnable = from.internals.data.disableEnable;
#if UNITY_EDITOR
            to.internals.data.muteEnable = from.internals.data.muteEnable;
            //to.internals.data.soloEnable = from.internals.data.soloEnable;
#endif
            to.internals.data.soundEventModifier = from.internals.data.soundEventModifier;

            // Settings
            to.internals.data.polyphonyMode = from.internals.data.polyphonyMode;
            to.internals.data.audioMixerGroup = from.internals.data.audioMixerGroup;
            to.internals.data.soundMix = from.internals.data.soundMix;
            to.internals.data.soundPolyGroup = from.internals.data.soundPolyGroup;
            to.internals.data.soundPolyGroupPriority = from.internals.data.soundPolyGroupPriority;
            to.internals.data.cooldownTime = from.internals.data.cooldownTime;
            to.internals.data.probability = from.internals.data.probability;
            to.internals.data.passParameters = from.internals.data.passParameters;

            to.internals.data.ignoreLocalPause = from.internals.data.ignoreLocalPause;
            to.internals.data.ignoreGlobalPause = from.internals.data.ignoreGlobalPause;

            // Intensity
            to.internals.data.expandIntensity = from.internals.data.expandIntensity;
            to.internals.data.intensityAdd = from.internals.data.intensityAdd;
            to.internals.data.intensityMultiplier = from.internals.data.intensityMultiplier;
            to.internals.data.intensityRolloff = from.internals.data.intensityRolloff;
            to.internals.data.intensitySeekTime = from.internals.data.intensitySeekTime;
            to.internals.data.intensityCurve = from.internals.data.intensityCurve;
            to.internals.data.intensityThresholdEnable = from.internals.data.intensityThresholdEnable;
            to.internals.data.intensityThreshold = from.internals.data.intensityThreshold;

            if (copyIntensity) {
                to.internals.data.intensityScaleAdd = from.internals.data.intensityScaleAdd;
                to.internals.data.intensityScaleMultiplier = from.internals.data.intensityScaleMultiplier;
            }

#if UNITY_EDITOR
            to.internals.data.intensityRecord = from.internals.data.intensityRecord;
            to.internals.data.intensityDebugResolution = from.internals.data.intensityDebugResolution;
            to.internals.data.intensityDebugZoom = from.internals.data.intensityDebugZoom;
            //to.internals.data.intensityDebugValueList = from.internals.data.intensityDebugValueList;
#endif

            // TriggerOnPlay
            to.internals.data.triggerOnPlayEnable = from.internals.data.triggerOnPlayEnable;
            if (copyTriggerOnSoundEvents) {
                to.internals.data.triggerOnPlaySoundEvents = from.internals.data.triggerOnPlaySoundEvents;
            }
            to.internals.data.triggerOnPlayWhichToPlay = from.internals.data.triggerOnPlayWhichToPlay;

            // TriggerOnStop
            to.internals.data.triggerOnStopEnable = from.internals.data.triggerOnStopEnable;
            if (copyTriggerOnSoundEvents) {
                to.internals.data.triggerOnStopSoundEvents = from.internals.data.triggerOnStopSoundEvents;
            }
            to.internals.data.triggerOnStopWhichToPlay = from.internals.data.triggerOnStopWhichToPlay;

            // TriggerOnTail
            to.internals.data.triggerOnTailEnable = from.internals.data.triggerOnTailEnable;
            if (copyTriggerOnSoundEvents) {
                to.internals.data.triggerOnTailSoundEvents = from.internals.data.triggerOnTailSoundEvents;
            }
            to.internals.data.triggerOnTailWhichToPlay = from.internals.data.triggerOnTailWhichToPlay;
            to.internals.data.triggerOnTailLength = from.internals.data.triggerOnTailLength;
            to.internals.data.triggerOnTailBpm = from.internals.data.triggerOnTailBpm;
            to.internals.data.triggerOnTailBeatLength = from.internals.data.triggerOnTailBeatLength;

            // SoundTag
            to.internals.data.soundTagEnable = from.internals.data.soundTagEnable;
            to.internals.data.soundTagMode = from.internals.data.soundTagMode;
            to.internals.data.soundTagGroups = from.internals.data.soundTagGroups;
            if (copySoundTagSoundEvents) {
                for (int i = 0; i < to.internals.data.soundTagGroups.Length; i++) {
                    to.internals.data.soundTagGroups[i].soundEvent = new SoundEventBase[0];
                }
            }
        }
    }
}