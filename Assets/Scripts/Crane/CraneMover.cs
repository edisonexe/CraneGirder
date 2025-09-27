using InputSystem;
using UnityEngine;
using Utils;

namespace Crane
{
    public class CraneMover : MonoBehaviour
    {
        [Header("Axes")]
        [SerializeField] private Transform _bridge;
        [SerializeField] private Transform _trolley;
        [SerializeField] private Transform _hook;
        [SerializeField] private Transform _rope;
        
        [Header("Anchors")]
        [SerializeField] private Transform _BridgeA;
        [SerializeField] private Transform _BridgeB;
        [SerializeField] private Transform _TrolleyA;
        [SerializeField] private Transform _TrolleyB;
        [SerializeField] private Transform _HookA;
        [SerializeField] private Transform _HookB;
        
        [SerializeField] private Transform _ropeTop;
        [SerializeField] private Transform _ropeBottom; 
        
        [Header("Directional Speeds")]
        [SerializeField] float _speedNorth = 3f;
        [SerializeField] float _speedSouth = 3f;
        [SerializeField] float _speedEast  = 5f;
        [SerializeField] float _speedWest  = 5f;
        [SerializeField] float _speedUp    = 6f;
        [SerializeField] float _speedDown  = 6f;
        
        [Header("CraneSignals")]
        [SerializeField] private CraneSignals _eventBus;
        private bool _n, _s, _e, _w, _up, _down;
        private float _tBridge, _tTrolley, _tHook;
        private float _ropeScale = 0.5f;
        
        private void Awake()
        {
            _tBridge  = MathUtils.Project01(_bridge.position,  _BridgeA.position,  _BridgeB.position);
            _tTrolley = MathUtils.Project01(_trolley.position, _TrolleyA.position, _TrolleyB.position);
            _tHook   = MathUtils.Project01(_hook.position,    _HookA.position,    _HookB.position);
            ApplyAllMoves();
        }

        private void Update()
        {
            UpdateMovingParams();
            ApplyAllMoves();
        }
        
        void LateUpdate() => UpdateRopeMesh();

        private void OnEnable() => _eventBus.OnSignal += OnSignalMove;
        private void OnDisable() => _eventBus.OnSignal -= OnSignalMove;

        public void OnSignalMove(CraneDirection dir, bool held)
        {
            switch (dir)
            {
                case CraneDirection.North: _n = held; break;
                case CraneDirection.South: _s = held; break;
                case CraneDirection.East: _e = held; break;
                case CraneDirection.West: _w = held; break;
                case CraneDirection.Up: _up = held; break;
                case CraneDirection.Down: _down = held; break;
            }
        }
        
        private void UpdateMovingParams()
        {
            // length from A to B
            float Lb = Mathf.Max(0.001f, Vector3.Distance(_BridgeA.position, _BridgeB.position));
            float Lt = Mathf.Max(0.001f, Vector3.Distance(_TrolleyA.position, _TrolleyB.position));
            float Lh = Mathf.Max(0.001f, Vector3.Distance(_HookA.position, _HookB.position));
            
            // velocity
            float vb = (_n && !_s) ? +_speedNorth : (_s && !_n) ? -_speedSouth : 0f;
            float vt = (_e && !_w) ? +_speedEast : (_w && !_e) ? -_speedWest  : 0f;
            float vh = (_down && !_up) ? +_speedDown : (_up && !_down) ? -_speedUp : 0f;

            // new pos
            _tBridge = Mathf.Clamp01(_tBridge + (vb / Lb) * Time.deltaTime);
            _tTrolley = Mathf.Clamp01(_tTrolley + (vt / Lt) * Time.deltaTime);
            _tHook = Mathf.Clamp01(_tHook + (vh / Lh) * Time.deltaTime);
        }
        
        private void MoveBridge() => 
            _bridge.position = Vector3.Lerp(_BridgeA.position, _BridgeB.position, _tBridge);
        
        private void MoveTrolley() => 
            _trolley.position = Vector3.Lerp(_TrolleyA.position, _TrolleyB.position, _tTrolley);
        
        private void MoveHook() => 
            _hook.position = Vector3.Lerp(_HookA.position, _HookB.position, _tHook);
        
        private void ApplyAllMoves()
        {
            MoveBridge();
            MoveTrolley();
            MoveHook();
        }

        void UpdateRopeMesh()
        {
            Vector3 a = _ropeTop.position;
            Vector3 b = _ropeBottom.position;
            Vector3 dir = (b - a);
            float len = dir.magnitude;
            if (len < 1e-6f) return;

            _rope.position = (a + b) * 0.5f; // центр между точками
            _rope.up = dir.normalized;       // ось Y цилиндра вдоль троса

            var s = _rope.localScale;
            s.y = len * _ropeScale;
            _rope.localScale = s;
        }
    }
}