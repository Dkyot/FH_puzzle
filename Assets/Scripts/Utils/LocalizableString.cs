using System;
using UnityEngine;

namespace FH.Utils {
    [Serializable]
    public sealed class LocalizableString {
        [field: SerializeField] public string Value { get; private set; }
        [field: SerializeField] public bool IsLocalizable { get; private set; }
    }
}
