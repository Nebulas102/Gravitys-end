using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Gear", menuName = "ScriptableObjects/Gear")]
    public class Gear : ScriptableObject
    {
        // In percentage
        public float HealthModifier;

        // In percentage
        public float DamageModifier;
    }
}