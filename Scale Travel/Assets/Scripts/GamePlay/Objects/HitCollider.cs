using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    [HideInInspector]
    public bool destroy = true;
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.transform.CompareTag("Player"))
        {
            if(destroy)GameManager.Instance.DestroyPlayer();
            Animator anima = GetComponentInChildren<Animator>();
            if(anima)anima.SetTrigger("shake");
        }
    }
}
