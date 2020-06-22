using System;
using UnityEngine;

public class PhysicalProperties
{
    public PhysicalProperties()
    {
        Velocity = new Vector3();
        AngularVelocity = 0;
    }
    
    public Vector2 Velocity { get; set; }
    
    public float AngularVelocity { get; set; }

    internal void Update(Rigidbody2D rigidbody2D)
    {
        Velocity = rigidbody2D.velocity;
        AngularVelocity = rigidbody2D.angularVelocity;

    }
    public void CombineWithRigidbody(Rigidbody2D rigidbody2D, float weigth)
    {
        rigidbody2D.velocity = rigidbody2D.velocity*(1-weigth)+Velocity*weigth;
        rigidbody2D.angularVelocity= rigidbody2D.angularVelocity * (1 - weigth) + AngularVelocity * weigth;
    }
}