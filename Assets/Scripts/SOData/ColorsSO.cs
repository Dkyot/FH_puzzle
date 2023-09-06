using System.Collections.Generic;
using UnityEngine;

namespace FH.SO {
    [CreateAssetMenu(fileName = "pallete_", menuName = "SOData/Colors")]
    public class ColorsSO : ScriptableObject {
        public List<Color> pallete = new List<Color>();
    }
}
