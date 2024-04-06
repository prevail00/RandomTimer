using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Blinking : MonoBehaviour
{
    [SerializeField] GameObject manager;
    Timer timer;
    public bool crOn = false;

    void Start()
    {
        timer = manager.GetComponent<Timer>();
        ButtonInteract(false);
        crOn = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (timer.reset)
        {
            if (!crOn)
                StartCoroutine(BlinkingInputRoutine(gameObject));
        }
        else
        {
            if (crOn)
            {
                StopCoroutine(BlinkingInputRoutine(gameObject));
                crOn = false;
                ButtonInteract(false);
                SetVisibility(gameObject, false);                
            }
        }
    }

    IEnumerator BlinkingInputRoutine(GameObject currentPanel)
    {
        crOn = true;
        const float flickTime = 0.5f;
        bool v = false;
        ButtonInteract(true);
        while (timer.reset)
        {
            SetVisibility(gameObject, v);
            v = !v;
            yield return new WaitForSeconds(flickTime);
        }
        ButtonInteract(false);
        SetVisibility(gameObject, false);
        crOn = false;
    }

    void SetVisibility(GameObject currentObject, bool visible)
    {
        Image[] images = currentObject.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            Color tempColor = image.color;
            if (visible)
                tempColor.a = 1f;
            else
                tempColor.a = 0f;
            image.color = tempColor;
        }
    }

    void ButtonInteract(bool interactable)
    {
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = interactable;
        }
    }
}
