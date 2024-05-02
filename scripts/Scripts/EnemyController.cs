using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float speed = 3.0f;
    public bool vertical;
    public float changeTime = 3.0f;

    public ParticleSystem smokeEffect;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;

    Animator animator;

    bool broken = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // mun hætta í kóðanum ef óvinurinn er brotinn
        if (!broken)
        {
            return;
        }

        // telur niður
        timer -= Time.deltaTime;

        // þegar teljarinn er kominn undir 0 mun óvinurinn breyta um stefnu og teljarinn byrjar aftur
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {

        if (!broken)
        {
            return;
        }

        // láta óvininn fara fram og til baka
        Vector2 position = rigidbody2D.position;
        
        if(vertical)
        {
            // hreyfðu óvininn á y ásnum með því að nota float breytur á animator
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            // hreyfðu óvininn á x-ásinn
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2D.MovePosition(position);
    }

    // skaða Ruby við árekstur við óvininn
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        // hindrar óvininn
        broken = false;
        rigidbody2D.simulated = false;

        // spilaðu fasta animation og stöðva reykagnir
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        
        
    }
}
