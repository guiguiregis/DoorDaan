using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour {

    public GameObject characterSelection;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowCharaBtn()
    {
        characterSelection.SetActive(true);
    }

    public void GoToFight()
    {
        SceneManager.LoadScene("Level0");
    }


}
