using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    private float totalTime = 0f;
    private Text timerText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject); // Ha másik jelenetbe véletlen bekerülne új példány
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Késleltetés a jelenet teljes betöltése után
        Debug.Log("Jelenet betöltve: " + scene.name);
        Invoke(nameof(FindTimerText), 0.1f);
    }

    private void FindTimerText()
    {
        GameObject found = GameObject.FindWithTag("TimerText");
        if (found != null)
        {
            timerText = found.GetComponent<Text>();
            Debug.Log("TimerText megtalálva: " + found.name);
        }
        else
        {
            Debug.LogWarning(" TimerText nem található ebben a jelenetben: " + SceneManager.GetActiveScene().name);
        }
    }


    void Update()
    {
        totalTime += Time.deltaTime;

        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(totalTime / 60f);
            int seconds = Mathf.FloorToInt(totalTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public float GetElapsedTime()
    {
        return totalTime;
    }
}
