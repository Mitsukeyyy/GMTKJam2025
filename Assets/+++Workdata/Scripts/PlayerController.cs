using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed = 3.5f;
    [SerializeField] public float maxSpeed = 5f;
    [SerializeField] private float direction = 0f;
    [SerializeField] private float jumpForce = 20f;

    private bool canMove = true; 
    private Rigidbody2D rb;
    
    [SerializeField] Transform transformGroundCheck;
    private LayerMask layerGround;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        layerGround = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            {
                direction = 1;
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                JumpFunction();
            }
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        }
    }

     void JumpFunction()
    {
        if (Physics2D.OverlapCircle(transformGroundCheck.position, 0.3f, layerGround)) 
            rb.linearVelocity = new Vector2(0, jumpForce); 
        Debug.Log("Jump");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("dead");
        }
    }
    
}
