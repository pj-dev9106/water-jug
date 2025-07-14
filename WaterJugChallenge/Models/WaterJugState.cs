namespace WaterJugChallenge.Models
{
    /// <summary>
    /// Represents a state of the water jugs during the solution process
    /// </summary>
    public class WaterJugState
    {
        /// <summary>
        /// Current amount of water in jug X in liters
        /// </summary>
        /// <example>3</example>
        public int XAmount { get; set; }

        /// <summary>
        /// Current amount of water in jug Y in liters
        /// </summary>
        /// <example>2</example>
        public int YAmount { get; set; }

        /// <summary>
        /// Action performed in this step
        /// </summary>
        /// <example>Pour Y to X</example>
        public string? Action { get; set; }
    }
}