using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioAnalysis
{
    public class Note
    {

        private int startSample;
        private int endSample;
        private double startTime;
        private double endTime;
        private double lengthTime;
        private double lengthSamples;
        private int maximumSampleValue;
        private double frequency;
        private int track;
        private double amplitude;
        private int song;


        private int composer;


       

        public Note()
        {

        }
        public int Composer
        {
            get { return composer; }
            set { composer = value; }
        }


        public int Song
        {
          get { return song; }
          set { song = value; }
        }

        public int Track
        {
            get
            {
                return track;
            }
            set
            {
                track = value;
            }
        }
        public double Frequency
        {
            get
            {
                return frequency;
            }
            set
            {
                frequency = value;
            }
        }
        public double LengthTime
        {
            get
            {
                return lengthTime;
            }
            set
            {
                lengthTime = value;
            }
        }
        public double EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                endTime = value;
            }
        }
        public double StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
            }
        }
       
        public int StartSample
        {
            get
            {
                return startSample;
            }
            set
            {
                startSample = value;
                startTime = Convert.ToDouble(startSample) / 44100.0;
            }


        }
        public int EndSample
        {
            get
            {
                return endSample;
            }
            set
            {
                endSample = value;
                endTime = Convert.ToDouble(endSample) / 44100.0;
                lengthSamples = endSample - startSample;
                lengthTime = endTime - startTime;
            }


        }

        public double Amplitude
        {
            get { return amplitude; }
            set { amplitude = value; }
        }

    }
}
