namespace NeuralNet
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;

    internal static class Program
    {
        private const int NodeWidth = 14;
        private const int NodeHeight = 6;
        private const int Iterations = 1000;

        private static readonly Dictionary<double[], double[]> TrainingData = new Dictionary<double[], double[]>();

        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new PrivateSetterContractResolver(),
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };

        private static void Main(string[] args)
        {
            NeuralNetwork nn;
            if (args.Length > 0)
            {
                nn = LoadNet(args[0]);
            }
            else
            {
                Console.Write("Create net: ");
                nn = CreateNet(Console.ReadLine());
            }

            if (nn == null)
            {
                Console.WriteLine("Not found!");

                return;
            }

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Input: ");
                var input = Console.ReadLine();
                if (input == "q")
                {
                    break;
                }

                if (input.StartsWith("s"))
                {
                    var filename = input.Substring(2);
                    SaveNet(nn, filename);

                    continue;
                }

                var nnData = input.Split(" ").Select(double.Parse).ToList();

                var output = nn.Run(nnData);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Output: [{string.Join(", ", output)}]");
            }
        }

        private static NeuralNetwork CreateNet(string name)
        {
            NeuralNetwork nn;
            switch (name)
            {
                case "xor":
                    nn = CreateXor();
                    break;
                case "swap-vars":
                    nn = CreateSwapVars();
                    break;
                default:
                    return null;
            }

            var randomisedData = new List<KeyValuePair<double[], double[]>>();

            for (var i = 0; i < Iterations; i++)
            {
                randomisedData.AddRange(TrainingData.Select(x => x));
            }

            randomisedData = randomisedData.OrderBy(x => Guid.NewGuid()).ToList();

            Console.CursorVisible = false;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (var data in randomisedData)
            {
                nn.Train(data.Key, data.Value);

                Display(nn);
            }

            stopwatch.Stop();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\nTraining took: {stopwatch.Elapsed:g}\n");
            Console.CursorVisible = true;

            return nn;
        }

        private static NeuralNetwork LoadNet(string filename)
        {
            if (!File.Exists(filename))
            {
                return null;
            }

            var json = File.ReadAllText(filename);

            return JsonConvert.DeserializeObject<NeuralNetwork>(json, SerializerSettings);
        }

        private static void SaveNet(NeuralNetwork nn, string filename)
        {
            var json = JsonConvert.SerializeObject(nn, SerializerSettings);

            File.WriteAllText(filename, json);

            Console.WriteLine("Saved!");
        }

        private static NeuralNetwork CreateSwapVars()
        {
            var nn = new NeuralNetwork("swap-vars", 10, new[] { 2, 4, 2 });

            TrainingData.Add(new[] { 0d, 1d }, new[] { 1d, 0d });
            TrainingData.Add(new[] { 1d, 0d }, new[] { 0d, 1d });

            return nn;
        }

        private static NeuralNetwork CreateXor()
        {
            var nn = new NeuralNetwork("xor", 5, new[] { 2, 2, 1 });

            TrainingData.Add(new[] { 0d, 1d }, new[] { 1d });
            TrainingData.Add(new[] { 1d, 0d }, new[] { 1d });
            TrainingData.Add(new[] { 1d, 1d }, new[] { 0d });
            TrainingData.Add(new[] { 0d, 0d }, new[] { 0d });

            return nn;
        }

        private static void Display(NeuralNetwork nn)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);

            Console.WriteLine($"Name: {nn.Name} | Run: {nn.Runs:N0} | Training iterations: {nn.TrainingIterations:N0}");

            for (var i = 0; i < nn.NumLayers; i++)
            {
                if (i == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Input layer");
                }
                else if (i == nn.NumLayers - 1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Output layer");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Layer " + i);
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