using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var students = new List<Student>
        {
            new Student("Дорохов", "Игнат", "Олегович", 2007, "Группа 1"),
            new Student("Горохов", "Вадим", "Скелетович", 2015, "Группа 2")
        };

        var teachers = new List<Teacher>
        {
            new Teacher("Юшина", "Дарья", "Денисовна", 2005, "Программирование")
        };

        var journal = new Journal();

        while (true)
        {
            Console.WriteLine("Выберите аккаунт для входа: Студент, Преподаватель, Администратор");
            var role = Console.ReadLine()?.ToLower();

            switch (role)
            {
                case "студент":
                    if (AuthenticateUser("студент", "student"))
                    {
                        Console.WriteLine("Добро пожаловать, студент! Ваш журнал:");
                        journal.ShowEntries();
                    }
                    break;

                case "преподаватель":
                    if (AuthenticateUser("препод", "prepod"))
                    {
                        Console.WriteLine("Добро пожаловать, преподаватель!");
                        TeacherActions(journal);
                    }
                    break;

                case "администратор":
                    if (AuthenticateUser("админ", "admin"))
                    {
                        Console.WriteLine("Добро пожаловать, администратор!");
                        AdminActions(students, teachers, journal);
                    }
                    break;

                default:
                    Console.WriteLine("Неверная роль. Попробуйте снова.");
                    break;
            }
        }
    }

    static bool AuthenticateUser(string login, string password)
    {
        Console.Write("Введите логин: ");
        string inputLogin = Console.ReadLine();
        Console.Write("Введите пароль: ");
        string inputPassword = Console.ReadLine();

        if (inputLogin == login && inputPassword == password)
        {
            return true;
        }

        Console.WriteLine("Неверные логин или пароль.");
        return false;
    }

    static void TeacherActions(Journal journal)
    {
        while (true)
        {
            Console.WriteLine("1. Просмотреть журнал\n2. Добавить оценку\n3. Изменить оценку\n4. Удалить оценку\n5. Выход");
            switch (Console.ReadLine())
            {
                case "1":
                    journal.ShowEntries();
                    break;
                case "2":
                    journal.AddEntry();
                    break;
                case "3":
                    journal.UpdateEntry();
                    break;
                case "4":
                    journal.RemoveEntry();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    static void AdminActions(List<Student> students, List<Teacher> teachers, Journal journal)
    {
        while (true)
        {
            Console.WriteLine("1. Добавить студента\n2. Удалить студента\n3. Добавить преподавателя\n4. Удалить преподавателя\n5. Просмотреть журнал\n6. Выход");
            switch (Console.ReadLine())
            {
                case "1":
                    students.Add(Student.Create());
                    break;
                case "2":
                    Console.Write("Введите фамилию студента для удаления: ");
                    var studentToRemove = Console.ReadLine();
                    students.RemoveAll(s => s.LastName == studentToRemove);
                    Console.WriteLine("Студент удалён.");
                    break;
                case "3":
                    teachers.Add(Teacher.Create());
                    break;
                case "4":
                    Console.Write("Введите фамилию преподавателя для удаления: ");
                    var teacherToRemove = Console.ReadLine();
                    teachers.RemoveAll(t => t.LastName == teacherToRemove);
                    Console.WriteLine("Преподаватель удалён.");
                    break;
                case "5":
                    journal.ShowEntries();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }
}

class Student
{
    public string LastName { get; }
    public string FirstName { get; }
    public string MiddleName { get; }
    public int BirthYear { get; }
    public string Group { get; }

    public Student(string lastName, string firstName, string middleName, int birthYear, string group)
    {
        LastName = lastName;
        FirstName = firstName;
        MiddleName = middleName;
        BirthYear = birthYear;
        Group = group;
    }

    public static Student Create()
    {
        Console.Write("Фамилия: ");
        var lastName = Console.ReadLine();
        Console.Write("Имя: ");
        var firstName = Console.ReadLine();
        Console.Write("Отчество: ");
        var middleName = Console.ReadLine();
        Console.Write("Год рождения: ");
        var birthYear = int.Parse(Console.ReadLine());
        Console.Write("Группа: ");
        var group = Console.ReadLine();

        return new Student(lastName, firstName, middleName, birthYear, group);
    }
}

class Teacher
{
    public string LastName { get; }
    public string FirstName { get; }
    public string MiddleName { get; }
    public int BirthYear { get; }
    public string Discipline { get; }

    public Teacher(string lastName, string firstName, string middleName, int birthYear, string discipline)
    {
        LastName = lastName;
        FirstName = firstName;
        MiddleName = middleName;
        BirthYear = birthYear;
        Discipline = discipline;
    }

    public static Teacher Create()
    {
        Console.Write("Фамилия: ");
        var lastName = Console.ReadLine();
        Console.Write("Имя: ");
        var firstName = Console.ReadLine();
        Console.Write("Отчество: ");
        var middleName = Console.ReadLine();
        Console.Write("Год рождения: ");
        var birthYear = int.Parse(Console.ReadLine());
        Console.Write("Дисциплина: ");
        var discipline = Console.ReadLine();

        return new Teacher(lastName, firstName, middleName, birthYear, discipline);
    }
}

class Journal
{
    private readonly Dictionary<string, int> _grades = new();

    public void ShowEntries()
    {
        if (_grades.Count == 0)
        {
            Console.WriteLine("Журнал пуст.");
            return;
        }

        foreach (var (subject, grade) in _grades)
        {
            Console.WriteLine($"{subject}: {grade}");
        }
    }

    public void AddEntry()
    {
        Console.Write("Введите предмет: ");
        var subject = Console.ReadLine();
        Console.Write("Введите оценку (1-5): ");
        var grade = int.Parse(Console.ReadLine());
        _grades[subject] = grade;
        Console.WriteLine("Оценка добавлена.");
    }

    public void UpdateEntry()
    {
        Console.Write("Введите предмет для изменения оценки: ");
        var subject = Console.ReadLine();

        if (_grades.ContainsKey(subject))
        {
            Console.Write("Введите новую оценку (1-5): ");
            var grade = int.Parse(Console.ReadLine());
            _grades[subject] = grade;
            Console.WriteLine("Оценка изменена.");
        }
        else
        {
            Console.WriteLine("Такого предмета нет в журнале.");
        }
    }

    public void RemoveEntry()
    {
        Console.Write("Введите предмет для удаления: ");
        var subject = Console.ReadLine();

        if (_grades.Remove(subject))
        {
            Console.WriteLine("Оценка удалена.");
        }
        else
        {
            Console.WriteLine("Такого предмета нет в журнале.");
        }
    }
}
