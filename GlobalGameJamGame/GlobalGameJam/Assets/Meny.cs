using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Meny : MonoBehaviour {


	bool gameStarts = false;
	public Image play;
	public Image options;
	public Image quit;
	float time = 0;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Quit();
		}


		if (gameStarts)
		{
			time += Time.deltaTime;
			play.color = new Color(1, 1, 1, (2 - time) / 2);
			options.color = new Color(1, 1, 1, (2 - time) / 2);
			quit.color = new Color(1, 1, 1, (2 - time) / 2);
			if (time > 2)
			{
				SceneManager.LoadScene("Level01");
			}
		}
	}


	public void startGame()
	{
		gameStarts = true;
	}
	public void Options()
	{
		
	}
	public void Quit()
	{
		Application.Quit();
	}


}
