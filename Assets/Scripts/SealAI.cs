using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SealAI : MonoBehaviour
{
    Rigidbody2D rig;
    Rigidbody2D player;
    Animator anim;
    public GameObject proj;
    public float projForce;

    public float minDelay, maxTpDelay;
    float shootDelay, tpDelay;

    public float fireRange, visionRange;

    public GameObject particles;

    public int maxHp;
    [HideInInspector]
    public int hp;

    Text txt;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        txt = GameObject.FindGameObjectWithTag("Counter").GetComponent<Text>();
        hp = maxHp;
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

        shootDelay += Time.fixedDeltaTime;
        tpDelay += Time.fixedDeltaTime;

        Vector2 dir = player.position - rig.position;

        float d = Vector2.Distance(player.position, rig.position);
        if (d > fireRange && d <= visionRange && tpDelay >= maxTpDelay)
        {
            // move
            tpDelay = 0;
            rig.position += dir / Random.Range(1.25f, 2f);
            GameObject p = Instantiate(particles, rig.position + new Vector2(0.04f, -0.4f), Quaternion.identity);
            Destroy(p, 1.5f);
        }
        else if (d <= fireRange && shootDelay >= minDelay)
        {
            // shoot
            shootDelay = 0;
            GameObject r = Instantiate(proj, rig.position, Quaternion.identity);
            r.GetComponent<Rigidbody2D>().AddForce(dir.normalized * projForce);
            Destroy(r, 1.5f);
        }

        // animations
        dir.Normalize();
        if (dir.x > 0) anim.SetFloat("h", 1);
        if (dir.x < 0) anim.SetFloat("h", -1);
        if (dir.y > 0) anim.SetFloat("v", 1);
        if (dir.y < 0) anim.SetFloat("v", -1);

        if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x)) anim.SetBool("isHorizontal", false);
        else anim.SetBool("isHorizontal", true);
    }
}
