using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rig;
    public Vector2Int damage;
    Camera cam;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        Destroy(gameObject, 1.5f);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            rig.velocity = Vector2.zero;
            anim.SetBool("hasCollided", true);
        }
        if (collision.CompareTag("Enemy"))
        {
            int finalDamage = Random.Range(damage.x, damage.y + 1);
            Debug.Log(finalDamage);
            if (collision.name == "Slime(Clone)") collision.GetComponent<SlimeAI>().hp -= finalDamage;
            else if (collision.name == "Seal(Clone)") collision.GetComponent<SealAI>().hp -= finalDamage;
            else if (collision.name == "EvilBird(Clone)") collision.GetComponent<EvilBirdAI>().hp -= finalDamage;
            cam.GetComponent<CameraController>().counter = 0.3f;
        }
        if (collision.CompareTag("Weapon") && collision.name == "Rock(Clone)")
        {
            Destroy(collision.gameObject);
        }
    }
}
