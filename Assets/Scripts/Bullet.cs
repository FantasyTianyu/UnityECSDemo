using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100;
    // Start is called before the first frame update
    public float destroyTimmer = 1;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        destroyTimmer -= Time.deltaTime;
        if (destroyTimmer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }

    }
}
