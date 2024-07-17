using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{

	//transition animator visible in the inspector
	public Animator transition;

	public void Back()
	{
		//go back to the main menu
		StartCoroutine(GoBack());
	}

	IEnumerator GoBack()
	{
		//wait for the transition and load the very first scene
		transition.SetTrigger("fade");

		yield return new WaitForSeconds(0.5f);

		SceneManager.LoadScene(0);
	}
}
