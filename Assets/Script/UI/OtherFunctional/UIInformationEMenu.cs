using Registrator;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UIInformationEMenu : UIInformationE
{
    [SerializeField] private Image dinamicImg;
    protected override void TextEnableOn(Construction player, int recipientHash)
    {
        if (dinamicImg.enabled == false)
        {
            if (thisHash == player.Hash) { currentRecipientHash = recipientHash; dinamicImg.enabled = true; }
        }
    }
    protected override void TextEnableOff(int recipientHash)
    {
        if (dinamicImg.enabled == true && currentRecipientHash == recipientHash) { dinamicImg.enabled = false; }
    }
    protected override void SetClass()
    {
        if (!isRun)
        {
            if (trackingObject != null)
            {
                dinamicImg.enabled = false;

                isRun = true;
            }
            else { isRun = false; }
        }
    }
    protected override void LateUpdate()
    {

    }
}

