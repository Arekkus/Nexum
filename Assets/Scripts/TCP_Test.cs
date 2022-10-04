using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCP_Test : MonoBehaviour {

    private NetworkManager nm;

    private void Start() {
        nm = new NetworkManager();
        nm.Start();
    }

    private void OnDestroy() {
        nm.Stop();
    }

}

