/*
Autor: Marcel Bockovac
Projekt: HNL
Predmet: Osnove programiranje
Ustanova: VŠMTI
Godina: 2019
*/
using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Text.RegularExpressions;
namespace konstruk
{
    class Program
    {
        
       /*   
       Vecina funkcija na pocetku ima streamwriter, mogao sam napraviti funkciju
       koja bi samo isprintala vrijeme, ali razlog zasto sam dodao streamwriter u svaku zasebnu
       funkciju je taj da mogu tocno isprintati sto zelim. Recimo kada dodam klub napisat ce
       'Vrijeme - Dodan klub Hajduk' ili 'Vrijeme - Obrisan igrac 124'
        Te funkcije se ponavljaju i dodaju poprilicno dosta linija kodu, mogao sam to drugacije izvest
        ali trenutno nije potrebno

       */
       ////Funkcija koja prikazuje izbornik
        public static void izbornik(){
            Console.WriteLine("1 - Ažuriraj klubove");
            Console.WriteLine("2 - Ažuriraj igrače");
            Console.WriteLine("3 - Prijelaz");
            Console.WriteLine("4 - Prikaz klubova sa igračima");
            Console.WriteLine("5 - Odigraj utakmicu");
            Console.WriteLine("6 - Prikaži rang listu");
            Console.WriteLine("7 - Odjava \n");
        }
        ////Funkcija za povratak ili kraj
        public static void enter_esc(){
            Console.WriteLine("Pristinite enter za povratak u glavni izbornik ili escape za kraj. ");
            ConsoleKeyInfo key;
            key = Console.ReadKey();
            if(key.Key == ConsoleKey.Enter){
                izbornik();
            }
            else if(key.Key == ConsoleKey.Escape){
                return;
            }

        }



        //Funkcija sluzi za dodavanje ili brisanje klubova
        //Funkcija ucitava klubove iz datoteke te na temelju inputa korisnika brise ili upisuje u datoteku
        public static void azurirajklubove(){
            
        int izbor;
        Console.WriteLine("1.Dodaj klub");
        Console.WriteLine("2.Obriši klub");
        
        izbor = Convert.ToInt32(Console.ReadLine());
        
        if(izbor == 1){
        XDocument doc = XDocument.Load("klubovi.xml");

        //Prebroji koliko se puta pojavljuje izraz "grad", ako je manje od 10 dopusti, ako ih je 9
        //Dopustit ce jos jednom
        int count = doc.Descendants("Grad").Count();
        
        

        if(count < 10) {
        //Console.WriteLine(count); <-- Provjera (Molim te nemoj ovo obrisat)
            string naziv = Console.ReadLine();
            string grad = Console.ReadLine();
            string stadion = Console.ReadLine();

            string putanja = @"logovi.txt";
            StreamWriter oFile = new StreamWriter(putanja, true);
            oFile.Write(DateTime.Now + " - Dodan klub " + naziv + "\n");
            oFile.Flush();
            oFile.Close();
            
            //Dodavanje novog unosa u XML
            //Klubvi nemaju atribute nego su samo elementi, razlog tome je jednostavnost koristenja
            XElement klub = new XElement("Klub",
            new XElement("Naziv", naziv),
            new XElement("Grad", grad),
            new XElement("Stadion", stadion));
            
            doc.Root.Add(klub);
            doc.Save("klubovi.xml");
        }
        else if(count == 10){
            Console.WriteLine("Već postoji 10 klubova!");
        }
        }
        
        else if(izbor == 2){
        string sXml = @"klubovi.xml";
        Console.WriteLine("Unesi Naziv kluba koji želiš izbrisati: ");
        string unos = Console.ReadLine();

        string putanja = @"logovi.txt";
        StreamWriter oFile = new StreamWriter(putanja, true);
        oFile.Write(DateTime.Now + " - Obrisan klub " + unos + "\n");
        oFile.Flush();
        oFile.Close();


        XmlDocument doc = new XmlDocument();
        doc.Load(sXml);
        ////Trazi nazive
        var node = doc.GetElementsByTagName("Naziv");
        int counter = 0;
        foreach(XmlNode bla in node){
            if(node[counter].InnerXml == unos){
                
            XmlNode parent = node[counter].ParentNode;

            parent.ParentNode.RemoveChild(parent);

            string newXml = doc.OuterXml;
            doc.Save(sXml);

                }
            counter += 1;
             }
             //Console.WriteLine("Tu sam"); <-- Debugger for lazy pep 
        XDocument doc1 = XDocument.Load("igraci.xml");
        var q = from node1 in doc1.Descendants("Igrac")
        let attr = node1.Attribute("Klub")
        where attr != null && attr.Value == unos
        select node1;
        q.ToList().ForEach(x => x.Remove());
        doc1.Save("igraci.xml");
       }
        enter_esc();

      }
 
        //Funkcija koja sluzi za dodavanje i/ili brisanje igraca
        public static void AzurirajIgrace(){
            Console.WriteLine("1.Dodaj");
            Console.WriteLine("2.Obriši");
            int izbor = Convert.ToInt32(Console.ReadLine());
            
            if(izbor == 1){
            Console.WriteLine("Unesi ime,prezime,klub i id igraca");
            string ime = Console.ReadLine();
            string prezime = Console.ReadLine();
            string klub = Console.ReadLine();
            int id = Convert.ToInt32(Console.ReadLine());
            string path = @"igraci.xml";

            string putanja = @"logovi.txt";
            StreamWriter oFile = new StreamWriter(putanja, true);
            oFile.Write(DateTime.Now + " - Dodan igrac " + ime + " " + prezime + " u klub: " + klub + "\n");
            oFile.Flush();
            oFile.Close();


            
            XDocument doc = XDocument.Load(path);
            XElement novi = new XElement("Igrac",
            new XAttribute("Ime", ime),
            new XAttribute("Prezime", prezime),
            new XAttribute("Klub",klub),
            new XElement("ID", id));
           
            
            doc.Root.Add(novi);
            doc.Save(path); 
            enter_esc();
            }
            
        else if(izbor == 2){
        Console.WriteLine("Unesi id igraca: ");
        int id = Convert.ToInt32(Console.ReadLine());
        
        string putanja = @"logovi.txt";
        StreamWriter oFile = new StreamWriter(putanja, true);
        oFile.Write(DateTime.Now + " - Obrisan igrac: " + id +  "\n");
        oFile.Flush();
        oFile.Close();
        
        int counter = 0;
        string sXml = @"igraci.xml";
        XmlDocument doc = new XmlDocument();
        doc.Load(sXml);
        var node = doc.GetElementsByTagName("ID");
        foreach(XmlNode bla in node){
            if(node[counter].InnerXml == id.ToString()){
                XmlNode parent = node[counter].ParentNode;

                parent.ParentNode.RemoveChild(parent);
                string newXml = doc.OuterXml;
                doc.Save(sXml);
            }
            counter+=1;
        }

                  }
            
        enter_esc();

        
        }
        /*Funkcija za prijelaz igraca iz jednog kluba u drugi, funkcija prvo makne igraca iz originalnog
        kluba te zatim doda igraca sa unesenim imenom i prezimenom u napisani klub*/
        public static void Prijelaz(){
            Console.WriteLine("Unesi ime igraca kojeg zelis prebaciti: ");
            Console.WriteLine("Unesi prezime igraca kojeg zelis prebaciti: ");
            Console.WriteLine("Unesi klub u koji ga zelis prebaciti: ");
            string ime = Console.ReadLine();
            string prezime = Console.ReadLine();
            string klub = Console.ReadLine();
           

            string putanja = @"logovi.txt";
            StreamWriter oFile = new StreamWriter(putanja, true);
            oFile.Write(DateTime.Now + " - Prijelaz " + ime +" "+ prezime + " u " + klub + "\n");
            oFile.Flush();
            oFile.Close();
            
            XDocument doc = XDocument.Load("igraci.xml");
            var q = from node in doc.Descendants("Igrac")
            let attr = node.Attribute("Ime")
                where attr != null && attr.Value == ime
                    select node;
                        q.ToList().ForEach(x => x.Remove());

            string path = @"igraci.xml";
            
            int id = 0;
            XElement novi = new XElement("Igrac",
            new XAttribute("Ime", ime),
            new XAttribute("Prezime", prezime),
            new XAttribute("Klub",klub),
            new XElement("ID", id));
           
            
            doc.Root.Add(novi);
            doc.Save(path); 
            enter_esc();

        }
        ////Funkcija koja slui za prikaz kluba i svih njegovih igraca, ukoliko klub nema 11 igraca
        ////Funkcija to isprinta
        public static void Prikazi(){
            string putanja = @"logovi.txt";
            StreamWriter oFile = new StreamWriter(putanja, true);
            oFile.Write(DateTime.Now + " - Odabran prikaz klubova i igraca" + "\n");
            oFile.Flush();
            oFile.Close();
        ////Prikazi igrace
        XmlDocument oXml = new XmlDocument();
        oXml.Load("igraci.xml");
        XmlNodeList oNodes = oXml.SelectNodes("//data/Igrac");

        ////Prikazi klubove
        XmlDocument print_klub = new XmlDocument();
        print_klub.Load("klubovi.xml");
        var novi = print_klub.GetElementsByTagName("Naziv");
            int counter = 0;
            int broj_igraca = 0;
            foreach(XmlNode node in novi){
                Console.WriteLine(novi[counter].InnerXml + "\n");
                foreach(XmlNode oNode in oNodes){
                    if(novi[counter].InnerXml==oNode.Attributes["Klub"].Value){
                        broj_igraca +=1;
                        Console.WriteLine(oNode.Attributes["Ime"].Value);
                        Console.WriteLine(oNode.Attributes["Prezime"].Value);
                        
                    }                    
                }
                if(broj_igraca < 11){
                            Console.WriteLine(novi[counter].InnerXml + " Nema dovoljan broj igraca!");
                            Console.WriteLine(broj_igraca);
                            broj_igraca = 0;
                        }
                else{
                    broj_igraca=0;
                }
                counter+=1;   
            }
            enter_esc();
        }
        /* Funkcija koja nije dio rjesenja, ova funkcija nije bila trazena nego sam ju napravio
           Kako bi u slucaju gresaka mogao ponovno generirati 110 igraca, funkcija se moze pozvati samo
           ako je korisnik prijavljen kao root.
        */
        public static void Temporary(){
            int id = 0;
            string ime = Console.ReadLine();
            string prezime = Console.ReadLine();
            string klub = Console.ReadLine();
            string path = @"igraci.xml";
            XDocument doc = XDocument.Load("igraci.xml");
            int counter = 1;
            for(int k=0;k<110; k++){
            string ime_gen = ime+k;
            string prezime_gen = prezime+k;
            string klub_gen = klub+counter;
            if(k%10==0){
                counter+=1;
            }
            id+=1;
            XElement novi = new XElement("Igrac",
            new XAttribute("Ime", ime_gen),
            new XAttribute("Prezime", prezime_gen),
            new XAttribute("Klub",klub_gen),
            new XElement("ID",id));
            doc.Root.Add(novi);
            doc.Save("igraci.xml");
            }


        }

        /*Funkcija koja je dio 5. zadatka, ova funkcija provjerava postoji li točno 10 klubova i 
        ima li svaki od njih minimalno 11 igraca*/
        public static bool broj_igraca_klub(){
        
        XmlDocument oXml = new XmlDocument();
        oXml.Load("igraci.xml");
        XmlNodeList oNodes = oXml.SelectNodes("//data/Igrac");
        XmlDocument print_klub = new XmlDocument();
        print_klub.Load("klubovi.xml");
        
        //// Broj timova
        XDocument doc = XDocument.Load("klubovi.xml");
        int count = doc.Descendants("Grad").Count();
        ///// Broj timova
        
        var novi = print_klub.GetElementsByTagName("Naziv");
            int counter = 0;
            int broj_igraca = 0;
            foreach(XmlNode node in novi){
                foreach(XmlNode oNode in oNodes){
                    if(novi[counter].InnerXml==oNode.Attributes["Klub"].Value){
                        broj_igraca +=1;
                    }
                }
                if(broj_igraca < 11){
                    return(false);
                    broj_igraca = 0;
                }
                else{
                    broj_igraca=0;
                }counter+=1;
            }
            if(count == 10){
            return(true);}
        return(false);

        }



        /*Funkcija koja nasumicno odigra utakmice, utakmice ce odigrati svako sa svakime(2 puta).
        //10 * 9 * 2 = 180, te zbog toga u rezultati.xml postoji 180 zapisa
        Maksimalan broj golova po utakmici ce biti 10 (5:5), bodovi se dodjeljuju kao razlika 
        pobjeda i poreza, ekipa sa najvise bodova je na vrhu, ekipa sa najmanje na dnu, itd.
        */
        public static void Odigraj(){
        
        string putanja = @"logovi.txt";
            StreamWriter oFile = new StreamWriter(putanja, true);
            oFile.Write(DateTime.Now + " - Pokrenuta simulacija utakmica" + "\n");
            oFile.Flush();
            oFile.Close();


        List<string> nova = new List<string>();
        XmlDocument print_klub = new XmlDocument();
        print_klub.Load("klubovi.xml");
        var novi = print_klub.GetElementsByTagName("Naziv");
            int counter = 0;
            foreach(XmlNode node in novi){
                //Console.WriteLine(novi[counter].InnerXml); <-- Jos jedna provjera
                nova.Add(novi[counter].InnerXml);
                counter++;
            }
            if(broj_igraca_klub() == true){
                Console.WriteLine("Ima 10 klubova i svaki ima barem 11 igraca");
            /*var random = new Random();
            int index = random.Next(nova.Count); <--- Ne brisi!
            Console.WriteLine(nova[index]);
            */
                int home_win = 0;
                int away_lost = 0;
                int away_win = 0;
                int home_lost = 0;
                int home_draw = 0 ;
                int away_draw = 0;
                int golovi_doma = 0;
                int golovi_tamo = 0;
                int domaci_daju = 0;
                int gosti_daju = 0;
                int domaci_dobiveni = 0;
                int gosti_dobiveni = 0;
                int opet_counter = 0;
                int counter_za_strane = 1;
            

            
            try{
            foreach(string z in nova){
                foreach(string y in nova){
                
                //Console.WriteLine(counter_za_strane); <--Provjera
                if(counter_za_strane == nova.Count()){
                    counter_za_strane = 0;
                    opet_counter+=1;
                    counter_za_strane = opet_counter+1;
                    if(opet_counter >= nova.Count() || counter_za_strane >= nova.Count()){
                        break;
                    }
                }

                for(int znj=0; znj<2;znj++){
                Random rnd = new Random();
                int gol_domaci = rnd.Next(1,6);
                int gol_gosti = rnd.Next(1,6);
                domaci_daju +=gol_domaci;
                gosti_daju +=gol_gosti;
                domaci_dobiveni += gol_gosti;
                
                string domaci = nova[opet_counter];
                string gosti = nova[counter_za_strane];
                if(gol_domaci > gol_gosti){
                home_win += 1; 
                away_lost +=1;
                
                string pobjednik = domaci;
                    }
                else if(gol_domaci < gol_gosti){
                away_win +=1;
                home_lost +=1;
                string pobjednik = gosti;
                    }
                else if(gol_domaci == gol_gosti){
                home_draw +=1;
                away_draw +=1;
                    }


                ////doc i doc 2 sluze za dodavanje utakmica doma
                
                XDocument doc = XDocument.Load("rezultati.xml");
                XElement novoooo = new XElement("Ekipa",
                new XAttribute("Ime", nova[opet_counter]),
                new XAttribute("Pobjede", home_win),          //home_win
                new XAttribute("Porazi", home_lost),          //home_lost
                new XAttribute("Nerijeseno", home_draw),      //home_draw
                new XAttribute("Golovi", domaci_daju),        //domaci_daju
                new XAttribute("Primljeni", domaci_dobiveni), //domaci_dobiveni
                new XElement("ID","PH"));
                doc.Root.Add(novoooo);
                doc.Save("rezultati.xml");

                XDocument doc2 = XDocument.Load("rezultati.xml");
                XElement strani = new XElement("Ekipa",
                new XAttribute("Ime", nova[counter_za_strane]),
                new XAttribute("Pobjede", away_win),
                new XAttribute("Porazi", away_lost),
                new XAttribute("Nerijeseno", away_draw),
                new XAttribute("Golovi", gosti_daju),
                new XAttribute("Primljeni", gol_domaci),
                new XElement("ID","PH"));
                doc2.Root.Add(strani);
                doc2.Save("rezultati.xml");
        

                gosti_daju = 0;
                away_lost = 0;
                away_win = 0;
                away_draw = 0;
                domaci_daju = 0;
                domaci_dobiveni = 0;
                home_win = 0;
                home_lost = 0;
                home_draw = 0;
                
                }
                counter_za_strane+= 1;
                
                } //<-- Try zagrada
        }

            }
            catch {
                Console.WriteLine("Greska sa indeksom");
            }
          
    }
            else if(broj_igraca_klub() == false){
                Console.WriteLine("Ne postoji 10 klubova i/ili jedan ili vise klubova ima nedovoljan broj igraca");
            }
        enter_esc();
        }


        ////Funkcije koje sluze za prikaz tablice u 6. funkciji
        ////Funkcija preuzeta sa: https://stackoverflow.com/questions/856845/how-to-best-way-to-draw-table-in-console-app-c
        ////Korisnik: Patrick McDonald je kreator funkcija
static int tableWidth = 100;

static void PrintLine()
{
    Console.WriteLine(new string('-', tableWidth));
}

static void PrintRow(params string[] columns)
{
    int width = (tableWidth - columns.Length) / columns.Length;
    string row = "|";

    foreach (string column in columns)
    {
        row += AlignCentre(column, width) + "|";
    }

    Console.WriteLine(row);
}

static string AlignCentre(string text, int width)
{
    text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

    if (string.IsNullOrEmpty(text))
    {
        return new string(' ', width);
    }
    else
    {
        return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
    }
}





        ////Funkcija koja prikazuje rang listu i poredak koji se temelji na rezultati.xml
        ////Sve se dodaje u liste, iz lista se cita te se liste na kraju obrisu
        public static void rang_lista(){
        string putanja = @"logovi.txt";
            StreamWriter oFile = new StreamWriter(putanja, true);
            oFile.Write(DateTime.Now + " - Prikaz rang liste" + "\n");
            oFile.Flush();
            oFile.Close();

        int redni_broj = 0;
        XmlDocument print_klub = new XmlDocument();
        print_klub.Load("klubovi.xml");
        var novi = print_klub.GetElementsByTagName("Naziv");
            int counter = 0;
            int broj_igraca = 0;
            
            XmlDocument oXml = new XmlDocument();
            oXml.Load("rezultati.xml");
            XmlNodeList oNodes = oXml.SelectNodes("//data/Ekipa");
            string ime_kluba = "";
            int wins = 0;
            int losess = 0;
            int draw = 0;
            int scored_goals = 0;
            int recived_goals = 0;
            int diffr = 0;

            List<string> nazivi = new List<string>();
            List<int> ukupno_tekme = new List<int>();
            List<int> pobjede = new List<int>();
            List<int> gubitci = new List<int>();
            List<int> izjedn = new List<int>();
            List<int> zab_gol = new List<int>();
            List<int> prim_gol = new List<int>();
            List<int> gol_dif = new List<int>();
            
            int brojac_ukupno = 0;
            
            foreach(XmlNode node in novi){
                //Console.WriteLine(novi[counter].InnerXml);
                foreach(XmlNode oNode in oNodes){
                    if(novi[counter].InnerXml == oNode.Attributes["Ime"].Value){
                        
                        ime_kluba = oNode.Attributes["Ime"].Value;
                        wins = wins + Convert.ToInt32(oNode.Attributes["Pobjede"].Value);
                        losess = losess + Convert.ToInt32(oNode.Attributes["Porazi"].Value);
                        draw = draw + Convert.ToInt32(oNode.Attributes["Nerijeseno"].Value);
                        scored_goals = scored_goals + Convert.ToInt32(oNode.Attributes["Golovi"].Value);
                        recived_goals = recived_goals + Convert.ToInt32(oNode.Attributes["Primljeni"].Value);
                        diffr = scored_goals - recived_goals;
                        
                        brojac_ukupno+=1;
                    }
                    
                }
                    nazivi.Add(ime_kluba);
                    pobjede.Add(wins);
                    gubitci.Add(losess);
                    izjedn.Add(draw);
                    zab_gol.Add(scored_goals);
                    prim_gol.Add(recived_goals);
                    ukupno_tekme.Add(brojac_ukupno);
                    gol_dif.Add(diffr);
                    
                    wins = 0;
                    losess = 0;
                    draw = 0;
                    scored_goals =0;
                    recived_goals =0;
                    brojac_ukupno = 0;
                    
                    counter+=1;
            }
        
        List<int> lista_bodova = new List<int>();
        List<int> za_sortirat = new List<int>();
        List<int> sortirano = new List<int>();
        
        int kanter = 0;
        int opt_kanter = 0;
        
        foreach(XmlNode jos_jedan in novi){
        
        int bodovi = (pobjede[opt_kanter]*3) + (izjedn[opt_kanter]*1);
        lista_bodova.Add(bodovi);
        za_sortirat.Add(bodovi);
        
        opt_kanter+=1;
        int indeks = 0;
        }
       
        za_sortirat.Sort();
        za_sortirat.Reverse();
        
        PrintRow("R.Br","Klub", "Broj utakmica", "Bodovi", "Pobjeda", "Nerijeseno", "Gol razlika");
        
        int rd_br = 1;
        
        int brojac=0;
        try{
        foreach(string naziv_klub in nazivi){
        while(brojac< 10){
            if(lista_bodova[kanter]==za_sortirat[brojac]){
            
            int gol_razlika = zab_gol[brojac]-prim_gol[brojac];
            
            PrintRow(rd_br.ToString(),nazivi[kanter].ToString(),ukupno_tekme[kanter].ToString(),lista_bodova[kanter].ToString(),pobjede[kanter].ToString(),izjedn[kanter].ToString(),gol_dif[kanter].ToString());
            
            nazivi.Remove(nazivi[kanter]);
            ukupno_tekme.Remove(ukupno_tekme[kanter]);
            lista_bodova.Remove(lista_bodova[kanter]);
            pobjede.Remove(pobjede[kanter]);
            izjedn.Remove(izjedn[kanter]);
            gol_dif.Remove(gol_dif[kanter]);
            
            gol_razlika = 0;
            rd_br+=1;   
            kanter=0;
            brojac+=1; 
            }
            
            else{
        kanter+=1;
        }


        
        }
        }
        } //<--Try zagrada
        catch{
            Console.WriteLine("Enum op may not execute(Greska sa indeksom vjv)");
        }
        enter_esc();
        }
        ////Funkcija koja provjerava da li je korisnik root, funkcija ima parametar username
        ////Taj paramater dobiva iz poziva funkcije u mainu
        public static bool is_root(string username){
       if(username=="root"){
        return true;
       }
       return false;
        }

        ////Funkcija koja sluzi za login, paramteri su username i password.
        ////Funkcija dodaje sve iz login.txt u listu te provjerava unesene podatke
        ////Podatci su zapisani na nacin da je svaki neparni red username a svaki parni password
        ////Zato provjerava username sa k, a password sa k+1        
        public static bool login(string username, string password){
        List<string> info = new List<string>();
     
        using (StreamReader sr = File.OpenText("login.txt"))
{
        string s = String.Empty;
        while ((s = sr.ReadLine()) != null)
        {
            info.Add(s);
        }
}       //Console.WriteLine(info.Count);
        for(int k = 0; k < info.Count; k++){
            if(username == info[k]){
                if(password == info[k+1]){
                    
                    return true;
                }
            }

        }

        return false;
        }
        ////Funkcija koja sluzi za odjavu korisnika, skoro cijeli main je nested u petlji
        ////Koja provjerava da li je odlogiraj false, kada korisnik pozove funkciju odlogiraj, ona 
        ////mjenja odlogiraj u true i vraca se ponovno na unesite ime i lozinku.
        public static bool odlogiraj(bool da){
            if(da == true){
                return true;
                }
            return false;
        }
        static void Main(string[] args)
        {
            string putanja = @"logovi.txt";
            StreamWriter oFile = new StreamWriter(putanja, true);
            oFile.Write(DateTime.Now + " - Pokretanje programa" + "\n");
            oFile.Flush();
            oFile.Close();
            int izbor = 0;
            bool logout = false;    
            bool logged_in = false;
            string username = "";
            string password = "";
             


            while(true) {
            if(logged_in == false)
{
            Console.WriteLine("Unesi korisnicko ime: ");
            Console.WriteLine("Unesi lozinku: ");
            
            username = Console.ReadLine();
            password = Console.ReadLine();

}            
          
            
            if(odlogiraj(logout)==false){
            


            if(login(username,password) == true){ 
            logged_in=true;
            izbornik();
            
            StreamWriter oFilee = new StreamWriter(putanja, true);
            oFilee.Write(DateTime.Now + " - Pokretanje glavnog izbornika" + "\n" +DateTime.Now +" - Prijava korisnika: " + username + "\n");
            oFilee.Flush();
            oFilee.Close();
            
            izbor = Convert.ToInt32(Console.ReadLine());
            if(izbor == 1){
                azurirajklubove();
            }
            else if(izbor == 2){
                AzurirajIgrace();
            }
            else if(izbor == 3){
                Prijelaz();
                
            }
            else if(izbor == 4){
                Prikazi();
            }
            else if(izbor == 5){
                Odigraj();
            }
            else if(izbor == 6){
                rang_lista();
            }
            else if(izbor == 7){
                 StreamWriter oFileee = new StreamWriter(putanja, true);
                 oFileee.Write(DateTime.Now + " - Odjava korisnika "+ username + "\n");
                 oFileee.Flush();
                 oFileee.Close();
                bool da = true;
                logged_in = false;
                odlogiraj(da);
            }

            else if(izbor == 7189){
                if(is_root(username) == true){
                    Console.WriteLine("Are you sure you want to generate 110 players?");
                    string ans = Console.ReadLine();
                    if(ans=="Yes" || ans=="yes"){
                    Temporary();
                    }
                    
                }
                else {
                    Console.WriteLine("Only root users can use command 7189");
                    izbornik();
                }
                
            }
            

            }
        }
        else{
            Console.WriteLine("Pogresno korisnicko ime i/ili lozinka!");
            izbornik();
        }
        }
        }
    }
}
