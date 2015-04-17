using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.IO;


public class LevelSelectorController : MonoBehaviour {
    TextAsset data;
    JSONObject saveData;
    Text[] Gui;

	// Use this for initialization
	void Start () {
        //if (checkForSave())
        //{
            Gui = gameObject.GetComponentsInChildren<Text>();
            data = getSaveData();
            saveData = new JSONObject(data.text);
            updateGUI();
       // }
        //else
        //{
        //    changeLevel(1);
        //}
        


        
	}

    private void updateGUI()
    {
        string title = saveData[0].GetField("title").str;
        Gui[0].text = title + "\n\n Time: " + saveData[0].GetField("time") + "\nScore: " + saveData[0].GetField("score") + "\nPercentage Restored: " + saveData[0].GetField("percentage");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private TextAsset getSaveData()
    {
        return Resources.Load("Save/Save") as TextAsset;   
    }

    private bool checkForSave()
    {

        if (File.Exists("/Resources/Save/Save.JSON"))
        {
            print("The file exists.");
            return true;
            
        }
        return false;
    }


    public void changeLevel(int id)
    {
        GameManager.instance.changeLevel(id);
    }
}
