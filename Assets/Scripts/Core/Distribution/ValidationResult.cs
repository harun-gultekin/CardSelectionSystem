using System.Collections.Generic;

namespace CardSelectionSystem.Core.Distribution
{
    public class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public int ValidCount { get; }
        public int TotalCount { get; }
        public List<string> Errors { get; }

        public ValidationResult(int validCount, int totalCount, List<string> errors)
        {
            ValidCount = validCount;
            TotalCount = totalCount;
            Errors = errors;
        }
    }
}
