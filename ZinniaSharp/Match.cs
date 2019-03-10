namespace ZinniaSharp
{
    /// <summary>
    /// Represents a single match result.
    /// </summary>
    public class Match
    {
        /// <summary>
        /// Gets the matched character.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Gets the score.
        /// </summary>
        public float Score { get; }

        internal Match(string value, float score)
        {
            this.Value = value;
            this.Score = score;
        }

        public override string ToString() => $"{Value} ({Score})";
    }
}
