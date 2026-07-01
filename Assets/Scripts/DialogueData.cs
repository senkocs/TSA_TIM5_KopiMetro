using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Object", menuName = "Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public Data[] dialogue;
}

[System.Serializable]
public class Data
{
    public Sprite image;
    [TextArea (5,10)]
    public string speech;
    public bool cooking;
    public bool active = false;
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