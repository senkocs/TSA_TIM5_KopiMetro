using UnityEngine;
using UnityEngine.UI;

public class Ingredient : MonoBehaviour
{
    public string ingredientName;
    public int totalInsert;
    public bool forRoasting;

    public Image[] imageCountDisplay;
    public Color imageColor = Color.red;

    public CookingManager cookingManager;

    public void AddIngredient()
	{
        if (totalInsert < 10 && cookingManager.currentMixIngredientCount < 10)
		{
            totalInsert++;

            if (!forRoasting) cookingManager.AddMixIngredient();
            imageCountDisplay[totalInsert - 1].color = imageColor;
        }
    }

    public void ResetCountDisplay()
	{
		for (int i = 0; i < imageCountDisplay.Length; i++)
		{
            imageCountDisplay[i].color = Color.white;
        }

        totalInsert = 0;
    }
}
