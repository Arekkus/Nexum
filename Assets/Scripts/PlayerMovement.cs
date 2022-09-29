using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float posX;
    public float posY;


    // Start is called before the first frame update
    void Start()
    {
        setPosition(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.P))
        {
            saveToJson();
        }
    }

    void setPosition(float posX, float posY) {
        this.posX = posX;
        this.posY = posY;
    }

    void Move() { 
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.position = transform.position + new Vector3(h, v, 0) * Time.deltaTime;
        setPosition(transform.position.x, transform.position.y);
    }

    void saveToJson() {
        String posJson = JsonUtility.ToJson(this, true);

        Debug.Log(posJson);
    }
}
