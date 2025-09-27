using System;
using Crane;
using UnityEngine;

namespace InputSystem
{
    public class CraneSignals : MonoBehaviour
    {
        public event Action<CraneDirection, bool> OnSignal;
        
        public void Emit(CraneDirection direction, bool pressed)
            => OnSignal?.Invoke(direction, pressed);
    }
}