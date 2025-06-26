[# Bochka_DS](https://img.shields.io/badge/C%2523-10.0-blue
https://img.shields.io/badge/.NET-6.0-purple
https://img.shields.io/badge/license-MIT-green

Bochka (Russian for "barrel") is a generic collection data structure implementation in C# that provides array-like functionality with dynamic resizing and additional features.

Features
🧩 Generic implementation works with any data type

⚡ Dynamic resizing when capacity is reached

🔍 Full collection interface implementation (IEnumerable<T>, ICollection<T>)

🔄 Comprehensive operator overloading (==, !=, <, >, <=, >=)

📏 Capacity management with TrimExcess()

🛡️ Thread-safe enumeration with version checking

📊 Multiple constructors for different initialization scenarios

Installation
Add the BochkaDS namespace to your project:

csharp
using Collection.BochkaDS;
Usage
Basic Operations
csharp
// Create a new Bochka with integers
var numbers = new Bochka<int>(1, 2, 3);

// Add elements
numbers.Add(4);
numbers.Insert(2, 10);

// Access elements
int value = numbers[1];  // Gets 2
numbers[1] = 5;          // Sets to 5

// Remove elements
numbers.Remove(3);
numbers.RemoveAt(0);

// Check contents
bool contains = numbers.Contains(5);
int index = numbers.IndexOf(10);
Collection Operations
csharp
// Copy to array
int[] array = new int[10];
numbers.CopyTo(array, 2);

// Clear the collection
numbers.Clear();

// Enumeration
foreach (var num in numbers)
{
    Console.WriteLine(num);
}
Comparison Operators
csharp
var set1 = new Bochka<int>(1, 2, 3);
var set2 = new Bochka<int>(1, 2, 3);
var set3 = new Bochka<int>(4, 5, 6);

Console.WriteLine(set1 == set2);  // True
Console.WriteLine(set1 != set3);  // True
Console.WriteLine(set1 < set3);   // True
Console.WriteLine(set3 > set1);   // True
Performance
Operation	Complexity
Add	O(1) amortized
Insert	O(n)
Remove	O(n)
Index access	O(1)
Contains	O(n)
IndexOf	O(n)
Advanced Features
Capacity Management
csharp
var items = new Bochka<int>(capacity: 100);
Console.WriteLine(items.Capacity);  // 100

// Reduce memory usage
items.TrimExcess();
Custom Object Support
csharp
public class Person : IComparable<Person>
{
    public string Name { get; }
    public int Age { get; }
    
    // Implementation omitted for brevity
}

var people = new Bochka<Person>(
    new Person("Alice", 30),
    new Person("Bob", 25)
);
Testing
The project includes comprehensive test cases in Program.cs that verify:

All basic collection operations

Edge cases and error conditions

Operator overloads

Performance characteristics

Contributing
Contributions are welcome! Please follow these steps:

Fork the repository

Create your feature branch (git checkout -b feature/AmazingFeature)

Commit your changes (git commit -m 'Add some amazing feature')

Push to the branch (git push origin feature/AmazingFeature)

Open a Pull Request

License
This project is licensed under the MIT License - see the LICENSE file for details.

Acknowledgments
Inspired by .NET's List<T> implementation

Special thanks to the C# language team for generics support)
