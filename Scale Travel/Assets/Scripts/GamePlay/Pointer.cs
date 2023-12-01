using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField]float upper_bound_velocity1, upper_bound_velocity2;
    [SerializeField]float lower_bound_velocity1, lower_bound_velocity2;

    [SerializeField] GameObject pointer1, pointer2;

    public void SetRotation(Vector3 velocity)
    {
        transform.LookAt(transform.position + velocity);
    }

}
