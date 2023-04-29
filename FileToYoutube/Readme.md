# FileToYoutube!

**FileToYoutube** is an app that is meant as a concept to store any file you wish to YouTube as an archive.

  

This repository is meant as a way for people to test it out. I believe this should not be allowed and Youtube should detect these kinds of videos.

As you will see the coding is not very good, but I wanted to make sure people can trust the code.

I don't want people to waste a lot of electricity for nothing.  Try to keep files small. <3  Also YouTube will throttle your download speed if you download too much data in a small period of time.

![encoding ubuntu 22.04](https://github.com/Keksmania/FileToYoutube/blob/d191b0677aaac9baeb045b76ff12b334729fe81a/FileToYoutube/ImageReadme.png?raw=true)  

WARNING: This app must not be used to distribute illegal files or shouldn’t be used to overwhelm YouTube servers. The TOS of YouTube clearly states uploading auto-generated videos is not allowed and your YouTube account could be terminated!

  

## Some facts:

- The ratio to file to encoded video is between 1:15 and 1:19

- The ratio to zipped file to downloaded video is 1:14

- The total size you will need on your hard drive for encoding is about 23.5x the size of your file (19x for the video, 3.5x for generated images, and 1x for the archive volumes)

- The total size you will need on your hard drive for decoding is about 19x the size of your file (14x for the video, 4x for the generated images, and 1x for the archive volumes)
  
- You will need about 5 hours to encode a 1 gb file on a 10th gen i5 CPU with 6 hardware cores

- Decoding is fast and the downloading of the video will depend on your internet speed



If that still sounds interesting here we go:

## Requirements

- Windows 7 or higher (was only tested on Windows 11)

- .Net 4.8 runtime

- Enough hard drive space


## How to use:

  

1. select file to encode.

  

2. select work folder (must be empty)

  

3. video settings can be left unchanged. Videos shouldn’t be under 720p since the YouTube compression will make it impossible to recover files. 
  
The frame rate was tested with 6, 24, 30

It is possible that certain frame rates won’t work since YouTube will add frames and remove frames to match a favored frame rate.

  

4. USE A KEY/PASSWORD. It is optional, but PLEASE use a key with a length of at least 9 characters. If your video is unlisted anybody could download your file and decode it.

  

5. Click start and wait for the process to finish. Then you can click “open path” to see the location where the video was created. Now you can upload that video to YouTube.

  

6. Wait for the processing to finish. You can verify it by looking if the 720p option has appeared in YouTube.

  

Since YouTube is only allowing videos under 12h the application will split video into 11h chunks. You can upload each file separately to YouTube. The application can use multiple YouTube links to decode your file. This shouldn’t be necessary since you can get a lot of data into an 11h video.

  

7. After the processing is finished you can get your files by adding the YouTube link to the application. Choose a work folder. Use the decoder key and then click start.

  

There is an option where you can add the video files manually (by downloading a private video for example)

Attention: "Youtube Short" videos that are directly downloaded form YouTube manually will have a water mark and won't work. Use the automatic downloader by adding the URL to the software to download the video without watermark

  

 

### This application works in Windows and uses different libraries and applications…

## Many thanks to:

  

- [YoutubeExplode](https://github.com/Tyrrrz/YoutubeExplode) (used to download videos from YouTube)

- [QrCoder](https://github.com/codebude/QRCoder) (used to encode bytes into qr codes)

- [QrCodeDecoderLibrary](https://github.com/Uzi-Granot/QRCode) (used to decode qr codes)

- [7zip](https://github.com/mcmilk/7-Zip) (used to zip files into volumes before turning them into qr codes)

- [Ffmpeg](https://github.com/FFmpeg/FFmpeg) (used to create a video out of images and the reverse)

  


Copyright © 2023 Keksmania

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.