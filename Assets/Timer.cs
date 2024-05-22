using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timer;
    public float time;
    public GameObject canvasLose;
    public GameObject canvasWon;
    public GameObject car;
    private void Start()
    {
        canvasLose.gameObject.SetActive(false);
        canvasWon.gameObject.SetActive(false);
    }


    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            timer.text = time.ToString("0.00");
        } else
        {
            car.GetComponent<CarController>().enabled = false;
            canvasLose.gameObject.SetActive(true);
            Invoke("ExitMainMenu", 5);
        }

    }
    public void ExitMainMenuT()
    {
        Invoke("ExitMainMenu", 5);
    }

    public void ExitMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
