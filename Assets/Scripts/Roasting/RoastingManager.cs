using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RoastingManager : MonoBehaviour
{
    [System.Serializable]
    public class RecipeData
    {
        public string nameRecipe;

        public Sprite mixedSprite;

        public Recipe[] recipe;
    }
    [System.Serializable]
    public class Recipe
    {
        public string nameIngredient;
        public int totalIngredient;
    }

    public RecipeData[] recipeData;

    public Ingredient[] ingredients;

    public string targetCoffee;

    // Mix Bottle
    public Image mixBottleImage;
    public GameObject[] mixIngredientsDisplay;
    public Sprite defaultSprite;
    public Sprite wrongIngredientSprite;

    public TextMeshProUGUI orderNameText;

    [HideInInspector] public int currentMixIngredientCount;

    public Button mixServeButton;
    public TextMeshProUGUI mixServeText;


    public DialogueManager dialogueManager;

    bool isMixingTrue;
 

    private void Start()
    {
        mixServeButton.onClick.AddListener(Mixing);
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
    private void Serve()
    {
        Debug.Log("Serve");
        dialogueManager.CookingRespon(isMixingTrue);

        ResetMix();
        ResetCooking();
    }
    public void ResetMix()
    {
    

        currentMixIngredientCount = 0;

        for (int i = 0; i < ingredients.Length; i++)
        {
            ingredients[i].ResetCountDisplay();
        }

        for (int i = 0; i < mixIngredientsDisplay.Length; i++)
        {
            mixIngredientsDisplay[i].SetActive(false);
        }

        mixBottleImage.sprite = defaultSprite;

        mixServeText.text = "MIX";

        mixServeButton.onClick.RemoveAllListeners();
        mixServeButton.onClick.AddListener(Mixing);
    }

    public void AddMixIngredient()
    {
        currentMixIngredientCount++;
        mixIngredientsDisplay[currentMixIngredientCount - 1].SetActive(true);
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
}
