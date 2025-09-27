using System;
using UnityEngine;
using UnityEngine.Events;

namespace BP.RefPool
{
    public class RefSpawner : MonoBehaviour
    {
        [SerializeField] private RefResource refResource;
        [SerializeField] private bool initOnAwake;

        [SerializeField] private bool autoSpawn = false;
        [SerializeField, Min(0)] private float spawnInterval = 1f;
        [SerializeField, Min(0)] private int spawnCount = 1;

        private float spawnTimer;

        [SerializeField] private UnityEvent<RefItem> OnSpawn;
        public event Action<RefItem> SpawnedObject;

        private void OnValidate()
        {
            spawnInterval = RefUtils.ClampMin(spawnInterval, 0);
            spawnCount = RefUtils.ClampMin(spawnCount, 0);
        }

        private void Awake()
        {
            if (initOnAwake)
            {
                if (refResource == null)
                {
                    Debug.LogWarning($"{name}: No pool resource assigned for initialization.", this);
                    return;
                }

                refResource.Prepare();
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
                for (int i = 0; i < spawnCount; i++)
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

            refItem.transform.SetPositionAndRotation(transform.position, transform.rotation);

            OnSpawn?.Invoke(refItem);
            SpawnedObject?.Invoke(refItem);
        }
    }
}
