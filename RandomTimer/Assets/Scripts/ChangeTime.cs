using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ChangeTime : MonoBehaviour
{
    Collider2D thisCollider;
    [SerializeField] GameObject managerObj;
    bool crOn;
    bool touched;
    [SerializeField] float minTime;
    [SerializeField] float maxTime;
    [SerializeField] float increment;
    public UnityEvent changeStartTime;

    Coroutine coroutine = null;

    void Start()
    {
        crOn = false;
        thisCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            if (thisCollider.OverlapPoint(touchPosition))
                touched = true;
            else
                touched = false;

            // Handle finger movements based on touch phase.
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    if (touched)
                    {
                        Debug.Log("Button Touched");
                        if (!crOn)
                            crOn = true;
                            coroutine = StartCoroutine(RampTime());
                    }                    
                    break;

                //Finger is lifted.
                case TouchPhase.Ended:
                    if (touched)
                    {
                        Debug.Log("Finger Lifted");
                        //touched = false;
                        if (crOn)
                            StopCoroutine(coroutine);
                        crOn = false;
                    }
                    break;
            }
        }
    }
    IEnumerator RampTime()
    {
        crOn = true;
        float waitTime = maxTime;
        while (touched)
        {
            changeStartTime.Invoke();
            yield return new WaitForSeconds(waitTime);
            if (waitTime > minTime)
                waitTime -= increment;
        }
        crOn = false;
    }
}
