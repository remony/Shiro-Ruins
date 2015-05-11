using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter]
class EndImporter : Tiled2Unity.ICustomTiledImporter
{


    public void HandleCustomProperties(UnityEngine.GameObject gameObject,
        IDictionary<string, string> props)
    {
        // Does this game object have a spawn property?
        if (!props.ContainsKey("spawn"))
            return;

        // Are we spawning an Appearing Block?
        if (props["spawn"] != "End")
            return;

       

        

        // Load the prefab assest and Instantiate it
        string prefabPath = "Assets/Prefabs/End.prefab";
        UnityEngine.Object spawn =
            AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));
        if (spawn != null)
        {
            GameObject spawnInstance =
                (GameObject)GameObject.Instantiate(spawn);


            spawnInstance.name = "End";
            spawnInstance.tag = "End";


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