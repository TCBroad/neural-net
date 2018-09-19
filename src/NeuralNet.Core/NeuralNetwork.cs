namespace NeuralNet.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public class NeuralNetwork
    {
        // ReSharper disable once MemberCanBePrivate.Global - for serialization
        public double LearningRate { get; private set; }

        public List<Layer> Layers { get; private set; }

        public int NumLayers { get; private set;}

        public int Runs { get; private set; }

        public int TrainingIterations { get; private set; }

        public string Name { get; private set; }

        public NeuralNetwork(string name, double learningRate, int[] layerDefinitions)
        {
            this.Name = name;
            this.LearningRate = learningRate;

            if (layerDefinitions.Length < 2)
            {
                throw new ArgumentException("Network requires more than 2 layers", nameof(layerDefinitions));
            }

            this.NumLayers = layerDefinitions.Length;
            this.Layers = new List<Layer>(this.NumLayers);

            this.Initialise(layerDefinitions);
        }

        protected NeuralNetwork()
        {
        }

        private void Initialise(int[] layerDefinitions)
        {
            for (var l = 0; l < this.NumLayers; l++)
            {
                var layer = new Layer(layerDefinitions[l]);
                for (var n = 0; n < layer.Size; n++)
                {
                    var neuron = new Neuron();

                    if (l == 0)
                    {
                        // input layer
                        neuron.Bias = 0;
                    }
                    else
                    {
                        for (var w = 0; w < layerDefinitions[l - 1]; w++)
                        {
                            neuron.Weights.Add(CryptoRandom.Next());
                        }
                    }

                    layer.Neurons.Add(neuron);
                }

                this.Layers.Add(layer);
            }
        }

        public double[] Run(List<double> input)
        {
            if (input.Count != this.Layers[0].Size)
            {
                throw new ArgumentException("Input size must match input layer size", nameof(input.Count));
            }

            this.Runs++;

            for (var l = 0; l < this.NumLayers; l++)
            {
                var layer = this.Layers[l];

                for (var n = 0; n < layer.Size; n++)
                {
                    var neuron = layer.Neurons[n];
                    if (l == 0)
                    {
                        neuron.Value = input[n];
                    }
                    else
                    {
                        var value = 0d;
                        for (var previous = 0; previous < this.Layers[l - 1].Size; previous++)
                        {
                            value += this.Layers[l - 1].Neurons[previous].Value * neuron.Weights[previous];
                        }

                        neuron.Value = Sigmoid(value + neuron.Bias);
                    }

                    #if TRACE
                    // Trace.TraceInformation($"Neuron[{l},{n}] = {neuron.Value}");
                    #endif
                }
            }

            #if TRACE
            // Trace.TraceInformation("-----------------------------------------");
            #endif

            var outputLayer = this.Layers[this.NumLayers - 1];
            var result = new double[outputLayer.Size];
            for (var i = 0; i < outputLayer.Size; i++)
            {
                result[i] = outputLayer.Neurons[i].Value;
            }

            return result;
        }

        public void Train(double[] input, double[] output)
        {
            if (input.Length != this.Layers[0].Size)
            {
                throw new ArgumentException("Input data must match input layer size", nameof(input.Length));
            }

            if (output.Length != this.Layers[this.NumLayers - 1].Size)
            {
                throw new ArgumentException("Output data must match output layer size", nameof(output.Length));
            }

            this.Run(input.ToList());

            this.TrainingIterations++;

            for (var i = 0; i < this.Layers[this.NumLayers - 1].Size; i++)
            {
                var neuron = this.Layers[this.NumLayers - 1].Neurons[i];

                neuron.Delta = neuron.Value * (1 - neuron.Value) * (output[i] - neuron.Value);

                for (var j = this.NumLayers - 2; j >= 1 ; j--)
                {
                    for (var k = 0; k < this.Layers[j].Size; k++)
                    {
                        var nextNeuron = this.Layers[j].Neurons[k];

                        nextNeuron.Delta = nextNeuron.Value *
                                           (1 - nextNeuron.Value) *
                                           this.Layers[j + 1].Neurons[i].Weights[k] *
                                           this.Layers[j + 1].Neurons[i].Delta;
                    }
                }
            }

            for (var i = this.NumLayers - 1; i >= 1; i--)
            {
                for (var j = 0; j < this.Layers[i].Size; j++)
                {
                    var neuron = this.Layers[i].Neurons[j];
                    neuron.Bias += this.LearningRate * neuron.Delta;

                    for (var k = 0; k < neuron.NumWeights; k++)
                    {
                        neuron.Weights[k] += this.LearningRate * this.Layers[i - 1].Neurons[k].Value * neuron.Delta;
                    }
                }
            }
        }

        private static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }
    }
}