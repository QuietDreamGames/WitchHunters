using System;
using System.Collections;
using System.Collections.Generic;
using Presentation.Feature;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    
    [SerializeField] private Animator _animator;
        
    [SerializeField] private string _horizontal;
    [SerializeField] private string _vertical;
    [SerializeField] private string _moving;

    public Vector3 Position
    {
        get => _transform.position;
        set => _transform.position = value;
    }

    public Vector3 Velocity { get; set; }
    
    public FlockGO Flock { get; set; }

    private void Update()
    {
        _animator.SetFloat(_horizontal, Velocity.x); 
        _animator.SetFloat(_vertical, Velocity.y);
        _animator.SetBool(_moving, true);
        
        var steer = Vector3.zero;
        var (isBounds, bounds) = Flock.Bounds(this);
        var (isSeparation, separation) = Flock.Separation(this);
        var (isAlignment, alignment) = Flock.Alignment(this);
        var (isCohesion, cohesion) = Flock.Cohesion(this);
        
        if (isBounds)
        {
            steer = bounds;
        }
        else if (isSeparation)
        {
            steer = separation * Time.deltaTime;
        }
        else if (isAlignment)
        {
            steer = alignment * Time.deltaTime;
        }
        else if (isCohesion)
        {
            steer = cohesion * Time.deltaTime;
        }

        Position += Velocity;
        Velocity += steer;
        
        if (Velocity.magnitude > Flock.MaxSpeed)
        {
            Velocity = (Velocity / Velocity.magnitude) * Flock.MaxSpeed;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (Flock == null)
        {
            return;
        }
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Position, Flock.SeparationWeight);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Position, Flock.AlignmentWeight);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Position, Flock.CohesionWeight);
    }
}
