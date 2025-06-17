using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    public DialogManagerUI m_HandlerUI;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDialogs(NpcInfo NpcData)
    {
        m_HandlerUI.PrepareDialogElements(NpcData);
    }

    private void Start()
    {

    }
}

[System.Serializable]
public struct DialogInfo
{
    public string dialog;
    public bool isPlayer;
}
