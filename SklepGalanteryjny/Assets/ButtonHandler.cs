using UnityEngine;
using UnityEngine.UI;
public class ButtonHandler : MonoBehaviour
{
    public BreakSystem breakSystem;
    public bool isExitButtonClicked = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExitSpecialItemShopButton()
    {
        isExitButtonClicked=true;
        breakSystem.BreakTimeOver();
    }

    public void BuySpecialItemButton(Image ItemInfoPanel)
    {
        if (ItemInfoPanel != null)
        {
            ItemInfoPanel.gameObject.SetActive(false); // Wy³¹cz okreœlony panel
        }
    }
}
