using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackType", menuName = "Combat/Attack Type")]
public class AttackType : ScriptableObject
{
    public string typeName;
    [TextArea]
    public string description;

    public StatDefinition scalingStat;   // Used to calculate attack power
                                         // Optional: public StatDefinition defensiveStat; // e.g. for resistance checks
    public StatDefinition defensiveStat; // e.g. for resistance checks
}
