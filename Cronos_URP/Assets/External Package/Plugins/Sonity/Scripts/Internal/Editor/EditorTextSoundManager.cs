// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using UnityEngine;

namespace Sonity.Internal {

    public class EditorTextSoundManager {

        public static string soundManagerTooltip =
        $"The {nameof(NameOf.SoundManager)} is the master object which is used to play sounds and manage global settings." + "\n" +
        "\n" +
        $"An instance of this object is required in the scene in order to play {nameof(NameOf.SoundEvent)}s." + "\n" +
        "\n" +
        $"You can add the pre-made prefab called “SoundManager” found in “Assets\\Plugins\\Sonity\\Prefabs” to your scene." + "\n" +
        "\n" +
        $"Or you can add the “Sonity - Sound Manager” component to an empty {nameof(GameObject)} in the scene, it works just as well." + "\n" +
        "\n" +
        $"Tip: If you need to play {nameof(NameOf.SoundEvent)}s on Awake() when starting your game you need to edit the Script Execution Order." + "\n" +
        "\n" +
        $"Go to \"Project Settings...\" -> \"Script Execution Order\" and add \"Sonity.SoundManager\"." + "\n" +
        "\n" +
        $"Then set it to a negative value (like -50) so it loads before the code which you want to use Awake() to play sounds when starting your game." + EditorTrial.trialTooltip;

        // Warnings
        public static string speedOfSoundScaleWarning = "Speed of Sound Scale is 0. It will have no effect.";
        public static string disablePlayingSoundsWarning = $"No {nameof(NameOf.SoundEvent)}s can be played";
        public static string audioSettingsRealVoicesWarning = $"Real Voices are lower than Voice Limit";
        public static string audioSettingsVirtualVoicesWarning = $"Virtual Voices are lower than Real Voices";

        // Reset
        public static string resetSettingsLabel = "Reset Settings";
        public static string resetSettingsTooltip = "Resets all settings." + EditorTrial.trialTooltip;

        public static string resetAllLabel = "Reset All";
        public static string resetAllTooltip = "Resets settings and statistics." + EditorTrial.trialTooltip;

        // Free Trial Text
        public static string enableSoundInBuildsLabel = "Enable Sound in Builds";
        public static string enableSoundInBuildsTooltip =
            "This feature is removed from the Free Trial version of Sonity." + "\n" +
            "\n" + " Please buy the full version to get this feature." + EditorTrial.trialTooltip;

        // Settings
        public static string disablePlayingSoundsLabel = "Disable Playing Sounds";
        public static string disablePlayingSoundsTooltip =
            "Disables all the Play functionality." + "\n" +
            "\n" +
            "Useful if you've for example implemented temp sounds and don't want everyone else to hear them." + EditorTrial.trialTooltip;

        public static string soundTimeScaleLabel = $"Sound Time Scale";
        public static string soundTimeScaleTooltip =
            $"Change the Time Scale used for sound related calculations ingame." + "\n" +
            "\n" +
            $"When the game is not playing in the editor, only Time.realtimeSinceStartup can be used (for preview, etc)." + "\n" +
            "\n" +
            $"The Time Scale is used to calculate:" + "\n" +
            $"- Modifier Delay" + "\n" +
            $"- Modifier Fade In/Out Length" + "\n" +
            $"- SoundEvent Timeline Delay" + "\n" +
            $"- SoundEvent Cooldown Time" + "\n" +
            $"- SoundEvent Intensity Seek Time" + "\n" +
            $"- SoundContainer Prevent End Clicks Fade" + "\n" +
            $"- SoundParameter Delay" + "\n" +
            $"- SoundManager Speed of Sound Delay" + "\n" +
            $"- SoundManager Voice Disable Time" + "\n" +
            $"- SoundManager SoundEvent Debug Live Fade" + EditorTrial.trialTooltip;

        public static string globalPauseLabel = $"Global Pause";
        public static string globalPauseTooltip =
            $"Pauses or unpauses all sound using {nameof(AudioListener)}.pause." + "\n" +
            "\n" +
            $"Except the {nameof(NameOf.SoundEvent)}s which are set to \"Ignore Global Pause\"." + "\n" +
            "\n" +
            $"Note that because {nameof(AudioListener)}.pause is used, all other non-Sonity AudioSources will also be paused." + "\n" +
            "\n" +
            $"This can be remedied with using AudioSource.ignoreListenerPause." + "\n" +
            "\n" +
            $"Can be set with {nameof(SoundManagerBase.Internals.SetGlobalPause)}() and {nameof(SoundManagerBase.Internals.SetGlobalUnpause)}() in the SoundManager." + EditorTrial.trialTooltip;

        public static string globalVolumeLabel = $"Global Volume";
        public static string globalVolumeTooltip =
            $"Sets the global volume using {nameof(AudioListener)}.volume." + "\n" +
            "\n" +
            $"Note that because {nameof(AudioListener)}.volume is used, all other non-Sonity AudioSources will also be affected." + "\n" +
            "\n" +
            $"This can be remedied with using AudioSource.ignoreListenerVolume." + "\n" +
            "\n" +
            $"Can be set with {nameof(SoundManagerBase.Internals.SetGlobalVolumeDecibel)}() and {nameof(SoundManagerBase.Internals.SetGlobalVolumeRatio)}() in the SoundManager." + EditorTrial.trialTooltip;

        public static string globalSoundTagLabel = $"Global {nameof(NameOf.SoundTag)}";
        public static string globalSoundTagTooltip = 
            $"The selected Global {nameof(NameOf.SoundTag)}." + "\n" +
            "\n" +
            $"{nameof(NameOf.SoundEvent)}s using global {nameof(NameOf.SoundTag)} are affected by this." + EditorTrial.trialTooltip;

        public static string distanceScaleLabel = "Distance Scale";
        public static string distanceScaleTooltip =
            "Global range scale multiplier for all the sounds in Sonity." + "\n" +
            "\n" +
            "Distance is calculated by Unity units of distance." + "\n" +
            "\n" +
            $"E.g. if Distance Scale is set to 100, a {nameof(NameOf.SoundEvent)} with the distance multiplier of 1 will be heard up to 100 Unity units away." + EditorTrial.trialTooltip;

        public static string overrideListenerDistanceLabel = "Override Listener Distance";
        public static string overrideListenerDistanceTooltip =
            $"If enabled an {nameof(NameOf.AudioListenerDistance)} component is required in the scene." + "\n" +
            "\n" +
            $"The position of the {nameof(NameOf.AudioListenerDistance)} component will determine all distance based calculations (like volume falloff)." + "\n" +
            "\n" +
            $"While the AudioListener position will be used for spatialization and Angle to Stereo Pan calculations." + "\n" +
            "\n" +
            $"Example of usage in a 3rd person or top down game:" + "\n" +
            "\n" +
            $"Enable \"Override Listener Distance\" in the {nameof(NameOf.SoundManager)}." + "\n" +
            "\n" +
            $"Put the AudioListener on the main camera and the {nameof(NameOf.AudioListenerDistance)} on the player character." + "\n" +
            "\n" +
            $"Try changing the Amount slider to find a nice balance between the different positions." + EditorTrial.trialTooltip;

        public static string overrideListenerDistanceAmountLabel = "Amount %";
        public static string overrideListenerDistanceAmountTooltip =
            $"How much weight the {nameof(NameOf.AudioListenerDistance)} position has over the AudioListener position." + "\n" +
            "\n" +
            $"The position is linearly interpolated between the two of them." + "\n" +
            "\n" +
            $"100% is at the {nameof(NameOf.AudioListenerDistance)} component position." + "\n" +
            "\n" +
            $"50% is halfway between them." + "\n" +
            "\n" +
            $"0% is at the AudioListener position.";

        public static string speedOfSoundEnabledLabel = "Enable Speed of Sound";
        public static string speedOfSoundEnabledTooltip = 
            $"Speed of sound is a delay based on the distance between the Audio Listener and a {nameof(NameOf.SoundEvent)}." + EditorTrial.trialTooltip;

        public static string speedOfSoundScaleLabel = "Speed of Sound Scale";
        public static string speedOfSoundScaleTooltip =
            $"Global speed of sound delay scale multiplier." + "\n" +
            "\n" +
            $"1 equals 430 Unity units per second." + "\n" +
            "\n" +
            $"Is calculated using the time scale selected in the {nameof(NameOf.SoundManager)}." + EditorTrial.trialTooltip;

        public static string debugWarningsLabel = "Debug Warnings";
        public static string debugWarningsTooltip =
            "Makes Sonity output Debug Warnings if anything is wrong." + EditorTrial.trialTooltip;

        public static string debugInPlayModeLabel = "Debug In Play Mode";
        public static string debugInPlayModeTooltip =
            "Makes Sonity output Debug Warnings if anything is wrong in Play Mode." + EditorTrial.trialTooltip;

        public static string guiWarningsLabel = "GUI Warnings";
        public static string guiWarningsTooltip =
            "Makes Sonity show GUI Warnings if anything is wrong in the editor." + EditorTrial.trialTooltip;

        public static string dontDestoyOnLoadLabel = "Use DontDestroyOnLoad()";
        public static string dontDestoyOnLoadTooltip =
            "Calls DontDestroyOnLoad() at Start for Sonity objects." + "\n" +
            "\n" +
            "Which makes them persistent when switching scenes." + "\n" +
            "\n" +
            "For this to work the parent is set to null, which can move the objects." + EditorTrial.trialTooltip;

        // Performance
        public static string voicePreloadLabel = "Voice Preload";
        public static string voicePreloadTooltip = 
            "How many Voices to preload on Awake()." + "\n" +
            "\n" +
            "Voice Limit cannot be lower than Voice Preload." + EditorTrial.trialTooltip;

        public static string voiceLimitLabel = "Voice Limit";
        public static string voiceLimitTooltip = 
            "Maximum number of Voices." + "\n" +
            "\n" +
            "If the limit is reached it will steal the Voice with the lowest priority." + "\n" +
            "\n" +
            "If you need extra performance, you could try lowering the real and virtual voices to a lower number." + "\n" +
            "\n" +
            "Voice Limit cannot be lower than Voice Preload." + EditorTrial.trialTooltip;

        public static string audioSettingsRealVoicesLabel = "Max Real Voices";
        public static string audioSettingsVirtualVoicesLabel = "Max Virtual Voices";
        public static string audioSettingsRealAndVirtualVoicesTooltip =
            $"Max Real Voices:" + "\n" +
            $"The maximum number of real (heard) {nameof(AudioSource)}s that can be played at the same time." + "\n" +
            "\n" +
            $"\"Real Voices\" should be the same as the \"Voice Limit\", or more if you play other sounds outside of Sonity." + "\n" +
            "\n" + 
            $"Max Virtual Voices:" + "\n" +
            $"The maximum number of virtual (not heard) {nameof(AudioSource)}s that can be played at the same time." + "\n" +
            "\n" +
            $"This should always be more than the number of real voices." + "\n" +
            "\n" +
            "You can change these values manually in:" + "\n" +
            "\"Edit\" > \"Project Settings\" > \"Audio\"" + EditorTrial.trialTooltip;

        public static string applyVoiceLimitToAudioSettingsLabel = "Apply to Project Audio Settings";
        public static string applyVoiceLimitToAudioSettingsTooltip =
            "Applies the Voice Limit to the Project Audio Settings." + "\n" +
            "\n" +
            "Sets \"Real Voices\" to the \"Voice Limit\"." + "\n" +
            "\n" +
            "You can change these values manually in:" + "\n" +
            "\"Edit\" > \"Project Settings\" > \"Audio\"" + EditorTrial.trialTooltip;

        public static string voiceStopTimeLabel = "Voice Disable Time";
        public static string voiceStopTimeTooltip =
            $"How long in seconds to wait before disabling a Voice when they've stopped playing. " + "\n" +
            "\n" +
            $"Retriggering a voice which is not disabled is more performant than retriggering a voice which is disabled." + "\n" +
            "\n" +
            $"But having a lot of voices enabled which aren't used is also not good for performance, so don't set this value too high." + "\n" +
            "\n" +
            $"Is calculated using the time scale selected in the {nameof(NameOf.SoundManager)}." + EditorTrial.trialTooltip;

        public static string voiceEffectLimitLabel = "Voice Effect Limit";
        public static string voiceEffectLimitTooltip =
            "Maximum number of Voice Effects which can be used at the same time." + "\n" +
            "\n" +
            "A Voice with any combination of waveshaper/lowpass/highpass counts as one Voice Effect." + "\n" +
            "\n" +
            "If the values of a Voice Effect doesn't have any effect it is disabled automatically (e.g. distortion amount is 0)." + "\n" +
            "\n" +
            "If the Voice Effect limit is reached, the Voice Effects are prioritized by the Voices with the highest volume * priority." + "\n" +
            "\n" +
            "Watch out for high load on the audio thread if set too high." + "\n" +
            "\n" +
            "Try setting the buffer size to \"Best Performance\" in \"Edit\" > \"Project Settings\" > \"Audio\" if you want to run more Voice Effects." + EditorTrial.trialTooltip;

        // Debug SoundEvents Live
        public static string voiceDebugSoundEventsLiveLabel = $"Debug {nameof(NameOf.SoundEvent)}s Live";
        public static string voiceDebugSoundEventsLiveTooltip =
            $"Debug {nameof(NameOf.SoundEvent)}s live draws the names of all currently playing {nameof(NameOf.SoundEvent)} in the scene and/or game view." + "\n" +
            "\n" +
            $"Useful for debugging when you want to see what is playing and where." + EditorTrial.trialTooltip;

        public static string debugSoundEventsInSceneViewEnabledLabel = $"In Scene View";
        public static string debugSoundEventsInSceneViewEnabledTooltip = 
            $"Draws debug names in the Unity scene view." + "\n" +
            "\n" +
            $"Doesn't work in Unity versions older than 2019.1." + EditorTrial.trialTooltip;

        public static string debugSoundEventsInGameViewEnabledLabel = $"In Game View";
        public static string debugSoundEventsInGameViewEnabledTooltip = 
            $"Draws debug names in the Unity game view." + "\n" +
            "\n" +
            $"Only applied in the Unity editor." + EditorTrial.trialTooltip;

        public static string debugSoundEventsFontSizeLabel = $"Font Size";
        public static string debugSoundEventsFontSizeTooltip = "The font size of the text.";

        public static string debugSoundEventsVolumeToAlphaLabel = "Volume to Alpha";
        public static string debugSoundEventsVolumeToAlphaTooltip = 
            $"How much of the volume of the {nameof(NameOf.SoundEvent)} will be applied to the transparency of the text." + "\n" +
            "\n" +
            $"E.g lower volumes will be more transparent." + EditorTrial.trialTooltip;

        public static string debugSoundEventsLifetimeToAlphaLabel = "Lifetime to Alpha";
        public static string debugSoundEventsLifetimeToAlphaTooltip =
            $"How much the lifetime of the {nameof(NameOf.SoundEvent)} will affect the transparency of the text." + EditorTrial.trialTooltip;

        public static string debugSoundEventsLifetimeFadeLengthLabel = "Lifetime Fade Length";
        public static string debugSoundEventsLifetimeFadeLengthTooltip =
            $"How long the fade should be." + "\n" +
            "\n" +
            $"Is calculated using the time scale selected in the {nameof(NameOf.SoundManager)}." + EditorTrial.trialTooltip;

        public static string debugSoundEventsColorStartLabel = "Lifetime Start Color";
        public static string debugSoundEventsColorStartTooltip = "The color the text should have when it starts playing." + EditorTrial.trialTooltip;

        public static string debugSoundEventsColorFadeToLabel = "Lifetime Fade Color";
        public static string debugSoundEventsColorEndTooltip = "Which color the text should fade to over the lifetime." + EditorTrial.trialTooltip;

        public static string debugSoundEventsColorOutlineLabel = "Outline Color";
        public static string debugSoundEventsColorOutlineTooltip = "The color of the text outline." + EditorTrial.trialTooltip;

        // Statistics
        public static string statisticsExpandLabel = "Global Statistics";
        public static string statisticsExpandTooltip = "Statistics of Sonity." + EditorTrial.trialTooltip;

        public static string statisticsSoundEventsLabel = $"{nameof(NameOf.SoundEvent)}s";
        public static string statisticsSoundEventsTooltip = $"Statistics of {nameof(NameOf.SoundEvent)}s." + EditorTrial.trialTooltip;

        public static string statisticsSoundEventsCreatedLabel = "Created";
        public static string statisticsSoundEventsCreatedTooltip = $"The number of instantiated {nameof(NameOf.SoundEvent)}s." + EditorTrial.trialTooltip;

        public static string statisticsSoundEventsActiveLabel = "Active";
        public static string statisticsSoundEventsActiveTooltip = $"The number of active {nameof(NameOf.SoundEvent)}s." + EditorTrial.trialTooltip;

        public static string statisticsSoundEventsDisabledLabel = "Disabled";
        public static string statisticsSoundEventsDisabledTooltip = $"The number of unused and disabled {nameof(NameOf.SoundEvent)}s." + EditorTrial.trialTooltip;

        public static string statisticsVoicesLabel = "Voices";
        public static string statisticsVoicesTooltip = "Statistics of Voices." + EditorTrial.trialTooltip;

        public static string statisticsVoicesPlayedLabel = "Played";
        public static string statisticsVoicesPlayedTooltip = "The number of played Voices since start." + EditorTrial.trialTooltip;

        public static string statisticsMaxSimultaneousVoicesLabel = "Max Simultaneous";
        public static string statisticsMaxSimultaneousVoicesTooltip = "The maximum number of simultaneously playing Voices since start." + EditorTrial.trialTooltip;

        public static string statisticsVoicesStolenLabel = "Stolen";
        public static string statisticsVoicesStolenTooltip = "The number of stolen Voices since start." + EditorTrial.trialTooltip;

        public static string statisticsVoicesCreatedLabel = "Created";
        public static string statisticsVoicesCreatedTooltip = "The number of Voices in the pool." + EditorTrial.trialTooltip;

        public static string statisticsVoicesActiveLabel = "Active";
        public static string statisticsVoicesActiveTooltip = "The number of Voices playing audio." + EditorTrial.trialTooltip;

        public static string statisticsVoicesInactiveLabel = "Inactive";
        public static string statisticsVoicesInactiveTooltip = "The number of inactive Voices in the pool." + EditorTrial.trialTooltip;

        public static string statisticsVoicesPausedLabel = "Paused";
        public static string statisticsVoicesPausedTooltip = "The number of paused Voices in the pool." + EditorTrial.trialTooltip;

        public static string statisticsVoicesStoppedLabel = "Stopped";
        public static string statisticsVoicesStoppedTooltip = "The number of stopped Voices in the pool." + EditorTrial.trialTooltip;

        public static string statisticsVoiceEffectsLabel = "Voice Effects";
        public static string statisticsVoiceEffectsTooltip = 
            "Statistics of Voice Effects." + "\n" +
            "\n" +
            "A Voice with any combination of waveshaper/lowpass/highpass counts as one Voice Effect." + EditorTrial.trialTooltip;

        public static string statisticsVoiceEffectsActiveLabel = "Active";
        public static string statisticsVoiceEffectsActiveTooltip = "The number of active Voice Effects." + EditorTrial.trialTooltip;

        public static string statisticsVoiceEffectsAvailableLabel = "Available";
        public static string statisticsVoiceEffectsAvailableTooltip = "How many Voice Effects are available." + EditorTrial.trialTooltip;

        public static string statisticsSoundEventInstancesLabel = "Instance Statistics";
        public static string statisticsSoundEventInstancesTooltip =
            $"Real-time statistics per {nameof(NameOf.SoundEvent)} Instance." + "\n" +
            "\n" +
            "Available in Playmode." + EditorTrial.trialTooltip;

        public static string statisticsSortingLabel = $"Sort By";
        public static string statisticsSortingTooltip =
            $"Which method to sort the list of {nameof(NameOf.SoundEvent)} Instances." + "\n" +
            "\n" +
            "Name" + "\n" +
            "Sorts by alphabetical order." + "\n" +
            "\n" +
            "Voices" + "\n" +
            "Sorts by voice count." + "\n" +
            "\n" +
            "Plays" + "\n" +
            "Sorts by number of plays." + "\n" +
            "\n" +
            "Volume" + "\n" +
            "Sorts by volume." + "\n" +
            "\n" +
            "Time" + "\n" +
            "Sorts by last time played." + EditorTrial.trialTooltip;

        public static string statisticsShowLabel = $"Show";
        public static string statisticsShowTooltip = 
            $"Toggle what information to show about the {nameof(NameOf.SoundEvent)} Instances." + "\n" +
            "\n" +
            "Show Active" + "\n" +
            "How many are currently active." + "\n" +
            "\n" +
            "Show Disabled" + "\n" +
            "How many are currently disabled." + "\n" +
            "\n" +
            "Show Voices" + "\n" +
            "How many voices are currently used." + "\n" +
            "\n" +
            "Show Plays" + "\n" +
            "The number of total plays." + "\n" +
            "\n" +
            "Show Volume" + "\n" +
            "The current average volume." + EditorTrial.trialTooltip;

        public static string statisticsResetLabel = "Reset";
        public static string statisticsResetTooltip = "Resets statistics." + EditorTrial.trialTooltip;
    }
}
#endif