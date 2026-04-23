using System;

namespace Projekt_Schiffeversenken
{
    class Program
    {
        static void Main(string[] args)
        {
            int länge = 7;
            int max = 6;
            Random rnd = new Random();

            do
            {
                char[,] myArr = new char[länge, länge];
                int[,] schiffe = SchiffeGenerieren(rnd, 3, max);
                int versenkt = 0;

                Console.WriteLine("Hallo und Willkommen zu Schiffeversenken");
                Console.WriteLine("---------------------------------------");
                System.Threading.Thread.Sleep(1200);
                Console.Clear();

                while (versenkt < 3)
                {
                    int firstnmb = 0;
                    int secondnmb = 0;
                    bool gültigeEingabe = false;

                    while (!gültigeEingabe)
                    {
                        Console.WriteLine($"Geben sie ihre Koordinaten ein zwischen 1 und {max} (z.B. 3, 5)");
                        try
                        {
                            string nmbs = Console.ReadLine();
                            string[] subs = nmbs.Split(", ");
                            firstnmb = Convert.ToInt32(subs[0]);
                            secondnmb = Convert.ToInt32(subs[1]);

                            if (firstnmb < 1 || firstnmb > max || secondnmb < 1 || secondnmb > max)
                                Console.WriteLine($"Koordinaten müssen zwischen 1 und {max} liegen. Bitte erneut eingeben.");
                            else
                                gültigeEingabe = true;
                        }
                        catch
                        {
                            Console.WriteLine("Bitte nach der Vorlage halten (z.B. 3, 5).");
                        }
                    }

                    Console.Clear();

                    bool treffer = false;
                    for (int s = 0; s < schiffe.GetLength(0); s++)
                    {
                        if (firstnmb == schiffe[s, 0] && secondnmb == schiffe[s, 1])
                        {
                            myArr[firstnmb, secondnmb] = 'X';
                            schiffe[s, 0] = -1;
                            schiffe[s, 1] = -1;
                            versenkt++;
                            treffer = true;
                            break;
                        }
                    }

                    if (treffer)
                    {
                        Console.WriteLine($"GETROFFEN! ({versenkt}/3 Schiffe versenkt)");
                        printMyFields(myArr);

                        if (versenkt == 3)
                        {
                            Console.WriteLine("Alle Schiffe versenkt! Du hast gewonnen!");
                            break;
                        }
                    }
                    else
                    {
                        myArr[firstnmb, secondnmb] = 'O';
                        Console.WriteLine("Guter Versuch, aber nichts getroffen.");
                        printMyFields(myArr);
                        Console.WriteLine("Willst du noch einmal schiessen? j oder n");
                        if (Console.ReadLine()?.ToLower() != "j")
                            break;
                    }
                }

                Console.WriteLine("Möchten sie noch einmal spielen? j oder n");

            } while (Console.ReadLine()?.ToLower() == "j");
        }

        static int[,] SchiffeGenerieren(Random rnd, int anzahl, int max)
        {
            int[,] schiffe = new int[anzahl, 2];
            for (int s = 0; s < anzahl; s++)
            {
                int x, y;
                do
                {
                    x = rnd.Next(1, max + 1);
                    y = rnd.Next(1, max + 1);
                } while (SchiffExistiert(schiffe, s, x, y));
                schiffe[s, 0] = x;
                schiffe[s, 1] = y;
            }
            return schiffe;
        }

        static bool SchiffExistiert(int[,] schiffe, int bisIndex, int x, int y)
        {
            for (int i = 0; i < bisIndex; i++)
                if (schiffe[i, 0] == x && schiffe[i, 1] == y)
                    return true;
            return false;
        }

        public static void printMyFields(char[,] myField)
        {
            for (int j = 0; j < myField.GetLength(0); j++)
            {
                for (int i = 0; i < myField.GetLength(1); i++)
                {
                    string mySign = "_";
                    if (myField[i, j] > 0)
                        mySign = myField[i, j].ToString();
                    Console.Write(mySign);
                }
                Console.WriteLine(" ");
            }
        }
    }
}
