using GTA;
using System;

namespace GTA
{
    public class Timer
    {
        public Timer()
        {
            this.Start = 0;
            this.Time = 0;
        }

        public Timer(int ms)
        {
            this.Start = Game.GameTime;
            this.Time = ms;
        }

        public void Set(int ms)
        {
            this.Start = Game.GameTime;
            this.Time = ms;
        }

        public bool IsOverTime
        {
            get
            {
                if (this.Start == 0) return true;
                return this.Current > this.Time;
            }
        }

        public int Current
        {
            get
            {
                return Game.GameTime - this.Start;
            }
        }

        public int Start { get; set; }

        public int Time { get; set; }
    }
}