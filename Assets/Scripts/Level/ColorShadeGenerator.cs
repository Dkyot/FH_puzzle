using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.Level {
    public class ColorShadeGenerator : MonoBehaviour {
        [Range(2, 18)]
        [SerializeField] private int numberOfColors = 2;

        [SerializeField] private Color firstColor;
        [SerializeField] private Color secondColor;

        private float rMax, rMin, gMax, gMin, bMax, bMin;

        [SerializeField] private ColorsSO pallete;

        private void Awake() {
            if (pallete == null)
                return;

            pallete.pallete = new List<Color>();

            ColorParsing();

            pallete.pallete = GenerateGradient();
        }

        #region Calculating Colors
        private void ColorParsing() {
            rMax = firstColor.r;
            gMax = firstColor.g;
            bMax = firstColor.b;

            rMin = secondColor.r;
            gMin = secondColor.g;
            bMin = secondColor.b;
        }

        private List<Color> GenerateGradient() {
            List<Color> colorList = new List<Color>();

            float diffR = rMax - rMin;
            float diffG = gMax - gMin;
            float diffB = bMax - bMin;

            float stepR = diffR / numberOfColors;
            float stepG = diffG / numberOfColors;
            float stepB = diffB / numberOfColors;

            //Debug.Log(stepR+" "+stepG+" "+stepB);

            for (int i = 0; i < numberOfColors; i++) {
                rMax -= stepR;
                gMax -= stepG;
                bMax -= stepB;
                colorList.Add(new Color(rMax, gMax, bMax, 1));
            }

            return colorList;
        }
        #endregion
    }
}
