using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rig;
    BoxCollider2D col;
    Animator anim;

    Camera cam;
    public GameObject fireball;
    public GameObject barrier;
    public Transform book;
    public GameObject part;
    public float speed;

    bool isGrounded = false;
    public LayerMask mask;
    public GameObject platform;

    public float bulletForce;
    public int fireMana;
    public int barrierMana;
    public int flyMana;

    [HideInInspector]
    public int hp, mana;

    public int hpRegen, manaRegen;
    public Vector2Int stats; // x for hp, y for mana
    public Image healthBar, manaBar;
    public Text healthTxt, manaTxt;

    float regenCounter;
    public float waitForRegen;

    public GameObject stoneImpact;

    float shootDelay = 0;
    public float maxShootDelay;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        cam = Camera.main;
        hp = stats.x;
        mana = stats.y;
    }

    void FixedUpdate()
    {
        // update stats
        regenCounter += Time.fixedDeltaTime;
        shootDelay += Time.fixedDeltaTime;

        if (hp <= 0) SceneManager.LoadScene(1);
        if (regenCounter >= waitForRegen)
        {
            regenCounter = 0;
            hp = Mathf.Clamp(hp + hpRegen, 0, stats.x);
            if (!isGrounded)
            {
                mana -= flyMana;
                if (mana < 0) SceneManager.LoadScene(1);
            } else mana = Mathf.Clamp(mana + manaRegen, 0, stats.y);
        }
        
        // input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // animations
        anim.SetFloat("h", h);
        anim.SetFloat("v", v);
        if (v == 0) anim.SetBool("isVertical", false);
        else anim.SetBool("isVertical", true);
        if (h == 0) anim.SetBool("isHorizontal", false);
        else anim.SetBool("isHorizontal", true);

        // movement
        Vector2 pos, mov;

        mov = new Vector2(h, v);
        mov.Normalize();
        mov *= speed * Time.fixedDeltaTime;

        pos = rig.position + mov;
        rig.position = pos;

        // fire
        if (Input.GetButton("Fire1") && mana >= fireMana && shootDelay >= maxShootDelay) Shoot();

        // barrier
        if (Input.GetButton("Fire2") && mana >= barrierMana && shootDelay >= maxShootDelay) DeployBarrier();

        // change terrain
        if (Input.GetKey("space") && shootDelay >= maxShootDelay)
        {
            shootDelay = 0;
            Vector2 dir = new Vector2(0, 0);
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleLeft")) dir = new Vector2(-1, 0);
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleRight")) dir = new Vector2(1, 0);
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleUp")) dir = new Vector2(0, 1);
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleDown")) dir = new Vector2(0, -1);

            if (dir != Vector2.zero && NearWater(dir))
            {
                GameObject g = Instantiate(part, rig.position + dir, Quaternion.identity);
                Destroy(g, 1f);

                col.enabled = false;
                rig.position += dir;
                col.enabled = true;
                if (isGrounded)
                {
                    platform.SetActive(true);
                    isGrounded = false;
                } else
                {
                    platform.SetActive(false);
                    isGrounded = true;
                }
            }
        }

        // update ui
        if (hp > 0 && mana > 0)
        {
            healthBar.fillAmount = (float)hp / (float)stats.x;
            manaBar.fillAmount = (float)mana / (float)stats.y;
            healthTxt.text = hp + " / " + stats.x;
            manaTxt.text = mana + " / " + stats.y;
        }
    }

    void Shoot()
    {
        mana -= fireMana;
        shootDelay = 0;

        Vector3 target = Input.mousePosition;
        target = cam.ScreenToWorldPoint(target);
        Vector2 route = target - book.position;
        route.Normalize();

        float angle = Vector2.Angle(Vector2.right, route);
        GameObject proj = Instantiate(fireball, book.position, Quaternion.Euler(0, 0, angle));
        proj.GetComponent<Rigidbody2D>().AddForce(route * bulletForce);
    }

    void DeployBarrier()
    {
        mana -= barrierMana;
        shootDelay = 0;

        Vector2 target = Input.mousePosition;
        target = cam.ScreenToWorldPoint(target);

        float rot = Vector2.SignedAngle(Vector2.up, rig.position - target);
        GameObject g = Instantiate(barrier, target, Quaternion.Euler(0, 0, rot));
        Destroy(g, 7f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            //Vector2 recieveDamage = collision.gameObject.GetComponent<SlimeAI>().damage;
            hp -= (int)UnityEngine.Random.Range(5, 10);
            if (collision.gameObject.name == "Rock(Clone)")
            {
                GameObject i = Instantiate(stoneImpact, collision.transform.position, Quaternion.identity);
                Destroy(i.gameObject, 1.5f);
                Destroy(collision.gameObject);
            }
        }
    }

    bool NearWater(Vector2 dir)
    {
        RaycastHit2D r;

        // left
        if (dir.x == -1 && dir.y == 0)
            r = Physics2D.Raycast(transform.position, Vector2.left, 1, mask);
        // right
        else if(dir.x == 1 && dir.y == 0) 
            r = Physics2D.Raycast(transform.position, Vector2.right, 1, mask);
        // up        
        else if(dir.x == 0 && dir.y == 1) 
            r = Physics2D.Raycast(transform.position, Vector2.up, 1, mask);
        // down
        else r = Physics2D.Raycast(transform.position, Vector2.down, 1, mask);

        return r.collider != null;
    }
}
