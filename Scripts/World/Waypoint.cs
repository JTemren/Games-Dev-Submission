using UnityEngine;

namespace World
{
    public class Waypoint : MonoBehaviour
    {
        public virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }
}
