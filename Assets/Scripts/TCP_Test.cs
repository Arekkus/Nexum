using UnityEngine;

public class TCP_Test : MonoBehaviour {

    public static NetworkManager nm { get; private set; }

    [SerializeField] private EntityManager em;

    private void Awake() {
        nm = new NetworkManager(em);
        nm.Start();
    }

    private void Update() {
        nm.HandleMessages();
    }

    private void OnDestroy() {
        nm.Stop();
    }

}

