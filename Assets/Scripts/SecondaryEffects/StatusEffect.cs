using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Combat/Status Effect")]
public class StatusEffect : ScriptableObject
{
    public string effectName;
    [TextArea] public string description;

    [Range(0f, 100f)]
    public float triggerChance = 100f;

    public bool applyStatModifier;
    public List <StatModifier> statModifier;

    public bool affectsResources;
    public List<ResourceEffect> resourceEffects = new();
}
