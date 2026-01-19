

namespace StudentAnalyzer;

public sealed class StudentConsole
{
     public void PresentStudents(IEnumerable<Student> students, string title)
    {
        Console.WriteLine($"\n=== {title} ===");
        foreach (var student in students)
        {
            Console.WriteLine(student.ToString());
        }
    }

    public void PresentFullNames(IEnumerable<string> names, string title)
    {
        Console.WriteLine($"\n=== {title} ===");
        foreach (var name in names)
        {
            Console.WriteLine(name);
        }
    }

    public void PresentSummaries(IEnumerable<object> summaries, string title)
    {
        Console.WriteLine($"\n=== {title} ===");
        foreach (var summary in summaries)
        {
            Console.WriteLine(summary?.ToString() ?? "N/A");
        }
    }

    public void PresentAges(IEnumerable<int> ages, string title)
    {
        Console.WriteLine($"\n=== {title} ===");
        Console.WriteLine($"Возрасты: {string.Join(", ", ages)}");
    }

    public void PresentGroups(IEnumerable<IGrouping<string, Student>> groups)
    {
        Console.WriteLine("\n=== Студенты по группам ===");
        foreach (var group in groups)
        {
            Console.WriteLine($"\nГруппа: {group.Key} ({group.Count()} студентов)");
            foreach (var student in group)
            {
                Console.WriteLine($"  - {student}");
            }
        }
    }

    public void PresentAverageByGroup(IEnumerable<(string Group, double Average)> averages)
    {
        Console.WriteLine("\n=== Средний балл по группам ===");
        foreach (var (group, average) in averages)
        {
            Console.WriteLine($"Группа {group}: {average:F2}");
        }
    }

    public void PresentGrades(IEnumerable<int> grades, string title)
    {
        Console.WriteLine($"\n=== {title} ===");
        var gradesList = grades.ToList();
        Console.WriteLine($"Оценки: {string.Join(", ", gradesList.Take(20))}");
        Console.WriteLine($"Всего оценок: {gradesList.Count}");
    }

    public void PresentGradePairs(IEnumerable<(int StudentId, int Grade)> pairs, string title)
    {
        Console.WriteLine($"\n=== {title} ===");
        foreach (var (studentId, grade) in pairs.Take(10))
        {
            Console.WriteLine($"StudentId: {studentId}, Grade: {grade}");
        }
        Console.WriteLine("... (показаны первые 10)");
    }

    public void PresentStudent(Student student, string title)
    {
        Console.WriteLine($"\n=== {title} ===");
        Console.WriteLine(student.ToString());
    }
}
