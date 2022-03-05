using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public void ReadStringInput(string s)
    {
        MainManager.playerName = s;
        Debug.Log("Player name is " + MainManager.playerName);
    }

    public void DisplayMain()
    {
        SceneManager.LoadScene(2);
        Debug.Log("Load scene 2");
    }

    public void DisplaySettings()
    {
        SceneManager.LoadScene(1);
        Debug.Log("Load scene 1");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
        Debug.Log("Load scene 0");
    }

    public void SetBallSpeed(string s)
    {
        float ballSpeed;
        float.TryParse(s, out ballSpeed);
        MainManager.ballSpeed = ballSpeed;
        Debug.Log("Ball Speed: " + ballSpeed);
    }

}
