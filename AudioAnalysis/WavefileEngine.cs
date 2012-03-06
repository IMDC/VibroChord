using System;

using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;


namespace AudioAnalysis
{
	/// <summary>
	/// Summary description for WaveFile.
	/// </summary>
	///
	
	public class WaveFile
	{
		
		/// <summary>
		/// The Riff header is 12 bytes long
		/// </summary>
		/// 
		
		
		public int glob_BlockAlign;
		

		class Riff
		{
			public Riff()
			{
				m_RiffID = new byte[ 4 ];
				m_RiffFormat = new byte[ 4 ];
			}

			public uint ReadRiff( FileStream inFS )
			{	
				inFS.Read( m_RiffID, 0, 4 );
				
				Debug.Assert( m_RiffID[0] == 82, "Riff ID Not Valid" );

				BinaryReader binRead = new BinaryReader( inFS );

				m_RiffSize = binRead.ReadUInt32( );
				inFS.Read( m_RiffFormat, 0, 4 );
				return m_RiffSize;
			}

			public byte[] RiffID
			{
				get { return m_RiffID; }
			}

			public uint RiffSize
			{
				get { return ( m_RiffSize ); }
			}

			public byte[] RiffFormat
			{
				get { return m_RiffFormat; }
			}

			private byte[]			m_RiffID;
			private uint			m_RiffSize;
			private byte[]			m_RiffFormat;
		}

		/// <summary>
		/// The Format header is 24 bytes long
		/// </summary>
		class Fmt
		{
			public Fmt()
			{
				m_FmtID = new byte[ 4 ];
			}

			public void ReadFmt( FileStream inFS )
			{
				inFS.Read( m_FmtID, 0, 4 );

				Debug.Assert( m_FmtID[0] == 102, "Format ID Not Valid" );

				BinaryReader binRead = new BinaryReader( inFS );

				m_FmtSize = binRead.ReadUInt32( );
				m_FmtTag = binRead.ReadUInt16( );
				m_Channels = binRead.ReadUInt16( );
				m_SamplesPerSec = binRead.ReadUInt32( );
				m_AverageBytesPerSec = binRead.ReadUInt32( );
				
				
				m_BlockAlign = binRead.ReadUInt16( );
				//glob_BlockAlign = m_BlockAlign;
				m_BitsPerSample = binRead.ReadUInt16( );

				// This accounts for the variable format header size 
				// 12 bytes of Riff Header, 4 bytes for FormatId, 4 bytes for FormatSize & the Actual size of the Format Header 
				//inFS.Seek( m_FmtSize + 20, System.IO.SeekOrigin.Begin );
			}

			public byte[] FmtID
			{
				get { return m_FmtID; }
			}

			public uint FmtSize
			{
				get { return m_FmtSize; }
			}

			public ushort FmtTag
			{
				get { return m_FmtTag; }
			}

			public ushort Channels
			{
				get { return m_Channels; }
			}

			public uint SamplesPerSec
			{
				get { return m_SamplesPerSec; }
			}

			public uint AverageBytesPerSec
			{
				get { return m_AverageBytesPerSec; }
			}

			public ushort BlockAlign
			{
				get { return m_BlockAlign; }
			}

			public ushort BitsPerSample
			{
				get { return m_BitsPerSample; }
			}

			private byte[]			m_FmtID;
			private uint			m_FmtSize;
			private ushort			m_FmtTag;
			private ushort			m_Channels;
			private uint			m_SamplesPerSec;
			private uint			m_AverageBytesPerSec;
			public ushort			m_BlockAlign;
			private ushort			m_BitsPerSample;
		}

		/// <summary>
		/// The Data block is 8 bytes + ???? long
		/// </summary>
		class Data
		{
			
			
			public Data()
			{
				m_DataID = new byte[ 1 ];
				m_DataSize2 = new byte[ 4 ];											
			}
              /*
			public Data(IProgressCallback callback)
			{
				m_DataID = new byte[ 1 ];
				m_DataSize2 = new byte[ 4 ];
				this.callback = callback;
				
				
			}
			
			public void ReadData( FileStream inFS, ushort BlockAlign)
			{
				int progressCounter = 10000;
				long position;
				try
				{
					while(true)
					{
						inFS.Read( m_DataID, 0, 1 );
						if(m_DataID[0] == 100)
						{
							inFS.Read( m_DataID, 0, 1 );
							if(m_DataID[0] == 97)
							{
								inFS.Read( m_DataID, 0, 1 );
								inFS.Read( m_DataID, 0, 1 );
								break;
							}
						}
					}
				}
				catch(EndOfStreamException e)
				{
					MessageBox.Show("Couldn't read wave file" + e.Message);
				}

				BinaryReader binRead = new BinaryReader( inFS );
				m_DataSize = 0;
				
				m_DataSize = (uint)binRead.ReadInt32( );
				//delete BLOCKALIGN ?????
				m_NumSamples = (int) ( m_DataSize / BlockAlign);
				m_Data = new Int16[ m_NumSamples ];
				
				//progress.Show();				
				if(BlockAlign == 2)
				{
					for ( int i = 0; i < m_NumSamples; i++)
					{
						//read wave data into array
                        try
                        {
                            m_Data[i] = binRead.ReadInt16();
                        }
                        catch(Exception ex)
                        {
                            //int deleteme = 0;
                            MessageBox.Show(ex.Message.ToString());
                            
                        }
					}
				}
				else
				{
					for ( int i = 0; i < m_NumSamples; i++)
					{
						try
						{
							m_Data[ i ] = binRead.ReadInt16( );

							position = binRead.BaseStream.Position;
							int temp = binRead.ReadInt16();

						}
						catch(System.IO.EndOfStreamException ex)
						{

							//MessageBox.Show("Finished" + ex.Message);
							i = m_NumSamples;
						}
						//int temp2;
						//temp2= Math.IEEERemainder(i+1, 10);
						progressCounter--;
						if(progressCounter == 0)
						{	
							progressCounter = 10000;
							
							if(callback != null)
							{
								this.callback.SetText("Loading Audio Data " + Convert.ToString(Convert.ToInt32((Convert.ToDouble(i + 1) / Convert.ToDouble(m_NumSamples))*100)) + "%");
								int percentage = Convert.ToInt32((Convert.ToDouble(i + 1) / Convert.ToDouble(m_NumSamples))*100);
								this.callback.StepTo(Convert.ToInt32((percentage * .25)) + 25);
								//progress.progressBarWaveRead.Value = Convert.ToInt32((Convert.ToDouble((i+1))/Convert.ToDouble((m_NumSamples)))*100);
							
								Thread.Sleep(1);

								//	int temp = Convert.ToInt32(((i+1)/(m_NumSamples))*100);
								//progress.labelWaveRead.Text = Convert.ToInt32((Convert.ToDouble((i+1))/Convert.ToDouble((m_NumSamples)))*100).ToString() + "%";
								this.callback.SetText("Loading Audio Data " + Convert.ToInt32((Convert.ToDouble((i+1))/Convert.ToDouble((m_NumSamples)))*100).ToString() + "%");
							}
								//Application.DoEvents();

						
							//	progress.Update();
							//	progress.Show();
						
						}
						//m_Data[ i ] = binRead.ReadInt16( );
						//	binRead.ReadInt16( );
					}
					/*int[] waveFileHistogram = new int[max + Math.Abs(min) + 100];

int debug = 0;
					for ( int i = 0; i < m_NumSamples; i++)
					{
						if(m_Data[i] <= 0)
						{
							waveFileHistogram[Math.Abs(m_Data[i])] = waveFileHistogram[Math.Abs(m_Data[i])] + 1;
						}
						else
						{debug = m_Data[i] + Math.Abs(min);
							waveFileHistogram[m_Data[i] + Math.Abs(min) - 2] = waveFileHistogram[m_Data[i] + Math.Abs(min)] + 1;
						}
						
					}
					for ( int i = 0; i < m_NumSamples; i++)
					{
						if(m_Data[i] <=0)
						{
							if(waveFileHistogram[Math.Abs(m_Data[i])] < 1500)
							{
								m_Data[i] = 0;
							}
						}
						else
						{
							if(waveFileHistogram[m_Data[i] + Math.Abs(min) - 2] < 1500)
							{
								m_Data[i] = 0;
							}
						}
					}
					 
					

				}
			} */
			public void ReadData( FileStream inFS,  ushort BlockAlign)
			{
				long position;
				try
				{
					while(true)
					{
						inFS.Read( m_DataID, 0, 1 );
						if(m_DataID[0] == 100)
						{
							inFS.Read( m_DataID, 0, 1 );
							if(m_DataID[0] == 97)
							{
								inFS.Read( m_DataID, 0, 1 );
								inFS.Read( m_DataID, 0, 1 );
								break;
							}
						}
					}
				}
				catch(EndOfStreamException e)
				{
					MessageBox.Show("Couldn't read wave file" + e.Message);
				}

				BinaryReader binRead = new BinaryReader( inFS );
				m_DataSize = 0;
				
				m_DataSize = (uint)binRead.ReadInt32( );

				m_NumSamples = (int) ( m_DataSize / BlockAlign );
				m_Data = new Int16[ m_DataSize / 2];
				
				
				if(BlockAlign == 2)
				{
					for ( int i = 0; i < m_NumSamples / 2; i++)
					{
						m_Data[ i ] = binRead.ReadInt16( );
						binRead.ReadInt16();
						/*if(Math.IEEERemainder(i, 1000000)== 0)
						{
							progress.progressBarWaveRead.Value = Convert.ToInt32(Convert.ToInt64((i+1)/(m_NumSamples))*100);
							int temp = Convert.ToInt32(((i+1)/(m_NumSamples))*100);
							progress.labelWaveRead.Text = Convert.ToInt32(((i+1)/m_NumSamples)*100).ToString();
							progress.Update();
						
						}*/
						//m_Data[ i ] = binRead.ReadInt16( );
						//	binRead.ReadInt16( );
					}
				}
				else
				{
					
					for ( int i = 0; i < m_NumSamples; i++)
					{
						try
						{
							m_Data[ i ] = binRead.ReadInt16( );
							//m_Data[ i ] = binRead.ReadInt16( );
							position = binRead.BaseStream.Position;
							int temp = binRead.ReadInt16();
							//binRead.BaseStream.Position = binRead.BaseStream.Position + 2;
							//m_Data[ i ] = binRead.ReadInt16( );
						}
						catch(System.IO.EndOfStreamException ex )
						{

							MessageBox.Show("Finished " + ex.Message);
							i = m_NumSamples;
						}
						//int temp2;
						//temp2= Math.IEEERemainder(i+1, 10);
						if(Math.IEEERemainder(i+1, 10000) == 0.0)
						{	
							
						
						}
						//m_Data[ i ] = binRead.ReadInt16( );
						//	binRead.ReadInt16( );
					}
				}
			}

			public byte[] DataID
			{
				get { return m_DataID; }
			}

			public uint DataSize
			{
				get { return m_DataSize; }
			}

			public Int32 this[ int pos ]
			{
				get { 
					if(pos >= m_Data.Length)
					{
						//int temp = 0;
						
					}
						return m_Data[ pos ]; }
			}

			public int NumSamples
			{
				get { return m_NumSamples; }
			}

			public byte[]			m_DataID;
			public byte[]		m_DataSize2;
			public uint			m_DataSize;
			public Int16[]			m_Data;
			public int				m_NumSamples;
			//public ushort BlockAlign;
		}


	

		public WaveFile( String inFilepath)
		{
			
			m_Filepath = inFilepath;
			m_FileInfo = new FileInfo( inFilepath );

            try
            {
                m_FileStream = m_FileInfo.OpenRead();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
			m_Riff = new Riff( );
			m_Fmt = new Fmt( );
			m_Data = new Data();
		
			
		}

		public void Read()
		{
			uint temp;
			temp = m_Riff.ReadRiff( m_FileStream );
			m_Fmt.ReadFmt( m_FileStream );
			m_Data.ReadData( m_FileStream,  m_Fmt.BlockAlign);
			
		}

		public void closeFile()
		{
				m_FileStream.Close();
		}
		public void Draw( PaintEventArgs pea, Pen pen )
		{
			Graphics grfx = pea.Graphics;

			if ( m_PageScale == 0.0f )
				m_PageScale = grfx.VisibleClipBounds.Width / m_Data.NumSamples;

			grfx.PageScale = m_PageScale;

			RectangleF visBounds = grfx.VisibleClipBounds;

			grfx.DrawLine( pen, 0, visBounds.Height / 2, visBounds.Width, visBounds.Height / 2 );

			grfx.TranslateTransform( 0, visBounds.Height );
			grfx.ScaleTransform( 1, -1 );

			Draw16Bit( grfx, pen, visBounds );
		}

		void Draw16Bit( Graphics grfx, Pen pen, RectangleF visBounds )
		{
			int val = m_Data[ 0 ];

			int prevX = 0;
			int prevY = (int) (( (val + 32768) * visBounds.Height ) / 65536 );

			for ( int i = 0; i < m_Data.NumSamples; i++ )
			{
				val = m_Data[ i ];

				int scaledVal = (int) (( (val + 32768) * visBounds.Height ) / 65536 );

				grfx.DrawLine( pen, prevX, prevY, i, scaledVal );

				prevX = i;
				prevY = scaledVal;

				if ( m_Fmt.Channels == 2 )
					i++;
			}
		}

		public void ZoomIn( )
		{
			m_PageScale /= 2;
		}

		public void ZoomOut( )
		{
			m_PageScale *= 2;
		}
		public int getDataPoint(int index) 
		{
			if(index > this.getNumSamples())
			{
				throw new System.Exception();

			}
            int value = m_Data.m_Data[index];
			
			return value;
		
				
		}
       
		public int getDataPoint(int index, int noiseGateThreshold, double noiseGateRatio) 
		{
			if(index > this.getNumSamples() || index >= m_Data.m_Data.Length)
			{
				throw new System.Exception();
			}
			
			if(m_Data[index] < noiseGateThreshold)
			{
				return (short)((double)m_Data[index] * noiseGateRatio);
			}
			else
			{
				return m_Data[index];
			}
		}
		public uint getFmtSize()
		{
			return m_Fmt.FmtSize;
		}
		public int getFmtTag()
		{
			return m_Fmt.FmtTag;
		}
		public int getChannels()
		{
			return m_Fmt.Channels;
		}
		public uint getAverageBytesPerSec()
		{
			return m_Fmt.AverageBytesPerSec;
		}
		public uint getSamplesPerSecond()
		{
			return m_Fmt.SamplesPerSec;
		}
		public int getBlockAlign()
		{
			return m_Fmt.BlockAlign;
		}
		public int getBitsPerSample()
		{
			return m_Fmt.BitsPerSample;
		}

		public byte[] getm_RiffID()
		{
			return m_Riff.RiffID;
		}
		public uint getm_RiffSize()
		{
			return m_Riff.RiffSize;
		}
		public byte[] getm_RiffFormat()
		{
			return m_Riff.RiffFormat;
		}
		
			

		public byte[] getDataID()
		{
			return m_Data.m_DataID;
		}

		public uint getDataSize()
		{
			return m_Data.m_DataSize;
		}

		public int getNumSamples()
		{
			return m_Data.m_NumSamples; 
		}


		private string			m_Filepath;
		private FileInfo		m_FileInfo;
		private FileStream		m_FileStream;

		private Riff			m_Riff;
		private Fmt				m_Fmt;
		private Data			m_Data;

		private float			m_PageScale = 0.0f;

      
	}
}