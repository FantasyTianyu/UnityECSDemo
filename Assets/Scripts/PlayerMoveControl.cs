using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerMoveControl : MonoBehaviour
{
    public float moveSpeed = 10;
    public float turnSpeed = 10;
    [Range(0, 1)]
    public float shootTime = 0.5f;

    public bool useSpawn = false;

    public bool useECS = false;

    public GameObject bulletPrefab;
    public GameObject bulletPrefabECS;
    public Transform shootTrans;


    private Rigidbody playerRigidBody;
    private float shootTimer = 0f;
    // Start is called before the first frame update

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        shootTimer = shootTime;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        float distance = 10;
        Quaternion right = transform.rotation * Quaternion.AngleAxis(30, Vector3.up);
        Quaternion left = transform.rotation * Quaternion.AngleAxis(30, Vector3.down);
        Vector3 n = transform.position + (Vector3.forward * distance);
        Vector3 leftPoint = left * n;
        Vector3 rightPoint = right * n;
        Debug.DrawLine(transform.position, leftPoint, Color.red);
        Debug.DrawLine(transform.position, rightPoint, Color.red);

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0f || v != 0f)
        {
            Vector3 direction = new Vector3(h, 0, v);
            Quaternion targetRotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), Time.deltaTime * turnSpeed);
            playerRigidBody.MoveRotation(targetRotation);
            playerRigidBody.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);


        }

        if (Input.GetMouseButton(0))
        {
            if (useECS)
            {
                ShootECS();
            }
            else
            {
                Shoot();
            }

        }
    }

    private void Shoot()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootTime)
        {
            if (useSpawn)
            {
                for (int x = -60; x < 62; x++)
                {
                    Quaternion dir = shootTrans.rotation * Quaternion.AngleAxis(x * 0.5f, Vector3.up);

                    for (int y = -1; y < 5; y++)
                    {
                        Instantiate(bulletPrefab, shootTrans.position + new Vector3(0, y * 0.3f, 0), dir);

                    }
                }
            }
            else
            {
                GameObject bullet = Instantiate(bulletPrefab, shootTrans.position, Quaternion.LookRotation(transform.forward, Vector3.up));
            }


            shootTimer = 0;
        }
    }

    private void ShootECS()
    {


        shootTimer += Time.deltaTime;

        if (shootTimer >= shootTime)
        {
            if (useSpawn)
            {
                int index = 0;
                EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();
                NativeArray<Entity> bullets = new NativeArray<Entity>(732, Allocator.Temp);
                entityManager.Instantiate(bulletPrefabECS, bullets);
                for (int x = -60; x < 62; x++)
                {
                    quaternion dir = shootTrans.rotation * Quaternion.AngleAxis(x * 0.5f, Vector3.up);

                    for (int y = -1; y < 5; y++)
                    {
                        entityManager.SetComponentData<Position>(bullets[index], new Position { Value = shootTrans.position + new Vector3(0, y * 0.3f, 0) });
                        entityManager.SetComponentData<Rotation>(bullets[index], new Rotation { Value = dir });

                        index++;
                    }
                }
                bullets.Dispose();
            }
            else
            {
                GameObject bullet = Instantiate(bulletPrefab, shootTrans.position, Quaternion.LookRotation(transform.forward, Vector3.up));
            }


            shootTimer = 0;
        }

    }
}
