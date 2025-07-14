using System;
using System.Collections.Generic;
using WaterJugChallenge.Models;

namespace WaterJugChallenge.Services
{
    public class WaterJugService
    {
        public (List<WaterJugState> Steps, string Error) Solution(int xCapacity, int yCapacity, int targetAmount)
        {
            if (!IsValidInput(xCapacity, yCapacity, targetAmount))
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                return (null, "Invalid input: Values must be positive integers.");
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

            if (!IsPossible(xCapacity, yCapacity, targetAmount))
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                return (null, "No solution possible.");
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

            return FindSolution(xCapacity, yCapacity, targetAmount);
        }

        private bool IsValidInput(int x, int y, int z)
        {
            return x > 0 && y > 0 && z >= 0;
        }

        private bool IsPossible(int x, int y, int z)
        {
            if (z > Math.Max(x, y)) return false;
            if (z == 0) return true;
            return z % GCD(x, y) == 0;
        }

        private int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        private (List<WaterJugState>, string) FindSolution(int xCapacity, int yCapacity, int targetAmount)
        {
            var visited = new HashSet<(int, int)>();
            var queue = new Queue<(int x, int y, List<WaterJugState> path)>();
            queue.Enqueue((0, 0, new List<WaterJugState> { }));

            while (queue.Count > 0)
            {
                var (x, y, path) = queue.Dequeue();

                if (x == targetAmount || y == targetAmount)
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                    return (path, null);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

                if (!visited.Add((x, y))) continue;

                // Possible actions
                var actions = new[]
                {
                    (xCapacity, y, "Fill X jug"),
                    (x, yCapacity, "Fill Y jug"),
                    (0, y, "Empty X jug"),
                    (x, 0, "Empty Y jug"),
                    (Math.Max(0, x - (yCapacity - y)), Math.Min(yCapacity, y + x), "Pour X to Y"),
                    (Math.Min(xCapacity, x + y), Math.Max(0, y - (xCapacity - x)), "Pour Y to X")
                };

                foreach (var (nextX, nextY, action) in actions)
                {
                    if (nextX == x && nextY == y) continue;
                    var newPath = new List<WaterJugState>(path) { new WaterJugState { XAmount = nextX, YAmount = nextY, Action = action } };
                    queue.Enqueue((nextX, nextY, newPath));
                }
            }

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            return (null, "No solution found.");
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }
    }
}