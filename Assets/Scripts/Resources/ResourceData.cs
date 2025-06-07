using UnityEngine;

[CreateAssetMenu(fileName = "NewResource", menuName = "Resources/Resource Definition")]
public class ResourceDefinition : ScriptableObject
{
    public string resourceName;
    public string description;
    public int resourceValue;

    public bool isRefillable; // Can this resource be refilled?
}
