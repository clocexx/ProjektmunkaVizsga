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
            Destroy(gameObject); // Ha m�sik jelenetbe v�letlen beker�lne �j p�ld�ny
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // K�sleltet�s a jelenet teljes bet�lt�se ut�n
        Debug.Log("Jelenet bet�ltve: " + scene.name);
        Invoke(nameof(FindTimerText), 0.1f);
    }

    private void FindTimerText()
    {
        GameObject found = GameObject.FindWithTag("TimerText");
        if (found != null)
        {
            timerText = found.GetComponent<Text>();
            Debug.Log("TimerText megtal�lva: " + found.name);
        }
        else
        {
            Debug.LogWarning(" TimerText nem tal�lhat� ebben a jelenetben: " + SceneManager.GetActiveScene().name);
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
