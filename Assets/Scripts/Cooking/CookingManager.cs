using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CookingManager : MonoBehaviour
{
    [System.Serializable]
    public class RecipeData
    {
        public string nameRecipe;

        public Sprite mixedSprite;

        public bool ice;
        public bool coal;
        public bool ginseng;
        public string coffe;

        public Recipe[] recipe;
    }

    [System.Serializable]
    public class RecipeCoffe
    {
        public string nameCoffe;
        public Recipe[] recipe;
    }

    [System.Serializable]
    public class Recipe
	{
        public string nameIngredient;
        public int totalIngredient;
	}

    public RecipeData[] recipeData;

    public RecipeCoffe[]recipeCoffe;

    public Ingredient[] ingredients;

    public Ingredient[] coffeIngredients;

    public string roastingResult;

    public string targetCoffee;

    // Mix Bottle
    public Image mixBottleImage;
    public GameObject[] mixIngredientsDisplay;
    public Sprite defaultSprite;
    public Sprite wrongIngredientSprite;

    public TextMeshProUGUI orderNameText;

    [HideInInspector] public int currentMixIngredientCount;

    public Button mixServeButton;
    public Button roastServeButton;
    public TextMeshProUGUI mixServeText;
    public TextMeshProUGUI roastServeText;


    [Header("Toggle")]
    public Toggle iceToogle;
    public TextMeshProUGUI iceText;
    public Toggle coalToogle;
    public TextMeshProUGUI coalText;
    public Toggle ginsengToogle;
    public TextMeshProUGUI ginsengText;

    public DialogueManager dialogueManager;

    bool isMixingTrue;
    bool isRoastingTrue;
    bool isIceOn;
    bool isCoalOn;
    bool isGinsengOn;

    [SerializeField] RecipeData currentRecipe;

	private void Start()
	{
        mixServeButton.onClick.AddListener(Mixing);
        roastServeButton.onClick.AddListener(Roast);
        roastServeButton.onClick.AddListener(dialogueManager.SetCanvas);
    }

    public void SetCooking(string targetCoffee)
	{
        this.targetCoffee = targetCoffee;
        orderNameText.text = targetCoffee;
    }

    public void ResetCooking()
	{
        orderNameText.text = string.Empty;
    }

    public void Mixing()
	{
        isMixingTrue = true;
        currentRecipe = null;

		for (int i = 0; i < recipeData.Length; i++)
		{
			if (targetCoffee == recipeData[i].nameRecipe)
			{
                currentRecipe = recipeData[i];

                if (isIceOn != recipeData[i].ice)
                    isMixingTrue = false;
                if (isCoalOn != recipeData[i].coal)
                    isMixingTrue = false;
                if (isGinsengOn != recipeData[i].ginseng)
                    isMixingTrue = false;
                if (roastingResult != recipeData[i].coffe)
                    isMixingTrue = false;

				for (int h = 0; h < recipeData[i].recipe.Length; h++)
				{
					if (recipeData[i].recipe[h].totalIngredient != GetIngredient(recipeData[i].recipe[h].nameIngredient).totalInsert)
					{
                        isMixingTrue = false;
                    }
                }
			}
		}

        mixServeText.text = "SERVE";

        mixServeButton.onClick.RemoveAllListeners();
        mixServeButton.onClick.AddListener(Serve);

        if (isMixingTrue)
		{
            Debug.Log("HEHE");

            mixBottleImage.sprite = currentRecipe.mixedSprite;
        }
		else
		{
            Debug.Log("HAHAHEHE");
            mixBottleImage.sprite = wrongIngredientSprite;
        }
	}

    public void Roast()
    {
        roastingResult = "BadCoffe";
        roastServeText.text = roastingResult;
        bool isRoast = true;
        string targetRoasting = null;
        int condition = 0;

        for (int j = 0; j < recipeData.Length; j++)
        {
            if (targetCoffee == recipeData[j].nameRecipe)
            {
                targetRoasting = recipeData[j].coffe;
            }
        }

        for (int i = 0; i < recipeCoffe.Length; i++)
        {
            if (targetRoasting == recipeCoffe[i].nameCoffe)
            {
                for (int h = 0; h < recipeCoffe[i].recipe.Length; h++)
                {
                    if (recipeCoffe[i].recipe[h].totalIngredient != GetIngredientCoffe(recipeCoffe[i].recipe[h].nameIngredient).totalInsert)
                    {
                        isRoast = false;
                        condition++;
                    }
                }
            }
            else
            {
                for (int h = 0; h < recipeCoffe[i].recipe.Length; h++)
                {
                    if (recipeCoffe[i].recipe[h].totalIngredient != GetIngredientCoffe(recipeCoffe[i].recipe[h].nameIngredient).totalInsert)
                    {
                        condition++;
                    }
                }
            }
        }

        if (isRoast) 
        {
            roastingResult = targetRoasting;
            roastServeText.text = roastingResult;
            roastServeButton.gameObject.SetActive(false);
        }
        else
        {
            if (condition == 4)
            {
                roastingResult = targetRoasting;
                roastServeText.text = "Resep Salah";
                roastServeButton.gameObject.SetActive(false);
            }
            else if (condition == 6)
            {
                roastingResult = null;
                roastServeText.text = "Resep Tidak Ditemukan";
                roastServeButton.gameObject.SetActive(true);

                for (int i = 0; i < coffeIngredients.Length; i++)
                {
                    coffeIngredients[i].ResetCountDisplay();
                }
            }
        }

        Debug.Log(condition + ", " + isRoast);
    }

	private void Serve()
	{
        Debug.Log("Serve");
        if (targetCoffee == "Random") isMixingTrue = true;
        dialogueManager.CookingRespon(isMixingTrue);
        dialogueManager.roastButton.SetActive(false);
        dialogueManager.cookingGroup.interactable = false;

        ResetMix();
        ResetCooking();
        currentRecipe = new RecipeData();
    }

    public void ResetRoast()
    {
        for (int i = 0; i < coffeIngredients.Length; i++)
        {
            coffeIngredients[i].ResetCountDisplay();
        }

        roastingResult = null;
        roastServeText.text = "";
        roastServeButton.gameObject.SetActive(true);
    }

    public void ResetMix()
    {
        iceToogle.isOn = false;
        coalToogle.isOn = false;
        ginsengToogle.isOn = false;

        isIceOn = false;
        isCoalOn = false;
        isGinsengOn = false;
        
        iceText.color = Color.white;
        coalText.color = Color.white;
        ginsengText.color = Color.white;

        currentMixIngredientCount = 0;

        for (int i = 0; i < ingredients.Length; i++)
        {
            ingredients[i].ResetCountDisplay();
        }

        for (int i = 0; i < coffeIngredients.Length; i++)
        {
            coffeIngredients[i].ResetCountDisplay();
        }

		for (int i = 0; i < mixIngredientsDisplay.Length; i++)
		{
            mixIngredientsDisplay[i].SetActive(false);
        }

        mixBottleImage.sprite = defaultSprite;

        mixServeText.text = "MIX";
        roastingResult = null;
        roastServeText.text = "";

        mixServeButton.onClick.RemoveAllListeners();
        mixServeButton.onClick.AddListener(Mixing);
        roastServeButton.gameObject.SetActive(true);

        Debug.Log(isIceOn);
    }

    public void AddMixIngredient()
	{
        currentMixIngredientCount++;
        mixIngredientsDisplay[currentMixIngredientCount - 1].SetActive(true);
    }

    public void IceToogle(bool isOn)
	{
        isIceOn = isOn;
        iceText.color = Color.green;
	}
    public void CoalToogle(bool isOn)
    {
        isCoalOn = isOn;
        coalText.color = Color.green;
    }
    public void GinsengToogle(bool isOn)
    {
        isGinsengOn = isOn;
        ginsengText.color = Color.green;
    }

    Ingredient GetIngredient(string nameIngredient)
	{
		for (int i = 0; i < ingredients.Length; i++)
		{
            if (ingredients[i].ingredientName == nameIngredient)
                return ingredients[i];
        }
        return null;
    }

    Ingredient GetIngredientCoffe(string nameIngredient)
    {
        for (int i = 0; i < coffeIngredients.Length; i++)
		{
            if (coffeIngredients[i].ingredientName == nameIngredient)
                return coffeIngredients[i];
        }
        return null;
    }
}
