namespace NeuralNet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    class Program
    {
        private const int NodeWidth = 14;
        private const int NodeHeight = 6;

        private static Dictionary<double[], double[]> TrainingData = new Dictionary<double[], double[]>();

        static void Main(string[] args)
        {
            var nn = new NeuralNetwork(10, new[] { 2, 4, 2 });

            TrainingData.Add(new[] { 0d, 1d }, new[] { 1d, 0d });
            TrainingData.Add(new[] { 1d, 0d }, new[] { 0d, 1d });

            for (var i = 0; i < 100; i++)
            {
                foreach (var data in TrainingData)
                {
                    nn.Train(data.Key, data.Value);

                    Display(nn);
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                Console.Write("Input: ");
                var input = Console.ReadLine();
                if (input == "q")
                {
                    break;
                }

                var nnData = input.Split(" ").Select(double.Parse).ToList();

                var output = nn.Run(nnData);

                Console.WriteLine($"Output: [{string.Join(", ", output)}]");
            }
        }

        private static void Display(NeuralNetwork nn)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);

            Console.WriteLine($"Run: {nn.Runs:N0} | Training iterations: {nn.TrainingIterations:N0}");

            for (var i = 0; i < nn.NumLayers; i++)
            {
                if (i == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else if (i == nn.NumLayers - 1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }

                var layer = nn.Layers[i];
                for (var j = 0; j < layer.Size; j++)
                {
                    var x = j * NodeWidth + 1;
                    var y = i * NodeHeight + 2;

                    DrawNode(layer.Neurons[j], x, y);
                }
            }
        }

        private static void DrawNode(Neuron n, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine(string.Join("", Enumerable.Repeat("-", NodeWidth - 1)) + " ");
            Console.SetCursorPosition(x, ++y);
            Console.WriteLine($"| v:{n.Value:N4}{(n.Value < 0 ? " " : "  ")}| ");
            Console.SetCursorPosition(x, ++y);
            Console.WriteLine($"| b:{n.Bias:N4}{(n.Bias < 0 ? " " : "  ")}| ");
            Console.SetCursorPosition(x, ++y);
            Console.WriteLine($"| d:{n.Delta:N4}{(n.Delta < 0 ? " " : "  ")}| ");
            Console.SetCursorPosition(x, ++y);
            Console.WriteLine(string.Join("", Enumerable.Repeat("-", NodeWidth - 1)) + " ");
        }
    }
}