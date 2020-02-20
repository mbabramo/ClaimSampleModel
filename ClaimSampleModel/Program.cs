using System;
using System.Linq;

namespace ClaimSampleModel
{
    class Program
    {

        static Random ran = new Random();
        static void Helper(int numClaimsAltogether, int numClaimsSelected, int repetitions, bool uniformDistribution)
        {
            Console.WriteLine($"Selecting {numClaimsSelected} from {numClaimsAltogether} (uniformly distributed: {uniformDistribution}) repetitions: {repetitions}");
            double[] trueValues;
            if (uniformDistribution)
                trueValues = Enumerable.Range(0, numClaimsAltogether).Select(x => ((double)(x + 0.5)) / numClaimsAltogether).ToArray();
            else
                trueValues = Enumerable.Range(0, numClaimsAltogether).Select(x => Math.Pow(100_000, (x + 1.0)/(numClaimsAltogether + 1.0))).ToArray();
            double trueValuesSum = trueValues.Sum();
            double[] payouts = new double[numClaimsAltogether];
            for (int r = 0; r < repetitions; r++)
            {
                int[] indices = new int[numClaimsSelected];
                double totalValue = 0;
                for (int s = 0; s < numClaimsSelected; s++)
                {
                    bool sameIndexPickedTwice = false;
                    do
                    {
                        indices[s] = ran.Next(0, numClaimsAltogether);
                        sameIndexPickedTwice = false;
                        for (int s2 = 0; s2 < s; s2++)
                            if (indices[s] == indices[s2])
                                sameIndexPickedTwice = true;
                    } while (sameIndexPickedTwice);
                    totalValue += trueValues[indices[s]];
                }
                for (int s = 0; s < numClaimsSelected; s++)
                    payouts[indices[s]] += trueValues[indices[s]] * trueValuesSum / (totalValue * repetitions);
            }
            double payoutsSum = payouts.Sum();
            Console.WriteLine("Payouts to true value for lowest: " + (payouts[0] / trueValues[0]));
            Console.WriteLine("Payouts to true value for highest: " + (payouts[numClaimsAltogether - 1] / trueValues[numClaimsAltogether - 1]));
        }

        static void Main(string[] args)
        {
            Helper(1000, 2, 500_000_000, true);
            Helper(1000, 2, 500_000_000, false);
            Helper(1000, 100, 10_000_000, true);
            Helper(1000, 100, 10_000_000, false);
        }
    }
}
