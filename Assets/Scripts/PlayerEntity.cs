using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerEntity : Entity
{

    [SerializeField]
    private float speedMultiplier = 2;

    // Start is called before the first frame update
    void Start()
    {
        setPositionData(transform.position.x, transform.position.y);
        type = "Player";
        gameObject.tag = type;
    }

    // Update is called once per frame
    void Update()
    {
        if (entityId == 1)
        {
            Move();
        } 
        if (Input.GetKeyDown(KeyCode.P))
        {
            saveToJson();
        }
    }

    void setPositionData(float posX, float posY) {
        this.posX = posX;
        this.posY = posY;
    }

    void Move() { 
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.position = transform.position + new Vector3(h, v, 0) * Time.deltaTime * speedMultiplier;
        setPositionData(transform.position.x, transform.position.y);
    }

    void saveToJson() {
        String posJson = JsonUtility.ToJson(this, true);

        Debug.Log(posJson);
    }
}

