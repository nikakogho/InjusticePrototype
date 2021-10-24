using UnityEngine;

namespace Injustice.Campaign
{
    [RequireComponent(typeof(CampaignManager))]
    public class CampaignData : MonoBehaviour
    {
        public Transform actParent;
        public Transform combatSlotParentParent;
        public GameObject actPrefab, battlePrefab, combatPrefab;

        public GameObject combatSlotParentPrefab;

        public Act[] acts;

        public static Character chosenPlayer;

        public static CampaignData instance;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            int actIndex = 0;
            int battleIndex = 0;
            int combatIndex = 0;

            foreach (var act in acts)
            {
                Transform actTransform = Instantiate(actPrefab, actParent.position, actParent.rotation, actParent).transform;

                actTransform.name = "Act " + actIndex + " : " + act.name;

                foreach (var battle in act.battles)
                {
                    Transform battleTransform = Instantiate(battlePrefab, actTransform.position, actTransform.rotation, actTransform).transform;

                    battleTransform.name = "Battle " + battleIndex + " : " + battle.name;

                    Transform combatSlotParent = Instantiate(combatSlotParentPrefab, battleTransform.position, battleTransform.rotation, combatSlotParentParent).transform;

                    BattleUI battleUI = battleTransform.GetComponent<BattleUI>();

                    foreach(var combat in battle.combats)
                    {
                        Transform combatTransform = Instantiate(combatPrefab, battleTransform.position, battleTransform.rotation, combatSlotParent).transform;

                        combatTransform.name = "Combat " + combatIndex + " : " + combat.name;

                        var slot = combatTransform.GetComponent<CombatSlot>();

                        slot.Apply(chosenPlayer, combat);
                    }

                    battleUI.Init(combatSlotParent);
                }
            }
        }

        [System.Serializable]
        public class Act
        {
            public string name;
            public Battle[] battles;

            [System.Serializable]
            public class Battle
            {
                public string name;
                public string description;
                public GameObject environment;
                public Combat[] combats;

                [System.Serializable]
                public class Combat
                {
                    public string name;
                    public string description;
                    public CustomCharacter opponent;
                }
            }
        }
    }
}