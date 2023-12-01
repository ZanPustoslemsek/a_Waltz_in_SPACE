using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [HideInInspector] public float radius = 1;
    float defRadius;
    public float mass = 1;
    [SerializeField] MeshRenderer mesh;

    [SerializeField] Planet upPlanet, downPlanet, leftPlanet, rightPlanet;

    public int number;

    private void Start() {
        radius = transform.localScale.x/2f;
        defRadius = radius;
        SetMass();
        mesh.transform.parent.eulerAngles = new Vector3(Random.Range(0f,360f), Random.Range(0f,360f), Random.Range(0f,360f));
    }

    public void ResetPlanet(bool instant = false)
    {
        radius = defRadius;
        SetMass();
        if(instant) transform.localScale = new Vector3(1f,1f,1f) * radius * 2;
    }

    bool isControlled = false;
    public void OutsideControll(bool b = true){isControlled = b;}

    void Update()
    {
        if(!isControlled)transform.localScale = Vector3.Lerp (transform.localScale, new Vector3(1f,1f,1f) * radius * 2, Time.deltaTime * Konst.scaleUpSpeed);
    }

    void SetMass()
    {
        mass = 4f * Mathf.PI * radius * radius * radius / 3f;
    }

    public void ChangeSizeMouse(float change)
    {
        radius += change*Time.deltaTime*Konst.scrollK;
        if(radius > Konst.maxRadius){radius = Konst.maxRadius;}
        else if(radius < Konst.minRadius){radius = Konst.minRadius;}
        SetMass();
    } 

    public void ChangeSizeKey(float change)
    {
        radius += change*Konst.keyScrollK;
        if(radius > Konst.maxRadius){radius = Konst.maxRadius;}
        else if(radius < Konst.minRadius){radius = Konst.minRadius;}
        SetMass();
    }

    public void AddMaterial(Material outline)
    {
        Material[] mats = mesh.materials;
        Material[] newMaterials = new Material[mats.Length + 1];
        for(int i = 0;i<mats.Length;i++){newMaterials[i] = mats[i];}
        newMaterials[mats.Length] = outline;
        mesh.materials = newMaterials;
    }

    public void RemoveMaterial()
    {
        Material[] mats = mesh.materials;
        Material[] newMaterials = new Material[mats.Length - 1];
        for(int i = 0;i<mats.Length - 1;i++){newMaterials[i] = mats[i];}
        mesh.materials = newMaterials;
    }

    public Planet GetNextPlanet(Vector2 input)
    {
        if(input.x == -1f)
        {
            if(leftPlanet)return leftPlanet;
            else return this;
        }
        else if(input.x == 1f)
        {
            if(rightPlanet)return rightPlanet;
            else return this;
        }
        else if(input.y == -1f)
        {
            if(downPlanet)return downPlanet;
            else return this;
        }
        else if(input.y == 1f)
        {
            if(upPlanet)return upPlanet;
            else return this;
        }
        return this;
    }

    public Planet GetNextPlanet(float input)
    {
        if(input < 0)
        {
            Planet[] planets = FindObjectsOfType<Planet>();
            int targetInt = (planets.Length + number - 1) % planets.Length;
            foreach(Planet pl in planets)
            {
                if(pl.number == targetInt){return pl;}
            }
            return this;
        }
        else
        {
            Planet[] planets = FindObjectsOfType<Planet>();
            int targetInt = (planets.Length + number + 1) % planets.Length;
            foreach(Planet pl in planets)
            {
                if(pl.number == targetInt){return pl;}
            }
            return this;
        }
    }

}
