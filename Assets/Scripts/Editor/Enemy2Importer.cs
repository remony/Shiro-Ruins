using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using Tiled2Unity;


[Tiled2Unity.CustomTiledImporter]
class Enemy2Importer : Tiled2Unity.ICustomTiledImporter
{

    int id = 0;

    public void HandleCustomProperties(UnityEngine.GameObject gameObject,
        IDictionary<string, string> props)
    {
        // Does this game object have a spawn property?
        if (!props.ContainsKey("spawn"))
            return;

        // Are we spawning an Appearing Block?
        if (props["spawn"] != "Enemy1")
            return;

        // Load the prefab assest and Instantiate it
        string prefabPath = "Assets/Prefabs/Enemy2.prefab";
        UnityEngine.Object spawn =
            AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));
        if (spawn != null)
        {
            GameObject spawnInstance =
                (GameObject)GameObject.Instantiate(spawn);


            spawnInstance.name = "enemy" + id.ToString();
            //spawnInstance.GetComponent<EnemyController>().enemy.health = props["health"].Contains;
            id++;


            // Use the position of the game object we're attached to
            spawnInstance.transform.parent = gameObject.transform;
            spawnInstance.transform.localPosition = new Vector3(0, 14f, 0);
        }
    }

    public void CustomizePrefab(UnityEngine.GameObject prefab)
    {
        // Do nothing
    }
}
