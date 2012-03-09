﻿using System;
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

        public Note()
        {

        }
        public double LengthTime
        {
            get
            {
                return lengthTime;
            }
        }
        public double EndTime
        {
            get
            {
                return endTime;
            }
        }
        public double StartTime
        {
            get
            {
                return startTime;
            }
        }
        public int MaxSampleValue
        {
            get
            {
               return maximumSampleValue;
            }
            set
            {
                maximumSampleValue = value;
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

    }
}