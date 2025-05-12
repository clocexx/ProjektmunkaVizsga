using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    private RectTransform arrow;
    private int currentPosition;

    private void Awake()
    {
        arrow = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        currentPosition = 0;
        ChangePosition(0);
    }
    private void Update()
    {
        //Change position of the selection arrow
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            ChangePosition(-1);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            ChangePosition(1);

        //Interact with options
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;


        if (currentPosition < 0)
            currentPosition = options.Length - 1;
        else if (currentPosition > options.Length - 1)
            currentPosition = 0;

        arrow.position = new Vector3(arrow.position.x, options[currentPosition].position.y, 0);
    }

    private void Interact()
    {

        //Access the button component on each option and call it's function
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
