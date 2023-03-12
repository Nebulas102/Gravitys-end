using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Gear", menuName = "ScriptableObjects/Gear")]
    public class Gear : ScriptableObject
    {
        public float HealthModifier;
        public float DamageModifier;
    }
}