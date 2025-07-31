using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public float speed = 3.5f;
    [SerializeField] public float jumpForce = 5f;
    private float direction = 1f;
    Rigidbody2D rb;

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
        rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);

        if (Physics2D.OverlapCircle(transformGroundCheck.position, 2.5f, layerGround))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}