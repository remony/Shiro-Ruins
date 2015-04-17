using UnityEngine;
using System.Collections;

public class LoadAdditive : MonoBehaviour {

	public void loadAddOnClick(int level)
    {
        Application.LoadLevelAdditive(level);
    }
}
