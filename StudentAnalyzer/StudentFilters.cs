namespace StudentAnalyzer;

public class StudentFilters
{
    public IEnumerable<Student> GetStudentsOlderThan(IEnumerable<Student> students, int age) => students.Where(student => student.Age > age);

    public IEnumerable<Student> GetTopStudents(IEnumerable<Student> students, double minAverage) =>
        students.Where(s => s.Grades.Count != 0 && s.Grades.Average() > minAverage);

    public IEnumerable<Student> GetStudentsFromGroup(IEnumerable<Student> students, string groupName) =>
        students.Where(student => string.Equals(student.Group, groupName, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Student> GetStudentsWithFailingGrades(IEnumerable<Student> students) =>
        students.Where(student => student.Grades.Any(grade => grade < 60));

    public IEnumerable<string> GetFullNames(IEnumerable<Student> students) =>
        students.Select(s => $"{s.FirstName} {s.LastName}");

    public IEnumerable<object> GetStudentSummaries(IEnumerable<Student> students)
    {
        return students.Select(s => new
        {
            s.Id,
            FullName = $"{s.FirstName} {s.LastName}",
            AverageGrade = s.Grades.Any() ? s.Grades.Average() : 0.0,
            GradeCount = s.Grades.Count,
        });
    }

    public IEnumerable<int> GetAges(IEnumerable<Student> students) => students.Select(s => s.Age);

    public IEnumerable<Student> GetStudentsByAge(IEnumerable<Student> students) => students.OrderBy(s => s.Age);

    public IEnumerable<Student> GetStudentsByAverage(IEnumerable<Student> students) =>
        students.OrderByDescending(s => s.Grades.Any() ? s.Grades.Average() : 0.0);

    public IEnumerable<Student> GetStudentsByGroupThenName(IEnumerable<Student> students) =>
        students.OrderBy(s => s.Group).ThenBy(s => s.LastName);

    public IEnumerable<IGrouping<string, Student>> GroupStudentsByGroup(IEnumerable<Student> students) =>
        students.GroupBy(s => s.Group);

    public IEnumerable<(string Group, double Average)> GetAverageGradeByGroup(IEnumerable<Student> students) =>
        students.GroupBy(s => s.Group)
            .Select(g => (Group: g.Key, Average: g.Average(s => s.Grades.Any() ? s.Grades.Average() : 0.0)));

    public IEnumerable<int> GetAllGrades(IEnumerable<Student> students) => students.SelectMany(s => s.Grades);

    public IEnumerable<(int StudentId, int Grade)> GetStudentGradePairs(IEnumerable<Student> students) =>
        students.SelectMany(s => s.Grades.Select(g => (StudentId: s.Id, Grade: g)));

    public IEnumerable<Student> GetStudentsPage(IEnumerable<Student> students, int pageNumber, int pageSize) =>
        students.Skip((pageNumber - 1) * pageSize).Take(pageSize);

    public Student FindStudentById(IEnumerable<Student> students, int id) =>
        students.Single(s => s.Id == id);

    public Student? TryFindStudent(IEnumerable<Student> students, int id) =>
        students.FirstOrDefault(s => s.Id == id);
}