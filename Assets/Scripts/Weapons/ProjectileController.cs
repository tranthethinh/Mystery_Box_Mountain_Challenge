using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BigRookGames.Weapons
{
    public class ProjectileController : MonoBehaviour
    {
        // --- Config ---
        public float speed = 200;
        public LayerMask collisionLayerMask;

        // --- Explosion VFX ---
        public GameObject rocketExplosion;

        // --- Projectile Mesh ---
        public MeshRenderer projectileMesh;

        // --- Script Variables ---
        private bool targetHit;

        // --- Audio ---
        public AudioSource inFlightAudioSource;

        // --- VFX ---
        public ParticleSystem disableOnHit;

        private GameManager gameManager;
        public int pointValue;
        private Rigidbody rb;
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        private void Update()
        {
            // --- Check to see if the target has been hit. We don't want to update the position if the target was hit ---
            if (targetHit) return;

            float searchDistance = 10f; // Adjust this value to define the search distance
            //find nearest S_Enemy or Enemy to look at it if it inside searchDistance
            GameObject nearestEnemy = FindNearestEnemyWithTag("Enemy", searchDistance);
            GameObject nearestS_Enemy = FindNearestEnemyWithTag("S_Enemy", searchDistance);
            GameObject targetEnemy = (nearestS_Enemy != null) ? nearestS_Enemy : nearestEnemy;
            if (targetEnemy != null)
            {
                // --- Calculate the direction towards the nearest enemy ---
                Vector3 targetPosition = targetEnemy.transform.position;
                targetPosition.y = targetEnemy.transform.position.y + 2;

                Vector3 direction = targetPosition - transform.position;
                direction.Normalize();

                // --- Move the game object towards the nearest enemy at the defined speed ---
                rb.AddForce(direction * speed, ForceMode.Force);
            }
            else if (targetEnemy == null)
            {
                transform.position += transform.forward * (speed * Time.deltaTime);
            }
        }

        private GameObject FindNearestEnemyWithTag(string tag, float maxDistance)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);

            GameObject nearestEnemy = null;
            float shortestDistance = Mathf.Infinity;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < maxDistance && distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestEnemy = enemy;
                }
            }

            return nearestEnemy;
        }



        /// <summary>
        /// Explodes on contact.
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            // --- return if not enabled because OnCollision is still called if compoenent is disabled ---
            if (!enabled) return;

            // --- Explode when hitting an object and disable the projectile mesh ---
            Explode();
            projectileMesh.enabled = false;
            targetHit = true;
            inFlightAudioSource.Stop();
            foreach(Collider col in GetComponents<Collider>())
            {
                col.enabled = false;
            }
            disableOnHit.Stop();


            // --- Destroy this object after 2 seconds. Using a delay because the particle system needs to finish ---
            Destroy(gameObject, 5f);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(collision.gameObject);
                gameManager.UpdateScore(1);
            }else if (collision.gameObject.CompareTag("S_Enemy"))
            {
                collision.gameObject.GetComponent<SliderController>().AttackAnimal(1);
            }
        }


        /// <summary>
        /// Instantiates an explode object.
        /// </summary>
        private void Explode()
        {
            // --- Instantiate new explosion option. I would recommend using an object pool ---
            GameObject newExplosion = Instantiate(rocketExplosion, transform.position, rocketExplosion.transform.rotation, null);


        }
    }
}