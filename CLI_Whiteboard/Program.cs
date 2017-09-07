using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
    public class Student:Students
    {
       
        public int incTrackNumber = 0;
        public int TrackStudents { get; set; }

        public Student() { }
        public Student(int sID, string fName, string lName, int tNum)
        {
            this.StudentID = sID;
            this.FirstName = fName;
            this.LastName = lName;
            this.TrackStudents = tNum;
        }
        public void CountTrack(int aStudent)
        {
            TrackStudents = incTrackNumber + 100 * aStudent;
            return;
        }
        public Student addStudent()
        {
            Console.Clear();
            Student student1 = new Student();
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

            student1.StudentID = (replyID);
            Console.Write("\nPlease enter a student first name:");

            string replyFirstName = Console.ReadLine();

            student1.FirstName = (replyFirstName);
            Console.Write("\nPlease enter a student last name:");

            string replyLastName = Console.ReadLine();

            student1.LastName = (replyLastName);

            Console.Write("\nStudent ID: {0} NAME: {1} {2}",student1.StudentID,student1.FirstName,student1.LastName);
            return student1;
        }

    }
    public class MenuActions
    {
        public void diplayMenu(string filePath,Student student1)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.WriteLine("Please select a number below:");
            //insert menu items here
            Console.WriteLine("1. Add Student");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("2. Update File");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("3. Erase old and Save new File");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("4. Read File");

            //insert menu items above
            Console.Write("\n\nSELECTION:");
            Console.ForegroundColor = ConsoleColor.White;
            string menuSelection = Console.ReadLine();
            switch (menuSelection)
            {
                case "1":
                    {
                        student1 = student1.addStudent();
                        break;
                    }
                case "2":
                    {
                        SaveFile(filePath,student1,1);
                        break;
                    }
                case "3":
                    {
                        SaveFile(filePath, student1, 2);
                        break;
                    }
                case "4":
                    {
                        ReadFile(filePath, student1);
                        break;
                    }
            }
            diplayMenu(filePath, student1);
        }
        public void SaveFile(string filePath, Student student1, int options)
        {
            MenuActions newMenu = new MenuActions();

            //Build string to save the data
            string saveThisData = "Student ID: " + student1.StudentID + " Name: " + student1.FirstName + " " + student1.LastName + "\n";
            Console.Write("\nAttempting to save data to file\n");
            //
            //Give some options to overwrite the existing file
            //
            switch (options)
            {
                case 1:
                    File.AppendAllText(filePath, saveThisData);

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
                    File.WriteAllText(filePath, "");
                    File.WriteAllText(filePath, saveThisData);
                    if (File.Exists(filePath))
                    {
                        Console.WriteLine("\nData saved\n\n");
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
            newMenu.diplayMenu(filePath, student1);


        }
        public void ReadFile(string filePath, Student student1)
        {
            MenuActions newMenu = new MenuActions();
            
            Console.WriteLine("Attempting to read the file\n");

            string readThisData = File.ReadAllText(filePath);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(readThisData);
            Console.WriteLine("\n\nPress any key to return to the menu...");
            Console.ReadKey();
            newMenu.diplayMenu(filePath, student1);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string appPath = Environment.CurrentDirectory.ToString();
            string filePath = appPath + "\\names.txt";
            Student student1 = new Student();
 
            //Call the Menu
            MenuActions newMenu = new MenuActions();
            newMenu.diplayMenu(filePath, student1);

        }
    }
}
