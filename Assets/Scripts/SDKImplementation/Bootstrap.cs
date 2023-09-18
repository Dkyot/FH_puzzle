using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SkibidiRunner.Managers
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private GameObject loadingPanel;
        
        [SerializeField] private List<MonoBehaviourInitializable> initObjects;
        [SerializeField] private List<MonoBehaviourInitializable> startInitObjects;

        private void Awake()
        {
            loadingPanel.SetActive(true);
            
            foreach (var obj in initObjects)
            {
                obj.TryInitialize();
            }
        }
        
        private void Start()
        {
            foreach (var obj in startInitObjects)
            {
                obj.TryInitialize();
            }
            
            loadingPanel.SetActive(false);
        }
    }
}