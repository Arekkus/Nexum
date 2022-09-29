using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCP_Test : MonoBehaviour
{

    private TcpClient client;

    public GameObject entity;

    private Vector3 entityPos;



    public String ipAddress = "172.18.6.168";
    public int port = 3399;

    [TextArea(20,40)]
    public String json;

    void Start() {
        Thread t = new Thread(Network);
        t.Start();
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.O))
        {
            EntityData data = JsonUtility.FromJson<EntityData> (json);

            switch (data.action)
            {
                case "CREATE":
                    createEntity(data);
                break;
                case "MOVE":
                    moveEntity(data);
                break;
                case "DESTROY":
                    deleteEntity(data);
                break;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            client.Close();
        }
    }

    private void Network() {
        byte[] buffer = new byte[4096];
        client = new TcpClient();
        client.Connect(ipAddress, port);
        while (true)
        {
            int bytesRead = client.GetStream().Read(buffer, 0 ,buffer.Length);
            String xxx = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Debug.Log("bytesRead: "+bytesRead);
            json = xxx;
            Debug.Log(xxx);
        }
    }

    void createEntity(EntityData data) {
        entityPos = new Vector3( data.posX , data.posY, 0);
        GameObject toCreate = Instantiate(entity, entityPos, transform.rotation);
        toCreate.GetComponent<PlayerEntity>().entityId = data.entityId;
        toCreate.GetComponent<PlayerEntity>().type = data.type;
    }

    void moveEntity(EntityData data) {
        GameObject[] toMoves = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in toMoves)
        {
            if (obj.GetComponent<Entity>().entityId == data.entityId)
            {
                obj.transform.position = new Vector3(data.posX , data.posY, 0);
                break;
            }
        }
    }

    void deleteEntity(EntityData data) {
        GameObject[] toMoves = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in toMoves)
        {
            if (obj.GetComponent<Entity>().entityId == data.entityId)
            {
                obj.transform.position = new Vector3(data.posX , data.posY, 0);
                break;
            }
        }
    }

    /*private void Update() {
        if (isConnected)
        {
            HandleCommunication();
        }
    }

 

    public void HandleCommunication() {
        reader = new StreamReader(client.GetStream(), Encoding.ASCII);
        writer = new StreamWriter(client.GetStream(), Encoding.ASCII);

        isConnected = true;
        String data = "banane"; 

         while (isConnected)
         {
            data = Console.ReadLine();
            
            writer.WriteLine(data);
            writer.Flush();
            
         }
    }
*/
}

