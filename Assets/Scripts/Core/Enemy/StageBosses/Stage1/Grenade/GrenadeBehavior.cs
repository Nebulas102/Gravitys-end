using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy.StageBosses.Stage1
{
    public class GrenadeBehavior : MonoBehaviour
    {
        [SerializeField]
        private float throwDuration = 2f;
        [SerializeField]
        private float curveHeight = 5f;
        [SerializeField]
        private int grenadeDamage = 20;

        private float _throwStartTime = 0f;

        private GameObject _boss;
        private GameObject _player;
        private Vector3 _startPosition;
        private Quaternion _startRotation;

        private Vector3 _targetPosition;
        private Quaternion _targetRotation;
        
        private GameObject _decal;

        private void Start()
        {
            _boss = BossManager.Instance.boss;
            _player = PlayerManager.Instance.player;
            _startPosition = transform.position;
            _startRotation = transform.rotation;
            _targetPosition = _player.transform.position;
            _throwStartTime = Time.time;

            PlaceDecal();

            StartCoroutine(ThrowGrenade());
        }

        private IEnumerator ThrowGrenade()
        {
            while (Time.time - _throwStartTime < throwDuration)
            {
                float normalizedTime = (Time.time - _throwStartTime) / throwDuration;

                Vector3 currentPos = CalculateThrowPosition(normalizedTime);
                transform.position = currentPos;

                Quaternion currentRot = CalculateThrowRotation(normalizedTime);
                transform.rotation = currentRot;

                yield return null;
            }

            Destroy(gameObject);
            Destroy(_decal);
        }

        private Vector3 CalculateThrowPosition(float normalizedTime)
        {
            Vector3 currentPos = Vector3.Lerp(_startPosition, _targetPosition, normalizedTime);
            currentPos.y += Mathf.Sin(normalizedTime * Mathf.PI) * curveHeight;
            return currentPos;
        }

        private Quaternion CalculateThrowRotation(float normalizedTime)
        {
            return Quaternion.Slerp(_startRotation, _targetRotation, normalizedTime);
        }

        private void PlaceDecal()
        {
            Vector3 decalPos = new Vector3(_targetPosition.x, 0, _targetPosition.z);

            _decal.transform.position = decalPos;
        }

        public void SetDecal(GameObject decal)
        {
            _decal = decal;
        }
    }
}
