using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StageGeneration.Rooms.RoomTypes;

public class BossController : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent agent;
    private Boss boss;
    private BossRoom bossRoom;

    private void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        boss = GetComponent<Boss>();
        bossRoom = GetComponent<BossRoom>();
    }

    private void Update()
    {
        if (boss.GetStartFight())
        {
            float distance = Vector3.Distance(target.position, transform.position);
                
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                FaceTarget();
            }
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
