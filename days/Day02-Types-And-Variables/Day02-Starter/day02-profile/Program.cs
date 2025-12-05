// See https://aka.ms/new-console-template for more information
Console.WriteLine("=== Welcome to your Profile ===");

Console.WriteLine(" ");
Console.WriteLine("What is your name?");
string? name = Console.ReadLine();

Console.WriteLine(" ");
Console.WriteLine("What is your age?");
string? age = Console.ReadLine();

Console.WriteLine(" ");
Console.WriteLine("What is your current salary?");
string? salaryInput = Console.ReadLine();
if (!double.TryParse(salaryInput, out double salary))
{
	// If parsing fails or input is null, default to 0.0
	salary = 0.0;
}

Console.WriteLine(" ");
Console.WriteLine("=== Your Information ===");
Console.WriteLine($"Name: {name}");
Console.WriteLine($"Current Age: {age}");
int ageIn10Years = int.Parse(age) + 10;
Console.WriteLine($"Age in 10 years: {ageIn10Years}");
Console.WriteLine($"Current Salary: ${salary:N2}");
double salaryWithRaise = salary * 1.1;
Console.WriteLine($"Salary with 10% raise: ${salaryWithRaise:N2}");
