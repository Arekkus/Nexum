using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerEntity : MonoBehaviour {

    [SerializeField]
    private float speedMultiplier = 2;

    void Start() {
    }

    void Update() {
        Move();
    }

    void Move() { 
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.position = transform.position + new Vector3(h, v, 0) * Time.deltaTime * speedMultiplier;
    }

}

