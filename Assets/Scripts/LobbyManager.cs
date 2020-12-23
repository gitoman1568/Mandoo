using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public GameObject LobbyGroup;
    public GameObject InGameGroup;

    static LobbyManager _instance;

    public static LobbyManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LobbyManager>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        Screen.SetResolution(1080, 1920, true);
        Application.targetFrameRate = 60;
    }

    public void GameStart()
    {
        InGameGroup.SetActive(true);
        LobbyGroup.SetActive(false);

        MandooManager.instance.GameStart();
    }

    public void ReturnRobby()
    {
        InGameGroup.SetActive(false);
        LobbyGroup.SetActive(true);
    }
}
