using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    DialogueSystem dialogue;

    [System.Serializable]
    public class Data
	{
        public Sprite image;
        public string speech;

        public bool cooking;
        public CookingDialogue cookingDialogue;
	}
    [System.Serializable]
    public class CookingDialogue
	{
        public string orderName;

        public Sprite orderImage;

        public string goodRespon;
        public string badRespon;
	}

    public Data[] dialogueData;

    public CanvasGroup cookingGroup;
    public Button speechNextButton;

    public CookingManager cookingManager;
    public DialogueSystem dialogueSystem;

    public Data dataPlayed;

    bool isMixingTrue;

    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = DialogueSystem.instance;
        NextButton();
    }

    public void NextButton()
	{
        if (!dialogue.isSpeaking || dialogue.isWaitingForUserInput)
        {
            if (index >= dialogueData.Length)
            {

                SceneManager.LoadScene("GameOver");

                return;
            }

            dataPlayed = dialogueData[index];

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
