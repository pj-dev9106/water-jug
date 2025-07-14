using System.ComponentModel.DataAnnotations;

namespace WaterJugChallenge.Models
{
    /// <summary>
    /// Request model for water jug problem parameters
    /// </summary>
    public class WaterJugRequest
    {
        /// <summary>
        /// Capacity of the first jug (X) in liters
        /// </summary>
        /// <example>3</example>
        [Required]
        public int XCapacity { get; set; }

        /// <summary>
        /// Capacity of the second jug (Y) in liters
        /// </summary>
        /// <example>5</example>
        [Required]
        public int YCapacity { get; set; }

        /// <summary>
        /// Target amount of water to measure in liters
        /// </summary>
        /// <example>4</example>
        [Required]
        public int TargetAmount { get; set; }
    }
}