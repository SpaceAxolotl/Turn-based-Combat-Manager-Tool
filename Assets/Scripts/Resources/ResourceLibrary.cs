using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceLibrary", menuName = "Resources/Resource Library")]
public class ResourceLibrary : ScriptableObject
{
    public List<ResourceDefinition> resourceDefinitions;
}
