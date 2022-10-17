using UnityEngine;
using Newtonsoft.Json;

public class PlayerEntity : MonoBehaviour {

    [SerializeField]
    private float speedMultiplier = 2;

    void Start() {
    }

    void Update() {
        move();
    }

    void FixedUpdate() {
        float timePassed = (Time.unscaledTime - TCP_Test.nm.startTime);
        ulong ticksPasses = (ulong)(timePassed / TCP_Test.nm.tickRate);
        TCP_Test.nm.Send(JsonConvert.SerializeObject(new PlayerUpdatePacket {
            packet_type = "update",
            tick = TCP_Test.nm.startTick + ticksPasses,
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

