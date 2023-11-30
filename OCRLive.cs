using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using Tesseract;

class Program
{
    static void Main()
    {
      
        int cameraIndex = 0;

     
        VideoCapture capture = new VideoCapture(cameraIndex);

 
        using (var engine = new TesseractEngine("./tessdata", "eng", EngineMode.Default))
        {

            while (true)
            {
               
                Mat frame = new Mat();
                capture.Read(frame);

                Mat grayImage = new Mat();
                CvInvoke.CvtColor(frame, grayImage, ColorConversion.Bgr2Gray);

           
                using (var page = engine.Process(grayImage.ToBitmap()))
                {
                    // Tanınan metni alır
                    string recognizedText = page.GetText();

                    // Tanınan metni işle ve konsola yazdırır
                    List<string> processedText = ProcessRecognizedText(recognizedText);
                    Console.WriteLine("Recognized Text:");
                    foreach (var line in processedText)
                    {
                        Console.WriteLine(line);
                    }
                }

         
                CvInvoke.Imshow("Live OCR", frame);

          
                if (CvInvoke.WaitKey(1) == 27)
                    break;
            }
        }

       
        capture.Dispose();
    }

    static List<string> ProcessRecognizedText(string recognizedText)
    {
        List<string> lines = new List<string>();

      
        string[] rawLines = recognizedText.Split('\n');

      
        foreach (var line in rawLines)
        {
            string cleanedLine = line.Trim();
            if (!string.IsNullOrWhiteSpace(cleanedLine))
            {
                lines.Add(cleanedLine);
            }
        }

        return lines;
    }
}
