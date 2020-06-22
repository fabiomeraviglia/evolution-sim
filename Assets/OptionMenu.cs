using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{

    public static bool gameIsPaused = false;
    public GameObject speedSlider;
    public GameObject optionMenu;
    public GameObject speedLabel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void Pause()
    {
        optionMenu.SetActive(true);
        speedSlider.GetComponent<Slider>().value = FindObjectOfType<Map>().timescale;

        FindObjectOfType<Map>().timescale = 0;
        gameIsPaused = true;
        DisplaySpeed();
    }

    public void Resume()
    {
        optionMenu.SetActive(false);
        FindObjectOfType<Map>().timescale = speedSlider.GetComponent<Slider>().value;
        gameIsPaused = false;
    }
    public void Restart()
    {
      //  SceneManager.UnloadSceneAsync("MainGame");
        SceneManager.LoadScene("MainGame");
    }
    public void DisplaySpeed()
    {
        float speed = speedSlider.GetComponent<Slider>().value;

        speedLabel.GetComponent<TextMeshProUGUI>().text = "Simulation speed: " + Math.Round(speed, 2) + "x";
    }
}
