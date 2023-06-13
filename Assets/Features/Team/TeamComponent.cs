using UnityEngine;

namespace Features.Team
{
    public class TeamComponent : MonoBehaviour
    {
        [SerializeField] private TeamIndex teamIndex = TeamIndex.None;

        public TeamIndex TeamIndex => teamIndex;
        
        public bool IsSameTeam(TeamComponent other)
        {
            return teamIndex == other.teamIndex;
        }
    }
}