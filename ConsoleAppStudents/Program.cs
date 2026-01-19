using StudentAnalyzer;

namespace ConsoleAppStudents;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("===== Часть 1: LINQ операторы =====\n");

        var repository = new StudentRepository();
        var filters = new StudentFilters();
        var presenter = new StudentConsolePresenter();

        var students = repository.GetTestStudents().ToList();
        presenter.PresentStudents(students, "Все студенты");

        DemonstrateFiltering(students, filters, presenter);
        DemonstrateTransformation(students, filters, presenter);
        DemonstrateSorting(students, filters, presenter);
        DemonstrateGrouping(students, filters, presenter);
        DemonstrateFlattening(students, filters, presenter);
        DemonstratePaginationAndSearch(students, filters, presenter);

        Console.WriteLine("\n\n===== Завершено =====");
    }

    private static void DemonstrateFiltering(
        IEnumerable<Student> students,
        StudentFilters filters,
        StudentConsolePresenter presenter)
    {
        Console.WriteLine("\n\n*** ЗАДАНИЕ 2: Фильтрация ***");

        var olderThan20 = filters.GetStudentsOlderThan(students, 20);
        presenter.PresentStudents(olderThan20, "Студенты старше 20 лет");

        var topStudents = filters.GetTopStudents(students, 80);
        presenter.PresentStudents(topStudents, "Отличники (средний > 80)");

        var csStudents = filters.GetStudentsFromGroup(students, "CS-101");
        presenter.PresentStudents(csStudents, "Студенты группы CS-101");

        var failingStudents = filters.GetStudentsWithFailingGrades(students);
        presenter.PresentStudents(failingStudents, "Студенты с оценками < 60");
    }

    private static void DemonstrateTransformation(
        IEnumerable<Student> students,
        StudentFilters filters,
        StudentConsolePresenter presenter)
    {
        Console.WriteLine("\n\n*** ЗАДАНИЕ 3: Преобразование ***");

        var fullNames = filters.GetFullNames(students);
        presenter.PresentFullNames(fullNames, "Полные имена");

        var summaries = filters.GetStudentSummaries(students);
        presenter.PresentSummaries(summaries, "Резюме студентов");

        var ages = filters.GetAges(students);
        presenter.PresentAges(ages, "Все возрасты");
    }

    private static void DemonstrateSorting(
        IEnumerable<Student> students,
        StudentFilters filters,
        StudentConsolePresenter presenter)
    {
        Console.WriteLine("\n\n*** ЗАДАНИЕ 4: Сортировка ***");

        var byAge = filters.GetStudentsByAge(students);
        presenter.PresentStudents(byAge, "Сортировка по возрасту");

        var byAverage = filters.GetStudentsByAverage(students);
        presenter.PresentStudents(byAverage, "Сортировка по среднему баллу");

        var byGroupAndName = filters.GetStudentsByGroupThenName(students);
        presenter.PresentStudents(byGroupAndName, "Сортировка по группе, затем по фамилии");
    }

    private static void DemonstrateGrouping(
        IEnumerable<Student> students,
        StudentFilters filters,
        StudentConsolePresenter presenter)
    {
        Console.WriteLine("\n\n*** ЗАДАНИЕ 5: Группировка ***");

        var groupedStudents = filters.GroupStudentsByGroup(students);
        presenter.PresentGroups(groupedStudents);

        var averageByGroup = filters.GetAverageGradeByGroup(students);
        presenter.PresentAverageByGroup(averageByGroup);
    }

    private static void DemonstrateFlattening(
        IEnumerable<Student> students,
        StudentFilters filters,
        StudentConsolePresenter presenter)
    {
        Console.WriteLine("\n\n*** ЗАДАНИЕ 6: Развертывание ***");

        var allGrades = filters.GetAllGrades(students);
        presenter.PresentGrades(allGrades, "Все оценки всех студентов");

        var gradePairs = filters.GetStudentGradePairs(students);
        presenter.PresentGradePairs(gradePairs, "Пары (ID студента, оценка)");
    }

    private static void DemonstratePaginationAndSearch(
        IEnumerable<Student> students,
        StudentFilters filters,
        StudentConsolePresenter presenter)
    {
        Console.WriteLine("\n\n*** ЗАДАНИЕ 7: Пагинация и поиск ***");

        var studentsList = students.ToList();
        var page1 = filters.GetStudentsPage(studentsList, 1, 5);
        presenter.PresentStudents(page1, "Страница 1 (5 студентов)");

        var page2 = filters.GetStudentsPage(studentsList, 2, 5);
        presenter.PresentStudents(page2, "Страница 2 (5 студентов)");

        var foundStudent = filters.FindStudentById(studentsList, 5);
        presenter.PresentStudent(foundStudent, "Найден студент с ID = 5");

        var maybeStudent = filters.TryFindStudent(studentsList, 999);
        if (maybeStudent != null)
        {
            presenter.PresentStudent(maybeStudent, "Найден студент с ID = 999");
        }
        else
        {
            Console.WriteLine("\n=== Безопасный поиск ===");
            Console.WriteLine("Студент с ID = 999 не найден (вернулся null)");
        }
    }
}