using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPickUp : MonoBehaviour
{
    public GameObject player;
    public CarController playerCar;

    private void OnTriggerEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            playerCar = player.GetComponentInParent<CarController>();

            playerCar.speedPickUp();

            Destroy(this.gameObject);
        }
    }
}
