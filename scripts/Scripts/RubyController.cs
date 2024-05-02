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
    public int health { get { return currentHealth;  } } // s�kir heilsu property svo �a� m� a�eins lesa hana

    public AudioClip throwSound;
    public AudioClip hitSound;

    int currentHealth;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0); // horf�u � stefnu Ruby til a� gefa state machine stefnu

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
        // s�kir horizontal og vertical axes
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // geymir x og y hreyfingu
        Vector2 move = new Vector2(horizontal, vertical);

        // athuga�u hvort move.x e�a move.y s� ekki jafnt og 0
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y); // move ruby
            lookDirection.Normalize(); 
        }

        // breytir animation og hra�a �t fr� stefnu og hra�a (lengd hreyfingar)
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // ef Ruby er invincible, mun gera hana svo � stuttan t�ma
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }

        // hleypir skoti af sta� �egar �tt er � C takkann
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        // raycast hvert sem Ruby er a� horfa �egar �tt er � X
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                // s�nir glugga ef raycast rekst � NPC
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
        // f�rir karakterinn � 3 units 
        Vector2 position = transform.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        // breytir st��u � rigidbody �annig a� hann h�ttir a� hreyfast ef hann rekst �
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        // athuga�u hvort breytingin s� minni en 0
        if (amount < 0)
        {
            // h�ttir k��a ef Ruby er n� �egar invincible
            if (isInvincible)
                return;

            // gerir r�b�n invincible og stillir teljarann
            isInvincible = true;
            invincibleTimer = timeInvincible;

            // kveikir � �hit� animation
            animator.SetTrigger("Hit");
            PlaySound(hitSound);
        }

        // tryggir a� heilsan fari ekki undir 0 e�a yfir maxHealth
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        // stillt gildi n�verandi heilsu � health bar
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        // b�r til afrit af skot og sta�setur hann vi� hendur Ruby's �n �ess a� sn�ast
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        // f�r skot �r projectile script og hleypir �v� af sta� � �� �tt sem Ruby sn�r a� og 300 krafti
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
