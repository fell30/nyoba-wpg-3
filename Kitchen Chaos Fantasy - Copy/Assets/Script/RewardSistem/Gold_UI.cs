

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gold_UI : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    public void UpdateGoldText(int goldAmount)
    {
        if (goldText != null)
        {
            goldText.text = goldAmount.ToString();
        }
    }
}
