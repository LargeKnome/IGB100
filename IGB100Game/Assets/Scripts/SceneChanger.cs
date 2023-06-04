using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
	public void ChangeScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
	public void Exit()
	{
		Application.Quit();
	}
}
