# Water Jug Challenge API

A REST API for solving the classic Water Jug problem. Given two jugs with different capacities and a target amount, the API finds the optimal sequence of steps to measure exactly the target amount of water.

## Table of Contents

- [Swagger Documentation](#swagger-documentation)
- [Problem Description](#problem-description)
- [Algorithm](#algorithm)
- [API Endpoints](#api-endpoints)
- [Setup and Installation](#setup-and-installation)
- [Running the Application](#running-the-application)
- [Testing](#testing)
- [Mathematical Background](#mathematical-background)

## Swagger Documentation

Visit the link after running the project.
`http://localhost:{portnumber}/swagger`

## Problem Description

You have two jugs with capacities X and Y. You can perform the following operations:
- Fill a jug to its capacity
- Empty a jug completely
- Pour water from one jug to another until the source jug is empty or the destination jug is full

The goal is to measure exactly Z units of water in one of the jugs.

## Algorithm

The solution uses a **Breadth-First Search (BFS)** algorithm to find the shortest path to the target amount. Here's how it works:

1. **State Representation**: Each state is represented as `(x, y)` where x and y are the current amounts in each jug
2. **Initial State**: Start with both jugs empty `(0, 0)`
3. **Goal State**: Reach a state where either jug contains exactly the target amount
4. **Actions**: At each state, generate all possible next states by:
   - Filling jug X to capacity
   - Filling jug Y to capacity
   - Emptying jug X completely
   - Emptying jug Y completely
   - Pouring from X to Y
   - Pouring from Y to X
5. **Search**: Use BFS to explore all possible states until the target is reached
6. **Path Tracking**: Maintain the sequence of actions taken to reach the solution

### Solvability Conditions

A solution exists if and only if:
1. `targetAmount ≤ max(xCapacity, yCapacity)`
2. `targetAmount` is divisible by the greatest common divisor (GCD) of `xCapacity` and `yCapacity`
3. `targetAmount ≥ 0`

## API Endpoints

### POST /api/water-jug

Solves the water jug problem and returns the sequence of steps needed to reach the target amount.

**Request:**
```http
POST /api/water-jug
Content-Type: application/json

{
  "xCapacity": 2,
  "yCapacity": 100,
  "targetAmount": 96
}
```

**Response (Success - 200 OK):**
```json
{
  "steps": [
    {
      "xAmount": 0,
      "yAmount": 100,
      "action": "Fill Y jug"
    },
    {
      "xAmount": 2,
      "yAmount": 98,
      "action": "Pour Y to X"
    },
    {
      "xAmount": 0,
      "yAmount": 98,
      "action": "Empty X jug"
    },
    {
      "xAmount": 2,
      "yAmount": 96,
      "action": "Pour Y to X"
    }
  ]
}
```

**Response (Error - 400 Bad Request):**
```json
{
  "error": "Invalid input: Values must be positive integers."
}
```

## Setup and Installation

### Prerequisites

- .NET 9.0 SDK

### Installation Steps

1. **Clone the repository:**
   ```bash
   git clone https://github.com/pj-dev9106/water-jug
   cd water-jug
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Build the application:**
   ```bash
   dotnet build
   ```

## Running the Application

### Development Mode

```bash
dotnet run --project WaterJugCallenge
```

The application will start on `http://localhost:5116` (or the next available port).

### Using the Provided Scripts

**Windows:**
```bash
run-script.bat
```

## Testing

### Running Unit Tests

```bash
dotnet test
```

### Running Specific Test Categories

```bash
# Run only unit tests
dotnet test --filter "FullyQualifiedName~WaterJugServiceTests"

# Run only integration tests
dotnet test --filter "FullyQualifiedName~WaterJugIntegrationTests"
```

### Using the Test Script

**Windows:**
```bash
run-test.bat
```

## Mathematical Background

### Greatest Common Divisor (GCD)

The GCD of two numbers is the largest number that divides both of them without leaving a remainder. For example:
- GCD(4, 6) = 2
- GCD(7, 11) = 1 (coprime)
- GCD(8, 12) = 4

### Coprime Numbers

Two numbers are coprime if their GCD is 1. Coprime jugs can measure any amount up to the larger jug's capacity.

**Examples:**
- (3, 5): GCD = 1, can measure 0, 1, 2, 3, 4, 5
- (7, 11): GCD = 1, can measure 0, 1, 2, ..., 11

### Non-Coprime Numbers

For non-coprime jugs, only amounts divisible by the GCD can be measured.

**Examples:**
- (4, 6): GCD = 2, can only measure 0, 2, 4, 6
- (8, 12): GCD = 4, can only measure 0, 4, 8, 12

### Algorithm Complexity

- **Time Complexity**: O(xCapacity × yCapacity) in worst case
- **Space Complexity**: O(xCapacity × yCapacity) for visited states
- **Optimality**: BFS guarantees the shortest solution path