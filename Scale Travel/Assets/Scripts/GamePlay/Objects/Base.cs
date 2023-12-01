using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] Animator anima;

    public static Base Instance;
    float speed = 1;
    float increment = 0, lifetimeIncrement = 0;
    float maxSpeed = 6, maxLifetime;
    void Awake() {Instance = this;}

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.Finish();
        }
    }

    void FixedUpdate(){anima.speed = Mathf.Lerp(anima.speed, speed, Time.deltaTime);}

    public void SetBase(float num)
    {
        increment = (maxSpeed-1) / num;
    }

    public void ReadyToFinish(bool b = true)
    {
        if(b){speed += increment;}
        else {speed = 1f;}

        if(speed >= maxSpeed-0.01f){particle.Play();AudioManager.Instance.Play("allballs");}
        else {particle.Stop();AudioManager.Instance.StopDynamicly("allballs",1f,false);}
    }
}
