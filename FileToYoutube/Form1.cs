using System;
using System.Diagnostics;
using System.Collections.Generic;

using System.Drawing;

using System.Text;
using System.IO;

using System.Windows.Forms;
using System.Threading;

using System.Drawing.Imaging;




namespace FileToYoutube
{

    public partial class Form1 : Form
    {


        public static void TurnFileToImages(string binaryFile, int imageWidth, int imageHeight, int threadIndex, string bilderPath)
        {

    
            int width = imageWidth;
            int height = imageHeight;
            int imageCount = (int)Math.Ceiling((double)binaryFile.Length * 8 / (height * width));
            //  imageCount = 1;

            int imageIndex = 0;
            int bitIndex = 0;
            int nameIndex = 0;

            Color woRemoveColor = Color.FromArgb(255, 255, 255);

            while (imageIndex < imageCount)
            {
                bool outBool = false;
                Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                using (Graphics gfx = Graphics.FromImage(bitmap))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 0, 0)))
                {
                    gfx.FillRectangle(brush, 0, 0, width, height);
                }
                
                for (int y = 0; y < height && !outBool; y++)
                {
                    for (int x = 0; x < width && !outBool; x++)
                    {
                        int byteIndex = bitIndex / 8;

                        if (byteIndex >= binaryFile.Length)
                        {
                            woRemoveColor = Color.FromArgb(255, 255, 255);
                            outBool = true;
                            break;
                        }

                        if (bitIndex < binaryFile.Length * 8)
                        {

                            byteIndex = bitIndex / 8;
                            if (byteIndex >= binaryFile.Length)
                            {
                                outBool = true;
                                //  woRemoveColor = Color.FromArgb(5, 5, 5);
                                break;
                            }
                            string testString = Convert.ToString(binaryFile[byteIndex], 2).ToString().PadLeft(8, '0');
                            if (testString[bitIndex % 8] == '0')
                            {

                                bitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                            }
                            else
                            {
                                bitmap.SetPixel(x, y, Color.FromArgb(255, 255, 255));

                            };

                            bitIndex++;

                        }

                    }

                }
                // ResizeImage(bitmap, (int)videoWidthNumber.Value, (int)videoHeightNumber.Value).Save(Path.Combine(bilderPath, getName(nameIndex, 20) + ".png"), ImageFormat.Png);
                bitmap.Save(Path.Combine(bilderPath, getName(threadIndex,3) + getName(nameIndex, 20) + ".png"), ImageFormat.Png);

                imageIndex++;
                nameIndex++;
            }


               Bitmap bitmap2 = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                using (Graphics gfx = Graphics.FromImage(bitmap2))
                using (SolidBrush brush = new SolidBrush(woRemoveColor))
                {
                    gfx.FillRectangle(brush, 0, 0, width, height);
                }

                // ResizeImage(bitmap2, (int)videoWidthNumber.Value, (int)videoHeightNumber.Value).Save(Path.Combine(bilderPath, getName(nameIndex, 20) + ".png"), ImageFormat.Png);
                bitmap2.Save(Path.Combine(bilderPath, getName(threadIndex, 3) + getName(nameIndex, 20) + ".png"), ImageFormat.Png);
                nameIndex++;

        }
        public static string getName(int index, int puffer)
        {
           

            string name = index.ToString();

            int myCounter = puffer - name.Length;
            for (int ix = 0; ix < myCounter; ix++)
            {
                name = "0" + name;
            }
            return name;
        }
        public Form1()
        {
            InitializeComponent();
        }

          const int imageWidth = 427;
          const int imageHeight = 240;
      //    const int imageWidth = 1280;
        //  const int imageHeight = 720;
        const int bitWidth = 1;
        string filePath = "", newFolderPath = "", bilderPath = "", filePathDecode = "", newFolderPathDecode = "", bilderPathDecode = "";
        const string ffmpegPath = @"C:\Users\Alper\Desktop\ffmpeg-5.1.2-full_build\bin\ffmpeg.exe"; // das muss mit im Programm included sein

        private void button1_Click(object sender, EventArgs e)
        {
            int bitIndex = 0;
      

            if (!Directory.Exists(newFolderPath) || !Directory.Exists(bilderPath) || !File.Exists(filePath))
            {
                infoLabel.Text = "Make sure that the folder and file are selected";
                return;
            }


            infoLabel.Text = "splitting Files...";

            // Start a thread that calls a parameterized instance method.



            int volumeSize = (int)volumeSizeNumber.Value;

            // The name of the 7-Zip executable
            string sevenZipPath = @"C:\Program Files\7-Zip\7z.exe"; // das muss mit im Programm included sein

            // The command to run 7-Zip
            string sevenZipCommand = $"a -v{volumeSize}k -tzip " + '"'+ Path.Combine(newFolderPath, Path.GetFileNameWithoutExtension(filePath))  + ".zip" + "\" \"" +filePath + "\"";

            // Start 7-Zip
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = sevenZipPath,
                    Arguments = sevenZipCommand,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            process.WaitForExit();

            Console.WriteLine("File split complete.");
            progressBar1.Value = 10;
            progressBar1.Refresh();

            string[] files = Directory.GetFiles(newFolderPath);

            infoLabel.Text = "turning volumes into images...";

            int nameIndex = 0;
            string mystring = "";


            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < files.Length; i++)
            {
                int index = i;

                Thread t = new Thread(() => TurnFileToImages(File.ReadAllText(files[index], Encoding.GetEncoding("ISO-8859-1")), imageWidth, imageHeight, index, bilderPath));
                t.Start();
                threads.Add(t);
            
             }
            for (int i = 0; i < files.Length; i++)
            {
                threads[i].Join();
            }
            // (File.ReadAllText(files[i], Encoding.GetEncoding("ISO-8859-1")),imageWidth,imageHeight,i,bilderPath)
            /*
                  string binaryFile = File.ReadAllText(files[i], Encoding.GetEncoding("ISO-8859-1"));





                  int width = imageWidth;
                  int height = imageHeight;
                  int imageCount = (int)Math.Ceiling((double)binaryFile.Length * 8 / ( height * width ));
                //  imageCount = 1;

                  float progressValue = 10 / (imageCount * files.Length);
                  float counter = 0;
                  int imageIndex = 0;
                  bitIndex = 0;

                  string stringRed = "";
                  string stringGreen = "";
                  string stringBlue = "";
                  Color woRemoveColor = Color.FromArgb(255,255,255);

                  while (imageIndex < imageCount)
                  {
                      bool outBool = false;
                      Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                      using (Graphics gfx = Graphics.FromImage(bitmap))
                      using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 0, 0)))
                      {
                          gfx.FillRectangle(brush, 0, 0, width, height);
                      }

                      for (int y = 0; y < height && !outBool; y++)
                      {
                          for (int x = 0; x < width && !outBool; x++)
                          {
                              int byteIndex = bitIndex / 8;

                              if (byteIndex >= binaryFile.Length)
                              {
                                  woRemoveColor = Color.FromArgb(255, 255, 255);
                                  outBool = true;
                                  break;
                              }

                              ushort r = 0;
                              ushort g = 0;
                              ushort b = 0;



                              // Problem ist das der falsche wert ab dem 2. pixel geschrieben wird 
                             //int bitShift = bitIndex % 8;
                              if (bitIndex < binaryFile.Length * 8)
                              {

                                      byteIndex = bitIndex / 8;
                                      if (byteIndex >= binaryFile.Length)
                                      {
                                          outBool = true;
                                        //  woRemoveColor = Color.FromArgb(5, 5, 5);
                                          break;
                                      }
                                      string testString = Convert.ToString(binaryFile[byteIndex], 2).ToString().PadLeft(8, '0');
                                  if (testString[bitIndex % 8] == '0')
                                  {
                                      bitmap.SetPixel(x, y, Color.FromArgb(0,0,0));
                                  }
                                  else {
                                      bitmap.SetPixel(x, y, Color.FromArgb(255, 255, 255));

                                  };

                                      bitIndex++;






                              }


                           //   Color color = Color.FromArgb(Convert.ToInt16(stringRed, 2), Convert.ToInt16(stringGreen, 2), Convert.ToInt16(stringBlue, 2));
                             // stringRed = "";
                             // stringBlue = "";
                             // stringGreen = "";

               //               bitmap.SetPixel(x, y, color);
                          }
                      }



                      mystring += "file '" + getName(nameIndex,20) + ".png'\n";

                     // ResizeImage(bitmap, (int)videoWidthNumber.Value, (int)videoHeightNumber.Value).Save(Path.Combine(bilderPath, getName(nameIndex, 20) + ".png"), ImageFormat.Png);
                      bitmap.Save(Path.Combine(bilderPath, getName(nameIndex,20) + ".png"), ImageFormat.Png);

                      imageIndex++;
                      nameIndex++;
                      counter += progressValue;
                      progressBar1.Value = 10 + (int)counter;
                      progressBar1.Refresh();
                  }
                  Bitmap bitmap2 = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                  using (Graphics gfx = Graphics.FromImage(bitmap2))
                  using (SolidBrush brush = new SolidBrush(woRemoveColor))
                  {
                      gfx.FillRectangle(brush, 0, 0, width, height);
                  }

                  mystring += "file '" + getName(nameIndex,20) + ".png'\n";
                 // ResizeImage(bitmap2, (int)videoWidthNumber.Value, (int)videoHeightNumber.Value).Save(Path.Combine(bilderPath, getName(nameIndex, 20) + ".png"), ImageFormat.Png);
                  bitmap2.Save(Path.Combine(bilderPath, getName(nameIndex,20) + ".png"), ImageFormat.Png);
                  nameIndex++;

              } */



            string[] filesBilder = Directory.GetFiles(bilderPath);
            for (int i = 0; i < filesBilder.Length; i++)
            {
                mystring += "file '" + filesBilder[i] + "'\n";
            }

             File.WriteAllText(Path.Combine(bilderPath,"WriteLines.txt"), mystring);

            progressBar1.Value = 45;
            progressBar1.Refresh();
            // make video out of images for each volume





            // C:\Users\Alper\Desktop\ffmpeg-5.1.2-full_build\bin\ffmpeg -r 1 -f concat -i WriteLines.txt -c:v  libx264rgb -shortest -s 480x480 -sws_flags neighbor  black.mp4
            // The command to run 7-Zip
            // libx265 -pix_fmt + -vf scale=1920:1080 -colorspace bt709
            //string ffmpegCommand = $"-r {(int)fpsNumber.Value} -f concat -i \"{Path.Combine(bilderPath, "WriteLines.txt")}\" -c:v  libx265  -x265-params lossless=1  -colorspace bt709 -s {(int)videoWidthNumber.Value}x{(int)videoHeightNumber.Value} -sws_flags neighbor \"{Path.Combine(newFolderPath, "black.mp4")}\"";

            //  string ffmpegCommand = $"-r {(int)fpsNumber.Value} -f concat -i \"{Path.Combine(bilderPath, "WriteLines.txt")}\" -c:v  libx265  -x265-params lossless=1  -colorspace bt709  \"{Path.Combine(newFolderPath, "black.mp4")}\"";
            //   string ffmpegCommand = $"-r {(int)fpsNumber.Value} -f concat -safe 0 -i {Path.Combine(bilderPath, "WriteLines.txt")} -c:v libx265 -b:v 1880k -s {(int)videoWidthNumber.Value}:{(int)videoHeightNumber.Value} -sws_flags neighbor  {Path.Combine(newFolderPath, "black.mp4")}";
            string ffmpegCommand = $"-r {(int)fpsNumber.Value} -f concat -safe 0  -i {Path.Combine(bilderPath, "WriteLines.txt")} -c:v  libx264rgb -preset faster -crf 0 -s {(int)videoWidthNumber.Value}:{(int)videoHeightNumber.Value} -sws_flags neighbor  {Path.Combine(newFolderPath, "black.mp4")}";

            ///  string ffmpegCommand = $"-hwaccel cuda -r {(int)fpsNumber.Value} -f concat -safe 0 -i {Path.Combine(bilderPath, "WriteLines.txt")} -vcodec h264_nvenc -preset lossless -s {(int)videoWidthNumber.Value}:{(int)videoHeightNumber.Value} -sws_flags neighbor  {Path.Combine(newFolderPath, "black.mp4")}";

            // -preset lossless 
            // Start 7-Zip
            Process process2 = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = ffmpegCommand,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
              process2.Start();
           process2.WaitForExit();
            progressBar1.Value = 100;
            progressBar1.Refresh();
            return;
            
            
                        List<byte> recoveredBinaryFile = new List<byte>();
                        string[] imageFiles =Directory.GetFiles(bilderPath, "*.png");                 
                        Array.Sort(imageFiles);

                        int fileCounter = 1;
                        bool endOfFile = false;
                        bitIndex = 0;

                        Color currentColor = new Color();
                        Color lastColor = new Color();

                        string currentChar = "";
                         StringBuilder sb1 = new StringBuilder();
                       //  string superString = "";
                     //  List<ushort> charList = new List<ushort>();
                        StringBuilder sb = new StringBuilder();
                         int komischerCounter = 0;
                        
                        foreach (string imageFile in imageFiles)
                        {
                            endOfFile = true;
                            Bitmap bitmap = new Bitmap(imageFile);
                            lastColor = bitmap.GetPixel(0, 0);
                            for (int y = 0; y < imageHeight && endOfFile; y++)
                            {
                                for (int x = 0; x < imageWidth && endOfFile; x++)
                                {
                             
                                   currentColor = bitmap.GetPixel(x, y);
 


                                if (lastColor != currentColor)
                                            {

                                                endOfFile = false;
                                                break;
                                            }
                                            lastColor = currentColor;
                                        }

                                    }

                            if (endOfFile)
                            {

                                if (sb1.Length > 0) {
                                    int removeBytes = sb1.Length;

                                    while (sb1[removeBytes - 1] == 'ÿ')
                                    {
                                        removeBytes -= 1;

                                    }
                                   // removeBytes -= bitWidth % 4;
                                    if(currentColor.R < 10)
                                    {
                                        removeBytes -= 1;
                                    } else if (currentColor.R < 200) {
                                        removeBytes -= 2;
                                       }

                                    sb1.Remove(removeBytes-1, sb1.Length - removeBytes + 1);
 //                                       File.WriteAllBytes(Path.Combine(newFolderPath, "myZip.zip.00" + fileCounter), recoveredBinaryFile.ToArray());
                                     File.WriteAllText(Path.Combine(newFolderPath, "myZip.zip.00" + fileCounter), sb1.ToString(), Encoding.GetEncoding("ISO-8859-1"));
                                     sb1.Clear();
                        recoveredBinaryFile.Clear();
                                    fileCounter++;
                                }
                            }
                            else
                            {
                                for (int y = 0; y < imageHeight; y++)
                                {
                                    for (int x = 0; x < imageWidth; x++)
                                    {
                                        currentColor = bitmap.GetPixel(x, y);

                                    int i;
                                         for ( i = 0; i < 1*3; i++)
                                        {
      
                                            if(i < 1) {
                                                  currentChar += Convert.ToString(currentColor.R, 2).PadLeft(8, '0')[i];
                                   
                                                    
                                }
                                            else if (i < 1*2) {
                                                 currentChar += Convert.ToString(currentColor.G, 2).PadLeft(8, '0')[i- 1];
                                }
                                            else
                                            {
                                                currentChar += Convert.ToString(currentColor.B, 2).PadLeft(8, '0')[i- 1*2];
                                }

                                if (currentChar.Length >= 8)
                                {



                                    // Console.WriteLine(currentChar);
                                    // Console.WriteLine(a);
                                    // Console.WriteLine(BitConverter.GetBytes(a));
                                    //  recoveredBinaryFile.Add(BitConverter.GetBytes((ushort)Convert.ToInt16(currentChar, 2))[0]);
                                    // recoveredBinaryFile.Add(BitConverter.GetBytes((ushort)Convert.ToInt16(currentChar, 2))[1]);

                                    //     Console.WriteLine(Encoding.GetEncoding("Macintosh").GetString(BitConverter.GetBytes((ushort)Convert.ToInt16(currentChar, 2))));



                                   // recoveredBinaryFile.Add(Convert.ToByte(currentChar, 2));
                                     sb1.Append((char)Convert.ToInt16(currentChar, 2));


                                                currentChar = "";
                                }
                                        }


                                    }
                                }
                                if (currentChar.Length > 0 )
                                {



                                    //    recoveredBinaryFile.Add(Convert.ToByte(currentChar, 2));
                        sb1.Append((char)Convert.ToInt16(currentChar, 2));

                        currentChar = "";
                    }
                            }


                        }

                        return;
                        


            // Console.WriteLine(outputArray.Length.ToString());

        }



        private void button4_Click(object sender, EventArgs e)
        {

          /*  if(bilderPathDecode == "" || filePathDecode == "" || newFolderPathDecode == "")
            {
                return;
            }
            // C:\Users\Alper\Desktop\ffmpeg-5.1.2-full_build\bin\ffmpeg.exe -i black.webm -r 1/1 $filename%03d.png
           // string ffmpegCommand = $"-i {filePathDecode} -r {fpsNumber.Value} -s {imageWidth}x{imageHeight} -sws_flags neighbor -pix_fmt rgb24  { Path.Combine(bilderPathDecode , "filename%10d.png")}";
            string ffmpegCommand = $"-i {filePathDecode} -pix_fmt rgb24  -s {imageWidth}x{imageHeight} -sws_flags neighbor { Path.Combine(bilderPathDecode, "filename%10d.png")}";
            //  string ffmpegCommand = $"-r {(int)fpsNumber.Value} -f concat -i {Path.Combine(bilderPath, "WriteLines.txt")} -c:v  libx264rgb -preset faster -crf 10 -s {(int)videoWidthNumber.Value}x{(int)videoHeightNumber.Value} -sws_flags neighbor  {Path.Combine(newFolderPath,"black.mp4")}";

            // Start 7-Zip

            progressBar2.Value = 10;
            progressBar2.Refresh();

            Process process2 = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = ffmpegCommand,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process2.Start();
            process2.WaitForExit();
            progressBar2.Value = 50;
            progressBar2.Refresh();
          */
           // return;
            List<byte> recoveredBinaryFile = new List<byte>();
            string[] imageFiles = Directory.GetFiles(bilderPathDecode, "*.png");
            //  Array.Sort(imageFiles);



            int fileCounter = 1;
            bool endOfFile = false;

            Color currentColor = new Color();


            string currentChar = "";
            StringBuilder sb1 = new StringBuilder();


            int imageIndex = -1;

            int progress = (100 - progressBar2.Value) / imageFiles.Length;

            Bitmap bitmap;
            foreach (string imageFile in imageFiles)
            {
                imageIndex++;
                if(imageIndex < 2 && !checkBox1.Checked) 
                {
                  continue;
                }

                progressBar2.Value += progress;
                endOfFile = true;
                bitmap = new Bitmap(imageFile);
                //bitmap = ResizeImage(bitmap, 64, 64);


                for (int y = 0; y < imageHeight && endOfFile; y++)
                {
                    for (int x = 0; x < imageWidth && endOfFile; x++)
                    {

                        currentColor = bitmap.GetPixel(x, y);



                        if ( 200 > (currentColor.R+ currentColor.G + currentColor.B) / 3)
                        {

                            endOfFile = false;
                            break;
                        }
                    }

                }

               

                if (endOfFile)
                {

                    if (sb1.Length > 0)
                    {
                  
                        //                                       File.WriteAllBytes(Path.Combine(newFolderPath, "myZip.zip.00" + fileCounter), recoveredBinaryFile.ToArray());
                        File.WriteAllText(Path.Combine(newFolderPathDecode, "myZip.zip." + getName(fileCounter, 3)), sb1.ToString(), Encoding.GetEncoding("ISO-8859-1"));
                        sb1.Clear();
                        recoveredBinaryFile.Clear();
                        fileCounter++;
                    }
                }
                else
                {
                    for (int y = 0; y < imageHeight; y++)
                    {
                        for (int x = 0; x < imageWidth; x++)
                        {
                            currentColor = bitmap.GetPixel(x, y);

                            if ((currentColor.R + currentColor.G + currentColor.B) / 3 < 2) {
                                currentChar += "0";

                                } else
                            {
                                currentChar += "1";
                            }

                               
                            if (currentChar.Length >= 8)
                            {

                                sb1.Append((char)Convert.ToInt16(currentChar, 2));
                                currentChar = "";
                            }
                            

                        }
                    }
                    if (currentChar.Length > 0)
                    {



                        //    recoveredBinaryFile.Add(Convert.ToByte(currentChar, 2));
                        sb1.Append((char)Convert.ToInt16(currentChar, 2));

                        currentChar = "";
                    }
                }
               // bitmap.Dispose();
            }
            //progressBar2.Value = 100;
        }

       
        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePathDecode = openFileDialog.FileName;
                Console.WriteLine("Selected file: " + filePathDecode);
            }

            textBox3.Text = filePathDecode;
            textBox3.Refresh();
        }

        private void Select1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                Console.WriteLine("Selected file: " + filePath);
            }

            textBox1.Text = filePath;
        }

        private void Select2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog.SelectedPath;
                Console.WriteLine("Selected folder: " + folderPath);

                string folderName = "Work";
                newFolderPath = Path.Combine(folderPath, folderName);
                if (!Directory.Exists(newFolderPath))
                {
                    Directory.CreateDirectory(newFolderPath);
                    bilderPath = Path.Combine(newFolderPath, "Images");
                    Directory.CreateDirectory(bilderPath);
                    Console.WriteLine("Folder created: " + newFolderPath);
                }
                else
                {
                    Console.WriteLine("Folder already exists: " + newFolderPath);
                }
            }

            textBox2.Text = newFolderPath;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string folderPath = folderBrowserDialog.SelectedPath;
                Console.WriteLine("Selected folder: " + folderPath);

                string folderName = "Work";
                newFolderPathDecode = Path.Combine(folderPath, folderName);
                if (!Directory.Exists(newFolderPathDecode))
                {
                    Directory.CreateDirectory(newFolderPathDecode);
                    bilderPathDecode = Path.Combine(newFolderPathDecode, "Images");
                    Directory.CreateDirectory(bilderPathDecode);
                    Console.WriteLine("Folder created: " + newFolderPathDecode);
                }
                else
                {
                    Console.WriteLine("Folder already exists: " + newFolderPathDecode);
                }
            }

            textBox4.Text = newFolderPathDecode;
            textBox4.Refresh();
        }

    }
}
