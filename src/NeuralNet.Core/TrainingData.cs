namespace NeuralNet.Core
{
    using System.Collections.Generic;

    public class TrainingData
    {
        public TrainingData(double[] input, double[] expected)
        {
            this.Input = input;
            this.Expected = expected;
        }

        protected bool Equals(TrainingData other)
        {
            return Equals(this.Input, other.Input) && Equals(this.Expected, other.Expected);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((TrainingData)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.Input != null ? this.Input.GetHashCode() : 0) * 397) ^ (this.Expected != null ? this.Expected.GetHashCode() : 0);
            }
        }

        public double[] Input { get; }

        public double[] Expected { get; }
    }

    public class NetworkConfiguration
    {
        public HashSet<TrainingData> TrainingData { get; set; }
    }
}