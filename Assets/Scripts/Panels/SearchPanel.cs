using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchPanel : MonoBehaviour, IPanel
{
    public InputField caseNumberInput;
    public void ProcessInfo()
    {
        AWSManager.Instance.GetCaseList(caseNumberInput.text, () =>
        {
            UIManager.Instance.selectPanel.gameObject.SetActive(true);
        });
    }
}
