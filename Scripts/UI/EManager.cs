using UnityEngine;

public class EManager : MonoBehaviour
{
    [SerializeField] private GameObject introductionPanel;

    private bool introActive = false;

    void Start()
    {
        if (introductionPanel != null)
        {
            introductionPanel.SetActive(true);
            Time.timeScale = 0f;
            introActive = true;
        }
    }

    void Update()
    {
        if (introActive && Input.GetKeyDown(KeyCode.E))
        {
            introductionPanel.SetActive(false);
            Time.timeScale = 1f;
            introActive = false;
        }
    }
}
