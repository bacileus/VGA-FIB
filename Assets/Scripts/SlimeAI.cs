using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeAI : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rig;
    Rigidbody2D player;
    public GameObject weapon;
    public float speed;
    public float attackRange;
    public float visionRange;
    public float maxAttackDelay;
    public Vector2 damage;
    public int maxHP;

    public int hp;
    float attackDelay = 0;

    Text txt;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        hp = maxHP;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        txt = GameObject.FindGameObjectWithTag("Counter").GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        if (hp <= 0)
        {
            GameObject.FindGameObjectWithTag("Creator").GetComponent<Spawner>().maxEnemies--;
            string s = GameObject.FindGameObjectWithTag("Creator").GetComponent<Spawner>().maxEnemies.ToString();
            txt.text = "Remaining enemies: " + s;
            Destroy(gameObject);
        }
        
        Vector2 dir = player.position - rig.position;
        dir.Normalize();

        attackDelay += Time.fixedDeltaTime;

        float dist = Vector2.Distance(player.position, rig.position);
        bool atSight = dist <= visionRange && dist >= 0.7f;
        bool atRange = dist <= attackRange && attackDelay >= maxAttackDelay;

        if (atSight && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            rig.position += dir * speed * Time.fixedDeltaTime;
        if (atRange || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            weapon.SetActive(true);
            attackDelay = 0;
        }
        else weapon.SetActive(false);

        // animations
        anim.SetFloat("Direction", dir.x);
        if (atSight) anim.SetBool("isMoving", true);
        else anim.SetBool("isMoving", false);
        if (atRange) anim.SetBool("isAttacking", true);
        else anim.SetBool("isAttacking", false);
    }
}
