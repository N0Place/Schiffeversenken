using System;

namespace Projekt_Schiffeversenken
{
    class Program

    {
        static void Main(string[] args)
        {
            int länge = 7;
            char[,] myArr = new char[länge, länge];

            Random rnd = new Random();
            rnd.Next(1);

            int rnd1 = rnd.Next(1, 6);
            int rnd2 = rnd.Next(1, 6);
            int firstnmb = 0;
            int secondnmb = 0;
            int a = 6;

            do
            {


                Console.WriteLine("Hallo und Wilkommen zu Schiffeversenken");
                Console.WriteLine("---------------------------------------");
                //Console.WriteLine(rnd1);
                System.Threading.Thread.Sleep(1200);
                Console.Clear();




                do
                {
                    //Console.Clear();

                    try
                    {


                        //Für das Zahleneinlesen
                        Console.WriteLine("Geben sie ihre Koordinaten ein zwischen 1 und 7 (z.B. 3, 5)");
                        string nmbs = Console.ReadLine();
                        string[] subs = nmbs.Split(", ");

                        firstnmb = Convert.ToInt32(subs[0]);
                        secondnmb = Convert.ToInt32(subs[1]);

                    }
                    catch
                    {
                        Console.WriteLine("Bitte nach der Vorlage halten.");
                    }

                    // Um zu verhindern das die Zahl zu gross ist und es ein OutofBounds gibt
                    if (firstnmb > a)
                    {

                        do
                        {
                            firstnmb--;

                        } while (firstnmb > a);

                    }

                    if (secondnmb > a)
                    {
                        do
                        {
                            secondnmb--;

                        } while (secondnmb > a);

                    }






                    Console.Clear();

                    //Um zu überprüfen ob der Nutzer getroffen hat, vergleich der Zahlen
                    if (firstnmb == rnd1 && secondnmb == rnd2)
                    {
                        myArr[rnd1, rnd2] = 'X';
                        Console.WriteLine("GETROFFEN!");
                        //Um das Array dem Spieler darzustellen

                        printMyFields(myArr);

                    }
                    else
                    {
                        Console.WriteLine("Guter versuch aber nichts getroffen");
                        myArr[firstnmb, secondnmb] = 'O';


                        printMyFields(myArr);

                        Console.WriteLine("Willst du noch einmal schiessen j oder n");

                    }



                } while (Console.ReadLine().ToLower() == "j");

                Console.WriteLine("Möchten sie noch einmal Spielen");
            } while (Console.ReadLine().ToLower() == "j");

         
          

         


        }
        // Eine Metohde um das Array zu generiern
        public static void printMyFields(char[,] myField)
        {
            for (int j = 0; j < myField.GetLength(0); j++)
            {

                for (int i = 0; i < myField.GetLength(1); i++)
                {
                    string mySign = "_";

                    if (myField[i, j] > 0)
                    {
                        mySign = myField[i, j].ToString();
                    }

                    Console.Write(mySign);
                }

                Console.WriteLine(" ");
            }
        }

    }   
}   


