using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter]
class ItemImporter : Tiled2Unity.ICustomTiledImporter
{

    int id = 0;
    int value = 100;

    public void HandleCustomProperties(UnityEngine.GameObject gameObject,
        IDictionary<string, string> props)
    {
        // Does this game object have a spawn property?
        if (!props.ContainsKey("spawn"))
            return;

        // Are we spawning an Appearing Block?
        if (props["spawn"] != "item")
            return;

        

        // Load the prefab assest and Instantiate it
        string prefabPath = "Assets/Prefabs/Item.prefab";
        UnityEngine.Object spawn =
            AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));
        if (spawn != null)
        {
            GameObject spawnInstance =
                (GameObject)GameObject.Instantiate(spawn);


            spawnInstance.name = "item" + id.ToString();
            

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