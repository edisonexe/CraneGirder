using Crane;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputSystem
{
    public class RCButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private CraneDirection _direction;
        [SerializeField] private CraneSignals _eventBus;
        
        public void OnPointerDown(PointerEventData eventData) => _eventBus.Emit(_direction, true);
        public void OnPointerUp(PointerEventData eventData) => _eventBus.Emit(_direction, false);
    }
}