using static FH.UI.Rang;

namespace FH.Utils {
    public static class RangHelpers {
        public static RangTypes CalculateRang(float score) => score switch {
            > 7500 => RangTypes.SS,
            > 6500 => RangTypes.S,
            > 5000 => RangTypes.A,
            > 3500 => RangTypes.B,
            > 2000 => RangTypes.C,
            _ => RangTypes.D
        };
    }
}