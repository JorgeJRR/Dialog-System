using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Info", menuName = "NPC")]

public class NpcInfo : ScriptableObject
{
    public Sprite portrait;
    public string NpcName;
    public List<DialogInfo> dialogs;
}
