using UnityEngine;
using UnityEngine.UI;

public class AccountSlot : MonoBehaviour
{
    public Text nameText;

    public void Select()
    {
        GameStarter.instance.SelectAccount(nameText.text);
    }
}
