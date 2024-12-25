using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpecialLaser : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Variables")] 
    [SerializeField] private float damage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveDuration;
    [SerializeField] LayerMask targetMask;

    [Header("Prefabs")]
    [SerializeField] GameObject laserPrefab;
    
    #endregion
    
    private float lastMoveTime;
    GameObject target;
    private GameObject beam;
    private LineRenderer line;

    private Vector3 targetOffset;

    private void Start()
    {
        lastMoveTime = Time.time;
        
        FindNearbyTarget();
        CreateBeam();
        ChangeTargetOffset();
        SetLaserEnds();

    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            MoveLaser();
        }
    }

    void FindNearbyTarget()
    {
        if (target == null)
        {
            float closestEnemy = Mathf.Infinity;

            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity, targetMask);

            if (potentialTargets.Length > 0)
            {
                for (int x = 0; x < potentialTargets.Length; x++)
                {
                    float distanceToEnemy =
                        Vector3.Distance(potentialTargets[x].transform.position, transform.position);

                    if (distanceToEnemy < closestEnemy)
                    {
                        closestEnemy = distanceToEnemy;
                        target = potentialTargets[x].gameObject;
                    }
                }
            }
        }
    }

    void ChangeTargetOffset()
    { 
        SphereCollider sphereCollider = target.GetComponent<SphereCollider>();
        targetOffset = target.transform.position + ((Random.onUnitSphere * sphereCollider.radius) * target.transform.lossyScale.x);
    }
    
    void CreateBeam()
    {
        if (laserPrefab != null)
        {
            beam = Instantiate(laserPrefab);
            beam.transform.position = transform.position;
            beam.transform.parent = transform;
            beam.transform.rotation = transform.rotation;

            line = beam.GetComponent<LineRenderer>();
            line.useWorldSpace = true;

            #if UNITY_5_5_OR_NEWER
                        line.positionCount = 2;
            #else
			    line.SetVertexCount(2); 
            #endif
        }
    }

    void SetLaserEnds()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, target.transform.position);
    }

    void MoveLaser()
    {
        if ((Time.time - lastMoveTime) > moveDuration)
        {
            lastMoveTime = Time.time;
            ChangeTargetOffset();
            //SetLaserEnds();
        }
        
        line.SetPosition(0, transform.position);
        Vector3 newLineEndPosition = Vector3.Lerp(line.GetPosition(1), targetOffset, (moveSpeed * Time.deltaTime));
        line.SetPosition(1, newLineEndPosition);
    }
}
