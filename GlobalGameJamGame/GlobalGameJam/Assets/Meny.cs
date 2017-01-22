using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Meny : MonoBehaviour {


	bool gameStarts = false;
	public Image play;
	public Image quit;
	public Image logo;
	public Image fadeImage;
	float time = 0;
	float startuptime = 0;

	public Image Firmalogo;
	public Image fadetoblack;

	public Transform players;
	AudioSource backgrundsound;

	// Use this for initialization
	void Start ()
	{
		backgrundsound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		startuptime += Time.deltaTime;
		//fadetoblack.color = new Color(1, 1, 1, startuptime);
		if (startuptime < 1)
		{
			
			fadetoblack.color = new Color(1, 1, 1, 1-startuptime);
		}
		else if (startuptime > 2 && startuptime < 4)
		{
			fadetoblack.color = new Color(1, 1, 1, startuptime-2);
		}
		else if (startuptime > 3 && startuptime < 5)
		{
			Firmalogo.color = new Color(1, 1, 1, 0);
		}
		else if (startuptime > 4)
		{
			backgrundsound.Play();
			players.position = new Vector3(0, 0, 0);
			fadetoblack.color = new Color(1, 1, 1, 6 - startuptime);
		}



		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Quit();
		}


		if (gameStarts)
		{
			time += Time.deltaTime;
			fadeImage.color = new Color(1, 1, 1, (2 - time) / 2);
			play.color = new Color(1 , 1, 1, (2 - time) / 2);
			logo.color = new Color(1, 1, 1, (2 - time) / 2);
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
