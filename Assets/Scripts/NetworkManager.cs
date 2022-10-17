using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class NetworkManager {

    private static string SERVER_IP = "localhost";
    private static ushort SERVER_PORT = 3399;

    private TcpClient client;
    private Thread clientThread;
    private bool clientRun;

    private EntityManager manager;
    private Queue<string> messageInBuffer;
    private Queue<string> messageOutBuffer;

    public ulong myEntityId { get; private set; }
    public ulong startTick { get; private set; }
    public float startTime { get; private set; }
    public float tickRate = 1 / 20.0f;

    public NetworkManager(EntityManager manager) {
        this.manager = manager;
        messageInBuffer = new Queue<string>();
        messageOutBuffer = new Queue<string>();
    }

    public void Start() {
        clientThread = new Thread(startThread);
        clientThread.Start();
    }

    public void Stop() {
        clientRun = false;
    }

    public void Send(string message) {
        messageOutBuffer.Enqueue(message + "\n");
    }

    public void HandleMessages() {
        while(messageInBuffer.Count > 0) {
            string hmm = messageInBuffer.Dequeue();
            handleMessage(hmm);
        }
        
    }

    private void startThread() {
        client = new TcpClient();
        client.Connect(SERVER_IP, SERVER_PORT);
        Debug.Log("Connected");
        
        byte[] buffer = new byte[4096];
        NetworkStream socket = client.GetStream();


        var data = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new LoginPacket("Bobb", "#ff0000")));
        socket.Write(data, 0, data.Length);
        clientRun = true;
        while (clientRun) {
            if(socket.DataAvailable) {
                int bytesRead = socket.Read(buffer, 0, buffer.Length);
                string message = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                //Debug.Log("Received " + bytesRead + "bytes.\nMessage: " + message);
                messageInBuffer.Enqueue(message);
            }

            while (messageOutBuffer.Count > 0) {
                socket.Write(System.Text.Encoding.UTF8.GetBytes(messageOutBuffer.Dequeue()));
            }
        }
        client.Close();
    }

    private void handleMessage(string message) {
        foreach(string line in message.Split("\n")) {
            if (line.Length <= 0) continue;
            //Debug.Log("Handling Message: " + line);
            DummyPacket packet = JsonConvert.DeserializeObject<DummyPacket>(line);

            if (packet.packet_type == "welcome") {
                handleWelcome(JsonConvert.DeserializeObject<WorldUpdatePacket>(line));
            } else if (packet.packet_type == "entity_create") {
                handleEntityCreate(JsonConvert.DeserializeObject<EntityCreatePacket>(line));
            } else if (packet.packet_type == "entity_update") {
                handleEntityUpdate(JsonConvert.DeserializeObject<EntityUpdatePacket>(line));
            } else if (packet.packet_type == "entity_destroy") {
                handleEntityDestroy(JsonConvert.DeserializeObject<EntityDestroyPacket>(line));
            }  else {
                Debug.Log("Unknown packet type");
                Stop();
            }
        }
    }

    private void handleWelcome(WorldUpdatePacket packet) {
        Debug.Log("Got EntityID");
        myEntityId = packet.player_entity_id;
        startTick = packet.tick;
        startTime = Time.unscaledTime - 0.25f;
    }

    private void handleEntityDestroy(EntityDestroyPacket packet) {
        manager.DestroyEntity(packet.entity_id);
    }

    private void handleEntityUpdate(EntityUpdatePacket packet) {
        manager.UpdateEntity(packet.entity_id, new Vector2(packet.x, packet.y));
    }

    private void handleEntityCreate(EntityCreatePacket packet) {
        manager.SpawnEntity(packet.entity_id, packet.entity_model, Vector2.zero);
    }


}