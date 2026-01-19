namespace StudentAnalyzer;

public sealed class Student(int id, string firstName, string lastName, int age, string group, List<int> grades)
{
    public int Id { get; } = id;

    public string FirstName { get; } = firstName;

    public string LastName { get; } = lastName;

    public int Age { get; } = age;

    public string Group { get; } = group;

    public IReadOnlyList<int> Grades { get; } = grades;

    public override string ToString()
    {
        var grades = string.Join(", ", Grades);
        return $"Id: {Id}, Name: {FirstName} {LastName}, Age: {Age}, Group: {Group}, Grades: [{grades}]";
    }
}