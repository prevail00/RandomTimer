using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject actionObj;
    [SerializeField] int gameMode = 0;
    [SerializeField] GameObject setupSoundObj;
    AudioSource setupAudio;

    public void OnPointerEnter(PointerEventData eventData)
    {
        setupAudio = setupSoundObj.GetComponent<AudioSource>();
        setupAudio.Play();
        Debug.Log("Setup button sound");
        Lines lines = actionObj.GetComponent<Lines>();
        lines.gameModeId = gameMode;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Lines lines = actionObj.GetComponent<Lines>();
        lines.gameModeId = 0;
    }
}
