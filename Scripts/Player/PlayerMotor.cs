using UnityEngine;

namespace Player
{
    public class PlayerMotor : MonoBehaviour
    {
        private CharacterController _characterController;
        private Vector3 _playerVelocity;
        [SerializeField] private bool isGrounded;
        private bool _isCrouching;
        private bool _lerpCrouching;
        private bool _isSprinting;

        public float speed = 4f;
        public float gravity = -9.8f;
        public float jumpHeight = .5f;
        public float crouchTimer;
    
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        private void Update()
        {
            isGrounded = _characterController.isGrounded;

            if (!_lerpCrouching) return;
            crouchTimer += Time.deltaTime;
            float i = crouchTimer / 1;
            i *= i;
            _characterController.height = Mathf.Lerp(_characterController.height, _isCrouching ? 1f : 2f, i);

            if (!(i > 1)) return;
            _lerpCrouching = false;
            crouchTimer = 0;
        }

        public void ProcessMove(Vector2 input)
        {
            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = input.x;
            moveDirection.z = input.y;
            _characterController.Move(transform.TransformDirection(moveDirection) * (speed * Time.deltaTime));
            _playerVelocity.y += gravity * Time.deltaTime;
            if (isGrounded && _playerVelocity.y < 0)
            {
                _playerVelocity.y = -2f;
            }
            _characterController.Move(_playerVelocity * Time.deltaTime);
        }
    

        public void Jump()
        {
            if (!isGrounded) return;
            _playerVelocity.y = Mathf.Sqrt(Mathf.Abs(jumpHeight * -3f * gravity));
        }

        public void Crouch()
        {
            _isCrouching = !_isCrouching;
            crouchTimer =  0;
            _lerpCrouching = true;
            speed = _isCrouching ? 2f : 4f;
        }

        public void Sprint()
        {
            _isSprinting = !_isSprinting;
            speed = _isSprinting ? 6f : 4f;
            if (!_isSprinting) return;
            _isCrouching = false;
        }
    }
}
