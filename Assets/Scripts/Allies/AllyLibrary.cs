using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllyLibrary", menuName = "Allies/Ally Library")]
public class AllyLibrary : ScriptableObject
{
    public StatLibrary statLibrary;
    public TypeLibrary typeLibrary;
    public List<AllyData> allies;
}
