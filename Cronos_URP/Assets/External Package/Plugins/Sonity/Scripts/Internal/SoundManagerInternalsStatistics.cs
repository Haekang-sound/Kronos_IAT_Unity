// Created by Victor Engström
// Copyright 2023 Sonigon AB
// http://www.sonity.org/

#if UNITY_EDITOR

using System;

namespace Sonity.Internal {

    [Serializable]
    public class SoundManagerInternalsStatistics {

        public bool statisticsGlobalExpand = true;
        public bool statisticsInstancesExpand = true;
        public SoundManagerStatisticsSorting statisticsSorting = SoundManagerStatisticsSorting.Voices;

        public bool statisticsShowActive = true;
        public bool statisticsShowDisabled = true;
        public bool statisticsShowVoices = false;
        public bool statisticsShowPlays = false;
        public bool statisticsShowVolume = false;

        [NonSerialized]
        public int statisticsVoicesPlayed;
        [NonSerialized]
        public int statisticsMaxSimultaneousVoices;
    }
}
#endif