using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace szinhaz
{
    class SzinhaziJegyErtekesitoProgram
    {
        static char[,] nezoTerek = new char[16, 15]; // Színház nézőtere

        static List<Foglalas> foglalasok = new List<Foglalas>(); // Foglalások listája

        static Random random = new Random();

        static void Main(string[] args)
        {
            Inicializalas();
            BetoltFoglalasok();

            while (true)
            {
                Console.WriteLine("1 - Szabad és foglalt székek megjelenítése");
                Console.WriteLine("2 - Foglalás módosítása vagy törlése");
                Console.WriteLine("3 - Új foglalás");
                Console.WriteLine("4 - Kilépés");

                int valasztas = GetValidIntegerInput();
                Console.WriteLine();

                switch (valasztas)
                {
                    case 1:
                        SzabadEsFoglaltSzekekMegjelenitese();
                        SzabadHelyekValasztasa();
                        break;
                    case 2:
                        FoglalasModositasaVagyTorlese();
                        break;
                    case 3:
                        UjFoglalas();
                        break;
                    case 4:
                        MentFoglalasok();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Hibás választás!");
                        break;
                }

                Console.WriteLine("A folytatáshoz nyomjon egy gombot...");
                Console.ReadKey();
                Console.Clear(); // Konzoltisztítás a menüpontok között
            }
        }


        static int GetValidIntegerInput()
        {
            int valasztas;
            while (true)
            {
                Console.Write("Válasszon menüpontot: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out valasztas))
                {
                    Console.WriteLine("Hibás bemenet! Kérem adjon meg egy érvényes számot.");
                    Console.WriteLine("A folytatáshoz nyomjon egy gombot...");
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("1 - Szabad és foglalt székek megjelenítése");
                    Console.WriteLine("2 - Foglalás módosítása vagy törlése");
                    Console.WriteLine("3 - Új foglalás");
                    Console.WriteLine("4 - Kilépés");
                }
                else
                {
                    return valasztas;
                }
            }
        }



        static void Inicializalas()
        {
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (random.Next(1, 101) <= 10) // 10% eséllyel legyenek foglaltak kezdetben
                        nezoTerek[i, j] = 'X'; // Foglalt hely
                    else
                        nezoTerek[i, j] = 'O'; // Szabad hely
                }
            }
        }

        static void SzabadEsFoglaltSzekekMegjelenitese()
        {
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    Console.Write(nezoTerek[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void SzabadHelyekValasztasa()
        {
            Console.WriteLine("Szabad helyek:");

            for (int i = 0; i < 16; i++)
            {
                Console.Write($"A {i + 1}. sorban lévő szabad székek: ");

                List<int> szabadSzekek = new List<int>();

                for (int j = 0; j < 15; j++)
                {
                    if (nezoTerek[i, j] == 'O')
                    {
                        szabadSzekek.Add(j + 1);
                    }
                }

                if (szabadSzekek.Count > 0)
                {
                    Console.Write(string.Join(",", szabadSzekek));
                }
                else
                {
                    Console.Write("Nincs szabad hely");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }


        static void FoglalasModositasaVagyTorlese()
        {
            Console.WriteLine("Adja meg a nevet: ");
            string nev = Console.ReadLine();

            Console.WriteLine($"Foglalások {nev} név alatt:");

            List<Foglalas> foglalasokNevSzerint = foglalasok.Where(f => f.Nev == nev).ToList();

            if (foglalasokNevSzerint.Count == 0)
            {
                Console.WriteLine("Nincs ilyen névvel foglalás!");
                return;
            }

            for (int i = 0; i < foglalasokNevSzerint.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Sor: {foglalasokNevSzerint[i].Sor}, Szék: {foglalasokNevSzerint[i].Szek}");
            }

            Console.Write("Adja meg a módosítani vagy törölni kívánt foglalás sorszámát (0 - kilépés): ");
            string input = Console.ReadLine();

            if (input == "0") // Kilépés az input beolvasása után
                return;

            int valasztottIndex;
            if (!int.TryParse(input, out valasztottIndex))
            {
                Console.WriteLine("Hibás bemenet! Kérem adjon meg egy érvényes számot.");
                return;
            }

            valasztottIndex--; // Mivel a felhasználó által megadott sorszám 1-gyel nagyobb, mint a listában való indexelés

            if (valasztottIndex < 0 || valasztottIndex >= foglalasokNevSzerint.Count)
            {
                Console.WriteLine("Hibás sorszám!");
                return;
            }

            Console.WriteLine($"Sor: {foglalasokNevSzerint[valasztottIndex].Sor}, Szék: {foglalasokNevSzerint[valasztottIndex].Szek}");

            Console.WriteLine("1 - Módosítás");
            Console.WriteLine("2 - Törlés");
            Console.WriteLine("3 - Mégsem");
            Console.Write("Válasszon: ");
            int modValasztas = int.Parse(Console.ReadLine());

            switch (modValasztas)
            {
                case 1:
                    Console.Write("Adja meg az új nevet: ");
                    string ujNev = Console.ReadLine();
                    Console.Write("Adja meg az új sor számát: ");
                    int ujSor = int.Parse(Console.ReadLine());
                    Console.Write("Adja meg az új szék számát: ");
                    int ujSzek = int.Parse(Console.ReadLine());

                    // Ha megváltozott a hely, akkor a régi helyet szabadnak jelöljük
                    if (ujSor != foglalasokNevSzerint[valasztottIndex].Sor || ujSzek != foglalasokNevSzerint[valasztottIndex].Szek)
                    {
                        nezoTerek[foglalasokNevSzerint[valasztottIndex].Sor - 1, foglalasokNevSzerint[valasztottIndex].Szek - 1] = 'O'; // Régi hely szabad
                                                                                                                                        // Új hely foglalása
                        nezoTerek[ujSor - 1, ujSzek - 1] = 'X'; // A választott helyet foglaltnak jelöljük
                    }

                    foglalasokNevSzerint[valasztottIndex].Nev = ujNev;
                    foglalasokNevSzerint[valasztottIndex].Sor = ujSor;
                    foglalasokNevSzerint[valasztottIndex].Szek = ujSzek;

                    Console.WriteLine("Foglalás módosítva!");
                    break;
                case 2:
                    // Régi hely szabadítása
                    nezoTerek[foglalasokNevSzerint[valasztottIndex].Sor - 1, foglalasokNevSzerint[valasztottIndex].Szek - 1] = 'O'; // A foglalt helyet szabadnak jelöljük
                    foglalasok.Remove(foglalasokNevSzerint[valasztottIndex]);
                    Console.WriteLine("Foglalás törölve!");
                    break;
                case 3:
                    break;
                default:
                    Console.WriteLine("Hibás választás!");
                    break;
            }
            Console.WriteLine();
        }



        static void UjFoglalas()
        {
            string nev;
            do
            {
                Console.Write("Adja meg a névét (Ha meg akraja szakítani akkor üssön le egy entert és semmi mást!): ");
                nev = Console.ReadLine().Trim(); // Szóközök levágása a string elejéről és végéről

                if (nev == "")
                {
                    Console.WriteLine("Foglalás megszakítva.");
                    return; // Kilépés, ha a felhasználó üres stringet adott meg
                }

            } 
            while (nev.Length < 1);

            Console.WriteLine("Hány helyet szeretne foglalni?");
            int helyekSzama = int.Parse(Console.ReadLine());

            for (int i = 0; i < helyekSzama; i++)
            {
                int sor, szek;
                do
                {
                    Console.Write($"Adja meg a(z) {i + 1}. hely sorát: ");
                    sor = int.Parse(Console.ReadLine());

                    if (sor < 1 || sor > 16)
                    {
                        Console.WriteLine("Hibás sor szám! Kérem adja meg újra.");
                    }
                } while (sor < 1 || sor > 16);

                do
                {
                    Console.Write($"Adja meg a(z) {i + 1}. hely székét: ");
                    szek = int.Parse(Console.ReadLine());

                    if (szek < 1 || szek > 15)
                    {
                        Console.WriteLine("Hibás szék szám! Kérem adja meg újra.");
                    }
                    else if (i > 0 && nezoTerek[sor - 1, szek - 1] == 'X')
                    {
                        Console.WriteLine("Ez a hely már foglalt. Kérem válasszon másikat.");
                        szek = -1; // Hely foglalt, új választás szükséges
                    }
                } while (szek < 1 || szek > 15);

                if (szek != -1)
                {
                    nezoTerek[sor - 1, szek - 1] = 'X'; // A választott helyet foglaltnak jelöljük
                    foglalasok.Add(new Foglalas { Sor = sor, Szek = szek, Nev = nev });
                }
            }

            Console.WriteLine("A hely(ek) foglalva!");
            Console.WriteLine();
        }


        static void MentFoglalasok()
        {
            using (StreamWriter writer = new StreamWriter("C:\\Users\\MSI GS63 Stealth 8RE\\Desktop\\11\\ikt\\mentes\\foglalasok.txt", true))
            {
                foreach (var foglalas in foglalasok)
                {
                    writer.WriteLine($"{foglalas.Sor},{foglalas.Szek},{foglalas.Nev}");
                }
            }
        }

        static void BetoltFoglalasok()
        {
            if (File.Exists("C:\\Users\\MSI GS63 Stealth 8RE\\Desktop\\11\\ikt\\mentes\\foglalasok.txt"))
            {
                string[] lines = File.ReadAllLines("C:\\Users\\MSI GS63 Stealth 8RE\\Desktop\\11\\ikt\\mentes\\foglalasok.txt");
                foreach (var line in lines)
                {
                    string[] parts = line.Split(',');
                    foglalasok.Add(new Foglalas { Sor = int.Parse(parts[0]), Szek = int.Parse(parts[1]), Nev = parts[2] });
                    nezoTerek[int.Parse(parts[0]) - 1, int.Parse(parts[1]) - 1] = 'X'; // Betöltött foglalásokat jelöljük foglaltnak
                }
            }
        }

    }

    class Foglalas
    {
        public int Sor { get; set; }
        public int Szek { get; set; }
        public string Nev { get; set; }
    }

}
