using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcScript : MonoBehaviour
{
    public NpcInfo data;

    private void OnMouseDown()
    {
        DialogManager.Instance.StartDialogs(data);
    }
}
