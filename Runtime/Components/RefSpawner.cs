using System;
using UnityEngine;
using UnityEngine.Events;

namespace BP.RefPool
{
    public class RefSpawner : MonoBehaviour
    {
        [Header("Pool")]
        [SerializeField] private RefResource refResource;
        [SerializeField] private bool initializeOnAwake;

        [Header("Spawning")]
        [SerializeField] private bool autoSpawn = false;
        [SerializeField, Min(0)] private float spawnInterval = 1f;
        [SerializeField, Min(0)] private int spawnCountPerInterval = 1;

        private float spawnTimer;

        [SerializeField] private UnityEvent<RefItem> OnSpawn;
        public event Action<RefItem> SpawnedObject;

        private void OnValidate()
        {
            spawnInterval = RefUtils.ClampMin(spawnInterval, 0);
            spawnCountPerInterval = RefUtils.ClampMin(spawnCountPerInterval, 0);
        }

        private void Awake()
        {
            if (initializeOnAwake)
            {
                if (refResource == null)
                {
                    Debug.LogWarning($"{name}: No pool resource assigned for initialization.", this);
                    return;
                }

                refResource.Initialize();
            }
        }

        private void Update()
        {
            if (!autoSpawn || refResource == null)
                return;

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

        public void Spawn()
        {
            if (!refResource) return;

            var refItem = refResource.Get();
            if (!refItem) return;

            refItem.SetActive(true);
            refItem.transform.SetPositionAndRotation(transform.position, transform.rotation);

            OnSpawn?.Invoke(refItem);
            SpawnedObject?.Invoke(refItem);
        }
    }
}
