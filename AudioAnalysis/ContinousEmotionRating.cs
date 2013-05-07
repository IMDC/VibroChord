using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioAnalysis
{
    class ContinousEmotionRating
    {

        private double time;


        private int rating;



        public double Time
        {
            get { return time; }
            set { time = value; }
        }
        public int Rating
        {
            get { return rating; }
            set { rating = value; }
        }
    }
}
