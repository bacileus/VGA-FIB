using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoodieAI : MonoBehaviour
{
    Rigidbody2D rig;
    Animator anim;
    public GameObject proj;
    public Rigidbody2D player;
    public float speed;
    public float projForce;

    public float minDelay;
    float delay;

    public float fireRange, visionRange;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        delay += Time.fixedDeltaTime;
        
        Vector2 dir = player.position - rig.position;
        dir.Normalize();

        float d = Vector2.Distance(player.position, rig.position);
        if (d > fireRange && d <= visionRange)
        {
            rig.position += dir * speed * Time.deltaTime; // move
            anim.SetBool("isRunning", true);
            anim.SetBool("isAttacking", false);
        } 
        else if (d <= fireRange && delay >= minDelay)
        {
            // shoot
            delay = 0;
            anim.SetBool("isRunning", false);
            anim.SetBool("isAttacking", true);
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                GameObject r = Instantiate(proj, rig.position, Quaternion.identity);
                r.GetComponent<Rigidbody2D>().AddForce(dir * projForce);
                Destroy(r, 1.5f);
            }
        } else if (delay < minDelay || d > visionRange)
        {
            // idle
            anim.SetBool("isRunning", false);
            anim.SetBool("isAttacking", false);
        }

        // animations
        if (dir.x > 0) anim.SetFloat("h", 1);
        if (dir.x < 0) anim.SetFloat("h", -1);
        if (dir.y > 0) anim.SetFloat("v", 1);
        if (dir.y < 0) anim.SetFloat("v", -1);

        if (dir.x > dir.y) anim.SetBool("isVertical", false);
        else anim.SetBool("isVertical", true);
    }
}
