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
        // eyðileggur game object þegar hann færist of langt í burtu frá Ruby
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
        
    }

    public void Launch(Vector2 direction, float force)
    {
        // hreyfðu skotfæri hvern ramma byggt á stefnu og krafti
        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // kalla á fix fallið þegar skotfæri rekst á óvin
        EnemyController e = other.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            e.Fix();
        }

        // eyðileggur skotfæri við árekstur
        Destroy(gameObject);
    }
}
