using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLibrary", menuName = "Enemies/Enemy Library")]
public class EnemyLibrary : ScriptableObject
{
    public StatLibrary statLibrary;
    public TypeLibrary typeLibrary;
    public List<EnemyData> enemies;
}