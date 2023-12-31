using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project
{
    public class MovementController : NetworkBehaviour
    {
        [SerializeField] private Transform _character;
        
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _groundLayerMask;

        [SerializeField] private float _lerpTime;

        private Coroutine _movementLerpCoroutine;
        private Coroutine _rotationLerpCoroutine;

        private void Start()
        {
            _camera = Camera.main;
        }


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            enabled = IsOwner;
        }


        private void OnEnable()
        {
            InputManager.instance.onMouseButton0.started += TryGoTo;
        }
        
        private void OnDisable()
        {
            InputManager.instance.onMouseButton0.started -= TryGoTo;
        }


        private void TryGoTo(InputAction.CallbackContext _)
        {
            if (Utilities.GetMouseWorldPosition(_camera, _groundLayerMask, out Vector3 position))
            {
                GoToServerRpc(position);
            }
        }

        [ServerRpc]
        private void GoToServerRpc(Vector3 position)
        {
            if (_movementLerpCoroutine != null) StopCoroutine(_movementLerpCoroutine); 
            if (_rotationLerpCoroutine != null) StopCoroutine(_rotationLerpCoroutine); 
            
            _movementLerpCoroutine = StartCoroutine(Utilities.LerpInTimeCoroutine(_lerpTime, _character.position, position, value =>
            {
                _character.position = value;
            }));
            _rotationLerpCoroutine = StartCoroutine(Utilities.LerpInTimeCoroutine(_lerpTime, _character.rotation, Quaternion.LookRotation(position - _character.position), value =>
            {
                _character.rotation = value;
            }));
        }
    }
}
