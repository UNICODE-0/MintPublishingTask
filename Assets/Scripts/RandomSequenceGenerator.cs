using System.Collections.Generic;

public static class RandomSequenceGenerator
{
    public static List<int> Generate(int length, int minValue, int maxValue)
    {
        List<int> sequence = new List<int>();

        if (length <= 0)
            return null;

        System.Random random = new System.Random();
        sequence.Add(random.Next(minValue, maxValue + 1));

        for (int i = 1; i < length; i++)
        {
            int previousNumber = sequence[i - 1];
            int randomNumber;

            do
            {
                randomNumber = random.Next(minValue, maxValue + 1);
            } while (randomNumber == previousNumber);

            sequence.Add(randomNumber);
        }

        return sequence;
    }
}
