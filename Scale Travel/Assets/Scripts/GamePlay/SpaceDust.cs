using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceDust : MonoBehaviour
{
    [SerializeField] float speed;
    private void OnTriggerStay(Collider other) {
        if(other.CompareTag("Player"))
        {
            PlayerMovement.Instance.AddVelocity(transform.forward * speed);
        }
    }
}
