using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Serialization;

public class TractorBeam : MonoBehaviour
{
    #region -- Serialized Fields --

    [Header("Variables")] 
    [SerializeField] private float tractorSpeed;
    [SerializeField] private float tractorRange;
    [SerializeField] LayerMask asteroidLayer;
    [SerializeField] LayerMask platformLayer;
    [SerializeField] private int targetRange;

    [Header("Prefabs")]
    public GameObject beamLineRendererPrefab; //Put a prefab with a line renderer onto here.

    private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;
    private LineRenderer line;

    [Header("Beam Options")]
    public float textureScrollSpeed = 0f; //How fast the texture scrolls along the beam, can be negative or positive.
    public float textureLengthScale = 1f;   //Set this to the horizontal length of your texture relative to the vertical. Example: if texture is 200 pixels in height and 600 in length, set this to 3
    [SerializeField] float lineWidth = 1f;
	
    [Header("Width Pulse Options")]
    public float widthMultiplier = 1.5f;
    private float customWidth;
    private float originalWidth;
    private float lerpValue = 0.0f;
    public float pulseSpeed = 1.0f;
    private bool pulseExpanding = true;
    
    #endregion

    private GameObject targetAsteroid;
    private GameObject targetPlatform;

    private void Start()
    {
        FindClosestAsteroid();
        FindNearbyPlatform();
        
        originalWidth = lineWidth;
        customWidth = originalWidth * widthMultiplier;
    }

    private void Update()
    {
        if (Time.timeScale == 1)
        {
            CheckAsteroidStatus();
            CheckPlatformStatus();
            
            MoveAsteroid();
        }
    }

    private void FixedUpdate()
    {
        ExtendTractorBeam();
    }

    private void FindClosestAsteroid()
    {
        if (targetAsteroid == null)
        {
            float closestEnemy = Mathf.Infinity;

            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, tractorRange, asteroidLayer);

            if (potentialTargets.Length > 0)
            {
                // for (int x = 0; x < potentialTargets.Length; x++)
                // {
                //     float distanceToEnemy =
                //         Vector3.Distance(potentialTargets[x].transform.position, transform.position);
                //
                //     if (distanceToEnemy < closestEnemy)
                //     {
                //         closestEnemy = distanceToEnemy;
                //         targetAsteroid = potentialTargets[x].gameObject;
                //     }
                // }
                
                // targetAsteroid = potentialTargets[UnityEngine.Random.Range(0, potentialTargets.Length)]
                //     .gameObject;
                //
                // if (targetAsteroid.TryGetComponent<MMAutoRotate>(out MMAutoRotate component))
                // {
                //     if (!component.enabled)
                //     {
                //         Debug.Log("asteroid already found");
                //     }
                // }
                bool isFree = false;
                int randomTarget = UnityEngine.Random.Range(0, potentialTargets.Length);
                
                while(!isFree)
                {
                    if (potentialTargets[randomTarget].TryGetComponent<MMAutoRotate>(out MMAutoRotate component))
                    {
                        if (component.enabled)
                        {
                            targetAsteroid = potentialTargets[randomTarget]
                                .gameObject;
                            isFree = true;
                            //break;
                        }
                        else
                        {
                            randomTarget = UnityEngine.Random.Range(0, potentialTargets.Length);
                        }
                    }
                }
            }

            if (targetAsteroid != null)
            {
                SpawnBeam();
                ExtendTractorBeam();
                RemoveAsteroidComponents();
            }
        }
    }

    void FindNearbyPlatform()
    {
        if (targetPlatform == null)
        {
            float closestEnemy = Mathf.Infinity;

            Collider[] potentialTargets = Physics.OverlapSphere(transform.position, Mathf.Infinity, platformLayer);

            if (potentialTargets.Length > 0)
            {
                for (int x = 0; x < potentialTargets.Length; x++)
                {
                    float distanceToEnemy =
                        Vector3.Distance(potentialTargets[x].transform.position, transform.position);

                    if (distanceToEnemy < closestEnemy)
                    {
                        closestEnemy = distanceToEnemy;
                        targetPlatform = potentialTargets[x].gameObject;
                    }
                }
            }
        }
    }

    void RemoveAsteroidComponents()
    {
        if(targetAsteroid != null)
            if(targetAsteroid.TryGetComponent<MMAutoRotate>(out MMAutoRotate component))
                component.enabled = false;
        //targetAsteroid.GetComponent<AsteroidBeltObject>().enabled = false;
    }

    void MoveAsteroid()
    {
        if (targetPlatform != null && targetAsteroid != null)
        {
            targetAsteroid.transform.position = Vector3.MoveTowards(targetAsteroid.transform.position, targetPlatform.transform.position, tractorSpeed * Time.deltaTime);
        }
    }

    void CheckAsteroidStatus()
    {
        if (targetAsteroid == null)
        {
            if(beam != null)
                Destroy(beam);
            
            FindClosestAsteroid();
        }
    }

    void CheckPlatformStatus()
    {
        if(targetPlatform == null)
            FindNearbyPlatform();
    }
    
    public void SpawnBeam()
    {
        if (beamLineRendererPrefab)
        {
            beam = Instantiate(beamLineRendererPrefab);
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
        else
        {
            Debug.LogError("A prefab with a line renderer must be assigned to the `beamLineRendererPrefab` field in the SciFiArsenalBeamStatic script on " + gameObject.name);
        }
    }

    void ExtendTractorBeam()
    {
        if (beam && targetAsteroid != null) 
        {
            Debug.Log(targetAsteroid.name);
            line.SetPosition(0, transform.position);
            // Vector3 end = transform.position + (targetAsteroid.transform.forward);
            Vector3 end = targetAsteroid.transform.position;
				
            line.SetPosition(1, end);
            // beamStart.transform.position = transform.position;
            // beamStart.transform.LookAt(end);
            // beamEnd.transform.position = end;
            // beamEnd.transform.LookAt(beamStart.transform.position);
            float distance = Vector3.Distance(transform.position, end);
            line.material.mainTextureScale = new Vector2(distance / textureLengthScale, 1); //This sets the scale of the texture so it doesn't look stretched
            line.material.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0); //This scrolls the texture along the beam if not set to 0
            
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

            float currentWidth = Mathf.Lerp(originalWidth, customWidth, Mathf.Sin(lerpValue * Mathf.PI));
		
            line.startWidth = currentWidth;
            line.endWidth = currentWidth;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, tractorRange);
    }
}
