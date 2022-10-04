using System.Collections.Generic;
using System;
using UnityEngine;

public class EntityManager : MonoBehaviour { 

    private Dictionary<ulong, Entity> entities;

    [Serializable]
    struct EntityModel {
        public string name;
        public GameObject prefab;
    }

    [SerializeField] private List<EntityModel> prefabs;

    private void Start(){
        entities = new Dictionary<ulong, Entity>();
    }

    public void SpawnEntity(ulong entityId, string entityModel, Vector2 pos) {
        Debug.Log("Creating Entity with ID: " + entityId + " and Model: " + entityModel);
        GameObject prefab = null;
        foreach(var p in prefabs) {
            if(p.name == entityModel) {
                prefab = p.prefab;
            }
        }
        if(prefab == null) {
            Debug.Log("Could not find Model");
            return;
        }

        var entity = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        entities[entityId] = entity.GetComponent<Entity>();
    }

    public void DestroyEntity(ulong entityId) {
        Debug.Log("Destroying Entity with ID: " + entityId);
        var entity = entities[entityId];
        if(entity == null) {
            Debug.Log("Could not find Entity");
            return;
        }
        Destroy(entity.gameObject);
    }

    public void UpdateEntity(ulong entityId, Vector2 pos) {
        Debug.Log("Updating Entity with ID: " + entityId + " and " + pos);
        var entity = entities[entityId];
        if(entity == null) {
            Debug.Log("Could not find Entity");
            return;
        }
        entity.transform.position = pos;
    }

}