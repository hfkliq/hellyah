using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

class Program
{
    static void Main()
    {
        var loginManager = new LoginManager();
        var dataManager = new DataManager();
        var journal = new Journal();

        while (true)
        {
            Console.WriteLine("Выберите способ входа (используйте стрелки для навигации):\n1. Студент\n2. Преподаватель\n3. Администратор\n4. Выход");
            int selectedOption = MenuNavigation(new[] { "Студент", "Преподаватель", "Администратор", "Выход" });

            switch (selectedOption)
            {
                case 0:
                    if (loginManager.Authenticate(loginManager.StudentLogins))
                    {
                        Console.WriteLine("Добро пожаловать, студент!");
                        journal.DisplayJournal();
                    }
                    break;
                case 1:
                    if (loginManager.Authenticate(loginManager.TeacherLogins))
                    {
                        Console.WriteLine("Добро пожаловать, преподаватель!");
                        TeacherActions(journal);
                    }
                    break;
                case 2:
                    if (loginManager.Authenticate(loginManager.AdminLogins))
                    {
                        Console.WriteLine("Добро пожаловать, администратор!");
                        AdminActions(dataManager, journal);
                    }
                    break;
                case 3:
                    return;
            }
        }
    }

    static void TeacherActions(Journal journal)
    {
        while (true)
        {
            Console.WriteLine("\nМеню преподавателя (используйте стрелки для навигации):\n1. Просмотреть журнал\n2. Добавить оценку\n3. Изменить оценку\n4. Удалить оценку\n5. Выход");
            int selectedOption = MenuNavigation(new[] { "Просмотреть журнал", "Добавить оценку", "Изменить оценку", "Удалить оценку", "Выход" });

            switch (selectedOption)
            {
                case 0:
                    journal.DisplayJournal();
                    break;
                case 1:
                    journal.AddGrade();
                    break;
                case 2:
                    journal.EditGrade();
                    break;
                case 3:
                    journal.DeleteGrade();
                    break;
                case 4:
                    return;
            }
        }
    }

    static void AdminActions(DataManager dataManager, Journal journal)
    {
        while (true)
        {
            Console.WriteLine("\nМеню администратора (используйте стрелки для навигации):\n1. Добавить студента\n2. Удалить студента\n3. Добавить преподавателя\n4. Удалить преподавателя\n5. Просмотреть журнал\n6. Выход");
            int selectedOption = MenuNavigation(new[] { "Добавить студента", "Удалить студента", "Добавить преподавателя", "Удалить преподавателя", "Просмотреть журнал", "Выход" });

            switch (selectedOption)
            {
                case 0:
                    dataManager.AddStudent();
                    break;
                case 1:
                    dataManager.RemoveStudent();
                    break;
                case 2:
                    dataManager.AddTeacher();
                    break;
                case 3:
                    dataManager.RemoveTeacher();
                    break;
                case 4:
                    journal.DisplayJournal();
                    break;
                case 5:
                    return;
            }
        }
    }

    static int MenuNavigation(string[] options)
    {
        int selected = 0;
        ConsoleKey key;

        do
        {
            Console.Clear();
            for (int i = 0; i < options.Length; i++)
            {
                if (i == selected)
                {
                    Console.WriteLine($"> {options[i]} <");
                }
                else
                {
                    Console.WriteLine(options[i]);
                }
            }

            key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow)
            {
                selected = (selected == 0) ? options.Length - 1 : selected - 1;
            }
            else if (key == ConsoleKey.DownArrow)
            {
                selected = (selected == options.Length - 1) ? 0 : selected + 1;
            }
        } while (key != ConsoleKey.Enter);

        return selected;
    }
}

class LoginManager
{
    public Dictionary<string, string> StudentLogins { get; } = new() { { "stu1", "pas1" }, { "stu2", "pas2" } };
    public Dictionary<string, string> TeacherLogins { get; } = new() { { "prep", "pasP" } };
    public Dictionary<string, string> AdminLogins { get; } = new() { { "admin", "pass" } };

    public bool Authenticate(Dictionary<string, string> logins)
    {
        Console.Write("Введите логин: ");
        string login = Console.ReadLine();
        Console.Write("Введите пароль: ");
        string password = Console.ReadLine();

        if (logins.TryGetValue(login, out var correctPassword) && correctPassword == password)
        {
            return true;
        }

        Console.WriteLine("Неверные логин или пароль.");
        return false;
    }
}

class DataManager
{
    private readonly List<Student> students = new()
    {
        new Student("Дорохов", "Игнатий", "Олегович", 2000, "Группа 1"),
        new Student("Федотов", "Матвей", "Александрович", 2001, "Группа 2")
    };

    private readonly List<Teacher> teachers = new()
    {
        new Teacher("Юшина", "Дарья", "Денисовна", 1985, "Информатика")
    };

    public void AddStudent()
    {
        students.Add(Student.Create());
        Console.WriteLine("Студент добавлен.");
    }

    public void RemoveStudent()
    {
        Console.Write("Введите фамилию студента для удаления: ");
        string lastName = Console.ReadLine();
        students.RemoveAll(s => s.LastName == lastName);
        Console.WriteLine("Студент удалён.");
    }

    public void AddTeacher()
    {
        teachers.Add(Teacher.Create());
        Console.WriteLine("Преподаватель добавлен.");
    }

    public void RemoveTeacher()
    {
        Console.Write("Введите фамилию преподавателя для удаления: ");
        string lastName = Console.ReadLine();
        teachers.RemoveAll(t => t.LastName == lastName);
        Console.WriteLine("Преподаватель удалён.");
    }
}

class Student
{
    public string LastName { get; }
    public string FirstName { get; }
    public string MiddleName { get; }
    public int YearOfBirth { get; }
    public string Group { get; }

    public Student(string lastName, string firstName, string middleName, int yearOfBirth, string group)
    {
        LastName = lastName;
        FirstName = firstName;
        MiddleName = middleName;
        YearOfBirth = yearOfBirth;
        Group = group;
    }

    public static Student Create()
    {
        Console.Write("Фамилия: ");
        string lastName = Console.ReadLine();
        Console.Write("Имя: ");
        string firstName = Console.ReadLine();
        Console.Write("Отчество: ");
        string middleName = Console.ReadLine();
        Console.Write("Год рождения: ");
        int year = int.Parse(Console.ReadLine());
        Console.Write("Группа: ");
        string group = Console.ReadLine();

        return new Student(lastName, firstName, middleName, year, group);
    }
}

class Teacher
{
    public string LastName { get; }
    public string FirstName { get; }
    public string MiddleName { get; }
    public int YearOfBirth { get; }
    public string Discipline { get; }

    public Teacher(string lastName, string firstName, string middleName, int yearOfBirth, string discipline)
    {
        LastName = lastName;
        FirstName = firstName;
        MiddleName = middleName;
        YearOfBirth = yearOfBirth;
        Discipline = discipline;
    }

    public static Teacher Create()
    {
        Console.Write("Фамилия: ");
        string lastName = Console.ReadLine();
        Console.Write("Имя: ");
        string firstName = Console.ReadLine();
        Console.Write("Отчество: ");
        string middleName = Console.ReadLine();
        Console.Write("Год рождения: ");
        int year = int.Parse(Console.ReadLine());
        Console.Write("Дисциплина: ");
        string discipline = Console.ReadLine();

        return new Teacher(lastName, firstName, middleName, year, discipline);
    }
}
class Journal
{
    private List<Grade> grades = new List<Grade>();

    public void DisplayJournal()
    {
        Console.WriteLine("\nЖурнал оценок:");
        if (grades.Count == 0)
        {
            Console.WriteLine("Журнал пуст.");
        }
        else
        {
            foreach (var grade in grades)
            {
                Console.WriteLine($"{grade.StudentLastName} - {grade.Subject}: {grade.Mark}");
            }
        }
    }

    public void AddGrade()
    {
        Console.Write("Введите фамилию студента: ");
        string lastName = Console.ReadLine();
        Console.Write("Введите предмет: ");
        string subject = Console.ReadLine();
        Console.Write("Введите оценку: ");
        string mark = Console.ReadLine();

        grades.Add(new Grade(lastName, subject, mark));
        Console.WriteLine("Оценка добавлена.");
    }

    public void EditGrade()
    {
        Console.Write("Введите фамилию студента для изменения оценки: ");
        string lastName = Console.ReadLine();
        var grade = grades.FirstOrDefault(g => g.StudentLastName == lastName);

        if (grade != null)
        {
            Console.Write("Введите новую оценку: ");
            grade.Mark = Console.ReadLine();
            Console.WriteLine("Оценка изменена.");
        }
        else
        {
            Console.WriteLine("Оценка не найдена.");
        }
    }

    public void DeleteGrade()
    {
        Console.Write("Введите фамилию студента для удаления оценки: ");
        string lastName = Console.ReadLine();
        int removed = grades.RemoveAll(g => g.StudentLastName == lastName);
        Console.WriteLine(removed > 0 ? "Оценка удалена." : "Оценка не найдена.");
    }
}

class Grade
{
    public string StudentLastName { get; set; }
    public string Subject { get; set; }
    public string Mark { get; set; }

    public Grade(string studentLastName, string subject, string mark)
    {
        StudentLastName = studentLastName;
        Subject = subject;
        Mark = mark;
    }
}
