using Player;
using UnityEngine;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private PlayerMotor _motor;
        private PlayerLook _look;
        public PlayerInput.OnFootActions onFootActions;
        private void Awake()
        {
            _playerInput = new PlayerInput();
            onFootActions = _playerInput.OnFoot;
        
            _motor = GetComponent<PlayerMotor>();
            _look = GetComponent<PlayerLook>();
        
            onFootActions.Crouch.performed += ctx => _motor.Crouch();
            onFootActions.Crouch.canceled += ctx => _motor.Crouch();
            onFootActions.Sprint.performed += ctx => _motor.Sprint();
        }
        private void FixedUpdate()
        {
            _motor.ProcessMove(onFootActions.Movement.ReadValue<Vector2>());
            _look.ProcessLook(onFootActions.Look.ReadValue<Vector2>());

            onFootActions.Jump.performed += ctx => _motor.Jump();
        }

        private void OnEnable()
        {
            onFootActions.Enable();
        }

        private void OnDisable()
        {
            onFootActions.Enable();
        }
    }
}
