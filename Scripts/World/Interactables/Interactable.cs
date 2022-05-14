using UnityEngine;

namespace World.Interactables
{
    public abstract class Interactable : MonoBehaviour
    {
        public string promptMessage;

        public void BaseInteract()
        {
            Interact();
        }
        protected virtual void Interact()
        {
        
        }
    }
}
