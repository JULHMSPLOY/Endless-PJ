using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;

    [Header("Movement Settings")]
    public float forwardSpeed = 15.0f;    // วิ่งไปข้างหน้าอัตโนมัติ
    public float sideSpeed = 15.0f;       // ความเร็วเลี้ยวซ้าย-ขวา
    public float rotationSpeed = 30f;
    public float gravity = 80.0f;

    [Header("Jump & Slide Settings")]
    public float jumpHeight = 20.0f;
    public float jumpDuration = 1.0f;
    public float slideDuration = 1.0f;

    private Vector3 moveDirection = Vector3.zero;
    private bool m_Jumping;
    private bool m_Sliding;
    private float m_JumpTimer;
    private float m_SlideTimer;
    private float m_VerticalVelocity;

    private float originalHeight;
    private Vector3 originalCenter;

    void Start()
    {
        Time.timeScale = 1;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        if (characterController != null)
        {
            originalHeight = characterController.height;
            originalCenter = characterController.center;
        }

        // เริ่มต้นให้เล่นแอนิเมชันวิ่งทันที
        if (animator != null)
        {
            animator.SetBool("Run", true);
        }
    }

    void Update()
    {
        // 1. รับ Input แกนซ้าย-ขวา (A/D หรือปุ่มลูกศร)
        float horizontalInput = Input.GetAxis("Horizontal");

        // 2. เปลี่ยนปุ่ม Up/Down เป็นการกระโดดและสไลด์
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) Jump();
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) Slide();

        // 3. คำนวณการเคลื่อนที่ (วิ่งไปข้างหน้าตลอด + เลี้ยวซ้ายขวา)
        // x = ซ้ายขวา, z = วิ่งไปข้างหน้า
        moveDirection = new Vector3(horizontalInput * sideSpeed, 0, forwardSpeed);

        // 4. จัดการระบบกระโดดและสไลด์
        HandleJumpAndSlide();

        // 5. หมุนตัวละครเล็กน้อยตามทิศทางการเลี้ยว
        RotateTowardsMovement(horizontalInput);

        // 6. สั่งเคลื่อนที่ผ่าน CharacterController
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void RotateTowardsMovement(float horizontalInput)
    {
        float targetRotationY = horizontalInput * 15f; 
        Quaternion targetRotation = Quaternion.Euler(0, targetRotationY, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    void Jump()
    {
        // กระโดดได้เฉพาะตอนแตะพื้นเท่านั้น
        if (characterController.isGrounded && !m_Sliding)
        {
            m_Jumping = true;
            animator.SetTrigger("Jump");
        }
    }

    void Slide()
    {
        if (!m_Sliding)
        {
            if (m_Jumping) m_Jumping = false;
            m_SlideTimer = 0;
            m_Sliding = true;
            
            // เรียกใช้ Parameter "Slide" (Bool)
            animator.SetBool("Slide", true);

            // ย่อขนาด Collider
            characterController.height = originalHeight / 2;
            characterController.center = new Vector3(originalCenter.x, originalCenter.y / 2, originalCenter.z);
        }
    }

    void HandleJumpAndSlide()
    {
        if (characterController.isGrounded)
        {
            m_VerticalVelocity = -0.5f; 

            // เมื่อถึงพื้น: ปรับแอนิเมชันให้กลับไปวิ่ง (ปิดท่ากระโดด)
            animator.SetBool("Jump", false);

            if (m_Jumping)
            {
                m_VerticalVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
                m_Jumping = false;
            
                // เมื่อเริ่มกระโดด: สั่งให้แอนิเมชันกระโดดทำงาน
                animator.SetBool("Jump", true);
            }
        }
        else
        {
            // ขณะลอยอยู่: แรงโน้มถ่วงดึงลง
            m_VerticalVelocity -= gravity * Time.deltaTime;
        
            // มั่นใจว่าแอนิเมชันกระโดดจะยังค้างอยู่ถ้ายังไม่ถึงพื้น
            animator.SetBool("Jump", true);
        }

        moveDirection.y = m_VerticalVelocity;

        // ระบบ Slide เหมือนเดิม...
        if (m_Sliding)
        {
            m_SlideTimer += Time.deltaTime;
            if (m_SlideTimer >= slideDuration) StopSliding();
        }
    }
    void StopSliding()
    {
        if (m_Sliding)
        {
            m_Sliding = false;
            // ปิดแอนิเมชัน Slide กลับไปท่าวิ่ง
            animator.SetBool("Slide", false);
            
            characterController.height = originalHeight;
            characterController.center = originalCenter;
        }
    }
}