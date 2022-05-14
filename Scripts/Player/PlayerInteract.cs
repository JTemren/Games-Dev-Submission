using System;
using Input;
using UnityEngine;
using World.Interactables;

namespace Player
{
    public class PlayerInteract : MonoBehaviour
    {
        private Camera _camera;
        private InputManager _inputManager;
        [SerializeField] private PlayerUI playerUI;
        [SerializeField] private LayerMask mask;

        [SerializeField] private float distance = 3f;
        // Start is called before the first frame update
        private void Start()
        {
            _camera = GetComponent<PlayerLook>().camera;
            playerUI = GetComponent<PlayerUI>();
            _inputManager = GetComponent<InputManager>();
        }

        // Update is called once per frame
        private void Update()
        {
            playerUI.UpdateText(String.Empty);
            var transform1 = _camera.transform;
            Ray ray = new Ray(transform1.position, transform1.forward);
            Debug.DrawRay(ray.origin,ray.direction * distance, Color.red);
            RaycastHit hitInfo;
            if (!Physics.Raycast(ray, out hitInfo, distance, mask)) return;
            if (hitInfo.collider.GetComponent<Interactable>() == null) return;
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
            playerUI.UpdateText(interactable.promptMessage);
            if (_inputManager.onFootActions.Interact.triggered)
            {
                interactable.BaseInteract();
            }
        }
    }
}
