using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnCollisson : MonoBehaviour
{
    private void OnParticleCollision(GameObject other) 
    {
        Debug.Log("hit");
        if(other.CompareTag("particle"))
        {
            AudioManager.Instance.Play("firework");
        }
    }
}
