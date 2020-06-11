using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarEngine : MonoBehaviour
{

    public Transform path;

    private List<Transform> nodes;

    private int currentNode = 0;

    public float maxSteerAngle = 60f;

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    public float maxMotorTorque = 80f;
    public float maxBreakTorque = 150f;
    public float maxSpeed = 200f;
    public float currentSpeed;
    public float turnSpeed = 5f;

    private bool isBraking = false;
    private bool isAvoiding = false;
    private float targetSteerAngle = 0;

    [Header("Sensors")]
    public float sensorLength = 10f;
    public Vector3 frontSensorPosition = new Vector3(0, 0, 0.5f);
    public float sideSensorPosition = 0.8f;
    public float sensorAngle = 30f;

    // Start is called before the first frame update
    void Start()
    {
        Transform[] pathTransform = path.GetComponentsInChildren<Transform>();

        nodes = new List<Transform>();

        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (path.transform != pathTransform[i])
            {
                nodes.Add(pathTransform[i]);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Sensors();
        ApplySteer();
        Drive();
        CheckWaypointDistance();
        Braking();
        LerpToSteerAngle();
    }

    private void ApplySteer()
    {
        if (!isAvoiding)
        {
            Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);

            float newSteer = relativeVector.x / relativeVector.magnitude * maxSteerAngle;

            targetSteerAngle = newSteer;
        }
    }

    private void Drive()
    {
        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;

        if (currentSpeed < maxSpeed && !isBraking)
        {
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;
            wheelRL.motorTorque = maxMotorTorque;
            wheelRR.motorTorque = maxMotorTorque;
        }
        else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
            wheelRL.motorTorque = 0;
            wheelRR.motorTorque = 0;
        }
    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 1f)
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }

    private void Braking()
    {
        if (isBraking)
        {
            wheelRL.brakeTorque = maxBreakTorque;
            wheelRR.brakeTorque = maxBreakTorque;
        }
        else
        {
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
        }
    }
    
    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;

        float avoidMultiplier = 0;
        isAvoiding = false;

        // front right sensor
        sensorStartPos += transform.right * sideSensorPosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            isAvoiding = true;
            avoidMultiplier -= 1f;
        }

        // front right angled sensor
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(sensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            isAvoiding = true;
            avoidMultiplier -= 0.5f;
        }

        // front left sensor
        sensorStartPos -= transform.right * 2 * sideSensorPosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            isAvoiding = true;
            avoidMultiplier += 1f;
        }

        // front left angled sensor
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-sensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            isAvoiding = true;
            avoidMultiplier += 0.5f;
        }

        // front center sensor
        if (avoidMultiplier == 0)
        {
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                isAvoiding = true;
                if(hit.normal.x < 0)
                {
                    avoidMultiplier = -1;
                } else
                {
                    avoidMultiplier = 1; 
                }
            }
        }

        if(isAvoiding)
        {
            targetSteerAngle = maxSteerAngle * avoidMultiplier;
        }
    }

    private void LerpToSteerAngle()
    {
        wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
        wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
    }
}
