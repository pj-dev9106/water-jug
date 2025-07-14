using System.Collections.Generic;

namespace WaterJugChallenge.Models
{
    /// <summary>
    /// Response model for water jug solution
    /// </summary>
    public class WaterJugResponse
    {
        /// <summary>
        /// Sequence of steps to reach the target amount
        /// </summary>
       
        public List<WaterJugState> Steps { get; set; } = new List<WaterJugState>();
    }

    /// <summary>
    /// Error response model
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Error message
        /// </summary>
        /// <example>Invalid input: Values must be positive integers.</example>
        public string Error { get; set; } = string.Empty;
    }
} 