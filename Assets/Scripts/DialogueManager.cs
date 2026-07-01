using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    DialogueSystem dialogue;

    public DialogueData dialogueData;

    public CanvasGroup cookingGroup;
    public Button speechNextButton;

    public CookingManager cookingManager;
    public DialogueSystem dialogueSystem;

    public Data dataPlayed;
    private DaySystem daySystem;

    public GameObject transition;
    [SerializeField] private GameObject buttonGuide;

    bool isMixingTrue;

    internal int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = DialogueSystem.instance;

        daySystem = GetComponent<DaySystem>();
        //NextButton(); untuk memulai percakapan
    }

    public void NextButton()
	{
        if (!dialogue.isSpeaking || dialogue.isWaitingForUserInput)
        {
            if (index >= dialogueData.dialogue.Length)
            {

                if (daySystem.day == 3) SceneManager.LoadScene("GameOver");
                // ganti hari dan jika sudah habis baru masuk ke gameover
                else
                {
                    index = 0;
                    transition.SetActive(true);
                }
                return;
            }


            dataPlayed = dialogueData.dialogue[index];
            if (dataPlayed.active) buttonGuide.SetActive(true);

			if (dataPlayed.cooking)
			{
                cookingGroup.interactable = true;
                speechNextButton.interactable = false;

                cookingManager.SetCooking(dataPlayed.cookingDialogue.orderName);
            }
			else
			{
                cookingGroup.interactable = false;
            }

            Say(dataPlayed, false);
            index++;
        }
    }
    public void CookingRespon(bool isTrue)
	{
        dialogueSystem.characterImage.sprite = dataPlayed.cookingDialogue.orderImage;

        isMixingTrue = isTrue;
        Say(dataPlayed, true);

        speechNextButton.interactable = true;
        cookingGroup.interactable = false;
    }

    void Say(Data dataDialogue, bool isRespon)
    {
        string[] parts;
        if (!isRespon)
            parts = dataDialogue.speech.Split(':');
		else
		{
			if (isMixingTrue)
			{
                parts = dataDialogue.cookingDialogue.goodRespon.Split(':');
            }
			else
			{
                parts = dataDialogue.cookingDialogue.badRespon.Split(':');
            }
        }

        string speech = parts[0];
        string speaker = (parts.Length >= 2) ? parts[1] : "";

        dialogue.Say(speech, dataDialogue, isRespon, speaker);
    }
}
