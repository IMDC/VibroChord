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
            //FileStream audioStream;
            TextReader textStream;
            TextReader continuousEmotionRatingData;
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

            int song = 5;
            int composer = 3;
            double discreteEmotion = 2.23;
           //* Open audio streams **/
             ArrayList audioStreams = new ArrayList();

             for (int i = 1; i <= 26; i++)
             {

             }

             for (int i = 0; i < 8; i++)
             {
              //audioStreams.Add(new FileStream("D:\\SVNs\\Thesis\\Vibrotactile Compositions\\Dennis\\Sad\\dennis sad sperate tracks 16 bit\\sad_Track " + (i + 1) + ".wav", FileMode.Open));
             }


             /***** Read note information from file and create note objects for analysis *****/

             OpenFileDialog openFileDialog = new OpenFileDialog();
             openFileDialog.ShowDialog();
             textStream = new StreamReader(openFileDialog.FileName);

            char[] seperator = {'\t' };
            int counter = 0;
            int trackNumber = 0;
            ArrayList tracks = new ArrayList();

            while (true)
            {
                
                    string line = textStream.ReadLine();

                    counter++;
                    
                        try
                        {
                            String[] values = line.Split(seperator);

                            String startTime = values[0];
                            String endTime = values[1];

                            if (startTime.Contains("Track"))
                            {
                                String[] s_trackNumber = startTime.Split(' ');
                                trackNumber = Convert.ToInt32(s_trackNumber[1]);
                               
                                if (trackNumber == 1)
                                {
                                    continue;
                                }
                                else
                                {
                                    tracks.Add(notes);
                                    
                                    
                                    notes = new ArrayList();
                                    continue;
                                }
                            }
                            if (startTime.Contains("In"))
                            {
                                continue;
                            }

                            String[] time = startTime.Split(':');
                            double startTimeNum = Convert.ToDouble(time[0]) * 60.0 + Convert.ToDouble(time[1]);

                            time = endTime.Split(':');
                            double endTimeNum = Convert.ToDouble(time[0]) * 60.0 + Convert.ToDouble(time[1]);

                            double lengthNum = endTimeNum - startTimeNum;

                            Note newNote = new Note();

                            newNote.StartTime = startTimeNum;
                            newNote.EndTime = endTimeNum;
                            newNote.LengthTime = lengthNum;
                            Console.WriteLine(newNote.StartTime.ToString());
                            notes.Add(newNote);
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
               

            }

            tracks.Add(notes);


           // int returnValue = 2;           
           //((FileStream) audioStreams[0]).Seek(44, 0);
           // int windowCounter = 0;
           // bool foundStart = false;
           // bool foundEnd = false;
           // int sampleRate = 44100;
           // int currentNote = 0;
           // int startSample = 0;
           // int endSample = 0;

           // ArrayList noteSamples = new ArrayList();
           // ArrayList noteSamplesList = new ArrayList();

           // for (int track = 0; track < 8; track++)
           // {
           //     currentNote = 0;
           //     foundStart = false;
           //     foundEnd = false;
           //     startSample = 0;
           //     endSample = 0;
           //     currentSample = 0;

           //     if (((ArrayList)tracks[track]).Count == 0)
           //     {
           //         continue;
           //     }

           //     while (returnValue != 0 && currentNote < ((ArrayList)tracks[track]).Count)
           //     {

           //         try
           //         {
           //             returnValue = ((FileStream)audioStreams[track]).Read(buffer, 0, 2);//wave.getDataPoint(currentSample);


           //         }
           //         catch (Exception ex)
           //         {
           //             MessageBox.Show(ex.Message);
           //             break;
           //         }

           //         sample = (int)(BitConverter.ToInt16(buffer, 0));
           //         currentSample++;

                   

           //         startSample =  (int)((((Note)(((ArrayList)tracks[track])[currentNote])).StartTime) * (double)sampleRate);
           //         startSample += 200;
           //         endSample = (int)((((Note)(((ArrayList)tracks[track])[currentNote])).EndTime) * (double)sampleRate);
           //         endSample += -200;
           //         if (startSample == currentSample)
           //         {
           //             startSample = currentSample;
           //             foundStart = true;
           //             noteSamples = new ArrayList();

           //         }
           //         if (foundStart)
           //         {
           //             noteSamples.Add(sample);
           //         }
           //         if (endSample == currentSample)
           //         {
           //             foundStart = false;
           //             foundEnd = true;
           //             noteSamplesList.Add(noteSamples);

           //         }
           //         if (foundEnd)
           //         {
           //             int previousNumber = 0;
           //             int currentNumber = 0;
           //             int firstSampleNumber = 0;
           //             int secondSampleNumber = 0;
           //             int waveCount = 0;
           //             int waveLength = 0;
           //             ArrayList waveLengths = new ArrayList();
           //             for (int i = 1; i < noteSamples.Count; i++)
           //             {
           //                 currentNumber = (int)noteSamples[i];
           //                 previousNumber = (int)noteSamples[i-1];
           //                 if (previousNumber < 0 && currentNumber > 0)
           //                 {
           //                     //found first zero crossing
           //                     firstSampleNumber = i;

           //                 }
           //                 else if (previousNumber > 0 && currentNumber < 0)
           //                 {
           //                     //found second zero crossing
           //                     secondSampleNumber = i;
           //                     waveLength+= (secondSampleNumber - firstSampleNumber) * 2;
           //                     waveLengths.Add((secondSampleNumber - firstSampleNumber) * 2);
           //                     waveCount++;
           //                 }
                           

           //             }
           //             //waveLength = (int)(waveLength / waveCount);
           //            //double frequency = 44100 / waveLength;

                        
           //             /*** calculate frequency of note ******************************/
           //            /* Complex[] complex = new Complex[4096];
           //             try
           //             {
           //                 for (int i = 0; i < 4096 - 1; i++)
           //                 {
           //                     complex[i] = new Complex((int)noteSamples[i], 0);
           //                 }
           //             }
           //             catch (Exception ex)
           //             {

           //             }

           //             FourierTransform.FFT(complex, FourierTransform.Direction.Forward);
           //             double maximum = 0.0;
           //             int maxSlot = 0;
           //             for (int i = 0; i < complex.Length / 2; i++)
           //             {
           //                 if (complex[i].Re > maximum)
           //                 {
           //                     maximum = complex[i].Re;
           //                     maxSlot = i;
           //                 }
           //             }*/
           //             //((Note)(((ArrayList)tracks[track])[currentNote])).Frequency = frequency;//maxSlot * sampleRate / 4096; //(double)zeroCrossing / ((double)((Note)notes[currentNote]).LengthTime);
                        
           //             foundEnd = false;
           //             if (currentNote > ((ArrayList)(tracks[track])).Count - 1)
           //             {
           //                 break;
           //             }

           //             /*** calculate average amplitude of note ******************************/

           //             int amplitude = 0;
           //             for (int i = 0; i < noteSamples.Count; i++)
           //             {
           //                 if ((int)noteSamples[i] > amplitude)
           //                 {
           //                     amplitude = (int)noteSamples[i];
           //                 }
           //             }
           //             if (amplitude == 0)
           //             {
           //                 int p = 0;
           //             }
           //             if (track == 5)
           //             {
           //                 int p = 0;
           //             }

           //             ((Note)(((ArrayList)tracks[track])[currentNote])).Amplitude = amplitude;
           //             ((Note)(((ArrayList)tracks[track])[currentNote])).Song = song;
           //             ((Note)(((ArrayList)tracks[track])[currentNote])).Composer = composer;

           //             currentNote++;
           //         }





           //     }
           // }
         

            double averageNoteLength = 0;
            double averageNoteLengthSong = 0;
            int numberofNotes = 0;
            int numberofNotesInSong = 0;
            ArrayList trackAverageNotes = new ArrayList();
            ArrayList trackNumberofNotes = new ArrayList();
           
            for (int track = 0; track < 8; track++)
            {


                for (int note = 0; note < ((ArrayList)tracks[track]).Count; note++)
                {
                    averageNoteLength += ((Note)(((ArrayList)tracks[track])[note])).LengthTime;
                    averageNoteLengthSong += ((Note)(((ArrayList)tracks[track])[note])).LengthTime;
                    numberofNotes++;
                    numberofNotesInSong++;
                }
                averageNoteLength = averageNoteLength / (int)numberofNotes;
                trackAverageNotes.Add(averageNoteLength);
                trackNumberofNotes.Add(numberofNotes);

                averageNoteLength = 0;
                numberofNotes = 0;

            }
            averageNoteLengthSong = averageNoteLengthSong / numberofNotesInSong;
            int[] trackNoteCounters = new int[8] {0,0,0,0,0,0,0,0};
            Note pointerNote = null;
            ArrayList linearNoteList = new ArrayList();
            while (true)
            {
                ArrayList tempNoteList = new ArrayList();
                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                    tempNoteList.Add(((Note)((ArrayList)tracks[i])[trackNoteCounters[i]]));
                    }
                    catch(Exception ex){
                        tempNoteList.Add(null);
                    }


                    //(tracks[i]));

                }
                Note previousNote = null;
                Note firstNote = null;
                int trackCounter = 0;
                int currentTrack = 0;
                foreach (Note note in tempNoteList)
                {
                    if (note == null)
                    {
                        trackCounter++;
                        continue;
                    }
                    if (previousNote == null)
                    {
                        previousNote = note;
                        currentTrack = trackCounter;


                    }


                    else if (note.StartTime < previousNote.StartTime)
                    {
                        previousNote = note;
                        currentTrack = trackCounter;

                    }
                    trackCounter++;
                }

                pointerNote = previousNote;
                try
                {
                    pointerNote.Track = currentTrack + 1;
                }
                catch (Exception ex)
                {
                    break;
                }
                linearNoteList.Add(pointerNote);
                trackNoteCounters[currentTrack]++;

                int endofTrackCounter = 0;
                /*for (int i = 0; i < 8; i++)
                {
                    if (trackNoteCounters[i] > ((ArrayList)(tracks[i])).Count)
                    {
                        endofTrackCounter++;
                    }
                }
                if (endofTrackCounter == 8)
                {
                    break;
                }
                else
                {
                    endofTrackCounter = 0;
                }*/
            }

            /************** Sort the linear list *********************/ 
            Note tempNote;
            for (int i = 0; i < linearNoteList.Count; i++)
            {
               
                for (int j = 0; j < i; j++)
                {
                    if (((Note)linearNoteList[j]).StartTime > ((Note)linearNoteList[j + 1]).StartTime)
                    {
                        tempNote = (Note)linearNoteList[j];
                        linearNoteList[j] = linearNoteList[j + 1];
                        linearNoteList[j + 1] = tempNote;
                    }
                }

            }
            
            /********************* calculate jumps and jump length ***************************/
            Note beforeNote = null;
            int trackJumps = 0;
            int jumpLengths = 0;
            int changeInAmplitude = 0;
            double shortestNote = 999999;
            double longestNote = 0;
            int upJumpsPerSecond = 0;
            int downJumpsPerSecond = 0;


            double averageTrackPosition = 0.0;

            foreach (Note note in linearNoteList)
            {
                averageTrackPosition += note.Track;
                if (beforeNote == null)
                {
                    beforeNote = note;
                    continue;
                }
                if (beforeNote.Track != note.Track && beforeNote.StartTime != note.StartTime)
                {
                    trackJumps++;
                    jumpLengths += Math.Abs(beforeNote.Track - note.Track);//.Add(Math.Abs(beforeNote.Track - note.Track));
                    if (beforeNote.Track < note.Track)
                    {
                        upJumpsPerSecond++;
                    }
                    else if (beforeNote.Track > note.Track)
                    {
                        downJumpsPerSecond++;
                    }

                }
                if (note.LengthTime < shortestNote)
                {
                    shortestNote = note.LengthTime;
                }
                if(note.LengthTime > longestNote)
                {
                    longestNote = note.LengthTime;
                }
                changeInAmplitude += (int)Math.Abs(beforeNote.Amplitude - note.Amplitude);
                beforeNote = note;
            }
            changeInAmplitude = changeInAmplitude / linearNoteList.Count;

            averageTrackPosition = averageTrackPosition / linearNoteList.Count;



            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.ShowDialog();
            saveFileDialog.DefaultExt = "txt";
            //averageNoteLength = averageNoteLength / (double)numberofNotes;
           // System.IO.FileStream fs = System.IO.File.Create("C:\\Users\\Carmen\\Desktop\\Thesis\\Vibrotactile Compositions\\Brendan\\brendan_happy_Track 1_1.text");
            StreamWriter outfile = new StreamWriter(saveFileDialog.FileName);

            /*int isCurrentNoteAJump = 0;
            int lengthofJump = 0;
            
            double timeFromLastNoteStart = 0;
            Note previousNote2;
            Note currentNote2;
            outfile.WriteLine(
                    "Track" + "\t" +
                    "StartTime" + "\t" +
                    "EndTime" + "\t" +
                    "NoteLength" + "\t" +
                    "Amplitude" + "\t" +
                    "TimeFromLastNote" + "\t" +
                    "ChangeInAmplitude" + "\t" +
                    "IsCurrentNoteAJump" + "\t" +
                    "LengthOfJump" + "\t" +
                    "Song" + "\t" +
                    "Composer" + "\t" +
                    "DiscreteEmotion" + "\t"
                    );

            for (int i = 0; i < linearNoteList.Count; i++)
            {
                if(i != 0)
                {
                    previousNote2 = (Note)linearNoteList[i-1];
                    currentNote2 = (Note)linearNoteList[i];
                    if (previousNote2.Track != ((Note)currentNote2).Track)
                    {

                        isCurrentNoteAJump = 1;
                        lengthofJump = Math.Abs(previousNote2.Track - ((Note)currentNote2).Track);
                        changeInAmplitude = (int)Math.Abs(previousNote2.Amplitude - ((Note)currentNote2).Amplitude);
                        timeFromLastNoteStart = Math.Abs(previousNote2.StartTime - ((Note)currentNote2).StartTime);

                    }
                    else
                    {
                        isCurrentNoteAJump = 0;
                    }
                    

                }
                outfile.WriteLine(
                    ((Note)linearNoteList[i]).Track + "\t" +
                    ((Note)linearNoteList[i]).StartTime + "\t" +
                    ((Note)linearNoteList[i]).EndTime + "\t" +                    
                    ((Note)linearNoteList[i]).LengthTime + "\t" + 
                    ((Note)linearNoteList[i]).Amplitude + "\t" +
                    timeFromLastNoteStart + "\t" +
                    changeInAmplitude + "\t" +
                    isCurrentNoteAJump + "\t" +
                    lengthofJump + "\t" +
                    song + "\t" +
                    composer + "\t" +
                    discreteEmotion + "\t" 
                    );

                lengthofJump = 0;
                isCurrentNoteAJump = 0;
            }*/
             double averageAmplitudePerSong = 0;
             int totalNoteNumber = 0;
             for (int track = 0; track < 8; track++)
             {
                 totalNoteNumber = totalNoteNumber + ((ArrayList)tracks[track]).Count;
                 double averageAmplitude = 0;
                 
                 for (int note = 0; note < ((ArrayList)tracks[track]).Count; note++)
                 {
                     averageAmplitude = averageAmplitude + ((Note)(((ArrayList)tracks[track])[note])).Amplitude;
                     averageAmplitudePerSong = averageAmplitudePerSong + ((Note)(((ArrayList)tracks[track])[note])).Amplitude;
                 }
                 averageAmplitude = averageAmplitude / ((ArrayList)tracks[track]).Count;
                 outfile.WriteLine("Track " + track + " Average Amplitude: " + averageAmplitude);
             }
             averageAmplitudePerSong = averageAmplitudePerSong / totalNoteNumber;
             outfile.WriteLine("Song Average Amplitude: " + averageAmplitudePerSong);
             outfile.WriteLine("Song Change in Amplitude: " + changeInAmplitude);
             outfile.WriteLine("Song Average Note Length: " + averageNoteLengthSong);
             outfile.WriteLine("Song Track Jumps: " + trackJumps);
             outfile.WriteLine("Song Jump Length TOtal: " + jumpLengths);
             outfile.WriteLine("Shortest Note: " + shortestNote);
             outfile.WriteLine("Longest Note: " + longestNote);
             for (int track = 0; track < 8; track++)
             {


                 for (int note = 0; note < ((ArrayList)tracks[track]).Count; note++)
                 {
                     outfile.WriteLine((track + 1) + "\t" + ((Note)(((ArrayList)tracks[track])[note])).Frequency + "\t" + ((Note)(((ArrayList)tracks[track])[note])).StartTime + "\t" + ((Note)(((ArrayList)tracks[track])[note])).EndTime + "\t" + ((Note)(((ArrayList)tracks[track])[note])).LengthTime + "\t" + ((Note)(((ArrayList)tracks[track])[note])).Amplitude);
                 }

             }
            outfile.Close();
            
            sample += (int)(BitConverter.ToInt16(buffer, 0));


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
