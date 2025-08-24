using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Save Game Config")]
public class SaveGameSprite : ScriptableObject
{
    public List<SaveGameClass> saveGameClasses;
}
