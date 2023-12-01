using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] Animator anima;
    private Planet[] planets;
    [SerializeField] float mass = 1;
    [SerializeField] float baseSpeed = 5;
    [SerializeField] float boostSpeed = 10;
    [SerializeField] float boostTime = 3f, cooldownTime = 5f;
    float rotationspeed = 100f;
    float boostMultiply = 1;
    private bool canBoost = true;
    public bool isBoosting, canTurn = true;
    [SerializeField] ParticleSystem fire,canBoostFire;
    [SerializeField] Pointer pointer;
    private float speed;
    private Vector3 previousVelocity = new Vector3(0f,0f,0f), addedVelocity = new Vector3(0f,0f,0f), previouslyAddedVelocity = new Vector3(0f,0f,0f), previouslyRemovedVelocity = Vector3.zero;
    
    bool startDelay = false;

    public static PlayerMovement Instance;

    private void Awake() {
        Instance = this;
    }

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        speed = baseSpeed;
        canBoost = true;
        canTurn = true;
        rotationspeed = 50f;


        planets = FindObjectsOfType<Planet>();

        startDelay = false;
        Invoke("StartDelay", 1f);
    }

    void StartDelay()
    {
        startDelay = true;
        canBoostFire.Play();
        AudioManager.Instance.PlayDynamicly("baseFire",1f);
    }

    private void FixedUpdate() 
    {
        if(!startDelay)return;
        if(!canTurn)previousVelocity = previousVelocity.normalized * Mathf.Min(previousVelocity.magnitude, rb.velocity.magnitude);
        rb.velocity -= previousVelocity;
        rb.velocity -= previouslyAddedVelocity;
        rb.velocity += previouslyRemovedVelocity;
        GetPlanetForces();
        VelocityCap();
        
        rb.velocity += transform.forward * speed;
        previousVelocity = transform.forward * speed;
        
        SetRotation();

        rb.velocity += addedVelocity;
        previouslyAddedVelocity = addedVelocity;
        addedVelocity = Vector3.zero;

        CheckBoundry();        
    }

    void CheckBoundry()
    {
        Vector3 targetPos = transform.position, targetVel = Vector3.zero;
        if(transform.position.x >= GameManager.Instance.boundry.x) {targetPos = new Vector3(GameManager.Instance.boundry.x,targetPos.y,targetPos.z); targetVel += new Vector3(-rb.velocity.x,0,0);}
        if(transform.position.x <= -GameManager.Instance.boundry.x) {targetPos = new Vector3(-GameManager.Instance.boundry.x,targetPos.y,targetPos.z); targetVel += new Vector3(-rb.velocity.x,0,0);}
        if(transform.position.z >= GameManager.Instance.boundry.y) {targetPos = new Vector3(targetPos.x, targetPos.y, GameManager.Instance.boundry.y); targetVel += new Vector3(0,0,-rb.velocity.z);}
        if(transform.position.z <= -GameManager.Instance.boundry.y) {targetPos = new Vector3(targetPos.x, targetPos.y, -GameManager.Instance.boundry.y); targetVel += new Vector3(0,0,-rb.velocity.z);}

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime*5f);
        previouslyRemovedVelocity = targetVel;
        rb.velocity -= previouslyRemovedVelocity;
    }

    void VelocityCap()
    {
        float magnitude = rb.velocity.magnitude;
        if(magnitude > Konst.maxMag)
        {
            rb.velocity = (rb.velocity / magnitude) * Konst.maxMag;
        }
    }

    void SetRotation()
    {

        pointer.SetRotation(rb.velocity);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, pointer.transform.eulerAngles, Time.deltaTime*50f);
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
            rb.AddForce((pl.transform.position - transform.position)*GravForce(pl)*boostMultiply);
        }
    }

    public void AddVelocity(Vector3 vel)
    {
        if(startDelay)
        {
            if(previouslyAddedVelocity.magnitude < 0.01f)
            {
                addedVelocity = vel * 0.2f;
            }
            else if(previouslyAddedVelocity.magnitude >= vel.magnitude - 0.01f)
            {
                addedVelocity += vel;
            }
            else
            {
                addedVelocity = vel.normalized * previouslyAddedVelocity.magnitude*2;
            }

        }
    }

    public void StartBoost()
    {
        if(canBoost)StartCoroutine("Boost");
    }
    IEnumerator Boost()
    {
        canBoost = false;
        canTurn = false;
        speed = boostSpeed;
        isBoosting = true;
        boostMultiply = 0.3f;
        canBoostFire.Stop();
        AudioManager.Instance.PlayDynamicly("boostFire",0.5f);
        AudioManager.Instance.Stop("baseFire");

        fire.Play();
        yield return new WaitForSeconds(boostTime);
        fire.Stop();
        AudioManager.Instance.StopDynamicly("boostFire",0.5f);
        
        while(speed > baseSpeed && boostMultiply < 1){speed -= Time.deltaTime * 2; boostMultiply += Time.deltaTime * 0.2f; yield return  new WaitForSeconds(0f);}
        speed = baseSpeed;
        boostMultiply = 1f;
        isBoosting = false;

        yield return new WaitForSeconds(1f);
        canTurn = true;
        yield return new WaitForSeconds(cooldownTime-1);
        canBoost = true;
        canBoostFire.Play();
        AudioManager.Instance.PlayDynamicly("baseFire",0.5f);
    }

    public void Finish()
    {
        startDelay = false;
        canBoostFire.Stop();
    
        rb.velocity = (-this.transform.position + Base.Instance.transform.position) * 2f;
        anima.SetBool("finish",true);
        AudioManager.Instance.StopDynamicly("baseFire", 1f);
        AudioManager.Instance.StopDynamicly("boostFire", 1f);
        canBoostFire.Stop();
        fire.Stop();

        Invoke("ResetVelocity",0.5f);
    }
    void ResetVelocity()
    {
        rb.velocity = Vector3.zero;
    }

}
