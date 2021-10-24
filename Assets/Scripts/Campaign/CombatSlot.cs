using UnityEngine;
using UnityEngine.UI;

namespace Injustice.Campaign
{
    public class CombatSlot : MonoBehaviour
    {
        public Text descriptionText;

        public Character opponent;
        public CharacterSlot playerSlot, opponentSlot;

        public void Apply(Character player, CampaignData.Act.Battle.Combat combat)
        {
            name = combat.name;
            descriptionText.text = combat.description;

            opponent = combat.opponent.ToCharacter();

            playerSlot.Apply(player);
            opponentSlot.Apply(opponent);
        }

        public void StartCombat()
        {

        }
    }
}