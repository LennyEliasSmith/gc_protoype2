using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject cameraTarget;
    public Camera carCam;

    // Start is called before the first frame update
    void Start()
    {
        carCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
