using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 2f;

    private Rigidbody2D rb;
    private ObjectPool ownerPool;
    private float despawnTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Time.time >= despawnTime)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.Die();
            }
            ReturnToPool();
        }
        else if (other.CompareTag("Ground"))
        {
            ReturnToPool();
        }
    }
    public void Launch(ObjectPool pool, float direction)
    {
        ownerPool = pool;
        rb.linearVelocity = new Vector2(direction * speed, 0f);
        despawnTime = Time.time + lifetime;
    }

    private void ReturnToPool()
    {
        rb.linearVelocity = Vector2.zero;
        ownerPool.Return(gameObject);
    }
}
