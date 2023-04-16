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
        static Dictionary<int, int[]> filesToRecover = new Dictionary<int, int[]>();

        private static void TurnVideoToFile(int threadIndex, string workPath)
        {

            Console.WriteLine(threadIndex);
            StringBuilder sb1 = new StringBuilder();

            QRDecoder Decoder = new QRDecoder();
            byte[][] ResultArray;

            int test = 0;
            
            for (int z = filesToRecover[threadIndex][0]; z <= filesToRecover[threadIndex][2]; z++) // main Images
            {
                using (Image large = Bitmap.FromFile(imageFiles[z]))
                {

                    using (Image small = new Bitmap(large.Width / 10, large.Height / 10))
                    {
                        using (Graphics g = Graphics.FromImage(small))
                        {


                            int endeY = 9;
                            int anfangY = 0;

                            if (z == filesToRecover[threadIndex][0])
                            {
                                anfangY = ((filesToRecover[threadIndex][1] - filesToRecover[threadIndex][1] % 10) / 10);
             
                            }

                            if (z == filesToRecover[threadIndex][2])
                            {
                                endeY = ((filesToRecover[threadIndex][3] - filesToRecover[threadIndex][3] % 10) / 10);
                             
                            }
                            

                            for (int ii = anfangY; ii < endeY+1; ii++)
                            {
                                int endeX = 9;
                                int anfangX = 0;
                                if (z == filesToRecover[threadIndex][0] && ii == anfangY)
                                {
                                    anfangX = filesToRecover[threadIndex][1] % 10;
                                }

                                if (z == filesToRecover[threadIndex][2] && ii == endeY)
                                {
                                    endeX = filesToRecover[threadIndex][3] % 10;
                                }

                                for (int i = anfangX; i < endeX+1; i++)
                                {
                                    
                                    g.DrawImage(large, new Rectangle(0, 0, small.Width, small.Height), new Rectangle(i * small.Width, ii * small.Height, small.Width, small.Height), GraphicsUnit.Pixel);
                                    ResultArray = Decoder.ImageDecoder((Bitmap)small);

                                    if (ResultArray == null)
                                    {
                                       
                                        continue;
                                    }
                                    foreach (char c in ResultArray[0])
                                    {
                                        sb1.Append(c);
                                    }                   }
                            }

                        }
                    }
                }
            }
            Console.WriteLine(test);
            if(sb1.Length > 0) {

                int buffer = 3;
                if(threadIndex > 999)
                {
                    buffer = threadIndex.ToString().Length;
                }

                File.WriteAllText(Path.Combine(workPath, "myZip.zip." + getName(threadIndex, buffer)), sb1.ToString(), Encoding.GetEncoding("ISO-8859-1"));
                sb1.Clear();
            }

        }

        static void TurnFileToImages(string binaryFile, int imageWidth, int imageHeight, int threadIndex, string bilderPath)
        {


            Bitmap hugeBitmap = new Bitmap(imageWidth * 10, imageHeight * 10);
            Graphics g = Graphics.FromImage(hugeBitmap);

            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                using (Graphics gg = Graphics.FromImage(hugeBitmap))
                {

                    gg.FillRectangle(brush, 0, 0, imageWidth * 10, imageHeight * 10);
                }
            }


            int width = imageWidth;
            int height = imageHeight;

            int counter = 0;
            int realNameIndex = 0;
            int jumpWidth = 2953; //1273
            int x = 0;
            int y = 0;

            Bitmap qrCodeImage = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            for (int i = 0; i < binaryFile.Length; i += jumpWidth)
            {
                if (i + jumpWidth > binaryFile.Length)
                {
                    jumpWidth = binaryFile.Length - i;
                }

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(binaryFile.Substring(i, jumpWidth), QRCodeGenerator.ECCLevel.L);
                QRCode qrCode = new QRCode(qrCodeData);
                qrCodeImage = qrCode.GetGraphic(1);
                if (qrCodeImage.Width != 185)
                {
                    Bitmap temp = new Bitmap(185, 185);

                    using (Graphics gg = Graphics.FromImage(temp)) { 
                        gg.Clear(Color.White);
                        int top = (185 - qrCodeImage.Height) / 2;
                        int left = (185 - qrCodeImage.Width) / 2;
                        gg.DrawImage(qrCodeImage, left, top);
                    }
                    qrCodeImage = temp;




                }
    
                g.DrawImage(qrCodeImage, x*imageWidth, y*imageHeight);
              //  qrCodeImage.Save(Path.Combine(bilderPath, $@"{Guid.NewGuid()}.png"));         

                x++;
                if(x >= 10)
                {
                    x = 0;
                    y++;
                }
                counter++;
                if (counter % 100 == 0)
                {
                    hugeBitmap.Save(Path.Combine(bilderPath, getName(threadIndex, 3) + getName(realNameIndex, 20) + ".png"), ImageFormat.Png);
                    y = 0;
                    x = 0;
                    hugeBitmap = new Bitmap(imageWidth * 10, imageHeight * 10);
                    g = Graphics.FromImage(hugeBitmap);
                    using (SolidBrush brush = new SolidBrush(Color.Black))
                    {
                        using (Graphics gg = Graphics.FromImage(hugeBitmap))
                        {

                            gg.FillRectangle(brush, 0, 0, imageWidth * 10, imageHeight * 10);
                        }
                    }
                    realNameIndex++;
                }
             
            }

            Color woRemoveColor = Color.FromArgb(255, 255, 255);
            Bitmap bitmap2 = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            using (Graphics gfx = Graphics.FromImage(bitmap2))
            using (SolidBrush brush = new SolidBrush(woRemoveColor))
            {
                gfx.FillRectangle(brush, 0, 0, width, height);
            }

            g.DrawImage(bitmap2, x*imageWidth, y*imageHeight);
            bitmap2.Dispose();

            hugeBitmap.Save(Path.Combine(bilderPath, getName(threadIndex, 3) + getName(realNameIndex, 20) + ".png"), ImageFormat.Png);
            hugeBitmap.Dispose();
            qrCodeImage.Dispose();
            realNameIndex++;

            lock (myNames)
            {
                myNames.Add(threadIndex, realNameIndex);
            }

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

             volumeSize = (int)(fileSize / ((Environment.ProcessorCount)*16) / 1024);

             if (volumeSize < 100 ) 
             {
                 volumeSize = 100;
             }
             else if (volumeSize > 50000)
             {
                 volumeSize = 50000; // 12 core cpu would mean 12*50 MB so 600*3 = 1200 MB ram at least to put the images back into files, 3x because of bad code :)
             }

             // The name of the 7-Zip executable
             string sevenZipPath = @"C:\Program Files\7-Zip\7z.exe"; // das muss mit im Programm included sein

            string password = this.PasswortEncode.Text;

            if(password.Length > 0)
            {
                password = " -p\"" + password + '"';
            }

             // The command to run 7-Zip
             string sevenZipCommand = $"a -v{volumeSize}k -tzip " + '"' + Path.Combine(newFolderPath, Path.GetFileNameWithoutExtension(filePath)) + ".zip" + "\" \"" + filePath + $"\"{password}";

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
           

            int lastI = 0;
             List<Thread> threads = new List<Thread>();
             for (int i = 0; i < files.Length; i++)
             {

                 int index = i;

                 Thread t = new Thread(() => TurnFileToImages(File.ReadAllText(files[index], Encoding.GetEncoding("ISO-8859-1")), imageWidth, imageHeight, index, bilderPath));
                 t.Start();
                 threads.Add(t);

                if((i % (Environment.ProcessorCount+1) == 0)){     // if threads =
                    for (int ii = lastI+1; ii < i+1;ii++)
                    {
                        threads[ii].Join();
                    }

                        lastI = i;
                }
                 // }

             }
             foreach (Thread t in threads)
             {
                 t.Join();
             }


           StringBuilder myStringBuilder = new StringBuilder();


             int totalFrameCount = 0;

             for (int i = 0; i < myNames.Count; i++)
             {
                 for (int ii = 0; ii < myNames[i]; ii++)
                 {
                     totalFrameCount += 100;
                     myStringBuilder.Append("file '" + getName(i, 3) + getName(ii, 20) + ".png" + "'\n");

                 }

             }



           /*  int ExtraFrames = 0;


             if ((totalFrameCount) % (int)fpsNumber.Value > 0)
             {
                 ExtraFrames = (int)fpsNumber.Value - (totalFrameCount % (int)fpsNumber.Value);
             }

             for(int i = 0; i < ExtraFrames; i++)
             {

                 using (Bitmap tempBitmap = new Bitmap(imageWidth*10,imageHeight * 10)) {
                     using (Graphics gfx = Graphics.FromImage(tempBitmap))
                     using (SolidBrush brush = new SolidBrush(Color.Black))
                     {
                         gfx.FillRectangle(brush, 0, 0, tempBitmap.Width, tempBitmap.Height);
                     }
                     tempBitmap.Save(Path.Combine(bilderPath, $"ExtraFrame{i}.png"));
                 }
                 myStringBuilder.Append("file '" + $"ExtraFrame{i}.png" + "'\n");
             }
            */

            int Seperator = 60 * 60 * 11;  // 11 hours, youtube limit is: smaller than 12 hours, max 120GB, with the currect configuration 11H is about 17,6GB
            if(Seperator % (int)fpsNumber.Value > 0)
            {
                Seperator = (int)fpsNumber.Value - (Seperator % (int)fpsNumber.Value);

            }


            TimeSpan time = TimeSpan.FromSeconds(Seperator);

            string timeCut = time.ToString(@"hh\:mm\:ss");




            File.WriteAllText(Path.Combine(bilderPath, "WriteLines.txt"), myStringBuilder.ToString());

            progressBar1.Value = 45;
            progressBar1.Refresh();
            // make video out of images for each volume






            // string ffmpegCommand = $"-r {(int)fpsNumber.Value} -f concat -safe 0  -i {Path.Combine(bilderPath, "WriteLines.txt")} -c:v  libx264rgb -preset faster -crf 0 -s {(int)videoWidthNumber.Value}:{(int)videoHeightNumber.Value} -sws_flags neighbor  {Path.Combine(newFolderPath, "black.mp4")}";
            // string ffmpegCommand = $"-r {(int)fpsNumber.Value} -f concat -safe 0  -i {Path.Combine(bilderPath, "WriteLines.txt")} -c:v  libx264rgb -preset faster -crf 0 -s {(int)videoWidthNumber.Value}:{(int)videoHeightNumber.Value} -sws_flags neighbor {Path.Combine(newFolderPath, "black.mp4")}";
            string ffmpegCommand = $"-r {(int)fpsNumber.Value}/100 -f concat -safe 0  -i {Path.Combine(bilderPath, "WriteLines.txt")} -y -c:v libx264 -an -preset faster -movflags faststart -bf 2 -crf 30 -s {(int)videoWidthNumber.Value}:{(int)videoHeightNumber.Value} -sws_flags neighbor -map 0 -segment_time {timeCut} -f segment -reset_timestamps 1 -ignore_editlist 1 -filter:v {'"'}untile=10x10 , format=yuv420p{'"'} -force_key_frames {time.TotalSeconds} {Path.Combine(newFolderPath, "output%03d.mp4")}";
           // string ffmpegCommand = $"-r {(int)fpsNumber.Value} -f concat -safe 0  -i {Path.Combine(bilderPath, "WriteLines.txt")} -c:v libx264 -preset faster -movflags faststart -vf format=yuv420p -bf 2 -crf 25 -s {(int)videoWidthNumber.Value}:{(int)videoHeightNumber.Value} -sws_flags neighbor -map 0 -segment_time 06:00:00 -f segment -reset_timestamps 1 -ignore_editlist 1 {Path.Combine(newFolderPath, "output%03d.mp4")}";

          //  ffmpeg - i filename0000006563.png -y -filter:v untile = 10x10 out.mp4

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

        static  Dictionary<int, string> fileExtension = new Dictionary<int, string>();

        static YoutubeClient youtube = new YoutubeClient();

        private async Task downloadFile(string videoUrl, int videoId)
        {

            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoUrl);
            // ...or highest quality MP4 video-only stream
            var streamInfo = streamManifest
             .GetVideoOnlyStreams().TryGetWithHighestVideoQuality();

            if(streamInfo == null)
            {
                return;
            }
            var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

            lock(fileExtension){
                fileExtension.Add(videoId, streamInfo.Container.Name);


            }

            await youtube.Videos.Streams.DownloadAsync(streamInfo, Path.Combine(newFolderPathDecode, $"video{ getName(videoId, 3)}.{streamInfo.Container}"));

        }


        private void youtubeButton_Click(object sender, EventArgs e)
        {
            if (!youtubeText.Text.ToLower().Contains("http://") && !youtubeText.Text.ToLower().Contains("https://"))
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

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine((10000).ToString().Length);
            return;

            QRDecoder Decoder = new QRDecoder();


            using (Image large = Bitmap.FromFile(@"F:/TestTile/Work/Images/filename0000000000001.png"))
            {
                using (Image small = new Bitmap(large.Width / 10, large.Height / 10))
                {
                    using (Graphics g = Graphics.FromImage(small))
                    {
                        int test = 0;
                        for (int ii = 0; ii < 10; ii++)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                g.DrawImage(large, new Rectangle(0, 0, small.Width, small.Height), new Rectangle(i * small.Width, ii * small.Height, small.Width, small.Height), GraphicsUnit.Pixel);


                                byte[][] ResultArray = Decoder.ImageDecoder((Bitmap)small);
                                if (ResultArray[0] != null)
                                {
                                    test++;
                                }

                            }
                        }
                        Console.WriteLine(test);

                    }
                }
            }



        }



        static Dictionary<int, List<int[]>> fileMapping = new Dictionary<int, List<int[]>>();
        private static void findWhite(int startImage, int endImage, int threadIndex)  // this method finds the white images to be able to split the imagefiles 
        {
            Bitmap b;

            bool isWhite = true;
            List<int[]> mappings = new List<int[]>();

            int h = 0;

            for (int x = startImage; x <= endImage; x++)
            {

                b = new Bitmap(imageFiles[x]);
                h = 0;
                for (int i = (b.Height / 10) / 2; i < b.Height; i = i + b.Height / 10) // go to middle of images
                {

                    for (int ii = 0; ii < 10; ii++)  // scan each of the 10 images
                    {
                        isWhite = true;
                        for (int iii = ii * (b.Width / 10); iii < (ii + 1) * (b.Width / 10) && isWhite; iii++) // scan each pixel in the line
                        {

                            if (((b.GetPixel(iii, i).R + b.GetPixel(iii, i).B + b.GetPixel(iii, i).G)) / 3 < 200)
                            {
                                isWhite = false;

                            }
                        }
                        if (isWhite)
                        {
                            int[] temp = { x, h * 10 + ii };
                            mappings.Add(temp);
                        }


                    }
                    h += 1;
                }
                b.Dispose();

            }

            lock (fileMapping)
            {
                if (mappings.Count < 1)
                {
                    int[] temp = { -1, -1 };
                    mappings.Add(temp);
                }
                fileMapping.Add(threadIndex, mappings);
            }

        }



        static string[] imageFiles;
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
                    if (fileExtension.Count > videoId )
                    {
                        videoId++;
                    }

                }

                if(videoId < 1)
                {
                    return;
                }
               // if (videoId > 1) {

                    StringBuilder sbc = new StringBuilder();
                    for (int i = 0; i < videoId; i++)
                    {
                        sbc.Append($"file { Path.Combine(newFolderPathDecode, "video" + getName(i, 3) + $".{fileExtension[i]}").Replace("\\","/")}{System.Environment.NewLine}");
                       // string tempString = $"File {Path.Combine(newFolderPathDecode, "video" + getName(videoId, 3) + ".mp4")}{System.Environment.NewLine}File {Path.Combine(bilderPathDecode, "EmptyFrame.png")}";
                        


                    }
                    File.WriteAllText(Path.Combine(bilderPathDecode, "write.txt"), sbc.ToString());

                    /*  ffmpegCommand = $"-f concat -safe 0 -i {Path.Combine(bilderPathDecode, "write.txt")} -c copy {Path.Combine(newFolderPathDecode, "merge.mp4")}";
                      //  ffmpegCommand = $"-i {Path.Combine(newFolderPathDecode,"video" + getName(i,3) + ".mp4")} -pix_fmt rgb24  -s {imageWidth}x{imageHeight} -sws_flags neighbor { Path.Combine(bilderPathDecode, $"filename{getName(i,3)}%10d.png")}";
                      Process process3 = new Process
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
                      //-f concat -safe 0 -i {Path.Combine(bilderPathDecode, "write.txt")} -c copy
                      process3.Start();
                      process3.WaitForExit();*/
                    //for(int i = 0; i < videoId; i++)
                    //{
                    //  File.Delete(Path.Combine(newFolderPathDecode, "video" + getName(i, 3) + ".mp4"));
                    // }

                    // File.Move(Path.Combine(newFolderPathDecode, "merge.mp4"), Path.Combine(newFolderPathDecode, "video" + getName(0, 3) + ".mp4"));

               // }
                //    for (int i = 0; i < videoId; i++)
                //    {

                ffmpegCommand = $"-f concat -safe 0 -i {Path.Combine(bilderPathDecode, "write.txt")} -pix_fmt rgb24 -filter_complex \"tile=10x10\" -s {imageWidth * 10}x{imageHeight * 10}  -sws_flags neighbor { Path.Combine(bilderPathDecode, $"filename%10d.png")}";
               // ffmpegCommand = $" -i {Path.Combine(newFolderPathDecode, "video" + getName(0, 3) + ".mp4")} -pix_fmt rgb24  -filter_complex \"tile=10x10\" -s {imageWidth * 10}x{imageHeight * 10}  -sws_flags neighbor { Path.Combine(bilderPathDecode, $"filename%10d.png")}";
                    //  ffmpegCommand = $"-i {Path.Combine(newFolderPathDecode,"video" + getName(i,3) + ".mp4")} -pix_fmt rgb24  -s {imageWidth}x{imageHeight} -sws_flags neighbor { Path.Combine(bilderPathDecode, $"filename{getName(i,3)}%10d.png")}";
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

           //     }

            } 
        
            else
            {
                return;
            }


            progressBar2.Value = 50;
            progressBar2.Refresh();



            imageFiles = Directory.GetFiles(bilderPathDecode, "*.png", SearchOption.TopDirectoryOnly);

            int threadsToRun = Environment.ProcessorCount + 1;

            int imageChunks = (int)Math.Ceiling((double)(imageFiles.Length / threadsToRun));
            if (imageFiles.Length < threadsToRun)
            {
                threadsToRun = imageFiles.Length;
            }

            //  int lastIndex = 0;
            List<Task> tasks = new List<Task>();
            int xd = 0;
            bool weiter = true;
            for (int i = 0; i < imageFiles.Length && weiter; i = i + imageChunks + 1, xd++)
            {
                int temp = i + imageChunks;
                int temp2 = i;
                int temp3 = xd;
                if (temp > imageFiles.Length - 1)
                {
                    temp = imageFiles.Length - 1;
                    weiter = false;
                }
                //    int temp = lastIndex + imageChunks;
                int index = i;
                var t = Task<int>.Run(() => findWhite(temp2, temp, temp3));
                tasks.Add(t);

            }


            foreach (Task t in tasks)
            {
                t.Wait();
            }


            // now we can create a chunk list -> startPos 


            int fileIndex = 0;
            int lastEndImage = 0;
            int lastEndIndex = 0;
            for (int i = 0; i < fileMapping.Count; i++)
            {
                if (fileMapping[i][0][0] > -1)
                {
                    for (int ii = 0; ii < fileMapping[i].Count; ii++)
                    {

                        int tempStartImage = 0;
                        int tempStartIndex = 0;
                        if (lastEndIndex > 0)
                        {
                            tempStartIndex = lastEndIndex + 1;
                            if (tempStartIndex > 100) 
                            {
                                tempStartIndex = 0;
                                tempStartImage += 1;
                            }
                            else
                            {
                                tempStartImage = lastEndImage;
                            }

                        }



                        int[] temp = { tempStartImage, tempStartIndex, fileMapping[i][ii][0], fileMapping[i][ii][1] };
                        lastEndImage = fileMapping[i][ii][0];
                        lastEndIndex = fileMapping[i][ii][1];
                        filesToRecover.Add(fileIndex, temp);
                        fileIndex++;
                    }
                }
            }

            fileMapping.Clear();




            /*List<string[]> imageChunks = new List<string[]>();

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
               */
            List<Task> waitTasks = new List<Task>();
            for (int i = 0; i < filesToRecover.Count; i++)
            {
                int index = i;
                var t = Task<int>.Run(() => TurnVideoToFile(index, newFolderPathDecode));
                waitTasks.Add(t);

            }
            foreach (Task t in waitTasks)
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
