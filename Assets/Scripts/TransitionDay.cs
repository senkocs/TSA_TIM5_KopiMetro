using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransitionDay : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DaySystem daySystem;
    [SerializeField] private TextMeshProUGUI dayText;
    
    public void StartTransition()
    {
        daySystem.day++;
        dialogueManager.dialogueData = daySystem.dialogueDatas[daySystem.day - 1];
        dayText.text = "Day " + daySystem.day.ToString();
    }

    public void ShowText()
    {
        dayText.gameObject.SetActive(!dayText.gameObject.activeSelf);
    }

    public void EndTransition()
    {
        dialogueManager.NextButton();
        gameObject.SetActive(false);
    }
}