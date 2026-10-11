using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public static event Action<int> Killed;

    [SerializeField] private float speed = 2f;
    [SerializeField] private float patrolDistance = 3f;
    [SerializeField] private int points = 25;

    private Rigidbody2D rb;
    private float leftLimit;
    private float rightLimit;
    private float direction = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        leftLimit = transform.position.x - patrolDistance;
        rightLimit = transform.position.x + patrolDistance;
    }

    private void FixedUpdate()
    {
        if (rb.position.x >= rightLimit)
        {
            direction = -1f;
        }
        else if (rb.position.x <= leftLimit)
        {
            direction = 1f;
        }

        rb.linearVelocity = new Vector2(direction * speed, 0f);
    }
    public void Die()
    {
        Killed?.Invoke(points);
        gameObject.SetActive(false);
    }
}
