using System;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
//using System.Diagnostic;
using AForge.Math;



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
            FileStream audioStream;
            TextReader textStream;
            ArrayList noteList = new ArrayList();
            int sample = 0;                  //holds raw audio data sample
            int videoReadStatus = 1;
            int descriptionEndSample = 0;
            byte[] buffer = new byte[4];
            int currentSample = 0;
            int zeroCounter = 0;

            ArrayList sampleBuffer = new ArrayList();

            ArrayList rootMeanSquare = new ArrayList();

            ArrayList last200Maximums = new ArrayList();

            ArrayList notes = new ArrayList();

            //WaveFile wave = new WaveFile("C:\\Users\\Carmen\\Desktop\\Thesis\\Vibrotactile Compositions\\Brendan\\brendan_happy_Track 1_1.wav");
            //wave.Read();

            //tempStream = new FileStream("E:\\SVNs\\Thesis\\Vibrotactile Compositions\\Brendan\\brendan_happy_Track 1_1.wav", FileMode.Open);

            audioStream = new FileStream(" E:\\SVNs\\Thesis\\Vibrotactile Compositions\\Rob\\rob happy seperate tracks 16 bit\\rob_happy_Track 1.wav", FileMode.Open);
            textStream = new StreamReader(" E:\\SVNs\\Thesis\\Vibrotactile Compositions\\Rob\\rob happy seperate tracks 16 bit\\Rob - Happy.txt");

            char[] seperator = {'\t' };
            int counter = 0;


            while (true)
            {
                
                    string line = textStream.ReadLine();

                    counter++;
                    if (counter > 2)
                    {
                        try
                        {
                            String[] values = line.Split(seperator);

                            String startTime = values[0];
                            String endTime = values[1];
                            String length = values[2];

                            String[] time = startTime.Split(':');
                            double startTimeNum = Convert.ToDouble(time[0]) * 60.0 + Convert.ToDouble(time[1]);

                            time = endTime.Split(':');
                            double endTimeNum = Convert.ToDouble(time[0]) * 60.0 + Convert.ToDouble(time[1]);

                            double lengthNum = endTimeNum - startTimeNum;

                            Note newNote = new Note();

                            newNote.StartTime = startTimeNum;
                            newNote.EndTime = endTimeNum;
                            newNote.LengthTime = lengthNum;

                            notes.Add(newNote);
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                }

            }

           

            //bool noteBeginningFound = false;
            //int threshold = 100;
            int returnValue = 2;
           
            audioStream.Seek(44, 0);
           // int maximumSampleValue = 0;
           // bool lastValueZeroCounter = false;

            int windowCounter = 0;

            bool foundStart = false;
            bool foundEnd = false;

            int sampleRate = 44100;

            int currentNote = 0;
            int startSample = 0;
            int endSample = 0;

            ArrayList noteSamples = new ArrayList();
            ArrayList noteSamplesList = new ArrayList();

            
                while (returnValue != 0)
                {
                    
                    try
                    {
                        returnValue = audioStream.Read(buffer, 0, 2);//wave.getDataPoint(currentSample);
                        
                       
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        break;
                    }

                    sample = (int)(BitConverter.ToInt16(buffer, 0));
                    //sampleBuffer.Add(sample);
                   // windowCounter++;
                    currentSample++;
                    startSample = (int)((((Note)notes[currentNote]).StartTime) * (double)sampleRate);
                    startSample += 200;
                    endSample = (int)((((Note)notes[currentNote]).EndTime) * (double)sampleRate);
                    endSample += -200;
                    if ( startSample == currentSample)
                    {
                        startSample = currentSample;
                        foundStart = true;
                        noteSamples = new ArrayList();

                    }
                    if (foundStart)
                    {
                        noteSamples.Add(sample);
                    }
                    if (endSample == currentSample)
                    {
                        foundStart = false;
                        foundEnd = true;
                        noteSamplesList.Add(noteSamples);
                        
                    }
                    if (foundEnd)
                    {
                        
                        Complex[] complex = new Complex[4096];
                        try
                        {
                            for (int i = 0; i < 4096 - 1; i++)
                            {
                                complex[i] = new Complex((int)noteSamples[i], 0);
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        FourierTransform.FFT(complex, FourierTransform.Direction.Forward);
                        double maximum = 0.0;
                        int maxSlot = 0;
                        for (int i = 0; i < complex.Length / 2; i++)
                        {
                            if (complex[i].Re > maximum)
                            {
                                maximum = complex[i].Re;
                                maxSlot = i;
                            }
                        }
                        ((Note)notes[currentNote]).Frequency = maxSlot * sampleRate / 4096; //(double)zeroCrossing / ((double)((Note)notes[currentNote]).LengthTime);
                        currentNote++;
                        foundEnd = false;
                        if (currentNote > notes.Count - 1)
                        {
                            break;
                        }
                    }


                    
                }
            

           // System.IO.FileStream fs = System.IO.File.Create("C:\\Users\\Carmen\\Desktop\\Thesis\\Vibrotactile Compositions\\Brendan\\brendan_happy_Track 1_1.text");
            StreamWriter outfile = new StreamWriter("E:\\SVNs\\Thesis\\Vibrotactile Compositions\\Brendan\\brendan_happy_Track 1_1.text");

            for (int i = 0; i < notes.Count; i++)
            {
                outfile.WriteLine(i + "\t" + ((Note)notes[i]).Frequency + "\t" + ((Note)notes[i]).StartTime + "\t" + ((Note)notes[i]).EndTime);
            }

            outfile.Close();
            
            sample += (int)(BitConverter.ToInt16(buffer, 0));


        }
    }
}
