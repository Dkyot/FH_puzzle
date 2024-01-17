using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformFeatures.MetrikaFeatures
{
    public abstract class MetrikaFeatures : FeaturesSingletonBase<MetrikaFeatures>
    {
        public abstract void SendGameReady();
        public abstract void SendEvent(string eventName);
        public abstract void SendEvent(MetrikaEventEnum eventName);
    }
}