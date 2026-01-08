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

        public bool milk;
        public bool matcha;
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


    public Toggle milkToogle;
    public Toggle matchaToogle;

    public DialogueManager dialogueManager;

    bool isMixingTrue;
    bool isRoastingTrue;
    bool isMilkOn;
    bool isMatchaOn;

	private void Start()
	{
        mixServeButton.onClick.AddListener(Mixing);
        roastServeButton.onClick.AddListener(Roast);
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
        RecipeData currentRecipe = null;

		for (int i = 0; i < recipeData.Length; i++)
		{
			if (targetCoffee == recipeData[i].nameRecipe)
			{
                currentRecipe = recipeData[i];

                if (isMilkOn != recipeData[i].milk)
                    isMixingTrue = false;
                if (isMatchaOn != recipeData[i].matcha)
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
        bool isRoast = true;
        for (int i = 0; i < recipeCoffe.Length; i++)
        {
            for (int h = 0; h < recipeCoffe[i].recipe.Length; h++)
            {
                if (recipeCoffe[i].recipe[h].totalIngredient != GetIngredientCoffe(recipeCoffe[i].recipe[h].nameIngredient).totalInsert)
                {
                    isRoast = false;
                }
            }

            if (isRoast) 
            {
                roastingResult = recipeCoffe[i].nameCoffe;
            }
        }

        roastServeText.text = roastingResult;
        roastServeButton.gameObject.SetActive(false);
    }

	private void Serve()
	{
        Debug.Log("Serve");
        dialogueManager.CookingRespon(isMixingTrue);

        ResetMix();
        ResetCooking();
    }

    public void ResetMix()
    {
        milkToogle.isOn = false;
        matchaToogle.isOn = false;

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
        roastingResult = string.Empty;
        roastServeText.text = string.Empty;

        mixServeButton.onClick.RemoveAllListeners();
        mixServeButton.onClick.AddListener(Mixing);
        roastServeButton.gameObject.SetActive(true);
    }

    public void AddMixIngredient()
	{
        currentMixIngredientCount++;
        mixIngredientsDisplay[currentMixIngredientCount - 1].SetActive(true);
    }

    public void MilkToogle(bool isOn)
	{
        isMilkOn = isOn;
	}
    public void MatchaToogle(bool isOn)
    {
        isMatchaOn = isOn;
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
