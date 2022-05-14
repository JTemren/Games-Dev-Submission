using UnityEngine;

namespace Player
{
    public class PlayerLook : MonoBehaviour
    {
        public new Camera camera;
        private float _xRot = 0f;

        public float xSensitivity = 30f;
        public float ySensitivity = 30f;
        // Start is called before the first frame update

        public void ProcessLook(Vector3 input)
        {
            float mouseX = input.x;
            float mouseY = input.y;

            _xRot -= (mouseY * Time.deltaTime) * ySensitivity;
            _xRot = Mathf.Clamp(_xRot, -80f, 80f);

            camera.transform.localRotation = Quaternion.Euler(_xRot, 0, 0);
            transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
        }
    }
}
