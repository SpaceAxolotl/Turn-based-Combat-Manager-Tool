using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackTypeLibrary", menuName = "Combat/Attack Type Library")]
public class AttackTypeLibrary : ScriptableObject
{
    public List<AttackType> attackTypes;
}
