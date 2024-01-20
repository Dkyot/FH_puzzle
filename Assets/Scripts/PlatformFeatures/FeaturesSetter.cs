using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformFeatures
{
    public class FeaturesSetter : MonoBehaviour
    {
        [SerializeField] private bool debug;
        [SerializeField] private List<GameObject> editorFeatures;
        [SerializeField] private List<GameObject> buildFeatures;

        private void Awake()
        {
#if UNITY_EDITOR
            var currentFeatures = editorFeatures;
#else
            var currentFeatures = buildFeatures;
#endif
            if (debug)
            {
                currentFeatures = editorFeatures;
            }
            foreach (var featureObject in currentFeatures)
            {
                Instantiate(featureObject);
            }
        }
    }
}