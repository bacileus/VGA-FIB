using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvilBirdAI : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rig;
    Rigidbody2D player;
    public float speed;
    public float visionRange, attackRange;
    bool isAttacking = false;
    bool incapable = false;
    Vector2 finalAttackPos;

    public float maxDelay;
    float delay;

    public int maxHp;
    [HideInInspector]
    public int hp;

    public Vector2Int damage;

    float c = 0;

    Text txt;

    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        txt = GameObject.FindGameObjectWithTag("Counter").GetComponent<Text>();
        hp = maxHp;
        delay = maxDelay;
    }


    void FixedUpdate()
    {
        if (hp <= 0)
        {
            GameObject.FindGameObjectWithTag("Creator").GetComponent<Spawner>().maxEnemies--;
            string s = GameObject.FindGameObjectWithTag("Creator").GetComponent<Spawner>().maxEnemies.ToString();
            txt.text = "Remaining enemies: " + s;
            Destroy(gameObject);
        }
        if (!incapable) {
            Vector2 dir = player.position - rig.position;
            float distance = Vector2.Distance(player.position, rig.position);
            delay += Time.fixedDeltaTime;

            if (distance < 2 && !isAttacking && delay < maxDelay) isAttacking = false;
            else if (distance <= attackRange && !isAttacking && delay >= maxDelay)
            {
                // start attack
                delay = 0;
                isAttacking = true;
                finalAttackPos = rig.position + ((attackRange - distance) * 1.5f + 1) * dir;
            }
            else if (isAttacking && Vector2.Distance(rig.position, finalAttackPos) < 0.5f)
            {
                // end attack
                isAttacking = false;
            }
            else if (isAttacking && rig.position != finalAttackPos)
            {
                // is attacking
                dir = finalAttackPos - rig.position;
                dir.Normalize();
                rig.position += dir * speed * 3 * Time.fixedDeltaTime;
            }
            else if (distance <= visionRange && !isAttacking)
            {
                // move
                isAttacking = false;
                rig.position += dir.normalized * speed * Time.fixedDeltaTime;
            }
        }
        else
        {
            c += Time.fixedDeltaTime;
            if (c >= 7) incapable = false;
        }

        // animations
        if (isAttacking) anim.speed = 3;
        else anim.speed = 1;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            player.gameObject.GetComponent<PlayerController>().hp -= Random.Range(damage.x, damage.y);
        if (collision.gameObject.layer == 11) incapable = true;
    }
}
