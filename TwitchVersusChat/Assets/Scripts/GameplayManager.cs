using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    None,
    LandmarkDefense
}

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager SharedInstance;

    public bool Paused = false;

    public GameMode Mode = GameMode.LandmarkDefense;

    void Awake()
    {
        SharedInstance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Paused = !Paused;
        }

        if (Mode == GameMode.LandmarkDefense)
        {
            LandmarkDefenseUpdate();
        }
    }

    public bool PauseGameplay(float duration)
    {
        if (Paused) return false;

        Paused = true;
        Invoke("Unpause", duration);

        return true;
    }

    public void Unpause()
    {
        Paused = false;
    }

    private void LandmarkDefenseUpdate()
    {

    }
}