using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AllyDataReader : MonoBehaviour
{
    [Header("Ally Data")]
    [SerializeField] private AllyData allyData;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private TextMeshProUGUI movesText;
    [SerializeField] private TextMeshProUGUI typesText;
    [SerializeField] private TextMeshProUGUI resourcesText;
    [SerializeField] private Image spriteImage;

    private void Start()
    {
        if (allyData != null)
        {
            DisplayAllyData();
        }
    }

    public void SetAllyData(AllyData data)
    {
        allyData = data;
        DisplayAllyData();
    }

    private void DisplayAllyData()
    {
        if (allyData == null) return;

        // Display basic info
        if (nameText != null) nameText.text = $"Name: {allyData.allyName}";
        if (descriptionText != null) descriptionText.text = $"Description: {allyData.allyDescription}";
        if (spriteImage != null && allyData.sprite != null) spriteImage.sprite = allyData.sprite;

        // Display stats
        if (statsText != null)
        {
            string statsString = "Stats:\n";
            foreach (var stat in allyData.stats)
            {
                if (stat.statDefinition != null)
                {
                    statsString += $"- {stat.statDefinition.statName}: {stat.value}\n";
                }
            }
            statsText.text = statsString;
        }

        // Display moves
        if (movesText != null)
        {
            string movesString = "Moves:\n";
            foreach (var move in allyData.moves)
            {
                if (move != null)
                {
                    movesString += $"- {move.moveName} (Power: {move.power}, Accuracy: {move.accuracy}%)\n";
                    if (move.attackType != null)
                    {
                        movesString += $"  Type: {move.attackType.typeName}\n";
                    }
                }
            }
            movesText.text = movesString;
        }

        // Display types
        if (typesText != null)
        {
            string typesString = "Types:\n";
            foreach (var type in allyData.types)
            {
                if (type != null)
                {
                    typesString += $"- {type.typeName}\n";
                }
            }
            typesText.text = typesString;
        }

        // Display resources
        if (resourcesText != null)
        {
            string resourcesString = "Resources:\n";
            foreach (var resource in allyData.resources)
            {
                if (resource.resourceDefinition != null)
                {
                    resourcesString += $"- {resource.resourceDefinition.resourceName}: {resource.value}\n";
                }
            }
            resourcesText.text = resourcesString;
        }
    }

    // Method to update the display when AllyData changes in the inspector
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        DisplayAllyData();
    }
} 