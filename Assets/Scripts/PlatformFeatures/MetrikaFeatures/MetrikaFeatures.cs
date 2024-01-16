using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformFeatures.MetricaFeatures
{
    public abstract class MetrikaFeatures : FeaturesSingletonBase<MetrikaFeatures>
    {
        public abstract void SendGameReady();
        public abstract void SendEvent(string eventName);
        public abstract void SendEvent(MetrikaEventEnum eventName);
    }
}