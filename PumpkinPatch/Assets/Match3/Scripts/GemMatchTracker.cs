using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class GemMatchTracker
    {
        private Dictionary<GemType, int> matchCounts = new Dictionary<GemType, int>();
        private const int winThreshold = 20; // Number of matches required to win
        private GemType winGemType; // Target gem type required to win

        // Constructor to set the target gem type for the win condition
        public GemMatchTracker(GemType targetGemType)
        {
            if (targetGemType == null)
            {
                Debug.LogError("Target GemType is null! Please assign a valid GemType.");
                return;
            }

            winGemType = targetGemType;
        }

        // Track a match for the given gem type
        public void TrackMatch(GemType gemType)
        {
            if (gemType == null)
            {
                Debug.LogError("Cannot track a null GemType.");
                return;
            }

            if (!matchCounts.ContainsKey(gemType))
            {
                matchCounts[gemType] = 0;
            }

            matchCounts[gemType]++;
        }

        // Get the total matches for a given gem type
        public int GetMatchCount(GemType gemType)
        {
            return gemType != null && matchCounts.ContainsKey(gemType) ? matchCounts[gemType] : 0;
        }

        // Get the remaining matches needed for the win condition
        public int MatchesLeftForWin()
        {
            int currentMatches = GetMatchCount(winGemType);
            return Mathf.Max(0, winThreshold - currentMatches);
        }

        // Check if the player has matched enough gems to win
        public bool HasPlayerWon()
        {
            return GetMatchCount(winGemType) >= winThreshold;
        }
    }
}
