using System;

public class EntityData {

        public String action;
        public String type;
        public int entityId;
        public float posX;
        public float posY;

        public EntityData() {
            type = "nix";
            entityId = -1;
            posX = 0;
            posY = 0;
        }
    }