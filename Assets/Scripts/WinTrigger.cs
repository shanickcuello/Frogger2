using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] GameObject winPanel, loosePanel;

    internal void IWin() => winPanel.SetActive(true);

    internal void ILoose() => loosePanel.SetActive(true);
}
