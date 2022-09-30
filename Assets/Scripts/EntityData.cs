using System;

public struct LoginPacket {
    String packet_type;
    String username;
    String color;
}

public struct EntityCreatePacket {
    String packet_type;
    ulong entity_id;
    String entity_model;
    float x;
    float y;
    String additional_data;
}

public struct EntityUpdatePacket {
    String packet_type;
    ulong entity_id;
    float x;
    float y;
}

public struct EntityDestroyPacket {
    String packet_type;
    ulong entity_id;
}