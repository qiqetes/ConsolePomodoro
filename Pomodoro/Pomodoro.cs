using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro
{
    public class Pomodoro
    {
        private bool started = false;
        private bool ended = false;
        bool study;
        int min;
        int sec;
        int minLeft;
        int secLeft;
        DateTime startTime;
        Subject subject;
        Timer timer;

        public int MinLeft { get => (int)secLeft/60; set => minLeft = value; }
        public bool Study { get => study; set => study = value; }
        public Subject Subject { get => subject; set => subject = value; }
        public int Min { get => min; set => min = value; }

        public Pomodoro(int sec, bool study, Subject sub) {
            this.sec = sec;
            this.Min = (int)(sec / 60);
            this.Study = study;
            Subject = sub;



            timer = new Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(run);
            timer.Start();
        }

        public void run(object source, ElapsedEventArgs e) {
            if (!started) {
                startTime = DateTime.Now;
                started = true;
            }
            secLeft = sec - (int)((DateTime.Now - startTime).TotalSeconds);
            Console.SetCursorPosition(0, 2);
            Console.WriteLine(string.Format("\n\n {0}:{1:00}",
                             (int)TimeSpan.FromSeconds(secLeft).TotalMinutes, 
                             (int)(TimeSpan.FromSeconds(secLeft).TotalSeconds % 60)));

            if (secLeft <= 0) {
                secLeft = 0;
                end();
            }
        } 

        void end(){
            if (Study == true) {
                timer.Stop();

                Subject.TimeToday += Min;

                ended = true;


            }
        }

        public bool isFinished() {
            if (secLeft == 0) {
                Subject.TimeToday += Min;
                return true;
            }
            return false;
        }
    }
}
