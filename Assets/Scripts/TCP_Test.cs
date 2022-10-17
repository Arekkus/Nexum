using UnityEngine;

public class TCP_Test : MonoBehaviour {

    public static NetworkManager nm { get; private set; }

    [SerializeField] private EntityManager em;

    void Awake() {
        nm = new NetworkManager(em);
        nm.Start();
    }

    void Update() {
        nm.HandleMessages();
    }

    void FixedUpdate() {
    }

    void OnDestroy() {
        nm.Stop();
    }

}

