using Assets.Scripts.Controllers.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Controllers
{
    public class EnemyController : MonoBehaviour
    {
        public float lookRadius = 10f;
        public float minDistance = 2f;
        private NavMeshAgent agent;

        private Transform target;

        private EnemyAttackController enemyAttackController;

        private void Start()
        {
            target = PlayerManager.instance.player.transform; // See PlayerManager.cs for explanation
            agent = GetComponent<NavMeshAgent>();
            enemyAttackController = GetComponent<EnemyAttackController>();

            Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
        }

        private void Update()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            var distance = Vector3.Distance(target.position, transform.position);
            Vector3 enemyDirection = target.position - transform.position;

            if (distance > lookRadius)
                return;
            
            // Check if there is no wall in between the player and the enemy, if there is then return
            if (Physics.Raycast(transform.position, enemyDirection.normalized, out RaycastHit hit, distance, LayerMask.GetMask("Default")))
                if (hit.collider.tag != "Enemy")
                    return;
            
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                // Attack the player
                enemyAttackController.Attack();
                // Face the player
                FaceTarget();
            }

            foreach (GameObject enemy in enemies)
            {
                if (enemy != gameObject) // don't compare to itself
                {
                    // Checks distance between enemies
                    float enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);

                    if (enemyDistance < minDistance)
                    {
                        Vector3 direction = transform.position - enemy.transform.position;
                        direction.y = 0f; // don't move up/down
                        // Move enemies away from eachother so they don't collide
                        GetComponent<NavMeshAgent>().Move(direction.normalized * Time.deltaTime);
                    }
                }
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
