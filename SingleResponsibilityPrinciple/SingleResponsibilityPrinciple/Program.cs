namespace SingleResponsibilityPrinciple
{
    using System;
    using NLog;
    public class Program
    {
        static void Main(string[] args)
        {
            // Shoe1 class violating Single responsibility principle.
            var shoe1 = new Shoes1();
            shoe1.Size = 9;
            Console.WriteLine(shoe1.IsAvailableShoes());

            // Shoe2 class is following single responsibility principle.
            var shoe2 = new Shoes2 { Size = 9 };
            Console.WriteLine(shoe2.IsAvailableShoes("Nike"));
            Console.ReadKey();
        }
    }


    // shoe1 class performing three tasks(not considering exception handling as of now)(i.e this class is over loaded with different taks)
    // one issue in shoe1 class is that if new looging is to be introduced (event logging) then we need to change
    // the implementation of the shoe1 class
    public class Shoes1
    {
        public int Size { get; set; }
        public bool IsAvailableShoes(string brandType = null)
        {
            if (brandType != null)
            {
                try
                {
                    // A DB CALL
                    return true;
                }
                catch (Exception ex)
                {
                    System.IO.File.WriteAllText(@"c:\Error.txt", ex.ToString());
                }
            }

            return false;
        }
    }

    // Create a Class Shoe2 which follows single responsibility principle(i.e to perform only Single task)  
    // we create two new classes EvaluateExpression and ExceptionLogging whose sole perpose is to evaluate given expression and log the excetions respectively
    // and our new class shoe2 can focus on its purpose of checking Available shoes. 
    public class Shoes2
    {
        public int Size { get; set; }

        private readonly EvaluateExpression exp = new EvaluateExpression();

        private readonly ExceptionLogging exceptionLogging = new ExceptionLogging();

        public bool IsAvailableShoes(string brandType = null)
        {
            if (this.exp.CheckBrandType(brandType))
            {
                try
                {
                    // Do A DB call and return what ever boolean value DB returns
                    return true; // this is just a dummy return
                }
                catch (Exception ex)
                {
                    this.exceptionLogging.LogError(ex);
                }
            }

            return false;
        }
    }

    // Task of this class is to perform only logging (follows single responsibility principle).
    public class ExceptionLogging
    {
        private readonly ILogger fileLogger = LogManager.GetLogger("FileLogger");

        public void LogError(Exception e)
        {
            this.fileLogger.Error(e);
        }
    }

    // Task of this class is to evaluate expression (follows single responsibility principle).
    public class EvaluateExpression
    {
        public bool CheckBrandType(string brandType)
        {
            if (brandType == null)
            {
                return false;
            }

            return true;
        }
    }
}
