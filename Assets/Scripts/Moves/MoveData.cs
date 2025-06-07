using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewMove", menuName = "Moves/Move Data")]
public class MoveData : ScriptableObject
{
    public string moveName;
    [TextArea]
    public string description;

    public AttackType attackType; // Selected from AttackTypeLibrary

    public int power = 10;
    public int accuracy = 100;

    public bool usesResource;
    public ResourceDefinition resourceType;   // Selected from ResourceLibrary
    public int resourceCost;

    public TypeDefinition type;           // Selected from TypeLibrary

    
    public bool usesStatModifier;
    public List <StatModifier> appliedModifier;

    public float modifierTriggerChance = 100f;

    public bool usesStatusEffect;
    public List<StatusEffect> statusEffects;
    public float statusEffectChance = 100f;
    
    

}