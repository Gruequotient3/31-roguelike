using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton = null;

    public int worldSeed = 0;
    public int hubSeed = 0;

    public Timer worldTimer;  
    public uint currentWorldTimer;

    public void Awake() {
        if (singleton != null)
        {
            UnityEngine.GameObject.Destroy(gameObject);
            return;
        }

        singleton = this;
        DontDestroyOnLoad(gameObject);
        ResetTimer();

    }

    public void ResetTimer()
    {
        currentWorldTimer = worldTimer.hour * 60 + worldTimer.min * 60 + worldTimer.sec;
    }

    public void OnValidate()
    {
        if (worldTimer.sec >= 60)
        {
            worldTimer.min += worldTimer.sec / 60;
            worldTimer.sec = worldTimer.sec % 60;
        }
        if (worldTimer.min >= 60)
        {
            worldTimer.hour += worldTimer.min / 60;
            worldTimer.min = worldTimer.min % 60;
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InventoryManager inventoryManager;
        inventoryManager = GameObject.FindFirstObjectByType<InventoryManager>();
        inventoryManager.inventoryGroup = GameObject.FindWithTag("InventoryGroup");
        inventoryManager.SearchInventorySlot();
        inventoryManager.SearchCraftingSlot();
    }
}



[Serializable]
public struct Timer
{
    public uint sec;
    public uint min;
    public uint hour;
}