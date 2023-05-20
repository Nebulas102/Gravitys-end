using System.Collections;
using Controllers.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Controllers
{
    public class EnemyController : MonoBehaviour
    {
        public float lookRadius = 10f;
        public float minDistance = 2f;
        public Material hitMaterial;

        private NavMeshAgent _agent;

        private Material _originalMaterial;

        private Transform _target;
        private Renderer renderer;

        private void Start()
        {
            // See PlayerManager.cs for explanation
            _target = PlayerManager.Instance.player.transform;
            _agent = GetComponent<NavMeshAgent>();
            renderer = GetComponentInChildren<Renderer>();

            _originalMaterial = renderer.material;

            var o = gameObject;
            Physics.IgnoreLayerCollision(o.layer, o.layer);
        }

        private void Update()
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");

            var targetPosition = _target.position;
            var transformPosition = transform.position;
            var distance = Vector3.Distance(targetPosition, transformPosition);
            var enemyDirection = targetPosition - transformPosition;

            //test feedback hit
            // if (Input.GetKeyDown(KeyCode.H))
            // {
            //     StartCoroutine(HitFeedback());
            // }

            if (distance > lookRadius)
                return;

            // Check if there is no wall in between the player and the enemy, if there is then return
            if (Physics.Raycast(transform.position, enemyDirection.normalized, out var hit, distance,
                    LayerMask.GetMask("Entity")))
            {
                if (!hit.collider.CompareTag("Enemy"))
                    return;
            }

            if (distance < minDistance)
            {
                Vector3 retreatDestination = transform.position + (transform.position - targetPosition).normalized * 4;
                _agent.SetDestination(retreatDestination);
                Debug.Log("Retreating");
            } else {
                _agent.SetDestination(_target.position);
            }

            if (distance <= _agent.stoppingDistance)
            {
                // If melee enemy
                if (gameObject.GetComponent<EnemyMeleeAttackController>() != null)
                {
                    gameObject.GetComponent<EnemyMeleeAttackController>().Attack();
                }

                // Face the player
                FaceTarget();
            }

            foreach (var enemy in enemies)
                if (enemy != gameObject) // don't compare to itself
                {
                    // Checks distance between enemies
                    var enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);

                    if (!(enemyDistance < minDistance)) continue;

                    var direction = transform.position - enemy.transform.position;
                    direction.y = 0f; // don't move up/down
                    // Move enemies away from eachother so they don't collide
                    GetComponent<NavMeshAgent>().Move(direction.normalized * Time.deltaTime);
                }
        }

        private void OnTriggerEnter(Collider other)
        {
            //Hit on weapon or some logic needs to be implemented
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
            var direction = (_target.position - transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            // Use Quaternion.Slerp instead of lookRotation to smooth out the animation
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * .5f);
        }

        private IEnumerator HitFeedback()
        {
            renderer.material = hitMaterial;
            yield return new WaitForSeconds(.1f);
            renderer.material = _originalMaterial;
        }
    }
}
