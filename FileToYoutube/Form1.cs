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
using System.ComponentModel;


namespace FileToYoutube
{

    public partial class Form1 : Form
    {



        static QRCodeGenerator qrGenerator = new QRCodeGenerator();

        static Dictionary<int, int> myNames = new Dictionary<int, int>();
        static Dictionary<int, int[]> filesToRecover = new Dictionary<int, int[]>();

        private static void TurnVideoToFile(int threadIndex, string workPath)
        {

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

            if(sb1.Length > 0) {

                int buffer = 3;
                if(threadIndex > 999)
                {
                    buffer = threadIndex.ToString().Length;
                }

                File.WriteAllText(Path.Combine(workPath, "myZip.7z." + getName(threadIndex, buffer)), sb1.ToString(), Encoding.GetEncoding("ISO-8859-1"));
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
            int jumpWidth = 2953; //1744; //2953; //1273
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
                if (qrCodeImage.Width != imageWidth)
                {
                    Bitmap temp = new Bitmap(imageWidth, imageHeight);

                    using (Graphics gg = Graphics.FromImage(temp)) { 
                        gg.Clear(Color.White);
                        int top = (imageHeight - qrCodeImage.Height) / 2;
                        int left = (imageWidth - qrCodeImage.Width) / 2;
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
                    hugeBitmap.Save(Path.Combine(bilderPath, getName(threadIndex, 6) + getName(realNameIndex, 17) + ".png"), ImageFormat.Png);
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

            hugeBitmap.Save(Path.Combine(bilderPath, getName(threadIndex, 6) + getName(realNameIndex, 17) + ".png"), ImageFormat.Png);
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
        //  const string ffmpegPath = @"C:\Users\Alper\Desktop\ffmpeg-5.1.2-full_build\bin\ffmpeg.exe"; // das muss mit im Programm included sein
        const string ffmpegPath = @"ffmpeg.exe"; // das muss mit im Programm included sein
        const string sevenZipPath = @"7z.exe"; // das muss mit im Programm included sein
        const string ytdlpPath = @"yt-dlp_min.exe";
        const string par2Path = @"par2.exe";

        private void clearAllFields()
        {
            this.PasswordDecode.Text = "";
            this.PasswordEncode.Text = "";
            youtubeList.Items.Clear();
            textBox1.Text = "";
            this.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            toggleControls();


            backgroundWorker1.RunWorkerAsync();
         

        }

        static  Dictionary<int, string> fileExtension = new Dictionary<int, string>();


        private void downloadFile(string videoUrl, int videoId)
        {

            var stringdownloadFileCommand = $"-o \"{newFolderPathDecode}\\video{getName(videoId, 3)}.%(ext)s\" -f \"bestvideo[height>=720][ext=mp4]/bestvideo[height>=720]\" {videoUrl} --ignore-errors --update --no-overwrites --continue --verbose --no-check-certificate";

         
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ytdlpPath,
                    Arguments = stringdownloadFileCommand,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true

                }
            };


            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            // write output to console
            Console.WriteLine(output);
            process.WaitForExit();






            string[] fileNames = Directory.GetFiles(newFolderPathDecode, $"video{getName(videoId, 3)}.*");

            if(fileNames.Length < 1)
            {
                return;
            }



            lock (fileExtension) {
                fileExtension.Add(videoId, fileNames[0].Split('.')[fileNames[0].Split('.').Length-1]);
            }

        }


        private void youtubeButton_Click(object sender, EventArgs e)
        {

           
            if (checkBox1.Checked)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Youtube Files *.mp4|*.mp4|*.webm|*.webm";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    if (File.Exists(openFileDialog.FileName))
                    {
                        youtubeList.Items.Add(openFileDialog.FileName);
                    };

                }
               

            } else { 
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



        private void toggleControls()
        {
            this.button1.Enabled = !this.button1.Enabled;
            this.button3.Enabled = !this.button3.Enabled;
            this.button4.Enabled = !this.button4.Enabled;
            this.Select1.Enabled = !this.Select1.Enabled;
            this.Select2.Enabled = !this.Select2.Enabled;
            this.youtubeButton.Enabled = !this.youtubeButton.Enabled;
            this.videoHeightNumber.Enabled = !this.videoHeightNumber.Enabled;
            this.videoWidthNumber.Enabled = !this.videoWidthNumber.Enabled;
            this.fpsNumber.Enabled = !this.fpsNumber.Enabled;
            this.PasswordEncode.Enabled = !this.PasswordEncode.Enabled;
            this.PasswordDecode.Enabled = !this.PasswordDecode.Enabled;
            this.youtubeList.Enabled = !this.youtubeList.Enabled;
            this.startButton.Enabled = !this.startButton.Enabled;
            this.youtubeText.Enabled = !this.youtubeText.Enabled;
            this.button2.Enabled = !this.button2.Enabled;
            this.button5.Enabled = !this.button5.Enabled;
            this.keyButton.Enabled = !this.keyButton.Enabled;
            this.copyButton.Enabled = !this.copyButton.Enabled;
            this.checkBox1.Enabled = !this.checkBox1.Enabled;

        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (!Directory.Exists(newFolderPath) || !Directory.Exists(bilderPath) || !File.Exists(filePath))
            {
              
                return;
            }

            myNames.Clear();
          

            backgroundWorker1.ReportProgress(5);

         //   infoLabel.Text = "splitting Files...";

            // Start a thread that calls a parameterized instance method.



            int volumeSize = 0; // (int)volumeSizeNumber.Value;

            var file = File.OpenRead(filePath);
            var fileSize = file.Length;
            file.Close();

            volumeSize = (int)(fileSize / (100*Environment.ProcessorCount) / 1024);

           // volumeSize = 285; // by doing this we can void creating empty frames by fitting exactly one volume file into 1 image file containing 100 frames

            if (volumeSize < 285)
             {
                volumeSize = 285;
            }
            else if (volumeSize > 2850)
             {
                volumeSize = 2850; 
            }

            // The name of the 7-Zip executable


            string password = this.PasswordEncode.Text;

            if (password.Length > 0)
            {
                password = " -mhe=on -p\"" + password + '"';
            }

            // The command to run 7-Zip
            string sevenZipCommand = $"a -v{volumeSize}k -t7z " + '"' + Path.Combine(newFolderPath, Path.GetFileNameWithoutExtension(filePath)) + ".7z" + "\" \"" + filePath + $"\"{password}";

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
            backgroundWorker1.ReportProgress(10);
            Console.WriteLine("File split complete.");


            //----- create par2 files
            string par2Command = $"c -r10 -n1 x *.*";

            Process processX = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = par2Path,
                    Arguments = par2Command,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = newFolderPath
                }
            };
            processX.Start();
            var output = processX.StandardOutput.ReadToEnd();
            // write output to console
            Console.WriteLine(output);
            processX.WaitForExit();

            backgroundWorker1.ReportProgress(20);
            //--- create par2 files 

            string[] files = Directory.GetFiles(newFolderPath);

            Array.Sort(files, (a, b) => (a.Split('.')[0] == Path.Combine(newFolderPath, "x") || b.Split('.')[0] == Path.Combine(newFolderPath,"x")) ? -1: int.Parse(a.Split('.')[a.Split('.').Length - 1]) - int.Parse(b.Split('.')[b.Split('.').Length - 1])); // used to sort volumes by extension number

            // infoLabel.Text = "turning volumes into images...";

            float stepSize = 50 / files.Length + 20;

            int lastI = 0;
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < files.Length; i++)
            {

                int index = i;

                Thread t = new Thread(() => TurnFileToImages(File.ReadAllText(files[index], Encoding.GetEncoding("ISO-8859-1")), imageWidth, imageHeight, index, bilderPath));
                t.Start();
                threads.Add(t);

                if ((i % (Environment.ProcessorCount + 1) == 0))
                {     // if threads =
                    for (int ii = lastI + 1; ii < i + 1; ii++)
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
                    myStringBuilder.Append("file '" + getName(i, 6) + getName(ii, 17) + ".png" + "'\n");

                }

            }
            backgroundWorker1.ReportProgress(80);

            int Seperator = 60 * 60 * 11;  // 11 hours, youtube limit is: smaller than 12 hours, max 120GB, with the currect configuration 11H is about 17,6GB
            if (Seperator % (int)fpsNumber.Value > 0)
            {
                Seperator = (int)fpsNumber.Value - (Seperator % (int)fpsNumber.Value);

            }


            TimeSpan time = TimeSpan.FromSeconds(Seperator);

            string timeCut = time.ToString(@"hh\:mm\:ss");




            File.WriteAllText(Path.Combine(bilderPath, "WriteLines.txt"), myStringBuilder.ToString());

            // make video out of images for each volume

            string ffmpegCommand = $"-r {(int)fpsNumber.Value}/100 -f concat -safe 0  -i \"{Path.Combine(bilderPath, "WriteLines.txt")}\" -y -c:v libx264 -an -movflags faststart -bf 2 -crf 27 -s {(int)videoWidthNumber.Value}:{(int)videoHeightNumber.Value} -sws_flags neighbor -sws_dither none -map 0 -segment_time {timeCut} -f segment -reset_timestamps 1 -ignore_editlist 1 -filter:v {'"'}untile=10x10 , format=yuv420p{'"'} -force_key_frames {time.TotalSeconds} \"{Path.Combine(newFolderPath, "output%03d.mp4")}\"";


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

            backgroundWorker1.ReportProgress(100);


        }

        private void backgroundWorker1_ProgressChanged_1(object sender, ProgressChangedEventArgs e)
        {
            int value = e.ProgressPercentage;
            if (value > 100)
            {
                value = 100;
            }
            progressBar1.Value = value;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toggleControls();
            clearAllFields();
        //    backgroundWorker1.

        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            bool youtubeDownloadBool = youtubeList.Items.Count > 0;

            
            string ffmpegCommand = "";
            if (youtubeDownloadBool )
            {
                int videoId = 0;
                foreach (string s in youtubeList.Items)
                {
                    if (!checkBox1.Checked) { 
                        try
                        {
                            var t = Task<int>.Run(() => downloadFile(s, videoId));
                            t.Wait();
                        }
                        catch
                        {
                            MessageBox.Show("Link is not a Youtube link or video is private or processing is not finished.", "Error", 0, MessageBoxIcon.Error);
                            return;
                        }
                    } else
                    {
                        fileExtension.Add(videoId, s.Split('.')[1]);

                    }

                    backgroundWorker2.ReportProgress(videoId);
                    if (fileExtension.Count > videoId)
                    {
                        
                        if(File.Exists($"{ Path.Combine(newFolderPathDecode, "video" + getName(videoId, 3) + $".{fileExtension[videoId]}").Replace("\\", "/")}") || checkBox1.Checked)
                        {
                            videoId++;
                        } else
                        {
                            break;
                        }
                       
                    }

                }

                if (videoId < youtubeList.Items.Count)
                {
                    MessageBox.Show($"One or more videos could not be downloaded!{Environment.NewLine}Make sure that the video processing is completly done!", "Error", 0, MessageBoxIcon.Error);
                    return;
                }
                // if (videoId > 1) {

                StringBuilder sbc = new StringBuilder();
                for (int i = 0; i < videoId; i++)
                {
                    if (checkBox1.Checked)
                    {
                        sbc.Append("file '" + ((string)youtubeList.Items[i]).Replace("\\", "/") + "'"+ System.Environment.NewLine);
                    } else
                    {
                        sbc.Append($"file '{ Path.Combine(newFolderPathDecode, "video" + getName(i, 3) + $".{fileExtension[i]}").Replace("\\", "/")}'{System.Environment.NewLine}");
                    }
                  
                    // string tempString = $"File {Path.Combine(newFolderPathDecode, "video" + getName(videoId, 3) + ".mp4")}{System.Environment.NewLine}File {Path.Combine(bilderPathDecode, "EmptyFrame.png")}";



                }
                File.WriteAllText(Path.Combine(bilderPathDecode, "write.txt"), sbc.ToString());

                backgroundWorker2.ReportProgress(50);
               //   ffmpegCommand = $"-f concat -safe 0 -i \"{Path.Combine(bilderPathDecode, "write.txt")}\" -pix_fmt rgb24 -filter_complex \"tile=10x10\" -s {imageWidth * 10}x{imageHeight * 10}  -sws_flags neighbor \"{ Path.Combine(bilderPathDecode, $"filename%10d.png")}\"";

                ffmpegCommand = $"-f concat -safe 0 -i \"{Path.Combine(bilderPathDecode, "write.txt")}\" -pix_fmt pal8 -filter_complex \"tile=10x10\" -s {imageWidth * 10}x{imageHeight * 10}  -sws_flags neighbor -sws_dither none \"{ Path.Combine(bilderPathDecode, $"filename%10d.png")}\"";
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
            backgroundWorker2.ReportProgress(70);


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

                if(index % (Environment.ProcessorCount+1) == 0)
                {
                    t.Wait();
                }

            }


            foreach (Task t in tasks)
            {
                t.Wait();
            }

            backgroundWorker2.ReportProgress(80);
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

            backgroundWorker2.ReportProgress(90);

            string password = this.PasswordDecode.Text;

            if (password.Length > 0)
            {
                password = " -p\"" + password + '"';
            }

            // The command to run 7-Zip
            string sevenZipCommand = $"x -spf -y -o\"{newFolderPathDecode}\" \"{Path.Combine(newFolderPathDecode, "myZip")}.7z.*\"{password}";



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

            backgroundWorker2.ReportProgress(100);


        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int value = e.ProgressPercentage;
            if (value > 100)
            {
                value = 100;
            }
            progressBar2.Value = value;
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar2.Value = 100;
            this.Refresh();
            toggleControls();
            clearAllFields();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(newFolderPathDecode))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = newFolderPathDecode,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show(string.Format("{0} Directory does not exist!", newFolderPathDecode));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {



            if (Directory.Exists(newFolderPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = newFolderPath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show(string.Format("{0} Directory does not exist!", newFolderPath));
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            youtubeList.Items.Clear();
            if (checkBox1.Checked)
            {
                youtubeText.Enabled = false;
                youtubeGroup.Text = "Step 1: Add video files (first: part 1, second: part 2)";
            } else
            {
                youtubeText.Enabled = true;
                youtubeGroup.Text = "Step 1: Youtube links in order(first: part 1, second: part 2 and so on)";
            }
           

        }

        private void keyButton_Click(object sender, EventArgs e)
        {
            PasswordEncode.Text = Guid.NewGuid().ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(PasswordEncode.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            filesToRecover.Clear();
            fileExtension.Clear();
            fileMapping.Clear();

            toggleControls();
            backgroundWorker2.RunWorkerAsync();

           
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
                string[] testVar = Directory.GetFiles(folderBrowserDialog.SelectedPath, "*", SearchOption.AllDirectories);

                if (1 > testVar.Length)
                {
                    string folderPath = folderBrowserDialog.SelectedPath;
                    Console.WriteLine("Selected folder: " + folderPath);

                    //string folderName = "Work";
                    //    newFolderPath = Path.Combine(folderPath, folderName);
                    newFolderPath = folderPath;
                    // if (!Directory.Exists(newFolderPath))
                    // {
                    Directory.CreateDirectory(newFolderPath);
                    bilderPath = Path.Combine(newFolderPath, "Images");
                    Directory.CreateDirectory(bilderPath);
                    Console.WriteLine("Folder created: " + newFolderPath);
                    //   }
                    //   else
                    //   {
                    //     Console.WriteLine("Folder already exists: " + newFolderPath);
                    //   }
                    textBox2.Text = newFolderPath;
                    textBox2.Refresh();
                }
                else
                {
                    MessageBox.Show("Please select an empty folder!", "Error", 0, MessageBoxIcon.Error);
                    textBox2.Text = "";
                    textBox2.Refresh();
                }
            }

        
        }


        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

           

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK )
            {
                

                string[] testVar = Directory.GetFiles( folderBrowserDialog.SelectedPath, "*", SearchOption.AllDirectories);
                
                if (1 > testVar.Length) { 
                string folderPath = folderBrowserDialog.SelectedPath;
                Console.WriteLine("Selected folder: " + folderPath);

                //  string folderName = "Work";
                //  newFolderPathDecode = Path.Combine(folderPath, folderName);
                newFolderPathDecode = folderPath;
               // if (!Directory.Exists(newFolderPathDecode))
               // {
                    Directory.CreateDirectory(newFolderPathDecode);
                    bilderPathDecode = Path.Combine(newFolderPathDecode, "Images");
                    Directory.CreateDirectory(bilderPathDecode);
                    Console.WriteLine("Folder created: " + newFolderPathDecode);
                // }
                // else
                // {
                //    Console.WriteLine("Folder already exists: " + newFolderPathDecode);
                //}

                
                textBox4.Text = newFolderPathDecode;
                textBox4.Refresh();
                }
                else
                {
                    MessageBox.Show("Please select an empty folder!", "Error", 0,MessageBoxIcon.Error);
                    textBox4.Text = "";
                    textBox4.Refresh();
                }
            } 

        }

    }
}
