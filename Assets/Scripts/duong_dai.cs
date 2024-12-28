using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class duong_dai : MonoBehaviour
{
    public GameObject[] titlePrefabs;
    public float zSpawn = 0;
    public float titleLength = 30;
    public int numberOfTitles = 5;
    private List<GameObject> activeTitle = new List<GameObject>();

    public Transform playerTransform;

    private float safeZone = 50f; // Khoảng cách an toàn trước khi xóa
    void Start()
    {
        for (int i = 0; i < numberOfTitles; i++)
        {
            if (i == 0)
                SpawnTile(0);
            else
                SpawnTile(Random.Range(0, titlePrefabs.Length));
        }
    }

    void Update()
    {
        if (playerTransform.position.z - safeZone > activeTitle[0].transform.position.z + titleLength)
        {
            SpawnTile(Random.Range(0, titlePrefabs.Length));
            DeleteTile();
        }
    }

    public void SpawnTile(int titleIndex)
    {
        GameObject go = Instantiate(titlePrefabs[titleIndex], Vector3.forward * zSpawn, Quaternion.identity);
        activeTitle.Add(go);
        zSpawn += titleLength;
    }

    private void DeleteTile()
    {
        Destroy(activeTitle[0]);
        activeTitle.RemoveAt(0);
    }
}
