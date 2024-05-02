using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.TextCore.Text;

public class RubyController : MonoBehaviour

{
    public float speed = 3.0f;

    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public int health { get { return currentHealth;  } } // sækir heilsu property svo það má aðeins lesa hana

    public AudioClip throwSound;
    public AudioClip hitSound;

    int currentHealth;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0); // horfðu á stefnu Ruby til að gefa state machine stefnu

    public GameObject projectilePrefab;
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // sækir horizontal og vertical axes
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // geymir x og y hreyfingu
        Vector2 move = new Vector2(horizontal, vertical);

        // athugaðu hvort move.x eða move.y sé ekki jafnt og 0
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y); // move ruby
            lookDirection.Normalize(); 
        }

        // breytir animation og hraða út frá stefnu og hraða (lengd hreyfingar)
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // ef Ruby er invincible, mun gera hana svo í stuttan tíma
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }

        // hleypir skoti af stað þegar ýtt er á C takkann
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        // raycast hvert sem Ruby er að horfa þegar ýtt er á X
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                // sýnir glugga ef raycast rekst á NPC
                if (hit.collider != null)
                {
                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if (character != null)
                    {
                        character.DisplayDialog();
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        // færir karakterinn í 3 units 
        Vector2 position = transform.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        // breytir stöðu á rigidbody þannig að hann hættir að hreyfast ef hann rekst á
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        // athugaðu hvort breytingin sé minni en 0
        if (amount < 0)
        {
            // hættir kóða ef Ruby er nú þegar invincible
            if (isInvincible)
                return;

            // gerir rúbín invincible og stillir teljarann
            isInvincible = true;
            invincibleTimer = timeInvincible;

            // kveikir á „hit“ animation
            animator.SetTrigger("Hit");
            PlaySound(hitSound);
        }

        // tryggir að heilsan fari ekki undir 0 eða yfir maxHealth
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        // stillt gildi núverandi heilsu á health bar
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        // býr til afrit af skot og staðsetur hann við hendur Ruby's án þess að snúast
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        // fær skot úr projectile script og hleypir því af stað í þá átt sem Ruby snýr að og 300 krafti
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        PlaySound(throwSound);

    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
