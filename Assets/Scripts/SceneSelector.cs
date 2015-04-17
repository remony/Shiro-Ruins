﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class SceneSelector : MonoBehaviour
{
    private static bool TextIsActive = true;

    // Takes for granted we won't have more than 9 levels
    public List<String> scenes = new List<string>();

    private void Start()
    {
        // Set up the string we will display
        StringBuilder msg = new StringBuilder();
        //msg.AppendLine("Controls:");
        //msg.AppendLine("  Move: arrow keys");
        //msg.AppendLine("  Jump: space");
        //msg.AppendLine("Select scene by numeric key:");
        //for (int i = 0; i < this.scenes.Count(); ++i)
        //{
        //    msg.AppendFormat("  {0} : {1}\n", i+1, this.scenes[i]);
        //}
        //msg.AppendLine("(Toggle Hide/Show this message 'T')");
        msg.AppendLine("Use numeric keys to change scene [1-6]");

        Transform text = this.transform.FindChild("TextChildren/TextMain");
        text.GetComponent<GUIText>().text = msg.ToString();

        // Create text children to surrond us but with contrasting color so it stands out
        Vector3 childPosition = text.transform.position;
        childPosition.z = 0;

        Vector2[] offsets = new Vector2[8];
        offsets[0].x =  0;  offsets[0].y =  1;
        offsets[1].x =  1;  offsets[1].y =  1;
        offsets[2].x =  1;  offsets[2].y =  0;
        offsets[3].x =  1;  offsets[3].y = -1; 
        offsets[4].x =  0;  offsets[4].y = -1;
        offsets[5].x = -1;  offsets[5].y = -1;
        offsets[6].x = -1;  offsets[6].y =  0;
        offsets[7].x = -1;  offsets[7].y =  1;

        for (int i = 0; i < offsets.Length; ++i)
        {
            Transform child = (Transform)GameObject.Instantiate(text);
            child.name = String.Format("Text_{0}", i);
            child.parent = text.transform.parent;
            child.transform.localPosition = childPosition;
            GUIText guiText = child.GetComponent<GUIText>();
            guiText.color = new Color32(0, 38, 255, 255);
            guiText.pixelOffset = offsets[i];
        }

        // Do we start off with text active?
        Transform allMyChildren = this.transform.Find("TextChildren");
        allMyChildren.gameObject.SetActive(TextIsActive);
    }

    private void Update()
    {
        for (int i = 0; i < this.scenes.Count(); ++i)
        {
            string alpha = String.Format("Alpha{0}", i+1);
            string keypad = String.Format("Keypad{0}", i+1);

            KeyCode codeAlpha = (KeyCode)Enum.Parse(typeof(KeyCode), alpha);
            KeyCode codeKeypad = (KeyCode)Enum.Parse(typeof(KeyCode), keypad);

            if (Input.GetKeyUp(codeAlpha) || Input.GetKeyUp(codeKeypad))
            {
                Application.LoadLevel(this.scenes[i]);
                break;
            }
        }

        //if (Input.GetKeyUp(KeyCode.T))
        //{
        //    Transform children = this.transform.Find("TextChildren");
        //    TextIsActive = !TextIsActive;
        //    children.gameObject.SetActive(TextIsActive);
        //}

    }



}
