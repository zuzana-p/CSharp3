// zkoušela jsem si více možností, které mě napadly
public class FizzBuzz
{
    public void CountTo(int lastNumber)
    {
        CountTo_NewLine(lastNumber);
    }

    private void CountTo_If(int lastNumber)
    {
        for (int i = 1; i < lastNumber; i++)
        {
            if (i % 3 == 0)
            {
                Console.WriteLine("Fizz");
            }

            else if (i % 5 == 0)
            {
                Console.WriteLine("Buzz");
            }

            else if ((i % 3 == 0) && (i % 5 == 0))
            {
                Console.WriteLine("FizzBuzz");
            }

            else
            { Console.WriteLine(i); }
        }
    }

    private void CountTo_IfWithString(int lastNumber)
    {
        string numberOrFizzBuzz;
        for (int i = 1; i < lastNumber; i++)
        {
            numberOrFizzBuzz = i.ToString();

            if (i % 3 == 0)
            {
                numberOrFizzBuzz = "Fizz";
            }

            if (i % 5 == 0)
            {
                numberOrFizzBuzz = "Buzz";
            }

            if ((i % 3 == 0) && (i % 5 == 0))
            {
                numberOrFizzBuzz = "FizzBuzz";
            }

            Console.WriteLine(numberOrFizzBuzz);
        }
    }

    private void CountTo_Switch(int lastNumber)
    {
        for (int i = 0; i < lastNumber; i++)
        {
            switch (i % 3 == 0, i % 5 == 0)
            {
                case (true, true):
                {
                    Console.WriteLine("FizzBuzz");
                    break;
                }
                case (true, false):
                {
                    Console.WriteLine("Fizz");
                    break;
                }
                case (false, true):
                {
                    Console.WriteLine("Buzz");
                    break;
                }
                default:
                {
                    Console.WriteLine(i);
                    break;
                }
            }
        }
    }

    private void CountTo_NewLine(int lastNumber)
    {
        for (int i = 1; i < lastNumber; i++)
        {
            string numberOrNewLine = i.ToString();

            if (i % 3 == 0)
            {
                Console.Write("Fizz");
                numberOrNewLine = "";
            }

            if (i % 5 == 0)
            {
                Console.Write("Buzz");
                numberOrNewLine = "";
            }

            Console.WriteLine(numberOrNewLine);
        }
    }
}

