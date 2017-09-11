using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace CLI_Whiteboard
{

    public abstract class Students
        {
        public Students() { }
        // Create default attribute for StudentID, Student first and Last name.
       public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
     
    }
    [Serializable, XmlRoot("students")]
    public class Student:Students
    {
       
        
        public int TrackStudents { get; set; }
        
        public Student() { }
        public Student(int sID, string fName, string lName, int tNum)
        {
            this.StudentID = sID;
            this.FirstName = fName;
            this.LastName = lName;
            this.TrackStudents = tNum;
        }
        public List<Student> addStudent(List<Student> students)
        {
            Console.Clear();
            var testThis = "A";
            Regex r = new Regex(@"^[0-9]+?", RegexOptions.IgnoreCase);
            Match IsNum = r.Match(testThis);
            while (!IsNum.Success)
            {
                Console.Write("\nPlease enter a studentID as NUMBER:");
                testThis = Console.ReadLine();
                IsNum = r.Match(testThis);
            }
                var replyID = Convert.ToInt32(testThis);
            Console.Write("\nPlease enter a student first name:");
                string replyFirstName = Console.ReadLine();
            Console.Write("\nPlease enter a student last name:");
                string replyLastName = Console.ReadLine();
            Console.Write(students.Count);
            students.Add(new Student()
            {
                StudentID = replyID,
                FirstName = replyFirstName,
                LastName = replyLastName,
                TrackStudents = 0
            });
            Console.Write("\nStudent ID: {0}| NAME: {1} {2}| Called: {3}",StudentID,FirstName,LastName,TrackStudents);
            return students;
        }

    }
    public class MenuActions
    {
        public List<Student> loadDB(string filePath)
        {
            if(File.Exists(filePath))
            {
            XDocument doc = XDocument.Load(filePath);
            List<Student> students = doc.Element("ArrayOfStudent")
                .Elements("Student")
                .Select(s => new Student()
                {
                    FirstName = (string)s.Element("FirstName"),
                    LastName = (string)s.Element("LastName"),
                    StudentID = (int)s.Element("StudentID"),
                    TrackStudents = (int)s.Element("TrackStudents"),
                }).ToList();
            return students;
            }
            else
            {
                List<Student> students = new List<Student>();
                TextWriter writer = new StreamWriter(filePath);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));
                serializer.Serialize(writer, students);
                writer.Close();
                return students;
            }

        }
        public void diplayMenu(string filePath,List<Student> students)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.WriteLine("Save file location:{0}", filePath);
            Console.WriteLine("Please select a number below:");
            //insert menu items here
            Console.WriteLine("1. Add Student");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("2. Update File");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("3. Erase File");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("4. Read File");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("5. Random Name");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Q. Quit");
            Console.ForegroundColor = ConsoleColor.Cyan;

            //insert menu items above
            Console.Write("\n\nSELECTION:");
            Console.ForegroundColor = ConsoleColor.White;
            string menuSelection = Console.ReadLine().ToUpper();
            switch (menuSelection)
            {
                case "1":
                    {
                        Student newStudent = new Student();
                        newStudent.addStudent(students);
                        break;
                    }
                case "2":
                    {
                        SaveFile(filePath,students,1);
                        break;
                    }
                case "3":
                    {
                        SaveFile(filePath, students, 2);
                        break;
                    }
                case "4":
                    {
                        ReadFile(filePath, students);
                        break;
                    }
                case "5":
                    {
                        GenName(students, filePath);
                        break;
                    }
                case "Q":
                    {
                        Environment.Exit(0);
                        break;
                    }
            }
            diplayMenu(filePath, students);
        }
        public void SaveFile(string filePath, List<Student> students, int options)
        {
            MenuActions newMenu = new MenuActions();

            //Build string to save the data
            Console.Write("\nAttempting to save data to file\n");
            //
            //Give some options to overwrite the existing file
            //
            switch (options)
            {
                case 1:
                    TextWriter writer = new StreamWriter(filePath);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));
                    serializer.Serialize(writer, students);
                    writer.Close();
                    
                    if (File.Exists(filePath))
                    {
                        Console.WriteLine("\nData updated\n\n");
                        Console.WriteLine("\n\nPress any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Problem saving");
                        Console.WriteLine("\n\nPress any key to continue...");
                        Console.ReadKey();
                    }
                    break;
                case 2:
                    File.AppendAllText(filePath,"");

                    if (File.Exists(filePath))
                    {
                        Console.WriteLine("\nData updated\n\n");
                        Console.WriteLine("\n\nPress any key to continue...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Problem saving");
                        Console.WriteLine("\n\nPress any key to continue...");
                        Console.ReadKey();
                    }
                    break;
            }
            newMenu.diplayMenu(filePath, students);


        }
        public void ReadFile(string filePath, List<Student> students)
        {
            MenuActions newMenu = new MenuActions();

            string readThisData;

            Console.WriteLine("Attempting to read the file\n");
            if(File.Exists(filePath))
            {

                foreach (Student s in students)
                {
                    Console.WriteLine("Student ID: {0}| Name: {1} {2}| Times Called: {3}",s.StudentID,s.FirstName,s.LastName,s.TrackStudents);
                }
                readThisData = "";
            }
            else
            {
                readThisData = "File not found";
            }
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(readThisData);
            Console.WriteLine("\n\nPress any key to return to the menu...");
            Console.ReadKey();
            newMenu.diplayMenu(filePath, students);
        }
        public void GenName(List<Student> students, string filePath)
        {
            MenuActions newMenu = new MenuActions();
            Random rand = new Random();
            int thisStudent = rand.Next(students.Count);
            Console.WriteLine("Up next: {0} {1}",students[thisStudent].FirstName, students[thisStudent].LastName);
            Console.ReadKey();
            newMenu.diplayMenu(filePath, students);
        }
        public string GenCliName(List<Student> students, string filePath)
        {
            MenuActions newMenu = new MenuActions();
            Random rand = new Random();
            int thisStudent = rand.Next(students.Count);
            return students[thisStudent].FirstName + " " + students[thisStudent].LastName;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            string appPath = Environment.CurrentDirectory.ToString();
            string filePath = appPath + "\\XMLFILE1.xml";
            //Call the Menu
            MenuActions newMenu = new MenuActions();
            List<Student> students = newMenu.loadDB(filePath);
            if (args.Length == 0)
            {
                newMenu.diplayMenu(filePath, students);
            }
            else
            {
                switch (args[0])
                {
                    case "n":
                        string thisName;
                        thisName = newMenu.GenCliName(students, filePath);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(thisName);
                        Console.ForegroundColor = ConsoleColor.White;
                        ConsoleKeyInfo aKey = Console.ReadKey();
                        if (aKey.Key == ConsoleKey.Spacebar)
                        {
                            string[] thisArg = { "n" };
                            Main(thisArg);
                        }
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
