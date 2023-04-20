using System.Collections;
using Core.Enemy;
using StageGeneration.Rooms.RoomTypes;
using UnityEngine;
using UnityEngine.AI;

namespace Controllers.Enemy
{
    public class BossController : MonoBehaviour
    {
        [SerializeField]
        private float rotationSpeed;
        [SerializeField]
        private Material hitMaterial;

        private NavMeshAgent _agent;
        private Boss _boss;
        private BossRoom _bossRoom;
        private Transform _target;
        private Material _originalMaterial;
        private Renderer _renderer;

        private void Start()
        {
            _target = PlayerManager.Instance.player.transform;
            _agent = BossManager.Instance.boss.GetComponent<NavMeshAgent>();
            _boss = BossManager.Instance.boss.GetComponent<Boss>();
            _bossRoom = transform.root.gameObject.GetComponent<BossRoom>();
            _renderer = BossManager.Instance.boss.GetComponentInChildren<Renderer>();

            _originalMaterial = _renderer.material;
        }

        private void Update()
        {
            if (!_boss.GetStartFight()) return;

            var distance = Vector3.Distance(_target.position, transform.position);

            _agent.SetDestination(_target.position);

            if (distance <= _agent.stoppingDistance) FaceTarget();

            //Test boss hit
            // if (Input.GetKeyDown(KeyCode.H))
            // {
            //     StartCoroutine(HitFeedback());
            // }
        }

        private void FaceTarget()
        {
            var direction = (_target.position - transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        private IEnumerator HitFeedback() {
            _renderer.material = hitMaterial;
            yield return new WaitForSeconds(1f);
            _renderer.material = _originalMaterial;
        }

        private void OnCollisionEnter(Collision other)
        {
            //Hit on weapon or some logic needs to be implemented
            if (other.gameObject.tag == "Player") {
                StartCoroutine(HitFeedback());
            }
        }
    }
}
