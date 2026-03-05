namespace CardSelectionSystem.Core.Models
{
    public readonly struct BlockRange
    {
        public int Start { get; }
        public int End { get; }
        public int Width => End - Start + 1;

        public BlockRange(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}
