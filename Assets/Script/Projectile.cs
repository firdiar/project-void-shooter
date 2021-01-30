using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageAble {
    void Hit(int damage);
}

public class Projectile : MonoBehaviour
{
    [SerializeField]GameObject explosionEffect = null;
    public int damage = 0;
    private void Start()
    {
        Invoke("SelfDestruct", 10);
    }

    void SelfDestruct()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        ImageEffect.RipplePospProcessor.RippleCamCustom(transform.position, 20);
        Destroy(gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag != "Wall")
        {
            IDamageAble dma = collision.gameObject.GetComponent<IDamageAble>();
            //Debug.Log(collision.gameObject.name+" - "+ dma==null);
            if (dma != null)
                dma.Hit(damage);
            

            Instantiate(explosionEffect, transform.position , Quaternion.FromToRotation(Vector2.up, collision.transform.position- transform.position));
            ImageEffect.RipplePospProcessor.RippleCamCustom(transform.position, 20);
            Destroy(gameObject);
        }
    }
}
