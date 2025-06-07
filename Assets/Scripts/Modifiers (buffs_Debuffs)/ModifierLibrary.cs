using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModifierLibrary", menuName = "Modifiers/Modifier Library")]
public class ModifierLibrary : ScriptableObject
{
    public List<StatModifier> modifiers;
}