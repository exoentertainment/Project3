using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;

public class AsteroidGrabberGrab : MonoBehaviour
{
    #region -- Serialized Fields --

    [SerializeField] UnityEvent OnGrab;

    #endregion

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Asteroid"))
        {
            other.gameObject.transform.SetParent(transform.parent);
            other.gameObject.GetComponent<MMAutoRotate>().enabled = false;
            OnGrab?.Invoke();
        }
    }
}
