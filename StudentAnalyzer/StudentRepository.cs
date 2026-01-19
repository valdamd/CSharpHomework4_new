namespace StudentAnalyzer;

public class StudentRepository
{
    public IEnumerable<Student> GetTestStudents()
    {
        var random = new Random(42);

        return new List<Student>
        {
            new(1, "Иван", "Петров", 20, "CS-101", GenerateGrades(random, 85, 95)),
            new(2, "Мария", "Сидорова", 19, "CS-101", GenerateGrades(random, 90, 98)),
            new(3, "Алексей", "Иванов", 22, "MATH-201", GenerateGrades(random, 50, 65)),
            new(4, "Елена", "Смирнова", 21, "MATH-201", GenerateGrades(random, 75, 85)),
            new(5, "Дмитрий", "Козлов", 23, "PHYS-301", GenerateGrades(random, 60, 70)),
            new(6, "Анна", "Новикова", 18, "CS-101", GenerateGrades(random, 95, 100)),
            new(7, "Сергей", "Морозов", 24, "PHYS-301", GenerateGrades(random, 45, 58)),
            new(8, "Ольга", "Волкова", 20, "MATH-201", GenerateGrades(random, 80, 90)),
            new(9, "Павел", "Соколов", 25, "PHYS-301", GenerateGrades(random, 70, 80)),
            new(10, "Наталья", "Лебедева", 19, "CS-101", GenerateGrades(random, 88, 95)),
            new(11, "Андрей", "Козлов", 21, "MATH-201", GenerateGrades(random, 55, 65)),
            new(12, "Татьяна", "Новикова", 22, "PHYS-301", GenerateGrades(random, 92, 98)),
            new(13, "Михаил", "Федоров", 20, "CS-101", GenerateGrades(random, 65, 75)),
            new(14, "Екатерина", "Павлова", 23, "MATH-201", GenerateGrades(random, 50, 60)),
            new(15, "Николай", "Семенов", 18, "PHYS-301", GenerateGrades(random, 85, 95)),
        };
    }

    private static List<int> GenerateGrades(Random random, int min, int max)
    {
        var gradeCount = random.Next(5, 9);
        return Enumerable.Range(0, gradeCount)
            .Select(_ => random.Next(min, max + 1))
            .ToList();
    }
}