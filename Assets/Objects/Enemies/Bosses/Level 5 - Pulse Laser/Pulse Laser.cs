using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PulseLaser : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Variables")] 
    [SerializeField] private float fireRate;
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] float pulseSpeed;
    [SerializeField] float pulseDuration;
    [SerializeField] LayerMask targetMask;

    [Header("Prefabs")]
    [SerializeField] GameObject laserPrefab;
    
    [Header("Beam Options")]
    public float textureScrollSpeed = 0f; //How fast the texture scrolls along the beam, can be negative or positive.
    public float textureLengthScale = 1f;   //Set this to the horizontal length of your texture relative to the vertical. Example: if texture is 200 pixels in height and 600 in length, set this to 3
    [SerializeField] float lineWidth = 1f;
	   
    [Header("Width Pulse Options")]
    [SerializeField] float widthMultiplier = 1.5f;
    private float customWidth;
    private float originalWidth;
    private float lerpValue = 0.0f;
    [SerializeField] float beamPulseSpeed = 1.0f;
    private bool pulseExpanding = true;
    
    #endregion

    private float lastFireTime;
    float lastPulseTime;
    GameObject target;
    private GameObject beam;
    private LineRenderer line;
    private Vector3 targetOffset;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastFireTime = Time.time;
        lastPulseTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            CheckTargetStatus();
            Fire();
        }
    }

    private void FixedUpdate()
    {
        PulseBeam();
    }

    void FindNearbyTarget()
    {
            //laserComponent.SetActive(false);

            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, range, targetMask);

            if (potentialTargets.Length > 0)
            {
                target = potentialTargets[Random.Range(0, potentialTargets.Length)].gameObject;
            }
    }

    void Fire()
    {
        if ((Time.time - lastFireTime) > fireRate && target != null)
        {
            lastFireTime = Time.time;
            lastPulseTime = Time.time;

            CreateBeam();
            StartCoroutine(PulseLaserRoutine());
        }
    }

    IEnumerator PulseLaserRoutine()
    {
        while ((Time.time - lastPulseTime) < pulseDuration)
        {
            targetOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) + target.transform.position;
            SetLaserEnds();
            line.enabled = true;
            DealDamage();
            
            if(CameraManager.instance.IsObjectInView(transform))
                AudioManager.instance.PlayPulseLaserBossWeaponFire();

            if (target == null)
                break;
            
            yield return new WaitForSeconds(pulseSpeed);
            
            line.enabled = false;
            
            yield return new WaitForSeconds(pulseSpeed);
        }
        
        Destroy(beam);
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
            
            originalWidth = .5f;
            customWidth = originalWidth * widthMultiplier;
        }
    }

    void SetLaserEnds()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, targetOffset);
    }

    void PulseBeam()
    {
        if (beam != null && target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            line.material.mainTextureScale = new Vector2(distance / textureLengthScale, 1); //This sets the scale of the texture so it doesn't look stretched
            line.material.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0); //This scrolls the texture along the beam if not set to 0
            
            float currentWidth = Mathf.Lerp(originalWidth, customWidth, Mathf.Sin(lerpValue * Mathf.PI));
		
            line.startWidth = currentWidth;
            line.endWidth = currentWidth;
        }
        
        // Pulse the width of the beam
        if (pulseExpanding) 
        {
            lerpValue += Time.deltaTime * pulseSpeed;
        } 
        else 
        {
            lerpValue -= Time.deltaTime * pulseSpeed;
        }

        if (lerpValue >= 1.0f) 
        {
            pulseExpanding = false;
            lerpValue = 1.0f;
        } 
        else if (lerpValue <= 0.0f) 
        {
            pulseExpanding = true;
            lerpValue = 0.0f;
        }
    }
    
    void CheckTargetStatus()
    {
        if (target == null || Vector3.Distance(transform.position, target.transform.position) > range)
        {
            FindNearbyTarget();
        }
    }

    void DealDamage()
    {
        if (target.TryGetComponent<IDamageable>(out IDamageable targetHit))
        {
            targetHit.TakeDamage(damage);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
