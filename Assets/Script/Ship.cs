using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImageEffect;

public class Ship : MonoBehaviour , IDamageAble
{
    GameSceneManager gameManager = null;

    [Header("Camera")]
    [SerializeField]CameraShake camShake = null;

    [Header("UI Status")]
    [SerializeField] UnityEngine.UI.Text condition = null;

    [Header("Status")]
    [SerializeField] int health = 100;
    [SerializeField] int attack = 10;

    [Header("Movement")]
    [SerializeField] float speed = 10;
    float totalTime = 0;
    KeyCode currentCode = KeyCode.None;

    [Header("Shooting")]
    [SerializeField] Transform cannon = null;
    [SerializeField] Transform projectileSpawnPos = null;
    [SerializeField] Rigidbody2D projectilePrefabs = null;
    [SerializeField] GameObject smokeEffect = null;
    [SerializeField] float cooldownTime = 0.5f;
    float cooldownShoot = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       

        //Movement
        if (Input.GetKey(KeyCode.D))
        {
            if (currentCode == KeyCode.D)
            {
                totalTime = Mathf.Min(1, totalTime + Time.deltaTime);
                transform.position += Vector3.right * Time.deltaTime * speed * totalTime;
            }
            else
            {
                totalTime = Mathf.Max(0, totalTime - 2 * Time.deltaTime);
                transform.position += Vector3.left * Time.deltaTime * speed * totalTime;
                if (totalTime == 0)
                    currentCode = KeyCode.D;
            }
            
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (currentCode == KeyCode.A)
            {
                totalTime = Mathf.Min(1, totalTime + Time.deltaTime);
                transform.position += Vector3.left * Time.deltaTime * speed * totalTime;
            }
            else
            {
                totalTime = Mathf.Max(0, totalTime - 2*Time.deltaTime);
                transform.position += Vector3.right * Time.deltaTime * speed * totalTime;
                if (totalTime == 0)
                    currentCode = KeyCode.A;
            }
            
        }
        else {
            totalTime = Mathf.Max(0, totalTime - Time.deltaTime);
            if (currentCode == KeyCode.A)
                transform.position += Vector3.left * Time.deltaTime * speed * totalTime;
            else
                transform.position += Vector3.right * Time.deltaTime * speed * totalTime;
        }



        //Shooting
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RotateBody(cannon, (pos - (Vector2)transform.position).normalized, 10);


        cooldownShoot = Mathf.Max(0, cooldownShoot - Time.deltaTime);
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && cooldownShoot == 0)
        {
            cooldownShoot = cooldownTime;
            Instantiate(smokeEffect, projectileSpawnPos.position, Quaternion.identity);
            Rigidbody2D rb = Instantiate(projectilePrefabs, projectileSpawnPos.position, Quaternion.identity);
            Vector2 direction = cannon.GetChild(0).up;
            rb.AddForce(3000 * direction);
            rb.gameObject.GetComponent<Projectile>().damage = attack;

            camShake.StartShake();
            RipplePospProcessor.RippleCam(projectileSpawnPos.position);
            ChromaticAberration.AbrationCam(0.01f, 0.4f);

            if(direction.x < 0) // impact ke kanan
            {
                if (currentCode == KeyCode.A)//jika sedang ke kiri
                {
                    if (totalTime > 0.5f)
                    {
                        totalTime -= 0.5f;
                    }
                    else
                    {
                        totalTime = Mathf.Abs(totalTime - 0.5f);
                        currentCode = KeyCode.D;//bekolan ke kanan
                    }
                }
                else { //jika sedang ke kanan
                    totalTime = Mathf.Max(1 , totalTime + 0.5f);
                    currentCode = KeyCode.D;
                }
            }
            else
            {
                if (currentCode == KeyCode.D)//jika sedang ke kanan
                {
                    if (totalTime > 0.5f)
                    {
                        totalTime -= 0.5f;
                    }
                    else
                    {
                        totalTime = Mathf.Abs(totalTime - 0.5f);
                        currentCode = KeyCode.A;//bekolan ke kiri
                    }
                }
                else
                { //jika sedang ke kiri
                    totalTime = Mathf.Max(1, totalTime + 0.5f);
                    currentCode = KeyCode.A;
                }
            }


            Debug.Log("Shooting");
        }

        

    }

    public void RotateBody(Transform body , Vector2 direction, float speed = 6f , float limit = 80)
    {
        Quaternion fromRot = body.rotation;

        Quaternion targetRot = Quaternion.FromToRotation(Vector2.up, direction);

        //transform.eulerAngles = Quaternion.RotateTowards(fromRot, targetRot, speed).eulerAngles;
        float val = Quaternion.Slerp(fromRot, targetRot, (speed / 2f) * Time.deltaTime).eulerAngles.z;
        if (val > 180)
            val -= 360;

        //Debug.Log(val);
        body.eulerAngles = new Vector3(0, 0, Mathf.Clamp( val , -limit , limit) );
    }

    public void Hit(int damage)
    {
        health -= damage;
        condition.text = "Condition : " + health;
        if (health <= 0)
        {
            InvokeRepeating("Explode", 0.5f, 0.4f);
        }
    }

    void Explode()
    {
        Instantiate(smokeEffect, transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity);
    }
}
