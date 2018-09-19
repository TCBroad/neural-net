namespace NeuralNet.Mnist
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class MnistReader
    {
        private const string TrainImages = "train-images-idx3-ubyte";
        private const string TrainLabels = "train-labels-idx1-ubyte";
        private const string TestImages = "t10k-images-idx3-ubyte";
        private const string TestLabels = "t10k-labels-idx1-ubyte";

        public static IEnumerable<Image> ReadTrainingData()
        {
            foreach (var item in Read(TrainImages, TrainLabels))
            {
                yield return item;
            }
        }

        public static IEnumerable<Image> ReadTestData()
        {
            foreach (var item in Read(TestImages, TestLabels))
            {
                yield return item;
            }
        }

        private static IEnumerable<Image> Read(string imagesPath, string labelsPath)
        {
            var labels = new BinaryReader(new FileStream(labelsPath, FileMode.Open));
            var images = new BinaryReader(new FileStream(imagesPath, FileMode.Open));

            var magicNumber = images.ReadBigInt32();
            var numberOfImages = images.ReadBigInt32();
            var width = images.ReadBigInt32();
            var height = images.ReadBigInt32();

            var magicLabel = labels.ReadBigInt32();
            var numberOfLabels = labels.ReadBigInt32();

            for (var i = 0; i < numberOfImages; i++)
            {
                var bytes = images.ReadBytes(width * height);
                // var arr = new byte[height, width];

                //arr.ForEach((j,k) => arr[j, k] = bytes[j * height + k]);

                yield return new Image
                {
                    Data = bytes,
                    Label = labels.ReadByte()
                };
            }
        }
    }

    public class Image
    {
        public byte Label { get; set; }
        public byte[,] Data2d { get; set; }
        public byte[] Data { get; set; }
    }

    public static class Extensions
    {
        public static int ReadBigInt32(this BinaryReader br)
        {
            var bytes = br.ReadBytes(sizeof(int));
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToInt32(bytes, 0);
        }

        public static void ForEach<T>(this T[,] source, Action<int, int> action)
        {
            for (var w = 0; w < source.GetLength(0); w++)
            {
                for (var h = 0; h < source.GetLength(1); h++)
                {
                    action(w, h);
                }
            }
        }
    }
}