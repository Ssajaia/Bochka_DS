using System;
using Collection.BochkaDS;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Testing Bochka<int> ===");
        TestIntBochka();

        Console.WriteLine("\n=== Testing Bochka<string> ===");
        TestStringBochka();

        Console.WriteLine("\n=== Testing Bochka<Person> (custom class) ===");
        TestPersonBochka();

        Console.WriteLine("\nAll tests completed.");
    }

    static void TestIntBochka()
    {
        // Constructor tests
        var emptyBochka = new Bochka<int>();
        Console.WriteLine($"Empty Bochka: {emptyBochka.Stringify()} (Count: {emptyBochka.Count})");

        var numbers = new Bochka<int>(1, 2, 3, 4, 5);
        Console.WriteLine($"Initialized with values: {numbers.Stringify()}");

        // Add method
        numbers.Add(6);
        Console.WriteLine($"After Add(6): {numbers.Stringify()}");

        // Indexer
        Console.WriteLine($"numbers[2] = {numbers[2]}");
        numbers[2] = 100;
        Console.WriteLine($"After numbers[2] = 100: {numbers.Stringify()}");

        // Contains
        Console.WriteLine($"Contains 100? {numbers.Contains(100)}");
        Console.WriteLine($"Contains 999? {numbers.Contains(999)}");

        // IndexOf
        Console.WriteLine($"IndexOf 100: {numbers.IndexOf(100)}");
        Console.WriteLine($"IndexOf 999: {numbers.IndexOf(999)}");

        // Insert
        numbers.Insert(3, 55);
        Console.WriteLine($"After Insert(3, 55): {numbers.Stringify()}");

        // Remove
        bool removed = numbers.Remove(100);
        Console.WriteLine($"Remove(100) result: {removed}, new content: {numbers.Stringify()}");

        // RemoveAt
        numbers.RemoveAt(0);
        Console.WriteLine($"After RemoveAt(0): {numbers.Stringify()}");

        // CopyTo
        int[] array = new int[10];
        numbers.CopyTo(array, 2);
        Console.WriteLine($"Copied to array: {string.Join(", ", array)}");

        // Clear
        numbers.Clear();
        Console.WriteLine($"After Clear(): {numbers.Stringify()} (Count: {numbers.Count})");

        // Operator tests
        var bochka1 = new Bochka<int>(1, 2, 3);
        var bochka2 = new Bochka<int>(1, 2, 3);
        var bochka3 = new Bochka<int>(4, 5, 6);

        Console.WriteLine($"\nOperator Tests:");
        Console.WriteLine($"bochka1 == bochka2: {bochka1 == bochka2}");
        Console.WriteLine($"bochka1 == bochka3: {bochka1 == bochka3}");
        Console.WriteLine($"bochka1 != bochka3: {bochka1 != bochka3}");
        Console.WriteLine($"bochka1 < bochka3: {bochka1 < bochka3}");
        Console.WriteLine($"bochka3 > bochka1: {bochka3 > bochka1}");
        Console.WriteLine($"bochka1 <= bochka2: {bochka1 <= bochka2}");
        Console.WriteLine($"bochka1 >= bochka2: {bochka1 >= bochka2}");

        // Capacity tests
        var capacityBochka = new Bochka<int>();
        Console.WriteLine($"\nInitial Capacity: {capacityBochka.Capacity}");
        for (int i = 0; i < 10; i++)
        {
            capacityBochka.Add(i);
            Console.WriteLine($"Count: {capacityBochka.Count}, Capacity: {capacityBochka.Capacity}");
        }
        capacityBochka.TrimExcess();
        Console.WriteLine($"After TrimExcess(): Capacity: {capacityBochka.Capacity}");

        // Enumeration test
        Console.WriteLine("\nEnumeration Test:");
        foreach (var num in capacityBochka)
        {
            Console.Write($"{num} ");
        }
        Console.WriteLine();
    }

    static void TestStringBochka()
    {
        var words = new Bochka<string>("apple", "banana", "cherry");
        Console.WriteLine($"Initial words: {words.Stringify()}");

        // Add null value
        words.Add(null);
        Console.WriteLine($"After adding null: {words.Stringify()}");

        // Contains with null
        Console.WriteLine($"Contains null? {words.Contains(null)}");

        // IndexOf with null
        Console.WriteLine($"IndexOf null: {words.IndexOf(null)}");

        // String-specific operations
        words[1] = "blueberry";
        Console.WriteLine($"After words[1] = 'blueberry': {words.Stringify()}");

        // Comparison tests
        var words2 = new Bochka<string>("apple", "blueberry", "cherry", null);
        Console.WriteLine($"words == words2: {words == words2}");
    }

    static void TestPersonBochka()
    {
        var people = new Bochka<Person>(
            new Person("Alice", 30),
            new Person("Bob", 25),
            new Person("Charlie", 35)
        );

        Console.WriteLine($"Initial people: {people.Stringify()}");

        // Contains with custom object
        var Saba = new Person("Saba", 30);
        Console.WriteLine($"Contains Saba? {people.Contains(Saba)}");

        // IndexOf with custom object
        Console.WriteLine($"IndexOf Saba: {people.IndexOf(Saba)}");

        // Operator tests with custom objects
        var people2 = new Bochka<Person>(
            new Person("Saba", 30),
            new Person("Giorgi", 25),
            new Person("vighaca", 35)
        );

        var people3 = new Bochka<Person>(
            new Person("Levani", 40),
            new Person("Totla", 35)
        );

        Console.WriteLine($"people == people2: {people == people2}");
        Console.WriteLine($"people == people3: {people == people3}");
        Console.WriteLine($"people < people3: {people < people3}");
    }
}

class Person : IComparable<Person>
{
    public string Name { get; }
    public int Age { get; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public override bool Equals(object obj)
    {
        return obj is Person person &&
               Name == person.Name &&
               Age == person.Age;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Age);
    }

    public int CompareTo(Person other)
    {
        if (other is null) return 1;
        int nameComparison = Name.CompareTo(other.Name);
        if (nameComparison != 0) return nameComparison;
        return Age.CompareTo(other.Age);
    }

    public override string ToString()
    {
        return $"{Name} ({Age})";
    }
}