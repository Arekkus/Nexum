using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

public struct DummyPacket {
    public string packet_type;
}

public struct WorldUpdatePacket{
    public string packet_type;
    public ulong your_entity_id;
    public JArray entities;
}

public struct PlayerUpdatePacket {
    public string packet_type;
    public float x;
    public float y;
}

public struct LoginPacket {
    public string packet_type;
    public string username;
    public string color;

    public LoginPacket(string username, string color) {
        this.packet_type = "login";
        this.username = username;
        this.color = color;
    }
}

public struct EntityCreatePacket {
    public string packet_type;
    public ulong entity_id;
    public string entity_model;
    public float x;
    public float y;
    public string additional_data;
}

public struct EntityUpdatePacket {
    public string packet_type;
    public ulong entity_id;
    public float x;
    public float y;
}

public struct EntityDestroyPacket {
    public String packet_type;
    public ulong entity_id;
}