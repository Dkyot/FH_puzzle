using System.Collections.Generic;
using UnityEngine;

namespace YG.Example
{
    public class YandexMetricaExample : MonoBehaviour
    {
        public static void TestSend()
        {
            YG2.MetricaSend("target");


            YG2.MetricaSend("levelTest", "level", "5");


            var eventData = new Dictionary<string, object>
            {
                { "stars", 3 },
                { "time", 10 }
            };
            YG2.MetricaSend("levelsFinish", "1", eventData);


            eventData = new Dictionary<string, object>
            {
                { "stars", 2 },
                { "time", 5 }
            };
            YG2.MetricaSend("levelsFinish", "1", eventData);


            var eventData1 = new Dictionary<string, string>
            {
                { "action", "click" },
                { "element", "button" }
            };
            YG2.MetricaSend("test2", eventData1);


            var eventData2 = new Dictionary<string, object>
            {
                { "action", "click" },
                { "element", "button" },
                { "user", new Dictionary<string, object>
                    {
                        { "level", 5 },
                        { "experience", 1020 }
                    }
                }
            };
            YG2.MetricaSend("test3", eventData2);
        }
    }
}