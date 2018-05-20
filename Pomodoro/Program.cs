using System;
using static System.Console;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro
{
    class Program
    {
        // Paths to saved data
        // TODO: add settings
        // TODO: change the path to the app actual path
        public static string dataPath = "C:/Users/Qiqete/Desktop/subjectsData.txt";
        public static string userPrefPath = "C:/Users/Qiqete/Desktop/subjects.txt";
        

        static List<Subject> subjects;                              //Subjects will load here

        static int pomodoroTime = 2 * 60;                           // Default study time
        static int shortBreakTime = 5 * 60;                         // Default short break time
        static int longBreakTime = 5 * 60;                          // Default long break time                    
        static string finishMotivation = "Nicely done!";

        static Pomodoro actualPm;                                   // Pomodoro timer that you are using
        static Pomodoro[] lastPomodoros = new Pomodoro[2];          // Array to take in account if the break time 
                                                                    // counts as study time or it will be discarded


        // MAIN PROGRAM START
        static void Main(string[] args) {

            subjects = LoadSubjects();
            SelectApp();

            Console.WriteLine("Closing app");
            ReadKey();
        }

        // Main menu
        static void SelectApp() {
            Clear();
            WriteLine("POMODORO app by qiqete");
            WriteLine("Select an option:" +
                                "\n \t 1.- Start studying" +
                                "\n \t 2.- See time studied" +
                                "\n \t 3.- Options" +
                                "\n \t 4.- Exit");


            if (Console.ReadLine() == "1") {
                SelectSubjectStudy();
            } else { SelectApp(); }
        }


        // Read the files and store the data in the subject class list
        static List<Subject> LoadSubjects() {

            
            int lines = 5;          // Amount of lines that takes to store every Subject in the dataText
            Subject[] subjects;     // Store the total amount of subjects read in the text

            if (File.Exists(dataPath)){
                WriteLine("Loading your data...");

                string[] textData = File.ReadAllText(@dataPath).Replace("\r", "").Split('\n');
                // TODO: change this per a settings file
                string[] subjectNames = File.ReadAllText(userPrefPath).Replace("\r", "").Split('\n'); 

                int totalSubjects = textData.Length / lines;

                subjects = new Subject[totalSubjects];
                for (int i = 0; i < totalSubjects; i++) {
                    if (DateTime.Now.DayOfYear == Int32.Parse(textData[i*lines + 3])) {
                        subjects[i] = new Subject(textData[i * lines + 1], i, Int32.Parse(textData[i * lines + 4]), Int32.Parse(textData[i * lines + 3]));
                    } else {
                        subjects[i] = new Subject(textData[i*lines + 1], i, 0, Int32.Parse(textData[i * lines + 3]));
                    }
                }
            } else {
                File.Create(dataPath);
                // TODO: create the settings file

                subjects = new Subject[0];
            }

            return subjects.ToList();
        }

        // Select the subject menu
        // TODO: Make it return a subject instead of a silly menu
        static void SelectSubjectStudy() {
            Clear();
            WriteLine("What subject do you want to study?");

            int i = 0;
            for (i=0; i < subjects.Count; i++) {
                WriteLine("\t" + (i + 1) + ".- " + subjects[i].Name);
                if(i == subjects.Count - 1) {
                    WriteLine("\t" + (i + 2) + ".- None");
                    WriteLine("\t" + (i + 3) + ".- Add a subject to study");
                    WriteLine("\t" + (i + 4) + ".- Go back");
                }
            }

            int key = Int32.TryParse(Console.ReadLine().ToString(), out key) ? key : 0; // TODO: IMPROVE
            Console.WriteLine(key);
            if (key == 0 || key > i + 4) { SelectSubjectStudy(); } else if (key <= i) {
                startStudy(subjects[key - 1]);
            } else if (key == i + 1) {
                //startStudy();
            } else if (key == i + 2) {
                if (Yon("you want to add a new subject (Y/n)")){
                    addSubject();

                    //startStudy(subjects[subjects.Count - 1]);
                } else {
                    SelectSubjectStudy();
                }
            } else if (key == i + 3) {
                SelectApp();
            }
        }

        // Menu for adding new subject
        static void addSubject() {
            WriteLine("Subject name please");
            string name = ReadLine();
            if(Yon(name + " is the name you want? (Y/n)")) {
                Subject sub = new Subject(name);
                subjects.Add(sub);
                using (StreamWriter sw = File.AppendText(dataPath)) {
                    sw.WriteLine();
                    sw.WriteLine("-");
                    sw.WriteLine(name);
                    sw.WriteLine("0");
                    sw.WriteLine(DateTime.Now.DayOfYear);
                    sw.WriteLine("0");
                }
            }
        }


        static void startStudy(Subject sub) {
            Clear();
            Write("You started studying " + sub.Name);
            Pomodoro pom = new Pomodoro(pomodoroTime, true, sub);
        }

        static bool Yon(string form) {
            WriteLine("\n Are you sure "+ form);
            ConsoleKeyInfo ck = ReadKey();
            if (ck.Key == ConsoleKey.Enter || ck.Key == ConsoleKey.Y) {
                return true;
            } else if(ck.Key == ConsoleKey.N) {
                return false;
            } else {
                return Yon(form);
            }
        }


        static void RemoveSubject(Subject sub) {
            for (int i = 0; i < subjects.Count; i++) {
                if(subjects[i] == sub) {
                    subjects.RemoveAt(i);
                    string[] lines = File.ReadAllLines(@dataPath);
                    for (int x = 0; x < lines.Length; x++) {
                        WriteLine("textDataLine: " + lines[x]);
                    }
                    string[] newlines = new string[lines.Length - 5];
                    for (int x = 0; x < newlines.Length; x++) {
                        WriteLine("textDataNewLine: " + newlines[x]);
                    }

                    int count = 0;
                    for (int j = 0; j < lines.Length; j++) {
                        if(j < i*5 || j>= i*5+5) {  //May be something wrong
                            WriteLine(lines[j]);
                            newlines[count] = lines[j];
                            count++;
                        }
                    }
                    for (int x = 0; x < newlines.Length; x++) {
                        WriteLine("FinalNewLines: " + newlines[x]);
                    }
                    File.WriteAllLines(@dataPath, newlines);
                    return;
                }
            }
        }

        public static void choosePlan(Subject sub) {
            Clear();
            WriteLine("Select an option:" +
                                "\n \t 1.- Short Break" +
                                "\n \t 2.- Long Break" +
                                "\n \t 3.- Keep studying (not recomended)" +
                                "\n \t 4.- Change Subject" +
                                "\n \t 5.- Back to menu");

            ConsoleKeyInfo ck = ReadKey();
            if (ck.Key == ConsoleKey.NumPad1 || ck.Key == ConsoleKey.D1) {
                actualPm = new Pomodoro(shortBreakTime, false, sub);
            }
            if (ck.Key == ConsoleKey.NumPad2 || ck.Key == ConsoleKey.D2) {
                actualPm = new Pomodoro(shortBreakTime, false, sub);
            }
            if (ck.Key == ConsoleKey.NumPad3 || ck.Key == ConsoleKey.D3) {

            }
            if (ck.Key == ConsoleKey.NumPad4 || ck.Key == ConsoleKey.D4) {

            }
            if (ck.Key == ConsoleKey.NumPad5 || ck.Key == ConsoleKey.D5) {

            }
        }

        public static void changePomodoro(Pomodoro pm) {
            if (lastPomodoros[0].Study && !lastPomodoros[1].Study) {
                lastPomodoros[1].Subject.TimeToday += lastPomodoros[1].Min;
            }

            for (int i = 0; i < lastPomodoros.Length -1; i++) {
                lastPomodoros[i] = lastPomodoros[i + 1];
            }
        }

    }
}
