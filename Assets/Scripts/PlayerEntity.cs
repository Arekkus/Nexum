using UnityEngine;
using Newtonsoft.Json;

public class PlayerEntity : MonoBehaviour {

    [SerializeField]
    private float speedMultiplier = 2;

    void Start() {
    }

    void Update() {
        move();
        TCP_Test.nm.Send(JsonConvert.SerializeObject(new PlayerUpdatePacket {
            packet_type = "update",
            x = transform.position.x,
            y = transform.position.y
        }));
    }

    private void move() { 
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.position = transform.position + new Vector3(h, v, 0) * Time.deltaTime * speedMultiplier;
    }

}

