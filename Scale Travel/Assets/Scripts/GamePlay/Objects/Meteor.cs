using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private Rigidbody rb;
    private Planet[] planets;
    [SerializeField] float mass = 0.2f;

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        planets = FindObjectsOfType<Planet>();
    }

    private void FixedUpdate() 
    {
        GetPlanetForces();
        VelocityCap();
    }

    void VelocityCap()
    {
        float magnitude = rb.velocity.magnitude;
        if(magnitude > Konst.maxMagMeteor)
        {
            rb.velocity = (rb.velocity / magnitude) * Konst.maxMagMeteor;
        }
    }

    float GravForce(Planet pl)
    {
        float dist = Vector3.Distance(this.transform.position, pl.transform.position);
        return Konst.G * mass * pl.mass / (dist*dist);
    }

    void GetPlanetForces()
    {
        foreach(Planet pl in planets)
        {
            rb.AddForce((pl.transform.position - transform.position)*GravForce(pl));
        }
    }
}
