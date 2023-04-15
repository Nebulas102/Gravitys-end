using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player")]
    public class Player : ScriptableObject
    {
        public Entity entity;
        public float time;
    }
}