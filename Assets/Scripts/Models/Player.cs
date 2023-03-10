using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private Entity _entity;

        private void Start()
        {
            _entity.SetBaseHealth(100);
            _entity.SetBaseDamage(10);
            _entity.Name = "Player";
        }
    }
}