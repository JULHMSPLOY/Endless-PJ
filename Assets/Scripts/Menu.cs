using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject playButtonCanvas; 
    public MonoBehaviour MovementScript; 

    [Header("Audio Settings")]
    public AudioSource footstepSource; // *** เพิ่มช่องสำหรับลากตัว FootStep มาใส่ ***

    void Start()
    {
        Time.timeScale = 0f; 
        
        // ตรวจสอบว่าถ้ามีเสียงเดินเล่นอยู่ตอนเริ่ม (Play On Awake) ให้หยุดไว้ก่อน
        if (footstepSource != null && footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        if (playButtonCanvas != null)
        {
            playButtonCanvas.SetActive(false);
        }

        if (MovementScript != null)
        {
            MovementScript.enabled = true;
        }

        // *** เพิ่มคำสั่งให้เล่นเสียงเดินเมื่อกด Play ***
        if (footstepSource != null)
        {
            footstepSource.Play();
        }
    }
}