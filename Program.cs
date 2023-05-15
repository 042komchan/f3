using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter the address of the input image data file:");
        string inputImageFilePath = Console.ReadLine();

        Console.WriteLine("Enter the address of the convolution kernel data file:");
        string kernelFilePath = Console.ReadLine();

        Console.WriteLine("Enter the address of the output image data file:");
        string outputImageFilePath = Console.ReadLine();

        // Step 2: Read input image data
        double[,] imageData = ReadImageDataFromFile(inputImageFilePath);

        // Step 3: Read convolution kernel data
        double[,] kernelData = ReadImageDataFromFile(kernelFilePath);

        // Step 4: Convolve the input image data with the convolution kernel
        double[,] convolvedImage = ConvolveImage(imageData, kernelData);

        // Step 5: Write resulting image data
        WriteImageDataToFile(outputImageFilePath, convolvedImage);

        Console.WriteLine("Image convolution completed. Result saved to output image file.");
    }

    static double[,] ReadImageDataFromFile(string imageDataFilePath)
    {
        string[] lines = System.IO.File.ReadAllLines(imageDataFilePath);
        int imageHeight = lines.Length;
        int imageWidth = lines[0].Split(',').Length;
        double[,] imageDataArray = new double[imageHeight, imageWidth];

        for (int i = 0; i < imageHeight; i++)
        {
            string[] items = lines[i].Split(',');
            for (int j = 0; j < imageWidth; j++)
            {
                imageDataArray[i, j] = double.Parse(items[j]);
            }
        }

        return imageDataArray;
    }

    static void WriteImageDataToFile(string imageDataFilePath, double[,] imageData)
    {
        int imageHeight = imageData.GetLength(0);
        int imageWidth = imageData.GetLength(1);
        string[] lines = new string[imageHeight];

        for (int i = 0; i < imageHeight; i++)
        {
            string[] items = new string[imageWidth];
            for (int j = 0; j < imageWidth; j++)
            {
                items[j] = imageData[i, j].ToString();
            }
            lines[i] = string.Join(",", items);
        }

        System.IO.File.WriteAllLines(imageDataFilePath, lines);
    }

    static double[,] ConvolveImage(double[,] imageData, double[,] kernelData)
    {
        int imageHeight = imageData.GetLength(0);
        int imageWidth = imageData.GetLength(1);
        int kernelSize = kernelData.GetLength(0);
        int kernelRadius = kernelSize / 2;

        double[,] convolvedImage = new double[imageHeight, imageWidth];

        for (int i = 0; i < imageHeight; i++)
        {
            for (int j = 0; j < imageWidth; j++)
            {
                double sum = 0.0;
                for (int k = -kernelRadius; k <= kernelRadius; k++)
                {
                    for (int l = -kernelRadius; l <= kernelRadius; l++)
                    {
                        int rowIndex = i + k;
                        int colIndex = j + l;

                        // Handle edge cases by using repeated texture
                        if (rowIndex < 0) rowIndex = 0;
                        if (colIndex < 0) colIndex = 0;
                        if (rowIndex >= imageHeight) rowIndex = imageHeight - 1;
                        if (colIndex >= imageWidth) colIndex = imageWidth - 1;

                        sum += imageData[rowIndex, colIndex] * kernelData[k + kernelRadius, l + kernelRadius];
                    }
                }
                 convolvedImage[i, j] = sum;
            }
        }

        return convolvedImage;
    }
}