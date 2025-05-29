using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace EUtazas2020;

internal class Program
{
    static void Main(string[] args)
    {
        // 1. Lista az összes utas adatának tárolására
        List<Berteletek> utasok = new List<Berteletek>();

        // 2. Fájl beolvasása soronként
        foreach (var sor in File.ReadLines(@"../../../src/utasadat.txt"))
        {
            utasok.Add(new Berteletek(sor));
        }

        // 3. Kiírjuk, hány utas szeretett volna felszállni
        Console.WriteLine($"3. feladat:\n {utasok.Count} utas szeretett volna felszállni a buszra.");
        
        int elutasitott = 0;

        foreach (var utas in utasok)
        {
            if (utas.JegyVBerlet == "JGY")
            {
                // Jegyek száma
                int jegyekSzama = int.Parse(utas.BerletErvenyesege);
                if (jegyekSzama == 0)
                {
                    elutasitott++;
                }
            }
            else
            {
                // Bérlet esetén ellenőrizzük a dátumot
                DateTime ervenyessegDatuma = DateTime.ParseExact(utas.BerletErvenyesege, "yyyyMMdd", CultureInfo.InvariantCulture);

                if (ervenyessegDatuma < utas.FelSzDatumI.Date)
                {
                    elutasitott++;
                }
            }
        }

        Console.WriteLine($"4. feladat:\n {elutasitott} utast kellett elutasítani.");
        // 5. feladat: Legforgalmasabb megálló meghatározása
        Dictionary<int, int> megalloStat = new Dictionary<int, int>();

        foreach (var utas in utasok)
        {
            if (!megalloStat.ContainsKey(utas.Megallo))
            {
                megalloStat[utas.Megallo] = 0;
            }
            megalloStat[utas.Megallo]++;
        }

        // Megkeressük a legtöbb felszállási kísérletet
        int legtobb = megalloStat.Values.Max();

        // Kiválasztjuk a legkisebb sorszámú megállót, ahol ez történt
        int legtobbMegallo = megalloStat
            .Where(kvp => kvp.Value == legtobb)
            .Select(kvp => kvp.Key)
            .Min();

        Console.WriteLine($"5. feladat:\n A legtöbb utas ({legtobb} fő) a {legtobbMegallo}. megállóban próbált felszállni.");

        // 6. feladat: Kedvezményes és ingyenes utazók száma
        int kedvezmenyes = 0;
        int ingyenes = 0;

        foreach (var utas in utasok)
        {
            if (utas.JegyVBerlet == "JGY") continue; // Jegy -> nem bérlet, kihagyjuk

            // Bérlet érvényesség ellenőrzése
            DateTime ervenyesseg;
            bool datumOK = DateTime.TryParseExact(utas.BerletErvenyesege, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out ervenyesseg);

            if (datumOK && ervenyesseg >= utas.FelSzDatumI.Date)
            {
                switch (utas.JegyVBerlet)
                {
                    case "TAB":
                    case "NYB":
                        kedvezmenyes++;
                        break;
                    case "NYP":
                    case "RVS":
                    case "GYK":
                        ingyenes++;
                        break;
                }
            }

        }

        Console.WriteLine($"6. feladat:\n Kedvezményesen utazók száma: {kedvezmenyes} fő");
        Console.WriteLine($"6. feladat:\n Ingyenesen utazók száma: {ingyenes} fő");

        // 7. feladat: Figyelmeztetés 3 napon belül lejáró érvényes bérlet esetén
        using StreamWriter sw = new(@"../../../src/figyelmeztetes.txt");

        foreach (var utas in utasok)
        {
            if (utas.JegyVBerlet == "JGY") continue; // nem bérlet
            if (!DateTime.TryParseExact(utas.BerletErvenyesege, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ervenyesseg))
                continue;

            if (ervenyesseg >= utas.FelSzDatumI.Date) // még érvényes
            {
                TimeSpan hatralevo = ervenyesseg - utas.FelSzDatumI.Date;
                if (hatralevo.TotalDays <= 3)
                {
                    string ervenyesFormazva = ervenyesseg.ToString("yyyy-MM-dd");
                    sw.WriteLine($"{utas.KartyaID} {ervenyesFormazva}");
                }
            }
        }

        Console.WriteLine("7. feladat:\n A figyelmeztetésre jogosult utasok kiírva a figyelmeztetes.txt fájlba.");
    }
}
