using UnityEngine;

public class Note : MonoBehaviour
{
    
    public float noteSpeed = 400f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 노트가 중력 영향 안 받도록
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; 
    }

    void FixedUpdate() // 물리 업데이트 프레임에서 실행
    {
        // Rigidbody2D를 이용해 오른쪽으로 이동
        rb.MovePosition(rb.position + Vector2.right * noteSpeed * Time.fixedDeltaTime);
    }
}
