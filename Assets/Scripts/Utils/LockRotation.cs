using UnityEngine;

namespace Utils
{
    public class LockRotation : MonoBehaviour
    {
        private Quaternion lockedRotation;

        private void Start()
        {
            lockedRotation = transform.rotation;
        }

        private void Update()
        {
            transform.rotation = lockedRotation;
        }
    }
}
