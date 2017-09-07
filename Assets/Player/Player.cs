using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Hitable
{
    public static bool mouseControls = false;
    public static Player current;
    public float speed;
    public Transform muzzle;
    public GameObject bullet;
    [Range(0.0f, 1f)] public float shotDelay;
    public int maxHP;
    public float gravity = -10;

    CharacterController controller;
    float lastShot;
    float yVelocity = 0;

    //this part is only for charging
    Bullet chargingBullet;

    void Awake()
    {
        current = this;
        controller = GetComponent<CharacterController>();
        health = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
    }

    void Move(){
        Vector2 input = Vector2.zero;
        float inputMag = 0;
        if (mouseControls)
        {
            input = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            inputMag = Input.GetButton("Walk") ? 1 : 0;
        }
        else
        {
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            inputMag = input.magnitude;
        }
        if (inputMag > 0.1f || mouseControls)
        {
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg, Vector3.up);
            yVelocity += gravity * Time.deltaTime;
            Vector3 velocity = transform.forward * (inputMag * speed * Time.deltaTime * (Input.GetButton("Aim")?0:1));
            velocity.y = yVelocity * Time.deltaTime;
            controller.Move(velocity);
            if (controller.isGrounded)
                yVelocity = 0;
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
            controller.Move(new Vector3(0, yVelocity * Time.deltaTime, 0));
            if (controller.isGrounded)
                yVelocity = 0;
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