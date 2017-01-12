//Brute-Force C# program
//Liam Harper
//31/08/2016

using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;



namespace BruteForceProgram
{
    class Program
    {
        public static int bruteForceAttempts = 0;
        public static int bruteForceAttemptsPerSecond = 0;
        public static System.Timers.Timer debuggingTimer = new System.Timers.Timer();


        static void Main(string[] args)
        {


            Random rngGenerator = new Random();
            Console.ForegroundColor = (ConsoleColor.Cyan);
            Console.WriteLine("Brute-Force Program.\n");
            Console.ForegroundColor = (ConsoleColor.Red);
            Console.Write("Please enter a password (a-z, A-Z): ");

            string passwordToBruteForce = Console.ReadLine();

            int passwordToBruteForceLength = passwordToBruteForce.Length;

            string passwordGuess = "";
            string plaintextPassword = "";

            //ASCII ranges (A-Z, a-z)
            int uppercaseASCIIRangeStart = 65;
            int uppercaseASCIIRangeEnd = 90;
            int lowercaseASCIIRangeStart = 97;
            int lowercaseASCIIRangeEnd = 122;
            int uppercaseCharCount = 0;
            int[] arrayOfCasesUsedPerCharOfPassword = new int[passwordToBruteForce.Length];

            bool isUppercaseCharPresent = false;

            //If char is uppercase set bool variable as true and set flag in array index to determine position
            foreach (Char c in passwordToBruteForce)
            {

                uppercaseCharCount++;

                if (Char.IsUpper(c))
                {

                    isUppercaseCharPresent = true;

                    arrayOfCasesUsedPerCharOfPassword[uppercaseCharCount - 1] = uppercaseCharCount;

                }

                else
                {
                    arrayOfCasesUsedPerCharOfPassword[uppercaseCharCount - 1] = -99;

                }

            }
            Console.ForegroundColor = (ConsoleColor.Green);
            Console.WriteLine("\nPlaintext Password: {0}", passwordToBruteForce);

            plaintextPassword = passwordToBruteForce;
            //Create hash of entered password
            using (SHA256 hash = SHA256Managed.Create())
            {
                passwordToBruteForce = String.Join("", hash
                  .ComputeHash(Encoding.UTF8.GetBytes(passwordToBruteForce))
                  .Select(item => item.ToString("x2")));
            }

            Console.ForegroundColor = (ConsoleColor.Yellow);
            Console.WriteLine("\nHashed form: {0} \n", passwordToBruteForce);


            uppercaseCharCount = uppercaseCharCount - 1;

            //Debugging timer
            //debuggingTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //debuggingTimer.Interval = 1000;
            //debuggingTimer.Enabled = true;

            //Calculates how long brute-force took, attempts per second etc
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //Algorithm uses only necessary ASCII ranges per character
            //As we know which characters are upper/lower case and their positions why use the full range?
            //If a char is upper case it only uses the upper case letter range of ASCII (65-90)
            //If a char is lower case it only uses the lower case letter range of ASCII (97-122)

            while (passwordGuess != passwordToBruteForce)

            {
                passwordGuess = "";
                {
                    for (int i = 0; i < passwordToBruteForceLength; i++)
                    {

                        if (arrayOfCasesUsedPerCharOfPassword[i] > 0 && isUppercaseCharPresent == true)
                        {

                            char ascii = (char)(rngGenerator.Next(uppercaseASCIIRangeStart, uppercaseASCIIRangeEnd));
                            passwordGuess += ascii;
                        }

                        else
                        {
                            char ascii = (char)(rngGenerator.Next(lowercaseASCIIRangeStart, lowercaseASCIIRangeEnd));
                            passwordGuess += ascii;
                        }

                    }


                    //Create hash of guessed password
                    using (SHA256 hash = SHA256Managed.Create())
                    {
                        passwordGuess = String.Join("", hash
                          .ComputeHash(Encoding.UTF8.GetBytes(passwordGuess))
                          .Select(item => item.ToString("x2")));
                    }
                    //Enabling code below makes console look pretty...
                    //but brute-force takes approx 229 times longer (literally)

                    //Console.ForegroundColor = (ConsoleColor)rngGenerator.Next(0, 16);
                    //Console.WriteLine(passwordGuess);

                    bruteForceAttempts++;
                    bruteForceAttemptsPerSecond++;
                }
                }

            stopWatch.Stop();
            debuggingTimer.Enabled = false;

            Console.ForegroundColor = (ConsoleColor.White);
            Console.WriteLine("\nHash of {0} brute-forced and found ({1}) after {2} attempts.", plaintextPassword, passwordToBruteForce, bruteForceAttempts);

                if (stopWatch.Elapsed.TotalSeconds < 1)
                {

                    Console.WriteLine("\nToo fast to calculate attempts per second! (Time elapsed under a second).");
                    Console.WriteLine("\nTime Elapsed: {0} Milliseconds.", Math.Floor(stopWatch.Elapsed.TotalMilliseconds));
                    Console.ReadLine();
                }

                else
                {
                    Console.WriteLine("\nApprox Attempts per Second: {0}", Math.Floor((bruteForceAttempts / stopWatch.Elapsed.TotalSeconds)));
                    Console.WriteLine("\nTime Elapsed: {0} Seconds.", Math.Floor(stopWatch.Elapsed.TotalSeconds));
                    Console.ReadLine();
                }



            }

            //public static void OnTimedEvent(object source, ElapsedEventArgs e)
            //{

            //debuggingTimer.Enabled = false;
            //Console.Write(Convert.ToString("..."));
            //debuggingTimer.Enabled = true;
            //bruteForceAttemptsPerSecond = 0;

            //}
        }


    }


