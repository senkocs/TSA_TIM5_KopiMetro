using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour 
{
	[System.Serializable]
	public class ELEMENTS
	{
		public Image characterImage;
		public TextMeshProUGUI speakerNameText;
		public TextMeshProUGUI speechText;
	}

	public static DialogueSystem instance;

	public ELEMENTS elements;

	public Image characterImage { get { return elements.characterImage; } }
	public TextMeshProUGUI speakerNameText { get { return elements.speakerNameText; } }
	public TextMeshProUGUI speechText { get { return elements.speechText; } }

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		
	}

	/// <summary>
	/// Say something and show it on the speech box.
	/// </summary>
	public void Say(string speech, DialogueManager.Data dataDiaogue, bool isResponse, string speaker = "")
	{
		StopSpeaking();

		speaking = StartCoroutine(Speaking(speech, dataDiaogue, isResponse, false, speaker));
	}

	/// <summary>
	/// Say something to be added to what is already on the speech box.
	/// </summary>
	public void SayAdd(string speech, DialogueManager.Data dataDiaogue, bool isResponse, string speaker = "")
	{
		StopSpeaking();

		speechText.text = targetSpeech;

		speaking = StartCoroutine(Speaking(speech, dataDiaogue, true, isResponse, speaker));
	}

	public void StopSpeaking()
	{
		if (isSpeaking)
		{
			StopCoroutine(speaking);
		}
		speaking = null;
	}
		
	public bool isSpeaking {get{return speaking != null;}}
	[HideInInspector] public bool isWaitingForUserInput = false;

	private string targetSpeech = "";
	Coroutine speaking = null;
	IEnumerator Speaking(string speech, DialogueManager.Data dataDiaogue, bool isResponse, bool additive, string speaker = "")
	{
		if (characterImage && dataDiaogue.image)
		{
			characterImage.gameObject.SetActive(true);
			characterImage.sprite = dataDiaogue.image;
		}
		else
		{
			characterImage.gameObject.SetActive(false);
		}

		targetSpeech = speech;

		if (!additive)
			speechText.text = "";
		else
			targetSpeech = speechText.text + targetSpeech;

		speakerNameText.text = DetermineSpeaker(speaker);//temporary

		isWaitingForUserInput = false;

		while(speechText.text != targetSpeech)
		{
			speechText.text += targetSpeech[speechText.text.Length];
			yield return new WaitForEndOfFrame();
		}

		//text finished
		isWaitingForUserInput = true;

		if (dataDiaogue.cooking && !isResponse)
		{
			yield return new WaitForSeconds(1);

			speechText.text = string.Empty;
			speakerNameText.text = string.Empty;
		}

		while (isWaitingForUserInput)
			yield return new WaitForEndOfFrame();

		StopSpeaking();
	}

	string DetermineSpeaker(string s)
	{
		string retVal = speakerNameText.text;//default return is the current name
		if (s != speakerNameText.text && s != "")
			retVal = (s.ToLower().Contains("narrator")) ? "" : s;

		return retVal;
	}
}