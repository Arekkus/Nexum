using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class NetworkManager {

    private static string SERVER_IP = "localhost";
    private static ushort SERVER_PORT = 3999;

    private TcpClient client;
    
    public NetworkManager() {

    }

    public void Start() {
        Thread thread = new Thread(startThread);
        thread.Start();
    }

    private void startThread() {
        client = new TcpClient();
        client.Connect(SERVER_IP, SERVER_PORT);
        
        byte[] buffer = new byte[4096];
        NetworkStream socket = client.GetStream();
        while(true) {
            int bytesRead = socket.Read(buffer, 0, buffer.Length);
            string message = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Debug.Log("Received " + bytesRead + "bytes.\nMessage: " + message);
            handleMessage(message);
        }
    }

    private void handleMessage(string message) {
        JsonUtility.FromJson(message);
    }


}