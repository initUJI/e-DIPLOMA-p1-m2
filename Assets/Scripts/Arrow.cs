using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float lifeSpan = 3f;
    public float spawnTime;
    public float timeAlive;
    public bool collided = false;

    private void Start()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        timeAlive = Time.time - spawnTime;

        if (timeAlive > lifeSpan && !collided)
            Destroy(gameObject);
        if (!collided)
            transform.position += transform.forward * 5f * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        collided = true;
        transform.parent = collision.transform;
        transform.GetComponent<Rigidbody>().velocity = default;
    }
}
