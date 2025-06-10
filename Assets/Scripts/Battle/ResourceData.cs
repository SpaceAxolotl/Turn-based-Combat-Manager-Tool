using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Battle/Resource Data")]
public class ResourceData : ScriptableObject
{
    public string resourceName;
    public int maxValue = 100;
    public int currentValue;
    public Color resourceColor = Color.white;
    [TextArea(3, 5)]
    public string description;
} 