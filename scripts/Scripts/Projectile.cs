using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    // awake is called when instantiate is called
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // ey�ileggur game object �egar hann f�rist of langt � burtu fr� Ruby
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
        
    }

    public void Launch(Vector2 direction, float force)
    {
        // hreyf�u skotf�ri hvern ramma byggt � stefnu og krafti
        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // kalla � fix falli� �egar skotf�ri rekst � �vin
        EnemyController e = other.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            e.Fix();
        }

        // ey�ileggur skotf�ri vi� �rekstur
        Destroy(gameObject);
    }
}
