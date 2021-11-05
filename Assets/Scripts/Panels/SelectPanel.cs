using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanel : MonoBehaviour, IPanel
{
    public Text infoText;
    public string caseName, caseLocation, caseDate;
    private void OnEnable()
    {
        caseName = UIManager.Instance.activeCase.name;
        caseLocation = UIManager.Instance.activeCase.location;
        caseDate = UIManager.Instance.activeCase.date;
        infoText.text = $"{caseName}\n{caseLocation}\n{caseDate}";
    }
    public void ProcessInfo()
    {
        UIManager.Instance.overviewPanel.gameObject.SetActive(true);
    }
}
