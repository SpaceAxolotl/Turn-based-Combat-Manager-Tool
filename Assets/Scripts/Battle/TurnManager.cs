using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatTurnManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private BattleUI battleUI;

    private bool isAllyTurn;
    private bool isWaitingForMoveSelection;
    private bool isBattleActive = false;

    // Public properties for UI
    public bool IsAllyTurn => isAllyTurn;
    public bool IsWaitingForMoveSelection => isWaitingForMoveSelection;

    private void Start()
    {
        if (battleUI == null)
            battleUI = FindFirstObjectByType<BattleUI>();
    }

    public void StartBattle()
    {
        if (!isBattleActive)
        {
            isBattleActive = true;
            DetermineFirstTurn();
        }
    }

    private void DetermineFirstTurn()
    {
        // Get speed stats from active ally and enemy
        float allySpeed = CurrentAllies.Instance.GetActiveAllySpeed();
        float enemySpeed = CurrentEnemies.Instance.GetActiveEnemySpeed();

        // Compare speeds to determine who goes first
        isAllyTurn = allySpeed >= enemySpeed;

        if (isAllyTurn)
        {
            Debug.Log($"Ally goes first (Speed: {allySpeed} vs Enemy Speed: {enemySpeed})");
            StartAllyTurn();
        }
        else
        {
            Debug.Log($"Enemy goes first (Speed: {enemySpeed} vs Ally Speed: {allySpeed})");
            StartEnemyTurn();
        }
    }

    private void StartAllyTurn()
    {
        isAllyTurn = true;
        isWaitingForMoveSelection = true;
        // Notify UI that it's the ally's turn
        if (battleUI != null)
        {
            battleUI.OnAllyTurnStart();
        }
    }

    private void StartEnemyTurn()
    {
        isAllyTurn = false;
        isWaitingForMoveSelection = false;
        // Notify UI that it's the enemy's turn
        if (battleUI != null)
        {
            battleUI.OnEnemyTurnStart();
        }
        // Start enemy AI logic here
        StartCoroutine(ExecuteEnemyTurn());
    }

    private IEnumerator ExecuteEnemyTurn()
    {
        // Wait a moment before enemy acts
        yield return new WaitForSeconds(1f);

        // Get a random move from the enemy's available moves
        var enemy = CurrentEnemies.Instance.ActiveEnemyData;
        if (enemy != null && enemy.moves.Count > 0)
        {
            int randomIndex = Random.Range(0, enemy.moves.Count);
            var selectedMove = enemy.moves[randomIndex];
            
            // Execute the move
            ExecuteMove(selectedMove, false);
        }

        // End enemy turn and start ally turn
        EndEnemyTurn();
    }

    public void OnMoveSelected(MoveData move)
    {
        if (isWaitingForMoveSelection)
        {
            ExecuteMove(move, true);
            isWaitingForMoveSelection = false;
        }
    }

    private void ExecuteMove(MoveData move, bool isAllyMove)
    {
        // Execute the move logic here
        Debug.Log($"Executing move: {move.moveName} by {(isAllyMove ? "Ally" : "Enemy")}");
        
        // Process the move through the combat system
        CombatSystem.Instance.ProcessMove(move, isAllyMove);
        
        // After move execution, switch turns
        if (isAllyMove)
        {
            EndAllyTurn();
        }
        else
        {
            EndEnemyTurn();
        }
    }

    private void EndAllyTurn()
    {
        StartEnemyTurn();
    }

    private void EndEnemyTurn()
    {
        StartAllyTurn();
    }

    public void EndBattle()
    {
        isBattleActive = false;
        isWaitingForMoveSelection = false;
        // Notify UI that battle has ended
        if (battleUI != null)
        {
            battleUI.OnBattleEnd();
        }
    }
}
