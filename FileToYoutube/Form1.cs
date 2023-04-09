using System;
using System.Diagnostics;
using System.Collections.Generic;

using System.Drawing;

using System.Text;
using System.IO;

using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using QRCodeDecoderLibrary;
using QRCoder;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;


namespace FileToYoutube
{

    public partial class Form1 : Form
    {

        static QRCodeGenerator qrGenerator = new QRCodeGenerator();

        static Dictionary<int, int> myNames = new Dictionary<int, int>();

        private void TurnVideoToFile(string[] chunk, int threadIndex, string bilderPath, string workPath)
        {

            bool endOfFile = false;

            Color currentColor = new Color();

            StringBuilder sb1 = new StringBuilder();


            int imageIndex = -1;

            Bitmap bitmap;
            QRDecoder Decoder = new QRDecoder();
            byte[][] ResultArray;


            for (int i = 0; i < chunk.Length; i++)
            {
                imageIndex++;

                endOfFile = true;
                bitmap = new Bitmap(chunk[i]);



                for (int x = 0; x < imageWidth && endOfFile; x++) // it's almost impossible that one straight line at the middle is white. To speed up the decode every image is only checked if it is white in the middle
                {

                    currentColor = bitmap.GetPixel(x, 92); // 185 / 2 = 92.5


                    if (200 > ((currentColor.R + currentColor.G + currentColor.B) / 3))
                    {

                        endOfFile = false;
                        break;
                    }
                }



                if (endOfFile)
                {

                    if (sb1.Length > 0)
                    {

                        File.WriteAllText(Path.Combine(newFolderPathDecode, "myZip.zip." + getName(threadIndex, 3)), sb1.ToString(), Encoding.GetEncoding("ISO-8859-1"));
                        sb1.Clear();
                     
                    }
                }
                else
                {
                    Decoder = new QRDecoder();
                    ResultArray = Decoder.ImageDecoder(bitmap);

                    if (ResultArray == null)
                    {
                        continue;
                    }
                    foreach (char c in ResultArray[0])
                    {

                        sb1.Append(c);
                    }
                    ResultArray = null;
                    bitmap.Dispose();
                    Decoder = null;

                }

            }
        }

        static void TurnFileToImages(string binaryFile, int imageWidth, int imageHeight, int threadIndex, string bilderPath)
        {

            


              int width = imageWidth;
              int height = imageHeight;

            int nameIndex = 0;
            int jumpWidth = 1273;
            Bitmap qrCodeImage = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            for (int i = 0; i < binaryFile.Length; i+= jumpWidth)
            {
                if(i + jumpWidth > binaryFile.Length)
                {
                    jumpWidth = binaryFile.Length - i;
                }

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(binaryFile.Substring(i, jumpWidth), QRCodeGenerator.ECCLevel.H);
                QRCode qrCode = new QRCode(qrCodeData);
                qrCodeImage = qrCode.GetGraphic(1);
                if(qrCodeImage.Width != 185)
                {
                    Bitmap temp = new Bitmap(185, 185);

                    Graphics g = Graphics.FromImage(temp);
                    g.Clear(Color.White);
                    int top = (185 - qrCodeImage.Height) / 2;
                    int left = (185 - qrCodeImage.Width) / 2;
                    g.DrawImage(qrCodeImage, left, top);

                    qrCodeImage = temp;


                }
                qrCodeImage.Save(Path.Combine(bilderPath, getName(threadIndex, 3) + getName(nameIndex, 20) + ".png"), ImageFormat.Png);
                nameIndex++;
              
              
            }



            Color woRemoveColor = Color.FromArgb(255, 255, 255);
            Bitmap bitmap2 = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using (Graphics gfx = Graphics.FromImage(bitmap2))
            using (SolidBrush brush = new SolidBrush(woRemoveColor))
            {
                gfx.FillRectangle(brush, 0, 0, width, height);
            }


            bitmap2.Save(Path.Combine(bilderPath, getName(threadIndex, 3) + getName(nameIndex, 20) + ".png"), ImageFormat.Png);
            nameIndex++;

            myNames.Add(threadIndex, nameIndex);
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

          const int imageWidth = 185;
          const int imageHeight = 185;

        const int bitWidth = 1;
        string filePath = "", newFolderPath = "", bilderPath = "", filePathDecode = "", newFolderPathDecode = "", bilderPathDecode = "";
        const string ffmpegPath = @"C:\Users\Alper\Desktop\ffmpeg-5.1.2-full_build\bin\ffmpeg.exe"; // das muss mit im Programm included sein

        private void button1_Click(object sender, EventArgs e)
        {

      

            if (!Directory.Exists(newFolderPath) || !Directory.Exists(bilderPath) || !File.Exists(filePath))
            {
                infoLabel.Text = "Make sure that the folder and file are selected";
                return;
            }

       

            infoLabel.Text = "splitting Files...";

            // Start a thread that calls a parameterized instance method.

            

            int volumeSize = 0; // (int)volumeSizeNumber.Value;

            var file = File.OpenRead(filePath);
            var fileSize = file.Length;
            file.Close();

            volumeSize = (int)(fileSize / (Environment.ProcessorCount+1) / 1024); 

            if(volumeSize < 500)
            {
                volumeSize = 500;
            } else if(volumeSize > 50000)
            {
                volumeSize = 50000; // 12 core cpu would mean 12*50 MB so 600 MB ram at least to put the images back into files
            }

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


            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < files.Length; i++)
            {

                    int index = i;

                    Thread t = new Thread(() => TurnFileToImages(File.ReadAllText(files[index], Encoding.GetEncoding("ISO-8859-1")), imageWidth, imageHeight, index, bilderPath));
                    t.Start();
                    threads.Add(t);
               // }

            }
            foreach (Thread t in threads)
            {
                t.Join();
            }
            
            


            StringBuilder myStringBuilder = new StringBuilder();


            for (int i = 0; i < myNames.Count; i++)
            {
                for (int ii = 0; ii < myNames[i]; ii++)
                {
                    myStringBuilder.Append("file '" + getName(i, 3) + getName(ii, 20) + ".png" + "'\n");

                }
              
            }
               


             File.WriteAllText(Path.Combine(bilderPath,"WriteLines.txt"), myStringBuilder.ToString());

            progressBar1.Value = 45;
            progressBar1.Refresh();
            // make video out of images for each volume





           
           // string ffmpegCommand = $"-r {(int)fpsNumber.Value} -f concat -safe 0  -i {Path.Combine(bilderPath, "WriteLines.txt")} -c:v  libx264rgb -preset faster -crf 0 -s {(int)videoWidthNumber.Value}:{(int)videoHeightNumber.Value} -sws_flags neighbor  {Path.Combine(newFolderPath, "black.mp4")}";
           // string ffmpegCommand = $"-r {(int)fpsNumber.Value} -f concat -safe 0  -i {Path.Combine(bilderPath, "WriteLines.txt")} -c:v  libx264rgb -preset faster -crf 0 -s {(int)videoWidthNumber.Value}:{(int)videoHeightNumber.Value} -sws_flags neighbor {Path.Combine(newFolderPath, "black.mp4")}";
            string ffmpegCommand = $"-r {(int)fpsNumber.Value} -f concat -safe 0  -i {Path.Combine(bilderPath, "WriteLines.txt")} -c:v libx264 -preset faster -movflags faststart -vf format=yuv420p -bf 2 -crf 25 -s {(int)videoWidthNumber.Value}:{(int)videoHeightNumber.Value} -sws_flags neighbor -map 0 -segment_time 06:00:00 -f segment -reset_timestamps 1 -ignore_editlist 1 {Path.Combine(newFolderPath, "output%03d.mp4")}";



            Process process2 = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = ffmpegCommand,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = bilderPath
                }
            };
              process2.Start();
           process2.WaitForExit();
            progressBar1.Value = 100;
            progressBar1.Refresh();
            return;
            
            
                 
        }

        static YoutubeClient youtube = new YoutubeClient();

        private async Task downloadFile(string videoUrl, int videoId)
        {

            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);
            // ...or highest quality MP4 video-only stream
            var streamInfo = streamManifest
             .GetVideoOnlyStreams()
             .GetWithHighestVideoQuality();
            var stream = await youtube.Videos.Streams.GetAsync(streamInfo);
            await youtube.Videos.Streams.DownloadAsync(streamInfo, Path.Combine(newFolderPathDecode, $"video{ getName(videoId,3)}.{streamInfo.Container}"));

        }


        private void youtubeButton_Click(object sender, EventArgs e)
        {
            if ( !youtubeText.Text.ToLower().Contains("http://") && !youtubeText.Text.ToLower().Contains("https://"))
            {
                youtubeText.Text = "https://" + youtubeText.Text.Trim();
            }
            var uriName = youtubeText.Text;
            Uri uriResult;
            bool result = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!youtubeList.Items.Contains(uriName) && result)
            {
                youtubeList.Items.Add(uriName);
            }

            youtubeText.Text = "";
            youtubeText.Refresh();
            youtubeList.Refresh();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            if (this.youtubeList.SelectedIndex > -1)
            {
                this.youtubeList.Items.RemoveAt(this.youtubeList.SelectedIndex);
            }
           
        }

        private void button4_Click(object sender, EventArgs e)
        {

            bool youtubeDownloadBool = youtubeList.Items.Count > 0;

            progressBar2.Value = 10;
            progressBar2.Refresh();

            string ffmpegCommand = "";
            if (youtubeDownloadBool)
            {
                int videoId = 0;
                foreach (string s in youtubeList.Items)
                {
                    var t = Task<int>.Run(() => downloadFile(s, videoId));
                    t.Wait();
                    videoId++;
                }

                for (int i = 0; i < videoId; i++)
                {
                    ffmpegCommand = $"-i {Path.Combine(newFolderPathDecode,"video" + getName(i,3) + ".mp4")} -pix_fmt rgb24  -s {imageWidth}x{imageHeight} -sws_flags neighbor { Path.Combine(bilderPathDecode, $"filename{getName(i,3)}%10d.png")}";
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

                }


            } else
            {
                return;
            }
            




            progressBar2.Value = 50;
            progressBar2.Refresh();
          


             string[] imageFiles = Directory.GetFiles(bilderPathDecode, "*.png", SearchOption.TopDirectoryOnly);
            List<string[]> imageChunks = new List<string[]>();

            int loopIndex = 0;
            int lastStart = 0;
            foreach( string image in imageFiles)
            {   
                FileInfo fi = new FileInfo(image);
                if (fi.Length < 4096)
                {
                    string[] chunk = new string[loopIndex - lastStart + 1 ];
                    Array.Copy(imageFiles, lastStart, chunk, 0,loopIndex - lastStart + 1);
                    imageChunks.Add(chunk);
                    lastStart = loopIndex+1;


                }
                
                loopIndex++;
            }


            List<Task> waitTasks = new List<Task>();
            for(int i = 0; i < imageChunks.Count; i++)
            {
                int index = i;
                  var t = Task<int>.Run(() => TurnVideoToFile(imageChunks[index], index, bilderPathDecode, newFolderPathDecode));

            }
            foreach(Task t in waitTasks)
            {
                t.Wait();
            }




            progressBar2.Value = 100;
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
