using Crane;
using UnityEngine;

namespace InputSystem
{
    public class KeyboardInput : MonoBehaviour
    {
        [SerializeField] private CraneSignals _eventBus;

        private void Update()
        {
            _eventBus.Emit(CraneDirection.North, Input.GetKey(KeyCode.W));
            _eventBus.Emit(CraneDirection.South, Input.GetKey(KeyCode.S));
            _eventBus.Emit(CraneDirection.East, Input.GetKey(KeyCode.D));
            _eventBus.Emit(CraneDirection.West, Input.GetKey(KeyCode.A));
            _eventBus.Emit(CraneDirection.Up, Input.GetKey(KeyCode.Q));
            _eventBus.Emit(CraneDirection.Down, Input.GetKey(KeyCode.E));
        }
    }
}