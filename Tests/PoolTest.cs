using BP.RefPool;
using System.Collections;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
    [SerializeField] private RefResource poolResource;

    private void OnEnable()
    {
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        while (true)
        {
            var refItem = poolResource.Get();
            refItem.transform.position = Random.insideUnitSphere * 5f;
            refItem.SetActive(true);
            yield return new WaitForSeconds(1f);
        }
    }
}

