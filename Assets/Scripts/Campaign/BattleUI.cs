using UnityEngine;
using UnityEngine.UI;

namespace Injustice.Campaign
{
    public class BattleUI : MonoBehaviour
    {
        Transform slotParent;

        public void Init(Transform slotParent)
        {
            this.slotParent = slotParent;
        }

        public void SelectBattle()
        {
            slotParent.gameObject.SetActive(true);
        }
    }
}
