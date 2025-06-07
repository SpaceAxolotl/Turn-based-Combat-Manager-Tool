using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewModifier", menuName = "Modifiers/Stat Modifier")]
public class StatModifier : ScriptableObject
{
    public string modifierName;
    [TextArea] public string description;

    // List of individual stat effects
    public List<StatEffect> effects = new List<StatEffect>();
}

[System.Serializable]
public class StatEffect
{
    public StatDefinition stat;      // The stat to affect
    public float multiplier = 1f;    // e.g., 1.2 means +20%, 0.8 means -20%
}
