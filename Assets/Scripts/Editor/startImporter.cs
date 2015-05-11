using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter]
class StartImporter : Tiled2Unity.ICustomTiledImporter
{


    public void HandleCustomProperties(UnityEngine.GameObject gameObject,
        IDictionary<string, string> props)
    {
        // Does this game object have a spawn property?
        if (!props.ContainsKey("spawn"))
            return;

        // Are we spawning an Appearing Block?
        if (props["spawn"] != "Start")
            return;

       

        

        // Load the prefab assest and Instantiate it
        string prefabPath = "Assets/Prefabs/Start.prefab";
        UnityEngine.Object spawn =
            AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));
        if (spawn != null)
        {
            GameObject spawnInstance =
                (GameObject)GameObject.Instantiate(spawn);


            spawnInstance.name = "Start";
            spawnInstance.tag = "Start";


            // Use the position of the game object we're attached to
            spawnInstance.transform.parent = gameObject.transform;
            spawnInstance.transform.localPosition = Vector3.zero;
        }
    }

    public void CustomizePrefab(UnityEngine.GameObject prefab)
    {
        // Do nothing
    }
}