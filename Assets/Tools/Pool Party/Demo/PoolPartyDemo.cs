using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolPartyDemo : MonoBehaviour
{
    public Rigidbody prefab;
    public int prefabCount = 10;

    void Awake()
    {
        PoolParty.SetPoolSize(prefab, prefabCount);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SpawnItem();
    }

    void SpawnItem()
    {
        Rigidbody item = PoolParty.Instantiate(prefab, Random.insideUnitSphere, Random.rotation, transform);

        item.velocity = Random.insideUnitSphere * 10f;

        PoolParty.Destroy(item.gameObject, 2f);
        // PoolParty.Destroy(item, 2f); also works
    }
}