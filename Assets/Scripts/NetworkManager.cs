using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager {

    private static string SERVER_IP = "localhost";
    private static ushort SERVER_PORT = 3999;

    private TcpClient client;
    private Thread clientThread;
    
    public NetworkManager() {

    }

    public void Start() {
        clientThread = new Thread(startThread);
        clientThread.Start();
    }

    public void Stop() {
        client.Close();
        clientThread.Abort();
    }

    private void startThread() {
        client = new TcpClient();
        client.Connect(SERVER_IP, SERVER_PORT);
        Debug.Log("Connected");
        
        byte[] buffer = new byte[4096];
        NetworkStream socket = client.GetStream();


        var data = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new LoginPacket("Bobb", "#ff0000")));
        socket.Write(data, 0, data.Length);
        while (true) {
            int bytesRead = socket.Read(buffer, 0, buffer.Length);
            string message = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Debug.Log("Received " + bytesRead + "bytes.\nMessage: " + message);
            handleMessage(message);
        }
    }

    private void handleMessage(string message) {
        
    }


}