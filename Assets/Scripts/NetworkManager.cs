using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class NetworkManager {

    private static string SERVER_IP = "192.168.1.250";
    private static ushort SERVER_PORT = 3999;

    private TcpClient client;
    private Thread clientThread;
    private bool clientRun;

    private EntityManager manager;
    private Queue<string> messageInBuffer;
    private Queue<string> messageOutBuffer;
    public ulong myEntityId { get; private set; }

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
        messageOutBuffer.Enqueue(message);
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
                // Debug.Log("Received " + bytesRead + "bytes.\nMessage: " + message);
                messageInBuffer.Enqueue(message);
            }

            while (messageOutBuffer.Count > 0) {
                socket.Write(System.Text.Encoding.UTF8.GetBytes(messageOutBuffer.Dequeue()));
            }
        }
        client.Close();
    }

    private void handleMessage(string message) {
        message = message.Split("\n")[0];
        DummyPacket packet = JsonConvert.DeserializeObject<DummyPacket>(message);

        if (packet.packet_type == "world") {
            handleWorldUpdate(JsonConvert.DeserializeObject<WorldUpdatePacket>(message));
        } else if (packet.packet_type == "entity_create") {
            handleEntityCreate(JsonConvert.DeserializeObject<EntityCreatePacket>(message));
        } else if (packet.packet_type == "entity_update") {
            handleEntityUpdate(JsonConvert.DeserializeObject<EntityUpdatePacket>(message));
        } else if (packet.packet_type == "entity_destroy") {
            handleEntityDestroy(JsonConvert.DeserializeObject<EntityDestroyPacket>(message));
        } else if (packet.packet_type == "welcome") {
            handleWelcome(JsonConvert.DeserializeObject<WorldUpdatePacket>(message));
        } else {
            Debug.Log("Unknown packet type");
            Stop();
        }
    }

    private void handleWelcome(WorldUpdatePacket packet) {
        Debug.Log("Got EntityID");
        myEntityId = packet.your_entity_id;
        handleWorldUpdate(packet);
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

    private void handleWorldUpdate(WorldUpdatePacket update) {
        if(update.entities == null) {
            Debug.Log("No Entities");
            return;
        }

        foreach(var token in update.entities) {
            string original = token.ToString();
            var packet = JsonConvert.DeserializeObject<DummyPacket>(original);
            if (packet.packet_type == "entity_create") {
                handleEntityCreate(JsonConvert.DeserializeObject<EntityCreatePacket>(original));
            } else if (packet.packet_type == "entity_update") {
                handleEntityUpdate(JsonConvert.DeserializeObject<EntityUpdatePacket>(original));
            } else if (packet.packet_type == "entity_destroy") {
                handleEntityDestroy(JsonConvert.DeserializeObject<EntityDestroyPacket>(original));
            } else {
                Debug.Log("Unknown packet type");
                Stop();
            }
        }
    }


}