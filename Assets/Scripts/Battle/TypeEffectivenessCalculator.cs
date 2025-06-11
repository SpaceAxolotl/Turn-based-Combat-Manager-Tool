using UnityEngine;

public static class TypeEffectivenessCalculator
{
    public static float CalculateEffectiveness(TypeDefinition attackerType, TypeDefinition defenderType)
    {
        float multiplier = 1f;

        // Offensive side from attacker
        if (attackerType.offensiveStrengths.Contains(defenderType))
            multiplier *= 2f;
        if (attackerType.offensiveWeaknesses.Contains(defenderType))
            multiplier *= 0.5f;

        // Defensive side from defender
        if (defenderType.defensiveWeaknesses.Contains(attackerType))
            multiplier *= 2f;
        if (defenderType.defensiveStrengths.Contains(attackerType))
            multiplier *= 0.5f;

        return multiplier;
    }

    public static float CalculateEffectivenessAgainstMultiple(
    TypeDefinition attackerType,
    TypeDefinition[] defenderTypes)
{
    float multiplier = 1f;

    foreach (var defenderType in defenderTypes)
    {
        multiplier *= CalculateEffectiveness(attackerType, defenderType);
    }

    return multiplier;
}
}

