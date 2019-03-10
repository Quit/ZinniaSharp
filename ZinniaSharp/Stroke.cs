namespace ZinniaSharp
{
    /// <summary>
    /// Identifies a "stroke" on a character.
    /// </summary>
    public class Stroke
    {
        private readonly Character character;
        private readonly int id;

        internal Stroke(Character character, int idx)
        {
            this.character = character;
            this.id = idx;
        }

        /// <summary>
        /// Adds another point to the stroke.
        /// </summary>
        /// <param name="x">The X value of the point.</param>
        /// <param name="y">The Y value of the point.</param>
        /// <returns></returns>
        public Stroke Add(int x, int y)
        {
            character.AddStroke(this.id, x, y);
            return this;
        }
    }
}
