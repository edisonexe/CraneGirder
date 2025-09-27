using Crane;
using UnityEngine;

namespace InputSystem
{
    public class SignalsLogger : MonoBehaviour
    {
        [SerializeField] private CraneSignals _events;
        [SerializeField] private bool _logsTurnOn;
        
        void OnEnable()
        {
            if(_logsTurnOn)
                _events.OnSignal += Handle;
        }

        void OnDisable()
        {
            if(_logsTurnOn)
                _events.OnSignal -= Handle;
        }

        void Handle(CraneDirection d, bool held) => Debug.Log($"{d}: {(held ? "DOWN" : "UP")}");

    }
}