using UnityEngine;

namespace SDKPlatforms.Settings
{
    public class RustoreSettingsSo : PlatformSettingsSoBase
    {
        [SerializeField, Space, Header("Project settings")] private string productName;
        [SerializeField] private string version;
        [SerializeField] private Sprite gameIcon;

        [SerializeField] private Color splashColor;
        [SerializeField] private Sprite splashIcon;
        
        public override void SetSettings()
        {
            
        }
    }
}