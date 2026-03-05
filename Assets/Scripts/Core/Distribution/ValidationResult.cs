using System.Collections.Generic;

namespace CardSelectionSystem.Core.Distribution
{
    public class ValidationResult
    {
        public bool IsValid { get; }
        public int ValidCount { get; }
        public int TotalCount { get; }
        public List<string> Errors { get; }

        public ValidationResult(bool isValid, int validCount, int totalCount, List<string> errors)
        {
            IsValid = isValid;
            ValidCount = validCount;
            TotalCount = totalCount;
            Errors = errors;
        }
    }
}
