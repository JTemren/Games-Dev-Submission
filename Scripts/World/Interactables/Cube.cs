using UnityEngine;

namespace World.Interactables
{
    public class Cube : Interactable
    {
        protected override void Interact()
        {
            Debug.Log("WOW. its a " + gameObject.name + "!");
        }
    }
}
