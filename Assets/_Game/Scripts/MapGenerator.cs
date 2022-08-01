using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private MapData data;
    [SerializeField] private Building buildingPrefab;



    //private void Awake()
    //{
    //    int start = Mathf.RoundToInt(-data.size / 2f);
    //    int end = -start;

    //   int buildingCd = 0;

    //    for (int x = start; x < end; x++)
    //    {
    //        for (int y = start; y < end; y++)
    //        {
    //            if (buildingCd <= 0 && Random.Range(0f, 1f) < data.density)
    //            {
    //                //build
    //                buildingBuilt = 30;
    //            }
    //        }
    //    }
    //}
}

[System.Serializable]
public struct MapData
{
    public float size;
    public float minSpaceBetweenBuildings;
    public float maxSpaceBetweenBuildings;
    [Range(0f, 1f)]
    public float density;
}