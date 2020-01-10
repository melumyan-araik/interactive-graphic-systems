using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine.SceneManagement;


public class MyGameManager : MonoBehaviour {

	public void RestartGame()
	{
		SceneManager.LoadScene("ChessGame");
	}
		
	public void ExitGame()
	{
		Application.Quit();
	}

	void Update()
	{
		if (Input.GetKey("escape"))
		{
            Application.Quit();			
		}
	}



}
