using UnityEngine;

namespace BP.RefPool
{
    [AddComponentMenu("RefPool/Ref Placer")]
    [RequireComponent(typeof(RefSpawner))]
    public class RefPlacer : MonoBehaviour
    {
        public enum PlacementMode
        {
            None = 0,
            Circle = 1,
            Sphere = 2,
            Cube = 3,
        }

        [Header("Position")]
        [SerializeField] private PlacementMode placementMode = PlacementMode.Sphere;
        [SerializeField] private Transform positionOverride;
        [SerializeField, Min(0)] private float randomPlacementRadius = 0f;

        [Header("Rotation")]
        [SerializeField] private bool randomYRotation = false;
        [SerializeField] private Quaternion fixedRotation = Quaternion.identity;
        [SerializeField] private bool useFixedRotation = false;

        private RefSpawner spawner;

        private void Awake()
        {
            spawner = GetComponent<RefSpawner>();
            spawner.SpawnedObject += Spawner_SpawnedObject;
        }
        private void OnDestroy()
        {
            spawner.SpawnedObject -= Spawner_SpawnedObject;
        }

        private void OnValidate()
        {
            randomPlacementRadius = RefUtils.ClampMin(randomPlacementRadius, 0);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Vector3 center = positionOverride ? positionOverride.position : transform.position;
            switch (placementMode)
            {
                case PlacementMode.Circle:
                    Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.identity, new Vector3(1, 1, float.Epsilon));
                    Gizmos.DrawWireSphere(Vector3.zero, randomPlacementRadius);
                    Gizmos.matrix = Matrix4x4.identity;
                    break;
                case PlacementMode.Sphere:
                    Gizmos.DrawWireSphere(center, randomPlacementRadius);
                    break;
                case PlacementMode.Cube:
                    Gizmos.DrawWireCube(center, Vector3.one * randomPlacementRadius);
                    break;
            }
        }
        private void Spawner_SpawnedObject(RefItem item)
        {
            item.transform.SetPositionAndRotation(GetSpawnPosition(), GetSpawnRotation());
        }

        private Vector3 GetSpawnPosition()
        {
            Vector3 overridePosition = positionOverride ? positionOverride.position : transform.position;
            var offset = new Vector3();
            switch (placementMode)
            {
                case PlacementMode.Circle:
                    offset = Random.insideUnitCircle * randomPlacementRadius;
                    break;
                case PlacementMode.Sphere:
                    offset = Random.insideUnitSphere * randomPlacementRadius;
                    break;
                case PlacementMode.Cube:
                    var halfRadius = randomPlacementRadius / 2;
                    offset = new Vector3(
                        Random.Range(-halfRadius, halfRadius),
                        Random.Range(-halfRadius, halfRadius),
                        Random.Range(-halfRadius, halfRadius)
                    );
                    break;
            }
            return overridePosition + offset;
        }


        private Quaternion GetSpawnRotation()
        {
            if (randomYRotation)
                return Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            return useFixedRotation ? fixedRotation : Quaternion.identity;
        }
    }
}
