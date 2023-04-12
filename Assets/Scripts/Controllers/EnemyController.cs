using UnityEngine;
using UnityEngine.AI;

namespace Controllers
{
    public class EnemyController : MonoBehaviour
    {
        public float lookRadius = 10f;
        private NavMeshAgent agent;

        private Transform target;

        private void Start()
        {
            target = PlayerManager.instance.player.transform; // See PlayerManager.cs for explanation
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            var distance = Vector3.Distance(target.position, transform.position);

            if (distance > lookRadius)
                return;

            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                // Attack the player
                // Face the player
                FaceTarget();
            }
        }

        // Draws a sphere around the enemy to visualize the range of where the enemy will start chasing you
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);
        }

        // When the player is too close to the enemy, it wont rotate anymore
        // This function fixes it

        private void FaceTarget()
        {
            var direction = (target.position - transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            // Use Quaternion.Slerp instead of lookRotation to smooth out the animation
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
