using static FH.UI.Rang;

namespace FH.Utils {
    public static class RangHelpers {
        public static RangTypes CalculateRang(float score) => score switch {
            > 7400 => RangTypes.SS,
            > 6400 => RangTypes.S,
            > 4500 => RangTypes.A,
            > 3000 => RangTypes.B,
            > 1500 => RangTypes.C,
            _ => RangTypes.D
        };
    }
}