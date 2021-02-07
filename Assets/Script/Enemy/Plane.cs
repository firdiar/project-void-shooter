using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Plane : MonoBehaviour, IDamageAble
{
    [Header("Status")]
    public float health = 10;
    public int attack = 2;
    [SerializeField] bool isMoving = false;
    float movingLeap = 0;
    Vector2 direction = Vector2.zero;

    [SerializeField] GameObject explosionEffect = null;
    [SerializeField] GameObject smokeEffect = null;
    [SerializeField] GameObject mask = null;
    [Header("Shooting")]
    [SerializeField] float cooldownShoot = 4;
    [SerializeField]protected Transform spawnPlace = null;
    [SerializeField]protected Projectile projectile = null;

    bool isFallDown = false;

    Rigidbody2D body = null;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        InvokeRepeating("Shoot", Random.Range(0.5f, 3), cooldownShoot);
    }


    private void Update()
    {
        if (isFallDown) {
            Vector3 euler = transform.eulerAngles;
            euler.z += Random.Range(-3f, 3f);
            transform.eulerAngles = euler;
        }

        if (isMoving) {
            if (movingLeap <= 0) {
                movingLeap = 3;
                direction = Random.Range(0f, 1f) < 0.5 ? Vector2.left : Vector2.right;
            }
            movingLeap -= Time.deltaTime;
            transform.position += (Vector3)direction * Time.deltaTime * 4;
        }
    }

    protected virtual GameObject Shoot() {
        Projectile p = Instantiate(projectile, spawnPlace.position, Quaternion.identity);
        p.damage = attack;
        return p.gameObject;
    }

    // Update is called once per frame
    void FallDown()
    {
        isMoving = false;
        body.constraints = RigidbodyConstraints2D.None;
        CancelInvoke("Shoot");

        mask.SetActive(true);
        isFallDown = true;
        Invoke("DestroyPlane", Random.Range(1 , 5));
        body.gravityScale = 2;
        GetComponent<Collider2D>().isTrigger = true;
        InvokeRepeating("Explode", 0.5f, 0.2f);
    }

    public void Hit(int damage)
    {
        health -= damage;
        if (!isFallDown && health <= 0){
            FallDown();
        }
    }

    void Explode() {
        Instantiate(smokeEffect, transform.position+(Vector3)Random.insideUnitCircle, Quaternion.FromToRotation(Vector2.up, body.velocity ));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag != "Wall" || isFallDown)
        {
            IDamageAble dma = collision.GetComponent<IDamageAble>();
            if (dma != null)
                dma.Hit(attack * 3);

            DestroyPlane();
        }
    }

    void DestroyPlane() {
        GameSceneManager.main.AddScore(attack);
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        ImageEffect.RipplePospProcessor.RippleCam(transform.position);
        ImageEffect.ChromaticAberration.AbrationCam(0.005f, 0.4f);
    }

    
}
