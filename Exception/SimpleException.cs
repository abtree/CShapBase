using System;

namespace SimpleException
{
    class SimpleException
    {
        public static void Main()
        {
            string userInput;
            int index;
            while (true)
            {
                try
                {
                    Console.Write("Input a number between 0 and 5 " + "(or just hit return to exit)>");
                    userInput = Console.ReadLine();
                    if (userInput == "")
                        break;
                    index = Convert.ToInt32(userInput);
                    if (index < 0 || index > 5)
                        throw new IndexOutOfRangeException("You typed in " + userInput);
                    Console.WriteLine("Your number was :" + index);
                }catch(IndexOutOfRangeException ex)
                {
                    Console.WriteLine("Exception: " + " Number should be between 0 - 5. {0}", ex.Message);
                }catch(Exception e)
                {
                    Console.WriteLine("An exception was thrown, Message was {0}", e.Message);
                }
                finally
                {
                    Console.WriteLine("Thank you");
                }
            }
        }
    }
}
