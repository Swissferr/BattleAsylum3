using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    public float duration;
    public float bulletSpeed;
    public bool right;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (right)
            rb.velocity = new Vector2(bulletSpeed, 0.0f);
        else
            rb.velocity = new Vector2(-bulletSpeed, 0.0f);

    }

    void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            Destroy(gameObject);
    }

    //void OnCollisionEnter2D(Collision2D coll)
    //{
    //    if (coll.gameObject.CompareTag("Player"))
    //        Destroy(gameObject);
    //}
}
