using UnityEngine;

namespace BP.RefPool
{
    public class PoolSpawner : MonoBehaviour
    {
        [SerializeField] private PoolResource poolResource;
        [SerializeField] private bool initializeOnAwake;

        [Header("Spawn Settings")]
        [SerializeField] private bool autoSpawn = false;
        [SerializeField] private float spawnInterval = 1f;
        [SerializeField] private int spawnCountPerInterval = 1;

        [Header("Position")]
        [SerializeField] private Transform positionOverride;
        [SerializeField] private float randomRadius = 0f;

        private float spawnTimer;

        private void Awake()
        {
            if (initializeOnAwake)
            {
                poolResource.Initialize();
            }
        }

        private void Update()
        {
            if (!autoSpawn || poolResource == null) return;

            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                spawnTimer = 0f;
                for (int i = 0; i < spawnCountPerInterval; i++)
                {
                    Spawn();
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Vector3 center = positionOverride ? positionOverride.position : transform.position;
            Gizmos.DrawWireSphere(center, randomRadius);
        }

        public void Spawn()
        {
            if (!poolResource) return;

            var gameObj = poolResource.Get();
            if (!gameObj) return;

            Vector3 basePosition = positionOverride != null ? positionOverride.position : transform.position;
            Vector3 offset = randomRadius > 0 ? Random.insideUnitSphere * randomRadius : Vector3.zero;

            gameObj.transform.position = basePosition + offset;
            gameObj.SetActive(true);
        }
    }
}
