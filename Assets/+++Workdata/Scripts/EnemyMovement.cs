using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public float speed = 3.5f;

    private float direction = 1f;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(speed * direction,0);
    }
}
