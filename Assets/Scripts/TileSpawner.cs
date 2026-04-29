using UnityEngine;
using System.Collections.Generic; // สำหรับใช้งาน List

public class TileSpawner : MonoBehaviour
{
    public GameObject tileCPrefab; // Prefab สิ่งกีดขวาง
    public GameObject tileDPrefab; // Prefab ไอเทม

    public Transform[] spawnPoints; 

    [Header("Spawn Settings")]
    [Range(0, 100)] public int obstacleChance = 100; 
    public int maxObstaclesPerTile = 2; // ปรับเพิ่มลดความยากได้ตรงนี้

    [Header("Item Row Settings")]
    public int itemsInRow = 5; 
    public float spaceBetweenItems = 5.0f; 

    void Start()
    {
        // ตัดการเช็คระยะ Z ออก เพื่อให้สปอนทันทีที่แผ่นพื้นถูกสร้าง
        if (spawnPoints.Length < 2) 
        {
            Debug.LogError("ควรมี SpawnPoints อย่างน้อย 2 จุดเพื่อป้องกันของสปอนทับกัน!");
            return;
        }

        SpawnEverything();
    }

    void SpawnEverything()
    {
        // สร้างลิสต์จุดเกิดเพื่อป้องกันการสปอนทับที่กัน
        List<int> availablePoints = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++) availablePoints.Add(i);

        // --- สุ่มสิ่งกีดขวางตามจำนวน maxObstaclesPerTile ---
        for (int i = 0; i < maxObstaclesPerTile; i++)
        {
            if (availablePoints.Count == 0) break;

            if (Random.Range(0, 100) < obstacleChance)
            {
                int randomIndex = Random.Range(0, availablePoints.Count);
                int pointIndex = availablePoints[randomIndex];

                SpawnObject(tileCPrefab, spawnPoints[pointIndex].position);
                availablePoints.RemoveAt(randomIndex); // ลบจุดที่ใช้แล้วออก
            }
        }

        // --- สุ่มไอเทมในจุดที่เหลือ ---
        if (availablePoints.Count > 0)
        {
            int itemPointIndex = availablePoints[Random.Range(0, availablePoints.Count)];
            Vector3 startPos = spawnPoints[itemPointIndex].position;

            for (int i = 0; i < itemsInRow; i++)
            {
                Vector3 spawnPos = startPos + new Vector3(0, 0, i * spaceBetweenItems);
                SpawnObject(tileDPrefab, spawnPos);
            }
        }
    }

    void SpawnObject(GameObject prefab, Vector3 position)
    {
        if (prefab == null) return;
        
        // ปรับความสูงเล็กน้อยเพื่อไม่ให้จมพื้น
        Vector3 finalPos = new Vector3(position.x, position.y + 0.5f, position.z);
        
        GameObject instance = Instantiate(prefab, finalPos, Quaternion.identity);
        instance.transform.SetParent(this.transform); 

        ObstacleManager manager = instance.GetComponent<ObstacleManager>();
        if (manager != null) manager.PickRandomItem();
    }
}