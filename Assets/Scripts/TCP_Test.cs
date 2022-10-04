using UnityEngine;

public class TCP_Test : MonoBehaviour {

    private NetworkManager nm;
    [SerializeField] private EntityManager em;

    private void Start() {
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

