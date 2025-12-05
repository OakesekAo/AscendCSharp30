using System;

// Day 03 — Control Flow: Complete Example
// Demonstrates: if/else, loops, input validation, grade calculation

Console.WriteLine("=== Grade Calculator ===\n");

// Step 1: Create array to store 5 scores
int[] scores = new int[5];

// Step 2: Collect and validate 5 test scores
for (int i = 0; i < 5; i++)
{
    int score = -1;
    
    // Keep asking until valid (0-100)
    while (score < 0 || score > 100)
    {
        Console.Write($"Enter test score {i + 1} (0-100): ");
        score = int.Parse(Console.ReadLine());
        
        if (score < 0 || score > 100)
        {
            Console.WriteLine("Invalid! Enter a number between 0-100.");
        }
    }
    
    scores[i] = score;
}

// Step 3: Calculate average
double average = 0;
for (int i = 0; i < scores.Length; i++)
{
    average += scores[i];
}
average /= scores.Length;

// Step 4: Assign letter grade
char grade;
if (average >= 90)
    grade = 'A';
else if (average >= 80)
    grade = 'B';
else if (average >= 70)
    grade = 'C';
else if (average >= 60)
    grade = 'D';
else
    grade = 'F';

// Step 5: Determine message
string message = grade switch
{
    'A' => "Excellent work!",
    'B' => "Good job!",
    'C' => "Average. Keep practicing.",
    'D' => "Below average. Study more.",
    'F' => "Failing. Get help!",
    _ => "Unknown"
};

// Step 6: Display results
Console.WriteLine("\n=== Results ===");
Console.Write("Scores: ");
for (int i = 0; i < scores.Length; i++)
{
    Console.Write(scores[i]);
    if (i < scores.Length - 1)
        Console.Write(", ");
}
Console.WriteLine();
Console.WriteLine($"Average: {average:N1}");
Console.WriteLine($"Grade: {grade}");
Console.WriteLine(message);

