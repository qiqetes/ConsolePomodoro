using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro
{
    public class Subject
    {
        static int lines = 5;
        string name;
        int ID;
        int timeToday;
        int timeTotal;

        public Subject(string name, int iD, int timeToday, int timeTotal) {
            this.name = name;
            ID = iD;
            this.timeToday = timeToday;
            this.timeTotal = timeTotal;
        }
        public Subject(string name) {
            this.name = name;
            timeToday = 0;
            timeTotal = 0;
        }

        public int TimeToday {
            get => timeToday;
            set {
                string[] linesText = File.ReadAllLines(Program.dataPath);
                using (StreamWriter writer = new StreamWriter(Program.dataPath)) {
                    for (int currentLine = 1; currentLine <= linesText.Length; ++currentLine) {
                        if (currentLine == ID * lines+5) {
                            writer.WriteLine(value.ToString());
                        } else {
                            writer.WriteLine(linesText[currentLine - 1]);
                        }
                    }
                }
                timeToday = value;
            }
        }

        public string Name { get => name; }
    }
}
