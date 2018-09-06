namespace NeuralNet
{
    using System.Collections.Generic;

    public class Layer
    {
        public int Size { get; private set;}

        public List<Neuron> Neurons { get; private set; }

        public Layer(int size)
        {
            this.Size = size;
            this.Neurons = new List<Neuron>(size);
        }

        protected Layer()
        {
        }
    }
}