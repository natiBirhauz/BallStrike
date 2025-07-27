// using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]
// public abstract class BaseBall : MonoBehaviour, IBall
// {
//     protected EnemySpawner spawner;
//     protected GameManager gameManager;
//     public float forwardSpeed = 10f;
//     private bool initialized = false;

//     // Assign your explosion particle effect prefab in the Inspector
//     public GameObject explosionPrefab;

//     public virtual void Initialize(EnemySpawner spawner)
//     {
//         this.spawner = spawner;
//         this.gameManager = spawner.GetComponent<GameManager>();
//         initialized = true;
//     }

//     void FixedUpdate()
//     {
//         if (!initialized || spawner == null || spawner.player == null) return;

//         // Move the ball towards the player's starting direction
//         transform.position += Vector3.back * forwardSpeed * Time.deltaTime;

//         // Despawn if it goes too far or falls off
//         if (transform.position.y < -5f || transform.position.z < spawner.player.position.z - 20f)
//         {
//             Respawn();
//         }
//     }

//     // This is now virtual. Specific ball types will override this.
//     public virtual void Hit()
//     {
//         // Every ball will create an explosion and then respawn.
//         if (explosionPrefab != null)
//         {
//             Instantiate(explosionPrefab, transform.position, Quaternion.identity);
//         }
//         Respawn();
//     }

//     public void Respawn()
//     {
//         // Let the spawner know a ball needs to be replaced before destroying this one.
//         spawner?.ReplaceBall();
//         Destroy(gameObject);
//     }
// }