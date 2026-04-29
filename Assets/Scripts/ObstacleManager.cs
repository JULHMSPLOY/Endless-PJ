using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject[] items; 
    public bool isCollectible; // ติ๊กถูกใน Inspector ถ้าก้อนนี้คือของที่เก็บได้
    
    public void PickRandomItem()
    {
        if (items.Length == 0) return;

        foreach (GameObject obj in items) obj.SetActive(false);

        int randomIndex = Random.Range(0, items.Length);
        GameObject selected = items[randomIndex];
        selected.SetActive(true);
        
        // กำหนด Tag ตามประเภทที่ตั้งไว้ใน Inspector
        selected.tag = isCollectible ? "Item" : "Enemy";

        // เพิ่มเติม: ตรวจสอบว่ามี Collider และตั้งเป็น Is Trigger หรือยัง
        if (selected.GetComponent<Collider>() != null)
        {
            selected.GetComponent<Collider>().isTrigger = true;
        }
    }
}