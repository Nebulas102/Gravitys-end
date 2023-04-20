using UnityEngine;

namespace StageGeneration
{
    public class FogOfWar : MonoBehaviour
    {
        [SerializeField]
        private Material transparent;

        [SerializeField]
        private Transform fog;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            fog.GetComponent<Renderer>().material = transparent;
        }
    }
}
