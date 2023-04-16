using Core.Enemy;
using StageGeneration.Rooms.RoomTypes;
using UnityEngine;
using UnityEngine.AI;

namespace Controllers.Enemy
{
    public class BossController : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Boss _boss;
        private BossRoom _bossRoom;
        private Transform _target;

        private void Start()
        {
            _target = PlayerManager.instance.player.transform;
            _agent = BossManager.Instance.boss.GetComponent<NavMeshAgent>();
            _boss = BossManager.Instance.boss.GetComponent<Boss>();
            _bossRoom = transform.root.gameObject.GetComponent<BossRoom>();
        }

        private void Update()
        {
            if (_boss.GetStartFight())
            {
                var distance = Vector3.Distance(_target.position, transform.position);

                _agent.SetDestination(_target.position);

                if (distance <= _agent.stoppingDistance) FaceTarget();
            }
        }

        private void FaceTarget()
        {
            var direction = (_target.position - transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
