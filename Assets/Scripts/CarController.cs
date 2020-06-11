using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class CarController : MonoBehaviour
{
    public InputManager manager;
    public List<WheelCollider> throttleWheels;
    public List<WheelCollider> steerWheels;
    public List<GameObject> steerWheelsVis;

    public float strengthCoefficient = 2000f;
    public float maxTurn = 30f;
    public float speedMultiplier = 1f;
    public float speed;

    public bool speedPickedUp;
    public float speedTimer;
    public float speedPickUpLimit;

    void Start()
    {
        manager = GetComponent<InputManager>();
    }

    void FixedUpdate()
    {
        foreach (WheelCollider wheel in throttleWheels)
        {
            wheel.motorTorque = strengthCoefficient * speedMultiplier * Time.deltaTime * manager.throttle;

        }

        foreach (WheelCollider wheel in steerWheels)
        {
            wheel.steerAngle = maxTurn * manager.steer;

        }

        if(manager.brake > 0)
        {
            foreach (WheelCollider wheel in throttleWheels)
            {
                wheel.brakeTorque = strengthCoefficient * Time.deltaTime;
            }
        } else {
            foreach (WheelCollider wheel in throttleWheels)
            {
                wheel.brakeTorque = 0;
            }
        }

        if(speedPickedUp)
        {
            if(speedTimer < speedPickUpLimit)
            {
                speedMultiplier = 2f;
                speedTimer = speedTimer + Time.deltaTime;
            } else
            {
                speedMultiplier = 1f;
                speedTimer = 0;
            }
        }

        /* foreach (GameObject wheel in steerWheelsVis)
        {
            Quaternion wheelRotate = new Quaternion(180, wheel.transform.rotation.y + maxTurn * manager.steer, 0, 0);

            wheel.transform.rotation = wheelRotate;
        } */

    }

    public void speedPickUp()
    {
        speedPickedUp = true;
    }

}
