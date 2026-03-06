using System;

namespace CardSelectionSystem.Core.Models
{
    [Serializable]
    public class CycleState
    {
        public string[] CycleSequence;
        public int CurrentRound;

        public CycleState() { }

        public CycleState(string[] cycleSequence, int currentRound)
        {
            CycleSequence = cycleSequence;
            CurrentRound = currentRound;
        }
    }
}
