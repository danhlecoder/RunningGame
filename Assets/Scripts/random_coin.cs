using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab; // Prefab đồng xu
    public Transform player;      // Transform của nhân vật
    public float minSpawnDistance = 50f;
    public float maxSpawnDistance = 150f;
    public float laneWidth = 5.0f;
    public float coinHeight = 1.0f;
    public int maxCoinsPerLane = 10;

    private int[] lanes = { -1, 0, 1 }; // Ba làn đường (-1: trái, 0: giữa, 1: phải)
    private List<GameObject> activeCoins = new List<GameObject>();

    void Update()
    {
        SpawnCoinsIfNeeded();
        RemoveDistantCoins();
    }

    void SpawnCoinsIfNeeded()
    {
        foreach (int lane in lanes)
        {
            int coinsInLane = CountActiveCoinsInLane(lane);
            if (coinsInLane < maxCoinsPerLane)
            {
                int coinsToSpawn = Random.Range(5, 11); // Sinh ngẫu nhiên từ 5 đến 11 đồng xu
                float startZ = player.position.z + Random.Range(minSpawnDistance, maxSpawnDistance);

                for (int i = 0; i < coinsToSpawn; i++)
                {
                    float spawnZ = startZ + i * 1f; // Khoảng cách giữa mỗi đồng xu
                    Vector3 spawnPosition = new Vector3(lane * laneWidth, coinHeight, spawnZ);
                    GameObject coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);

                    if (coin != null)
                    {
                        // Kiểm tra và thêm CapsuleCollider nếu thiếu
                        if (coin.GetComponent<CapsuleCollider>() == null)
                        {
                            CapsuleCollider collider = coin.AddComponent<CapsuleCollider>();
                            collider.isTrigger = true;  // Đặt Collider thành trigger
                            collider.center = Vector3.zero; // Tùy chỉnh vị trí collider (nếu cần)
                            collider.height = 1f; // Chiều cao mặc định
                            collider.radius = 0.5f; // Bán kính mặc định
                        }

                        activeCoins.Add(coin);
                    }
                }
            }
        }
    }

    int CountActiveCoinsInLane(int lane)
    {
        int count = 0;
        foreach (GameObject coin in activeCoins)
        {
            if (coin != null && Mathf.RoundToInt(coin.transform.position.x / laneWidth) == lane)
            {
                count++;
            }
        }
        return count;
    }

    void RemoveDistantCoins()
    {
        for (int i = activeCoins.Count - 1; i >= 0; i--)
        {
            GameObject coin = activeCoins[i];
            if (coin != null && coin.transform.position.z < player.position.z - 10f)
            {
                activeCoins.RemoveAt(i);
                Destroy(coin);
            }
        }
    }
}
