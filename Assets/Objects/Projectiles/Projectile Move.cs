using System;
using System.Collections;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] IProjectileScriptableObject projectileSO;

    private void Start()
    {
        Destroy(gameObject, projectileSO.lifeTime);
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            Move();
        }
    }

    void Move()
    {
        transform.position += transform.forward * (projectileSO.moveSpeed * Time.deltaTime);
    }
    
    private void OnCollisionEnter(Collision other)
    {   
        Destroy(gameObject);
    }
}
