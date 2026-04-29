using UnityEngine;
using UnityEngine.AI; 

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    [Header("Warp Settings")]
    public float maxDistance = 60f;      // ระยะห่างสูงสุดที่ยอมให้ห่างกันได้
    public float warpBehindOffset = 15f; // ระยะที่จะให้วาร์ปมาอยู่ข้างหลังผู้เล่น (เมตร)

    void Update()
    {
        if (player != null)
        {
            // 1. ตรวจสอบระยะห่างระหว่างศัตรูกับผู้เล่น
            float distance = Vector3.Distance(transform.position, player.position);

            // 2. ถ้าห่างเกินไป ให้วาร์ปมาหา
            if (distance > maxDistance)
            {
                WarpToPlayer();
            }
            else
            {
                // 3. ถ้ายังอยู่ในระยะปกติ ให้เดินไล่ตามปกติ
                if (agent.enabled && agent.isOnNavMesh)
                {
                    agent.SetDestination(player.position);
                }
            }
        }
    }

    void WarpToPlayer()
    {
        // คำนวณหาจุด "ข้างหลัง" ของผู้เล่น (ถอยออกมาตามแนวที่ผู้เล่นหันหน้าไป)
        Vector3 warpPosition = player.position - (player.forward * warpBehindOffset);
        
        // สุ่มเลนซ้าย-ขวาเล็กน้อยเพื่อให้ดูเป็นธรรมชาติ (ไม่ซ้อนทับกัน)
        warpPosition += player.right * Random.Range(-3f, 3f);

        NavMeshHit hit;
        // ตรวจสอบตำแหน่งที่ใกล้เคียงบน NavMesh ในรัศมี 10 เมตร
        if (NavMesh.SamplePosition(warpPosition, out hit, 10f, NavMesh.AllAreas))
        {
            // ใช้ Warp แทนการตั้งค่า transform.position ตรงๆ เพื่อไม่ให้ NavMeshAgent พัง
            agent.Warp(hit.position);
            
            // รีเซ็ตความเร็วเดิม เพื่อไม่ให้วาร์ปมาแล้วพุ่งทะลุตัวผู้เล่น
            agent.velocity = Vector3.zero;
            
            Debug.Log("Enemy warped to stay close!");
        }
    }
}
