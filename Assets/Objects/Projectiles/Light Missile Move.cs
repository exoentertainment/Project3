using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LightMissileMove : MonoBehaviour
{
    #region -- Serialized Fields --

    [FormerlySerializedAs("projectileSO")]
    [Header("Scriptable Object")] 
    [SerializeField] private MissileScriptableObject missileSO;

    [SerializeField] private float rotationTime;
    #endregion

    private GameObject target;
    float lastRotationTime;
    int swerveDirection = 1;
    private float moveSpeed;
    
    private void Start()
    {
        lastRotationTime = Time.time;
        moveSpeed = Random.Range(missileSO.minMoveSpeed, missileSO.maxMoveSpeed);
        FindTarget();
        SpawnDischargeEffect();
        Destroy(gameObject, missileSO.lifeTime);
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            Move();
        }
    }
    
    void FindTarget()
    {
        float closestEnemy = Mathf.Infinity;

        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, missileSO.attackRange, missileSO.targetLayer);

        if (potentialTargets.Length > 0)
        {
            target = potentialTargets[UnityEngine.Random.Range(0, potentialTargets.Length)].gameObject;
        }
    }
    
    void Move()
    {
        transform.position += transform.forward * (moveSpeed * Time.deltaTime);
        
        if (target != null)
        {
                transform.LookAt(target.transform, transform.up);
        }
        else
        {
            FindTarget();
        }

        // if ((Time.time - lastRotationTime) > rotationTime)
        // {
            
            
            float noiseyness = 1; //tweak this to adjust how random the motion is

            //calculate random number using x and y position
            float rand = Mathf.PerlinNoise(transform.position.x * noiseyness, transform.position.y * noiseyness);

            //if you're in 3d, you'll need to do it again to take into account the z position
            rand = Mathf.PerlinNoise(rand, transform.position.z * noiseyness);

            //randomize the last variable
            float rand_angle = Mathf.Lerp(rand * UnityEngine.Random.Range(1, 3), -5, 1);

            //transform.rotation *= Quaternion.AngleAxis(rand_angle, transform.right * UnityEngine.Random.Range(-1f, 1f));

            //calculate random number using x and y position
            rand = Mathf.PerlinNoise(transform.position.x * noiseyness, transform.position.y * noiseyness);

            //if you're in 3d, you'll need to do it again to take into account the z position
            rand = Mathf.PerlinNoise(rand, transform.position.z * noiseyness);

            //randomize the last variable
            rand_angle = Mathf.Lerp(-rand * UnityEngine.Random.Range(1, 3), 5, 1);
            //transform.rotation *= Quaternion.AngleAxis(rand_angle, transform.up * UnityEngine.Random.Range(-50f, 50f));
            
            transform.Translate(transform.up * swerveDirection * 1 * Time.deltaTime);
            transform.Translate(transform.right * swerveDirection * 1 * Time.deltaTime);
            //GetComponent<Rigidbody>().AddForce((transform.up * swerveDirection) * 5 , ForceMode.VelocityChange);
            if ((Time.time - lastRotationTime) > rotationTime)
            {
                lastRotationTime = Time.time;
                swerveDirection *= -1;
            }
    }
    

    void SpawnDischargeEffect()
    {
        if(missileSO.dischargeEffectPrefab != null)
            Instantiate(missileSO.dischargeEffectPrefab, transform.position, transform.rotation);    
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (1 << other.gameObject.layer == missileSO.targetLayer)
        {
            if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable hit))
            {
                hit.TakeDamage(missileSO.damage);
            }
        }

        if(CameraManager.instance.IsObjectInView(transform))
            AudioManager.instance.PlaySmallExplosion();
            
        Instantiate(missileSO.explodeEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, missileSO.attackRange);
    }
}
