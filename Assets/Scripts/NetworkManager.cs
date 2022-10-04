using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class NetworkManager {

    private static string SERVER_IP = "localhost";
    private static ushort SERVER_PORT = 3999;

    private TcpClient client;
    private Thread clientThread;
    private EntityManager manager;
    private Queue<string> messageBuffer;
    
    public NetworkManager(EntityManager manager) {
        this.manager = manager;
        messageBuffer = new Queue<string>();
    }

    public void Start() {
        clientThread = new Thread(startThread);
        clientThread.Start();
    }

    public void Stop() {
        client.Close();
        clientThread.Abort();
    }

    public void HandleMessages(){
        while(messageBuffer.Count > 0) {
            string hmm = messageBuffer.Dequeue();
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
        while (true) {
            int bytesRead = socket.Read(buffer, 0, buffer.Length);
            string message = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Debug.Log("Received " + bytesRead + "bytes.\nMessage: " + message);    
            messageBuffer.Enqueue(message);
        }
    }

    private void handleMessage(string message) {
        message = message.Split("\n")[0];
        DummyPacket packet = JsonConvert.DeserializeObject<DummyPacket>(message);

        if(packet.packet_type == "world" || packet.packet_type == "welcome") {
            handleWorldUpdate(JsonConvert.DeserializeObject<WorldUpdatePacket>(message));
        } else if(packet.packet_type == "entity_create") {
            handleEntityCreate(JsonConvert.DeserializeObject<EntityCreatePacket>(message));
        } else if(packet.packet_type == "entity_update") {
            handleEntityUpdate(JsonConvert.DeserializeObject<EntityUpdatePacket>(message));
        } else if(packet.packet_type == "entity_destroy") {
            handleEntityDestroy(JsonConvert.DeserializeObject<EntityDestroyPacket>(message));
        } else {
            Debug.Log("Unknown packet type");
            Stop();
        }
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

        Debug.Log("Number Entities " + update.entities.Length);

        foreach(string original in update.entities) {
            var packet = JsonConvert.DeserializeObject<DummyPacket>(original);
            if(packet.packet_type == "entity_create") {
                handleEntityCreate(JsonConvert.DeserializeObject<EntityCreatePacket>(original));
            } else if(packet.packet_type == "entity_update") {
                handleEntityUpdate(JsonConvert.DeserializeObject<EntityUpdatePacket>(original));
            } else if(packet.packet_type == "entity_destroy") {
                handleEntityDestroy(JsonConvert.DeserializeObject<EntityDestroyPacket>(original));
            } else {
                Debug.Log("Unknown packet type");
                Stop();
            }
        }
    }


}