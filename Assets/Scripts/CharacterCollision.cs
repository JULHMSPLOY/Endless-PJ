using UnityEngine;
using TMPro; // *** ต้องเพิ่มบรรทัดนี้ เพื่อให้ใช้ตัวอักษร UI ได้ ***

public class CharacterCollision : MonoBehaviour
{
    public int score = 0;

    [Header("UI Settings")]
    public TextMeshProUGUI scoreText; // *** ช่องสำหรับลาก ScoreText ในหน้าจอมาใส่ ***

    [Header("Audio Sources from AudioManager")]
    public AudioSource Sound;
    public AudioSource footstepSource;
    public AudioSource coinSource;
    public AudioSource gameOverSource;

    private void Start()
    {
        UpdateScoreUI(); // ตั้งค่าหน้าจอให้แสดง Score: 0 ตั้งแต่เริ่มเกม
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. ชนสิ่งกีดขวาง (Tag: Enemy)
        if (other.CompareTag("Enemy"))
        {
            if (gameOverSource != null) gameOverSource.Play();
            if (Sound != null) Sound.Stop();
            if (footstepSource != null) footstepSource.Stop();

            Debug.Log("Game Over!");
            Invoke("EndGame", 0.1f); 
        }
        
        // 2. เก็บไอเทม (Tag: Item)
        else if (other.CompareTag("Item"))
        {
            if (coinSource != null) coinSource.PlayOneShot(coinSource.clip);

            score += 1;
            UpdateScoreUI(); // *** สั่งให้อัปเดตตัวเลขบนหน้าจอ ***
            
            Debug.Log("Score: " + score);
            Destroy(other.gameObject);
        }
    }

    // ฟังก์ชันสำหรับอัปเดตข้อความบนหน้าจอ
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score; // แสดงผลเป็น "Score: 1"
        }
    }

    void EndGame()
    {
        Time.timeScale = 0f;
    }
}