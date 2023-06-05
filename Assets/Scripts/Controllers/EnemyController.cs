using System.Collections;
using Controllers.Enemy;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

namespace Controllers
{
    public class EnemyController : MonoBehaviour
    {
        public float lookRadius = 10f;
        public float retreatDistance = 2f;
        public Material hitMaterial;
        public LayerMask obstacleMask;

        [HideInInspector]
        public NavMeshAgent agent;

        private Material _originalMaterial;

        [HideInInspector]
        public Transform target;

        private Renderer renderer;
        private GameObject[] enemies;

        private BTree behaviorTree;

        private void Start()
        {   
            behaviorTree = GetComponent<BTree>();

            // See PlayerManager.cs for explanation
            target = PlayerManager.Instance.player.transform;
            agent = GetComponent<NavMeshAgent>();
            renderer = GetComponentInChildren<Renderer>();
            enemies = GameObject.FindGameObjectsWithTag("Enemy");

            _originalMaterial = renderer.material;

            var o = gameObject;
            Physics.IgnoreLayerCollision(o.layer, o.layer);

            behaviorTree.SetTree();
        }

        private void Update()
        {
            var targetPosition = target.position;
            var transformPosition = transform.position;
            var distance = Vector3.Distance(targetPosition, transformPosition);
            var enemyDirection = targetPosition - transformPosition;

            if (distance > lookRadius)
            {
                return;
            }

            // Check if there is no wall in between the player and the enemy, if there is then return
            if (Physics.Raycast(transform.position, enemyDirection.normalized, out var hit, distance,
                    LayerMask.GetMask("Entity")))
            {
                if (!hit.collider.CompareTag("Enemy"))
                    return;
            }

            // Face the player
            FaceTarget();

            if (distance <= agent.stoppingDistance)
            {
                // If melee enemy
                if (gameObject.GetComponent<EnemyMeleeAttackController>() != null)
                {
                    gameObject.GetComponent<EnemyMeleeAttackController>().Attack();
                }
            }

            foreach (var enemy in enemies)
                if (enemy != gameObject) // don't compare to itself
                {
                    // Checks distance between enemies
                    var enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);

                    if (!(enemyDistance < retreatDistance)) continue;

                    var direction = transform.position - enemy.transform.position;
                    direction.y = 0f; // don't move up/down
                    // Move enemies away from eachother so they don't collide
                    GetComponent<NavMeshAgent>().Move(direction.normalized * Time.deltaTime);
                }
        }

        private void OnTriggerEnter(Collider other)
        {
            //Hit on weapon or some logic needs to be implemented, this is bad
            if (other.gameObject.CompareTag("Item")) StartCoroutine(HitFeedback());
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
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 4f);
        }

        private IEnumerator HitFeedback()
        {
            renderer.material = hitMaterial;
            yield return new WaitForSeconds(.1f);
            renderer.material = _originalMaterial;
        }
    }
}
