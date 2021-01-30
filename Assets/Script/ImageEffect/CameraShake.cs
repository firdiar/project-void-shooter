using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] Vector2 radius = Vector2.zero;
    [SerializeField] Vector2 offset = Vector2.zero;
    Vector3 correctPosition = Vector2.zero;

    float time = 0;
    Vector3 target = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        correctPosition = transform.position;
        time = 1f;
        setTarget();
    }

    public void StartShake() {
        time = 3f;
    }


    Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target , ref velocity, Mathf.Clamp(3 - time, 0.2f, 1f));
            if (Vector3.Distance(transform.position, target) < 1f) {
                setTarget();
            }
            RotateBody(transform, target - transform.position, Mathf.Clamp(3 - time , 0.1f , 0.5f));
            time -= Time.deltaTime;
            
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, correctPosition, ref velocity, 0.5f);
            RotateBody(transform, Vector2.up , 0.05f);
        }
       
        
    }

    void setTarget() {
        target = Random.insideUnitCircle * radius + offset;
        target.z = transform.position.z;
    }

    Vector3 rotationVelocity = Vector3.zero;
    public void RotateBody(Transform body, Vector2 direction , float time , float limit = 5)
    {
        Quaternion fromRot = body.rotation;
        Vector2 vRot = new Vector2(0.3f * Vector2.Dot(Vector2.right, direction.normalized), 0.7f).normalized;
       // Debug.Log(Vector2.Dot(Vector2.right, direction.normalized));
        Quaternion targetRot = Quaternion.FromToRotation(Vector2.up,  vRot  );

        body.rotation = SmoothDampQuaternion(fromRot, targetRot, ref rotationVelocity, 6f);

    }

    public Quaternion SmoothDampQuaternion(Quaternion current, Quaternion target, ref Vector3 currentVelocity, float smoothTime)
    {
        Vector3 c = current.eulerAngles;
        Vector3 t = target.eulerAngles;
        return Quaternion.Euler(
          Mathf.SmoothDampAngle(c.x, t.x, ref currentVelocity.x, smoothTime),
          Mathf.SmoothDampAngle(c.y, t.y, ref currentVelocity.y, smoothTime),
          Mathf.SmoothDampAngle(c.z, t.z, ref currentVelocity.z, smoothTime)
        );
    }
}
