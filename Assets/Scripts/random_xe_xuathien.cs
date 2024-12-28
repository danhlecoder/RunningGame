using UnityEngine;
using System.Collections.Generic;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs; // Các mẫu xe (Prefabs)
    public Transform player;        // Nhân vật chính (Player)
    public float minSpawnDistance = 50f; // Khoảng cách tối thiểu giữa nhân vật và xe
    public float maxSpawnDistance = 150f; // Khoảng cách tối đa giữa nhân vật và xe
    public float laneWidth = 5.0f;  // Khoảng cách giữa các làn đường
    public int maxCars = 10;        // Số lượng xe tối đa có thể spawn

    private class LaneCarPositions
    {
        public List<float> forwardCars = new List<float>(); // Xe chạy tới
        public List<float> backwardCars = new List<float>(); // Xe chạy lui
    }
    private Dictionary<int, LaneCarPositions> laneCarPositions = new Dictionary<int, LaneCarPositions>();
    private List<GameObject> activeCars = new List<GameObject>(); // Danh sách các xe đang tồn tại
    private int[] lanes = { -1, 0, 1 }; // Ba làn đường (-1: trái, 0: giữa, 1: phải)

    void Start()
    {
        foreach (int lane in lanes)
        {
            laneCarPositions[lane] = new LaneCarPositions();
        }
    }

    void Update()
    {
        SpawnCarsIfNeeded();
        RemoveDistantCars();
    }

    void SpawnCarsIfNeeded()
    {
        int spawnAttempts = 0;

        while (activeCars.Count < maxCars && spawnAttempts < 50)
        {
            spawnAttempts++;

            int desiredLane = lanes[Random.Range(0, lanes.Length)];
            float spawnZ = player.position.z + Random.Range(minSpawnDistance, maxSpawnDistance);
            int direction = Random.value < 0.5f ? 1 : -1;

            if (CanSpawnCarInLane(desiredLane, spawnZ, direction))
            {
                Vector3 spawnPosition = new Vector3(desiredLane * laneWidth, 1.0f, spawnZ);
                GameObject carPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];
                GameObject car = Instantiate(carPrefab, spawnPosition, Quaternion.identity);

                // Gắn tốc độ ngẫu nhiên và hướng di chuyển
                CarMovement carMovement = car.AddComponent<CarMovement>();
                carMovement.moveSpeed = Random.Range(5f, 12f);
                carMovement.direction = direction;

                // Thêm Box Collider nếu chưa có
                if (car.GetComponent<BoxCollider>() == null)
                {
                    car.AddComponent<BoxCollider>();
                }

                // Gán tag là Obstacle
                car.tag = "Obstacle";

                activeCars.Add(car);

                if (direction == 1)
                    laneCarPositions[desiredLane].forwardCars.Add(spawnZ);
                else
                    laneCarPositions[desiredLane].backwardCars.Add(spawnZ);
            }
        }
    }


    bool CanSpawnCarInLane(int lane, float spawnZ, int direction)
    {
        LaneCarPositions laneCars = laneCarPositions[lane];

        if (direction == 1)
        {
            foreach (float z in laneCars.forwardCars)
            {
                if (Mathf.Abs(spawnZ - z) < 50f)
                    return false;
            }
            foreach (float z in laneCars.backwardCars)
            {
                if (Mathf.Abs(spawnZ - z) < 100f)
                    return false;
            }
        }
        else if (direction == -1)
        {
            foreach (float z in laneCars.backwardCars)
            {
                if (Mathf.Abs(spawnZ - z) < 50f)
                    return false;
            }
            foreach (float z in laneCars.forwardCars)
            {
                if (Mathf.Abs(spawnZ - z) < 100f)
                    return false;
            }
        }

        return true;
    }

    void RemoveDistantCars()
    {
        for (int i = activeCars.Count - 1; i >= 0; i--)
        {
            GameObject car = activeCars[i];

            if (car != null && car.transform.position.z < player.position.z - 20f)
            {
                int lane = Mathf.RoundToInt(car.transform.position.x / laneWidth);
                int direction = car.GetComponent<CarMovement>().direction;

                if (direction == 1)
                    laneCarPositions[lane].forwardCars.Remove(car.transform.position.z);
                else
                    laneCarPositions[lane].backwardCars.Remove(car.transform.position.z);

                Destroy(car);
                activeCars.RemoveAt(i);
            }
        }
    }
}
