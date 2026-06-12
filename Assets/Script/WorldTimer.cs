using System.Collections;

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldTimer : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateDisplay();
        StartCoroutine(StartTimer());
    }

    void Update()
    {
        UpdateDisplay();
    }

    public IEnumerator StartTimer()
    {
        GameManager gm = GameManager.singleton;
        while (gm != null && gm.currentWorldTimer != 0)
        {
            gm.currentWorldTimer--;
            yield return new WaitForSeconds(1.0f);
        }
        SceneManager.LoadSceneAsync(0);
    }

    public void UpdateDisplay()
    {
        GameManager gm = GameManager.singleton;
        if (gm == null) return;
        uint timer = gm.currentWorldTimer;
        uint hour = timer / 3600; timer = timer - hour * 3600;
        uint min = timer / 60; timer = timer - min * 60;
        uint sec = timer;
        string hourDisplay = hour / 10.0f < 1.0f ? "0"+ hour : hour.ToString();
        string minDisplay = min / 10.0f < 1.0f ? "0"+ min : min.ToString();
        string secDisplay = sec / 10.0f < 1.0f ? "0"+ sec : sec.ToString();
        textMesh.text =  hourDisplay + ":" + minDisplay + ":" + secDisplay;
    }

    

}
