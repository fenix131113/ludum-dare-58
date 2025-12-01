using ItemsSystem.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BookLevelData
{
    [field: SerializeField] public int LevelIndex { get; private set; }
    [field: SerializeField] public Image IconImage { get; private set; }
    [field: SerializeField] public Sprite ChangeIcon { get; private set; }
    [field: SerializeField] public Button StartButton { get; private set; }
    [field: SerializeField] public TMP_Text selectedText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI WarningText { get; private set; }
    [field: SerializeField] public ItemDataSO[] NeededItems { get; private set; }
    [field: SerializeField] public GameObject[] ObjectsToActivate { get; private set; }
    [field: SerializeField] public GameObject[] ObjectsToDeactivate { get; private set; }
}
