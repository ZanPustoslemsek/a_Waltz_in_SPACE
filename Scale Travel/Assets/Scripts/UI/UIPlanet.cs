using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIPlanet : MonoBehaviour
{
    [HideInInspector] public float radius = 0;
    public MeshRenderer mesh;
    Vector3 baseScale;
    [SerializeField] float minRadius = 24, maxRadius = 48;

    float timeToLerp = 1; //lerp for two seconds.
    float timeLerped = 0.0f;

    private void OnEnable() {
        radius = minRadius;
        mesh.transform.parent.eulerAngles = new Vector3(Random.Range(0f,360f), Random.Range(0f,360f), Random.Range(0f,360f));
        StartC();
    }
    void StartC()
    {
        if(gameObject.activeSelf)StartCoroutine(UpdateCo());
    }

    public void MakeBig()
    {
        transform.localScale = new Vector3(1f,1f,1f) * radius * 2;
        radius = maxRadius;
        timeLerped = 0f;
    }

    public void MakeSmall()
    {
        transform.localScale = new Vector3(1f,1f,1f) * radius * 2;
        radius = minRadius;
        timeLerped = 0f;
    }

    IEnumerator UpdateCo()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(0f);
            timeLerped += Time.unscaledDeltaTime;
            if(gameObject.activeSelf)transform.localScale = Vector3.Lerp (transform.localScale, new Vector3(1f,1f,1f) * radius * 2, timeLerped/timeToLerp);
        }
    }

    // private void Update() {
    //         transform.localScale = Vector3.Lerp (transform.localScale, new Vector3(1f,1f,1f) * radius * 2, Time.deltaTime);
    // }
    public void AddMaterial(Material outline)
    {
        Material[] mats = mesh.materials;
        Material[] newMaterials = new Material[mats.Length + 1];
        for(int i = 0;i<mats.Length;i++){newMaterials[i] = mats[i];}
        newMaterials[mats.Length] = outline;
        mesh.materials = newMaterials;
    }

    public void SetFirstMaterial(Material mat)
    {
        Material[] mats = mesh.materials;
        Material[] newMaterials = new Material[mats.Length];
        for(int i = 0;i<mats.Length;i++){newMaterials[i] = mats[i];}
        newMaterials[0] = mat;
        mesh.materials = newMaterials;
    }

    public void RemoveMaterial()
    {
        Material[] mats = mesh.materials;
        Material[] newMaterials = new Material[mats.Length - 1];
        for(int i = 0;i<mats.Length - 1;i++){newMaterials[i] = mats[i];}
        mesh.materials = newMaterials;
    }
}
