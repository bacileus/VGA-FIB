using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIncoming : MonoBehaviour
{
    public GameObject stoneImpact;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            if (collision.gameObject.name == "Rock(Clone)")
            {
                GameObject i = Instantiate(stoneImpact, collision.transform.position, Quaternion.identity);
                Destroy(i.gameObject, 1.5f);
                Destroy(collision.gameObject);
            }
        }
    }
}
