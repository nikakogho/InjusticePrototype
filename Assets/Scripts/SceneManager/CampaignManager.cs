using UnityEngine;

namespace Injustice.Campaign
{
    [RequireComponent(typeof(CampaignData))]
    public class CampaignManager : MonoBehaviour
    {
        public static CampaignManager instance;

        CampaignData data;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {

        }
    }
}