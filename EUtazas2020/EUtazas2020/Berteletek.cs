using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUtazas2020
{
    internal class Berteletek
    {
        // A megálló sorszáma, ahol az utas felszállt (0-29)
        public int Megallo { get; set; }

        // A felszállás dátuma és időpontja (pl. 20190326-0700 => 2019.03.26 07:00)
        public DateTime FelSzDatumI { get; set; }

        // A kártya azonosítója (7 jegyű szám)
        public int KartyaID { get; set; }

        // A jegy vagy bérlet típusa (pl. JGY, FEB, TAB, stb.)
        public string JegyVBerlet { get; set; }

        // Bérlet esetén a lejárat dátuma (ééééhhnn), jegy esetén a felhasználható jegyek száma (0-10)
        public string BerletErvenyesege { get; set; }

        // Konstruktor, amely feldolgozza az egy sort az utasadat.txt állományból
        public Berteletek(string sor)
        {
            // Sor felbontása szóközök mentén
            string[] adatok = sor.Split(' ');

            // 1. adat: megálló száma
            Megallo = int.Parse(adatok[0]);

            // 2. adat: felszállás dátuma és időpontja, például: "20190326-0700"
            FelSzDatumI = DateTime.ParseExact(adatok[1], "yyyyMMdd-HHmm", null);

            // 3. adat: kártya azonosítója
            KartyaID = int.Parse(adatok[2]);

            // 4. adat: jegy vagy bérlet típusa
            JegyVBerlet = adatok[3];

            // 5. adat: bérlet érvényességi dátuma VAGY jegyek száma
            BerletErvenyesege = adatok[4];
        }
    }
}
