using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Hitable
{
    public static Player current;
    [SerializeField] float speed;
    [SerializeField] Transform muzzle;
    [SerializeField] GameObject bullet;
    [SerializeField] [Range(0.0f, 1f)] float shotDelay;
    [SerializeField] int maxHP;
    [SerializeField] float chargeTime = 5;
    [SerializeField] float runSpeed;
    [SerializeField] AnimationCurve runEmissionBuildup;
    [SerializeField] ParticleSystem runParticles;
    [SerializeField] float gravity = -10;
    [SerializeField] MovementState movement = MovementState.Moving;
    [SerializeField] bool mouseControls;

    CharacterController controller;
    float lastShot;
    float chargeRun = 0;
    float yVelocity = 0;

    // Use this for initialization
    void Awake()
    {
        current = this;
        controller = GetComponent<CharacterController>();
        health = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        switch (movement)
        {
            case MovementState.Moving:
                Move();
                break;
            case MovementState.Aiming:
                Aim();
                break;
            case MovementState.Running:
                Run();
                break;
        }
    }

    void Move()
    {
        Vector2 input = Vector2.zero;
        float inputMag = 0;
        if(mouseControls){
            input = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            inputMag = Input.GetButton("Walk")?1:0;
        } else
        {
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            inputMag = input.magnitude;
        }
        if (inputMag > 0.1f || mouseControls)
        {
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg, Vector3.up);
            yVelocity += gravity * Time.deltaTime;
            Vector3 velocity = inputMag * speed * Time.deltaTime * transform.forward;
            velocity.y = yVelocity * Time.deltaTime;
            controller.Move(velocity);
            if (controller.isGrounded)
                yVelocity = 0;
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
            controller.Move(new Vector3(0, yVelocity * Time.deltaTime, 0));
            if(controller.isGrounded)
                yVelocity = 0;
        }
        if (Input.GetButtonDown("Shoot"))
        {
            Shoot();
        }

        if (Input.GetButton("Aim"))
        {
            transitionTo(MovementState.Aiming);
        }
    }

    void Aim()
    {
        yVelocity += gravity * Time.deltaTime;
        controller.Move(new Vector3(0, yVelocity * Time.deltaTime, 0));
        if (controller.isGrounded)
            yVelocity = 0;

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float inputMag = input.magnitude;
        if (inputMag > 0.1f)
        {
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg, Vector3.up);
            chargeRun += Time.deltaTime;
            if (!runParticles.isEmitting)
                runParticles.Play();
            ParticleSystem.EmissionModule m = runParticles.emission;
            m.rateOverTime = runEmissionBuildup.Evaluate(Mathf.Clamp01(chargeRun / chargeTime)) * 50;
        }
        else
        {
            chargeRun = 0;
            if (runParticles.isEmitting)
                runParticles.Stop();
        }
        if (Input.GetButtonDown("Shoot"))
        {
            Shoot();
            chargeRun = -Mathf.Infinity;
            if (runParticles.isEmitting)
                runParticles.Stop();
        }

        if (!Input.GetButton("Aim"))
        {
            if (chargeRun < chargeTime)
            {
                transitionTo(MovementState.Moving);
            }
            else
            {
                transitionTo(MovementState.Running);
            }
        }
    }

    void Run()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float inputMag = input.magnitude;
        if (inputMag > 0.1f)
        {
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg, Vector3.up);
            yVelocity += gravity * Time.deltaTime;
            Vector3 velocity = inputMag * runSpeed * Time.deltaTime * transform.forward;
            velocity.y = yVelocity * Time.deltaTime;
            controller.Move(velocity);
            if (controller.isGrounded)
                yVelocity = 0;
        }
        else
        {
            transitionTo(MovementState.Moving);
        }
        if (Input.GetButton("Aim"))
        {
            transitionTo(MovementState.Aiming);
        }
    }

    void transitionTo(MovementState state)
    {
        movement = state;
        switch (state)
        {
            case MovementState.Moving:
                runParticles.Stop();
                break;
            case MovementState.Aiming:
                chargeRun = 0;
                runParticles.Stop();
                break;
            case MovementState.Running:
                ParticleSystem.EmissionModule m = runParticles.emission;
                m.rateOverTime = 50;
                break;
        }
    }

    public override bool Hit(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //Die I guess
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        return true;
    }

    void Shoot()
    {
        if (Time.time - lastShot < shotDelay)
            return;
        Instantiate(bullet, muzzle.position, muzzle.rotation);
        lastShot = Time.time;
    }
}

enum MovementState{
    Moving,
    Aiming,
    Running
}