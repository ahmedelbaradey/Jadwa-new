namespace Domain.Entities.ResolutionManagement
{
    /// <summary>
    /// Value object representing a resolution code
    /// Encapsulates the business logic for generating resolution codes
    /// Format: fund code/resolution year/resolution no.
    /// Example: FUND001/2024/001
    /// Based on requirements in Sprint.md for resolution code generation
    /// </summary>
    public class ResolutionCode
    {
        /// <summary>
        /// The complete resolution code string
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Fund code component
        /// </summary>
        public string FundCode { get; private set; }

        /// <summary>
        /// Year component
        /// </summary>
        public int Year { get; private set; }

        /// <summary>
        /// Sequential number component
        /// </summary>
        public int SequentialNumber { get; private set; }

        /// <summary>
        /// Private constructor to enforce creation through factory methods
        /// </summary>
        private ResolutionCode(string fundCode, int year, int sequentialNumber)
        {
            FundCode = fundCode;
            Year = year;
            SequentialNumber = sequentialNumber;
            Value = $"{fundCode}/{year}/{sequentialNumber:D3}";
        }

        /// <summary>
        /// Creates a new resolution code
        /// </summary>
        /// <param name="fundCode">The fund code</param>
        /// <param name="year">The resolution year</param>
        /// <param name="sequentialNumber">The sequential number for this year</param>
        /// <returns>A new ResolutionCode instance</returns>
        public static ResolutionCode Create(string fundCode, int year, int sequentialNumber)
        {
            if (string.IsNullOrWhiteSpace(fundCode))
                throw new ArgumentException("Fund code cannot be null or empty", nameof(fundCode));

            if (year < 1900 || year > 9999)
                throw new ArgumentException("Year must be between 1900 and 9999", nameof(year));

            if (sequentialNumber < 1)
                throw new ArgumentException("Sequential number must be greater than 0", nameof(sequentialNumber));

            return new ResolutionCode(fundCode, year, sequentialNumber);
        }

        /// <summary>
        /// Parses a resolution code string into a ResolutionCode object
        /// </summary>
        /// <param name="codeString">The resolution code string to parse</param>
        /// <returns>A ResolutionCode instance</returns>
        public static ResolutionCode Parse(string codeString)
        {
            if (string.IsNullOrWhiteSpace(codeString))
                throw new ArgumentException("Code string cannot be null or empty", nameof(codeString));

            var parts = codeString.Split('/');
            if (parts.Length != 3)
                throw new ArgumentException("Invalid resolution code format. Expected format: fundcode/year/number", nameof(codeString));

            if (!int.TryParse(parts[1], out int year))
                throw new ArgumentException("Invalid year in resolution code", nameof(codeString));

            if (!int.TryParse(parts[2], out int sequentialNumber))
                throw new ArgumentException("Invalid sequential number in resolution code", nameof(codeString));

            return new ResolutionCode(parts[0], year, sequentialNumber);
        }

        /// <summary>
        /// Implicit conversion to string
        /// </summary>
        public static implicit operator string(ResolutionCode resolutionCode)
        {
            return resolutionCode?.Value ?? string.Empty;
        }

        /// <summary>
        /// Explicit conversion from string
        /// </summary>
        public static explicit operator ResolutionCode(string codeString)
        {
            return Parse(codeString);
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj)
        {
            return obj is ResolutionCode other && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(ResolutionCode? left, ResolutionCode? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ResolutionCode? left, ResolutionCode? right)
        {
            return !Equals(left, right);
        }
    }
}
