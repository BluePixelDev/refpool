using System.Collections;
using UnityEngine;

namespace BP.PoolIO
{
    public class PoolTest : MonoBehaviour
    {
        [SerializeField] private PoolResource poolResource;

        private void OnEnable()
        {
            StartCoroutine(Spawner());
        }

        IEnumerator Spawner()
        {
            while (true)
            {
                var obj = poolResource.Get();
                obj.transform.position = Random.insideUnitSphere * 5f;
                obj.SetActive(true);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
