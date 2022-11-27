using System;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public PlayerManager PlayerManager;
    public static GameManager Instance;
    private void Awake() {
        if (Instance != null) {
            throw new Exception("Already created GameManager instance");
        }

        Instance = this;
    }

}