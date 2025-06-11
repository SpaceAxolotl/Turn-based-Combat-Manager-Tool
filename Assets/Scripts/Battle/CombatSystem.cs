using UnityEngine;
using System.Collections.Generic;

public class CombatSystem : MonoBehaviour
{
    private static CombatSystem instance;
    public static CombatSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<CombatSystem>();
                if (instance == null)
                {
                    GameObject go = new GameObject("CombatSystem");
                    instance = go.AddComponent<CombatSystem>();
                }
            }
            return instance;
        }
    }

    [Header("Combat Settings")]
    [SerializeField] private float criticalHitMultiplier = 1.5f;
    [SerializeField] private float randomVariationRange = 0.1f; // ±10% variation
    [SerializeField] private float criticalHitChance = 0.1f; // 10% chance

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void OnApplicationQuit()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public void ProcessMove(MoveData move, bool isAllyMove)
    {
        if (move == null)
        {
            Debug.LogError("Invalid move!");
            return;
        }

        // Get attacker and defender data
        AllyData allyData = isAllyMove ? CurrentAllies.Instance.ActiveAllyData : null;
        EnemyData enemyData = isAllyMove ? null : CurrentEnemies.Instance.ActiveEnemyData;
        AllyData defenderAllyData = isAllyMove ? null : CurrentAllies.Instance.ActiveAllyData;
        EnemyData defenderEnemyData = isAllyMove ? CurrentEnemies.Instance.ActiveEnemyData : null;

        if ((isAllyMove && (allyData == null || defenderEnemyData == null)) ||
            (!isAllyMove && (enemyData == null || defenderAllyData == null)))
        {
            Debug.LogError("Missing attacker or defender data!");
            return;
        }

        // Calculate base damage
        float damage = CalculateDamage(move, isAllyMove ? allyData : enemyData, isAllyMove ? defenderEnemyData : defenderAllyData);

        // Apply type effectiveness
        float typeMultiplier = GetTypeEffectivenessMultiplier(move.type, isAllyMove ? defenderEnemyData.types : defenderAllyData.types);
        damage *= typeMultiplier;

        // Apply critical hit
        bool isCritical = Random.value < criticalHitChance;
        if (isCritical)
        {
            damage *= criticalHitMultiplier;
            Debug.Log("Critical hit!");
        }

        // Apply random variation
        float variation = 1f + Random.Range(-randomVariationRange, randomVariationRange);
        damage *= variation;

        // Apply the final damage
        int finalDamage = Mathf.RoundToInt(damage);
        ApplyDamage(finalDamage, isAllyMove ? defenderEnemyData : defenderAllyData, isAllyMove);

        // Log the attack
        string attackerName = isAllyMove ? allyData.allyName : enemyData.enemyName;
        string defenderName = isAllyMove ? defenderEnemyData.enemyName : defenderAllyData.allyName;
        string effectivenessText = typeMultiplier switch
        {
            2f => "It's super effective!",
            0.5f => "It's not very effective...",
            _ => ""
        };
        Debug.Log($"{attackerName} used {move.moveName} on {defenderName} for {finalDamage} damage! {effectivenessText}");
    }

    private float CalculateDamage(MoveData move, ScriptableObject attacker, ScriptableObject defender)
    {
        // Get the attack type for scaling stats
        AttackType attackType = move.attackType;
        if (attackType == null)
        {
            Debug.LogError("Move has no attack type!");
            return 0f;
        }

        // Get scaling stat from attacker
        float scalingStat;
        if (attacker is AllyData allyData)
        {
            var stat = allyData.stats.Find(s => s.statDefinition.statName == attackType.scalingStat.statName);
            scalingStat = stat?.value ?? 1f;
        }
        else if (attacker is EnemyData enemyData)
        {
            var stat = enemyData.stats.Find(s => s.statDefinition.statName == attackType.scalingStat.statName);
            scalingStat = stat?.value ?? 1f;
        }
        else
        {
            Debug.LogError("Invalid attacker type!");
            return 0f;
        }

        // Get defensive stat from defender
        float defensiveStat;
        if (defender is AllyData defenderAllyData)
        {
            var stat = defenderAllyData.stats.Find(s => s.statDefinition.statName == attackType.defensiveStat.statName);
            defensiveStat = stat?.value ?? 1f;
        }
        else if (defender is EnemyData defenderEnemyData)
        {
            var stat = defenderEnemyData.stats.Find(s => s.statDefinition.statName == attackType.defensiveStat.statName);
            defensiveStat = stat?.value ?? 1f;
        }
        else
        {
            Debug.LogError("Invalid defender type!");
            return 0f;
        }

        // Calculate base damage: (move power * scaling stat) - defensive stat
        float baseDamage = (move.power * scalingStat) - defensiveStat;

        // Ensure minimum damage of 1
        return Mathf.Max(1f, baseDamage);
    }

    private float GetTypeEffectivenessMultiplier(TypeDefinition moveType, List<TypeDefinition> defenderTypes)
    {
        if (moveType == null)
        {
            Debug.LogError("Move type is null!");
            return 1f;
        }

        if (defenderTypes == null || defenderTypes.Count == 0)
        {
            Debug.LogError($"Defender types are invalid! Count: {(defenderTypes == null ? "null" : defenderTypes.Count.ToString())}");
            return 1f;
        }

        Debug.Log($"Calculating type effectiveness for move type: {moveType.typeName}");
        Debug.Log($"Defender types: {string.Join(", ", defenderTypes.ConvertAll(t => t.typeName))}");

        float multiplier = 1f;

        foreach (var defenderType in defenderTypes)
        {
            if (defenderType == null)
            {
                Debug.LogError("Found null defender type!");
                continue;
            }

            // Check if move type is super effective against defender type
            if (moveType.offensiveStrengths.Contains(defenderType))
            {
                Debug.Log($"{moveType.typeName} is super effective against {defenderType.typeName}!");
                multiplier *= 2f;
            }
            // Check if move type is not very effective against defender type
            else if (moveType.offensiveWeaknesses.Contains(defenderType))
            {
                Debug.Log($"{moveType.typeName} is not very effective against {defenderType.typeName}!");
                multiplier *= 0.5f;
            }
            else
            {
                Debug.Log($"{moveType.typeName} has normal effectiveness against {defenderType.typeName}");
            }
        }

        Debug.Log($"Final type effectiveness multiplier: {multiplier}x");
        return multiplier;
    }

    private float GetStatValue(ScriptableObject character, string statName)
    {
        if (character is AllyData allyData)
        {
            var stat = allyData.stats.Find(s => s.statDefinition.statName == statName);
            return stat?.value ?? 1f;
        }
        else if (character is EnemyData enemyData)
        {
            var stat = enemyData.stats.Find(s => s.statDefinition.statName == statName);
            return stat?.value ?? 1f;
        }
        return 1f; // Default value if stat not found
    }

    private void ApplyDamage(int damage, ScriptableObject target, bool isAllyMove)
    {
        if (isAllyMove)
        {
            if (target is EnemyData enemyData)
            {
                var healthStat = enemyData.stats.Find(s => s.statDefinition.statName == "Health");
                if (healthStat != null)
                {
                    healthStat.value = Mathf.Max(0, healthStat.value - damage);
                    Debug.Log($"{enemyData.enemyName} took {damage} damage. Current Health: {healthStat.value}");
                }
            }
        }
        else
        {
            if (target is AllyData allyData)
            {
                var healthStat = allyData.stats.Find(s => s.statDefinition.statName == "Health");
                if (healthStat != null)
                {
                    healthStat.value = Mathf.Max(0, healthStat.value - damage);
                    allyData.currentHealth = healthStat.value; // Sync to currentHealth field
                    Debug.Log($"{allyData.allyName} took {damage} damage. Current Health: {allyData.currentHealth}");
                }
            }

        }
    }
}