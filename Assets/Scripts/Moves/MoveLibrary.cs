using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveLibrary", menuName = "Moves/Move Library")]
public class MoveLibrary : ScriptableObject
{
    public List<MoveData> moves;
}