using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ColliderProvider : MonoBehaviour
{
    [SerializeField] private Collider2D collider;

    public bool isEnabled;
    
    private void Awake()
    {
        isEnabled = collider.enabled;
    }
    
    private void FixedUpdate()
    {
        if (collider.enabled != isEnabled)
        {
            collider.enabled = isEnabled;
        }
    }
}
