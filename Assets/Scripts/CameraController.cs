using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    public float magnitude;
    [HideInInspector]
    public float counter = 0;

    public float scrollingSpeed;

    void FixedUpdate()
    {
        // change camera size
        float wheel = Input.GetAxisRaw("Mouse ScrollWheel") * scrollingSpeed;
        if (wheel != 0)
        {
            float size = gameObject.GetComponent<Camera>().orthographicSize;
            gameObject.GetComponent<Camera>().orthographicSize = Mathf.Clamp(size - wheel, 2, 6);
        }

        // update camera position
        Vector3 newPos = new Vector3(player.position.x, player.position.y, -10);
        if (counter > 0)
        {
            transform.position = newPos + Random.insideUnitSphere * magnitude;
            counter -= Time.fixedDeltaTime;
        } else
        {
            transform.position = newPos;
            counter = 0;
        }
    }
}
