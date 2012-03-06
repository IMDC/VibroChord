using System;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace AudioAnalysis
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileStream tempStream;
            ArrayList noteList = new ArrayList();
            int sample = 0;                  //holds raw audio data sample
            int videoReadStatus = 1;
            int descriptionEndSample = 0;
            byte[] buffer = new byte[4];
            int currentSample = 0;
            int zeroCounter = 0;

            //WaveFile wave = new WaveFile("C:\\Users\\Carmen\\Desktop\\Thesis\\Vibrotactile Compositions\\Brendan\\brendan_happy_Track 1_1.wav");
            //wave.Read();

            tempStream = new FileStream("C:\\Users\\Carmen\\Desktop\\Thesis\\Vibrotactile Compositions\\Brendan\\brendan_happy_Track 1_1.wav", FileMode.Open);

            Note tempNote = new Note();

            bool noteBeginningFound = false;
            int boundary = 500;
            int returnValue = 2;
            //tempStream.Seek(34, 0);
            int maximumSampleValue = 0;
            try
            {
                while (returnValue != 0)
                {
                    
                    try
                    {
                        returnValue = tempStream.Read(buffer, 0, 2);//wave.getDataPoint(currentSample);
                        sample = (int)(BitConverter.ToInt16(buffer, 0));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("File Complete");
                        break;
                    }

                    if (sample > (-1)*boundary && sample < boundary)
                    {
                        zeroCounter++;
                    }
                    if (zeroCounter >= 100 && (sample > boundary || sample < (-1 * boundary)) && noteBeginningFound == false)
                    {

                        zeroCounter = 0;
                        tempNote = new Note();
                        tempNote.StartSample = currentSample;
                        noteBeginningFound = true;
                        maximumSampleValue = sample;
                    }
                    if (noteBeginningFound == true)
                    {
                        if (sample > maximumSampleValue)
                        {
                            maximumSampleValue = sample;
                        }
                    }
                    if (noteBeginningFound == true && zeroCounter >= 100 && (sample > boundary || sample < (-1 * boundary)))
                    {
                        noteBeginningFound = false;
                        tempNote.MaxSampleValue = maximumSampleValue;
                        tempNote.EndSample = currentSample;
                        if (tempNote.EndSample - tempNote.StartSample > 1000)
                        {
                            noteList.Add(tempNote);
                        }
                        zeroCounter = 0;
                        //tempNote = null;
                    }
                    currentSample++;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("File Complete");
            }

           // System.IO.FileStream fs = System.IO.File.Create("C:\\Users\\Carmen\\Desktop\\Thesis\\Vibrotactile Compositions\\Brendan\\brendan_happy_Track 1_1.text");
            StreamWriter outfile = new StreamWriter("C:\\Users\\Carmen\\Desktop\\Thesis\\Vibrotactile Compositions\\Brendan\\brendan_happy_Track 1_1.text");

            for (int i = 0; i < noteList.Count; i++)
            {
                outfile.WriteLine(String.Format("{0:0.00}", ((Note)noteList[i]).StartTime) + "\t" + String.Format("{0:0.00}", ((Note)noteList[i]).EndTime) + "\t" + String.Format("{0:0.00}", ((Note)noteList[i]).LengthTime) + "\t" + String.Format("{0:0.00}", ((Note)noteList[i]).MaxSampleValue));
            }

            outfile.Close();
            MessageBox.Show("File Complete");
            sample += (int)(BitConverter.ToInt16(buffer, 0));


        }
    }
}
