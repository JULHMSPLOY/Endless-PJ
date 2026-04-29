using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation; // ต้องมีตัวนี้เพื่อจัดการ NavMesh แบบ Real-time

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;    // ใส่ Prefab พื้นที่มี NavMesh Surface & Link แล้ว
    public float tileLength = 100f;      // ความยาวแผ่นพื้น (ให้ตรงกับค่า Scale Z)
    public int numberOfTiles = 6;       // จำนวนแผ่นที่แสดงผลพร้อมกัน
    
    public Transform playerTransform;    // ลากตัวละคร Player มาใส่ที่นี่

    private List<GameObject> activeTiles = new List<GameObject>();
    private float zSpawn = 0;           // ตำแหน่งจุดเกิดแผ่นถัดไป

    void Start()
    {
        // สร้างแผ่นพื้นเริ่มต้นตอนเริ่มเกม
        for (int i = 0; i < numberOfTiles; i++)
        {
            // แผ่นแรก (i=0) ควรเป็นพื้นเรียบเสมอ เพื่อไม่ให้ AI หรือผู้เล่นตกใจ
            if (i == 0)
                SpawnTile(0); 
            else
                SpawnTile(Random.Range(0, tilePrefabs.Length));
        }
    }

    void Update()
    {
        // เช็คระยะห่าง: ถ้า Player วิ่งมาถึงจุดที่กำหนด ให้ทำลายแผ่นหลังและสร้างแผ่นหน้า
        // ระยะห่าง 1.5 - 2 เท่าของ tileLength กำลังดี เพื่อไม่ให้ผู้เล่นเห็นตอนแผ่นหายไป
        if (playerTransform.position.z - (tileLength * 1.5f) > zSpawn - (numberOfTiles * tileLength))
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            DeleteTile();
        }
    }

    public void SpawnTile(int tileIndex)
    {
        // สร้างแผ่นใหม่ตามพิกัด zSpawn
        GameObject go = Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);

        // เก็บไว้ใน List เพื่อรอการทำลาย
        activeTiles.Add(go);
        
        // ขยับพิกัดการเกิดแผ่นถัดไป
        zSpawn += tileLength;
    }

    private void DeleteTile()
    {
        // ลบแผ่นที่อยู่หลังสุด (แผ่นที่ 0 ใน List) ออกจากฉากและหน่วยความจำ
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}