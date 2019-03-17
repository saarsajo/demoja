using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;
/// <summary>
/// @author Markku Lehtonen Santeri Saarinen
/// @version 22.4.2016
/// maolleht@student.jyu.fi
/// santeri93saarinen@hotmail.com
/// Pelissä pelataan tarjoilijalla nimeltään Klauss. Tarkoituksena on kerätä viinaa, 
/// heitellä asiakkaita lautasilla, syödä purilaisia ja kukistaa kokki.
/// Pelin koodi pohjautuu Jypeli-nuorten Peliohjelmointikurssin 
/// ja Antti-Jussi Lakasen luentomateriaaleihin
/// Musiikit : SOS - Es Wird Viel Passier'n
/// Jaws-theme song
/// </summary>

///<summary>
///Pelin ääniefektejä, animaatioita, laskureita ja vakioita
/// </summary>
public class LaufKelner2_3 : PhysicsGame
{
    private SoundEffect huuto = LoadSoundEffect("huuto");
    private SoundEffect peto = LoadSoundEffect("peto");
    private SoundEffect omnom = LoadSoundEffect("omnom");
    private SoundEffect step = LoadSoundEffect("step");
    private SoundEffect gulp1 = LoadSoundEffect("gulp1");
    private SoundEffect hyppyaani = LoadSoundEffect("HYPPY");
    private SoundEffect anykey = LoadSoundEffect("anykey");
    private SoundEffect havio = LoadSoundEffect("havio");
    private SoundEffect havio2 = LoadSoundEffect("havio2");
    private SoundEffect heittoaani = LoadSoundEffect("heittoaani");
    private SoundEffect kavely = LoadSoundEffect("kavely");
    private SoundEffect kipu = LoadSoundEffect("kipu");
    private SoundEffect kuolema1 = LoadSoundEffect("kuolema1");
    private SoundEffect kuolema2 = LoadSoundEffect("kuolema2");
    private SoundEffect kuplinta = LoadSoundEffect("kuplinta");
    private SoundEffect lautanenrikki = LoadSoundEffect("lautanenrikki");
    private SoundEffect wheee = LoadSoundEffect("wheee");

    private const double KENTANLEVEYS = 800;
    private const double KENTANKORKEUS = 600;
    private const double RUUDUN_LEVEYS = 40;
    private const double RUUDUN_KORKEUS = 40;

    private const double HYPPY = 500;
    private const double NOPEUS = 150;
    private const double MASSA = 4;

    private PlatformCharacter pelaaja1;
    private Image pelaajanKuva = LoadImage("pelaaja1");

    private IntMeter pelaajan1Pisteet;
    private IntMeter pelaaja1Elama;
    private IntMeter lautastenMaara;
    private const int MAXPISTEET = 999999999;

    private EasyHighScore topLista = new EasyHighScore();
    // Pelaajan, kokin ja vastustajien Max ja Min elämät
    private const int PELAMAMAX = 5;
    private const int ELAMAMIN = 0;
    private const int KELAMAMAX = 15;
    private const int VELAMAMIN = 3;
    //Lautasten max ja min määrä
    private const int LMAX = 30;
    private const int LMIN = 0;

    private Image[] klausKavely = LoadImages("klaus01", "klaus02", "klaus03", "klaus04");
    private Image[] klausPaikallaan = LoadImages("pelaaja1");
    private Image[] klausHyppy = LoadImages("klaus01");
    private Image[] disko = LoadImages("happy1", "happy2");

    /// <summary>
    /// Laitetaan peli käyntiin
    /// </summary>
    public override void Begin()
    {
        AlkuValikko();
    }

    /// <summary>
    /// Luodaan alkuvalikko
    /// </summary>
    private void AlkuValikko()
    {
        Level.Background.CreateGradient(Color.Yellow, Color.OrangeRed);
        MultiSelectWindow alkuValikko = new MultiSelectWindow("~LAUF KELNER LAUF~", "Aloita seikkailusi", "Suunnilleen parhaat pisteet", "Luovuta... luuseri");
        alkuValikko.AddItemHandler(0, PelinAloitus);
        alkuValikko.AddItemHandler(1, topLista.Show);
        alkuValikko.AddItemHandler(2, Exit);
        alkuValikko.BorderColor = Color.Black;
        alkuValikko.Color = Color.DarkGray;
        alkuValikko.ActiveColor = Color.OrangeRed;
        alkuValikko.SelectionColor = Color.DarkRed;
        Add(alkuValikko);
    }


    ///<summary>
    ///Luodaan painovoima ja kamera, sekä kutsutaan aliohjelmia LuoKentta() ja lisaaNappaimet ja lisaaLaskurit
    /// </summary>
    private void PelinAloitus()
    {
        ClearAll();
        Gravity = new Vector(0, -1000);

        LuoKentta();
        LisaaNappaimet(pelaaja1);

        Camera.Follow(pelaaja1);
        Camera.ZoomFactor = 3.5;
        Camera.StayInLevel = true;

        LisaaLaskurit();
    }
    /// <summary>
    /// Aliohjelman, joka luo pelikentän ja jonka sisällä on TileMap jolla objektit lisätään pelimaailmaan
    /// </summary>
    private void LuoKentta()
    {
        TileMap ruudut = TileMap.FromLevelAsset("kentta1");
        ruudut.SetTileMethod('P', LuoPelaaja1);
        ruudut.SetTileMethod('x', LuoLattia);
        ruudut.SetTileMethod('Z', LuoTaso);
        ruudut.SetTileMethod('X', LuoLaava);
        ruudut.SetTileMethod('V', LuoVastustaja);
        ruudut.SetTileMethod('S', LuoSalaisuus);
        ruudut.SetTileMethod('#', LuoBurger);
        ruudut.SetTileMethod('*', LuoPiste);
        ruudut.SetTileMethod('K', LuoKokki);
        ruudut.SetTileMethod('L', LuoLautaskasa);
        ruudut.Execute(RUUDUN_LEVEYS, RUUDUN_KORKEUS);
        Level.CreateBorders();
        Level.Background.CreateGradient(Color.Yellow, Color.OrangeRed);
    }


    /// <summary>
    /// Aliohjelma joka luo pelaaja1:n, sen elämäpalkin ja kutsuu CollissionHandlereitä
    /// </summary>
    /// <param Paikka johon pelaaja1 luodaan="paikka"></param>
    /// <param name="leveys">pelaaja1 leveys</param>
    /// <param name="korkeus">pelaaja1 korkeus</param>
    private void LuoPelaaja1(Vector paikka, double leveys, double korkeus)
    {
        pelaaja1 = new PlatformCharacter(RUUDUN_LEVEYS * 0.55, RUUDUN_KORKEUS * 0.75);
        pelaaja1.AnimWalk = new Animation(klausKavely);
        pelaaja1.AnimWalk.FPS = 10;
        pelaaja1.AnimIdle = new Animation(klausPaikallaan);
        pelaaja1.AnimJump = new Animation(klausHyppy);
        pelaaja1.Image = pelaajanKuva;
        pelaaja1.Position = paikka;
        pelaaja1.Mass = MASSA;

        pelaaja1Elama = new IntMeter(PELAMAMAX, ELAMAMIN, PELAMAMAX);///Pelaajan elämäpalkki, jonka max arvo on PELAMAMAX ja min arvo pElamaMin
        pelaaja1Elama.LowerLimit += delegate { PelaajaKuolee(); };
        ProgressBar pelaajaElamaPalkki = new ProgressBar(RUUDUN_LEVEYS, RUUDUN_KORKEUS * 0.1);

        pelaajaElamaPalkki.Y = pelaajaElamaPalkki.Y + RUUDUN_KORKEUS * 0.8;
        pelaajaElamaPalkki.BindTo(pelaaja1Elama);
        pelaajaElamaPalkki.BarColor = Color.Green;
        pelaajaElamaPalkki.Color = Color.Black;
        pelaajaElamaPalkki.BorderColor = Color.Black;
        pelaaja1.Add(pelaajaElamaPalkki);

        AddCollisionHandler(pelaaja1, "Piste", TormaaPiste);
        AddCollisionHandler(pelaaja1, "LautasKasa", TormaaLautasKasa);
        AddCollisionHandler(pelaaja1, "Salaisuus", TormaaSalaisuus);
        AddCollisionHandler(pelaaja1, "puukko", delegate (PhysicsObject pelaaja1, PhysicsObject puukko) { PuukkoOsuuPelaaja1(pelaaja1, puukko, pelaaja1Elama); });
        AddCollisionHandler(pelaaja1, "Burge", delegate (PhysicsObject pelaaja1, PhysicsObject Burge) { TormaaBurgeriin(pelaaja1, Burge, pelaaja1Elama); });
        AddCollisionHandler(pelaaja1, "vastustaja", delegate (PhysicsObject pelaaja1, PhysicsObject vastustaja) { TormaaVastustajaan(pelaaja1, vastustaja, pelaaja1Elama); });
        AddCollisionHandler(pelaaja1, "Kokki", TormaaKokkiin);
        AddCollisionHandler(pelaaja1, "laava", TormaaLaava);
        AddCollisionHandler(pelaaja1, "taso", TormaaTaso);
        Add(pelaaja1);
    }


    /// <summary>
    /// Aliohjelma, joka luo lautasen ja heittää sen kun välilyöntiä painetaan
    /// </summary>
    /// <param name="tag">lautasen nimi</param>

    private void HeitaLautanen(PhysicsObject pelaaja1, String tag)
    {
        PhysicsObject lautanen = new PhysicsObject(RUUDUN_LEVEYS * 0.25, RUUDUN_KORKEUS * 0.25);
        lautastenMaara = new IntMeter(LMAX, LMIN, LMAX);///Pelaajan lautasten määrä, jonka max arvo on LMAX ja min arvo LMin
        lautanen.IgnoresCollisionResponse = true;
        lautanen.Image = LoadImage("lautanen");
        lautanen.Tag = "lautanen";
        if (pelaaja1Elama >= ELAMAMIN) //(&& lautastenMaara > LMIN) TÄMÄ PITÄÄ KORJATA SILLAI ETTÄ ETTÄ JOS LAUTASET ON NOLLA EI VOI HEITTÄÄ!!!
        {
            this.pelaaja1.Throw(lautanen, Angle.FromDegrees(20), (500));
            heittoaani.Play();
            lautastenMaara.Value--;
        }
        AddCollisionHandler(lautanen, "lattia", LautanenOsuu);
        AddCollisionHandler(lautanen, "lautanen", PuukkoOsuuLautanen);
    }


    /// <summary>
    /// Aliohjelma joka luo loppuvastustaja kokin, sekä kokin elämäpalkin.
    /// </summary>
    /// <param name="paikka">Paikka johon kokki luodaan</param>
    /// <param name="leveys">kokin leveys</param>
    /// <param name="korkeus">kokin korkeus</param>
    private void LuoKokki(Vector paikka, double leveys, double korkeus)
    {
        PlatformCharacter kokki = new PlatformCharacter(RUUDUN_LEVEYS * 0.65, RUUDUN_KORKEUS * 0.85);

        kokki.Position = paikka;
        kokki.Image = LoadImage("kokki");
        kokki.Tag = "Kokki";
        IntMeter kokkiElama = new IntMeter(KELAMAMAX, ELAMAMIN, KELAMAMAX);
        kokkiElama.LowerLimit += delegate { pelaajan1Pisteet.Value = pelaajan1Pisteet.Value + 50; kokki.Destroy(); VoititPelin(); };

        ProgressBar elamaPalkki = new ProgressBar(RUUDUN_LEVEYS, RUUDUN_KORKEUS * 0.1);
        elamaPalkki.Y = elamaPalkki.Y + RUUDUN_KORKEUS*0.85;
        elamaPalkki.BindTo(kokkiElama);
        elamaPalkki.Color = Color.Black;
        elamaPalkki.BorderColor = Color.DarkRed;
        kokki.Add(elamaPalkki);
        #region aivot
        FollowerBrain kokkiSeuraaAivot = new FollowerBrain(pelaaja1); ///Kokin tekoäly ja sen arvot
        kokkiSeuraaAivot.Speed = NOPEUS*1.3;
        kokkiSeuraaAivot.DistanceFar = RUUDUN_LEVEYS*10;
        kokki.Brain = kokkiSeuraaAivot;
        kokkiSeuraaAivot.DistanceToTarget.AddTrigger(RUUDUN_LEVEYS*16, TriggerDirection.Down, SoitaKauhua);
        AddCollisionHandler(kokki, "lautanen", delegate (PhysicsObject tormaaja, PhysicsObject lautanen) { KokkiTormaaLautanen(tormaaja, lautanen, kokkiElama); });
        #endregion
        Timer heittoAjastin = new Timer();///Ajastin, jonka mukaan kokki heittää veitsiä
        heittoAjastin.Interval = 1.0;
        heittoAjastin.Timeout += delegate { if (kokkiElama > 0) { HeitaPuukko(kokki); } };
        heittoAjastin.Start();
        Add(kokki);
    }


    /// <summary>
    /// Aliohjelma luo vastustajan ja arpoo sille randomisti kuvan
    /// </summary>
    /// <param name="paikka">Paikka, johon vastustaja luodaan</param>
    /// <param name="leveys">Vastustajan leveys</param>
    /// <param name="korkeus">Vastustajan korkeus</param>
    private void LuoVastustaja(Vector paikka, double leveys, double korkeus)
    {
        List<Image> kuvat = new List<Image>();
        kuvat.Add(LoadImage("juoppo"));
        kuvat.Add(LoadImage("juoppo2"));
        kuvat.Add(LoadImage("juoppo3"));
        kuvat.Add(LoadImage("puuma"));
        kuvat.Add(LoadImage("syoppo"));
        kuvat.Add(LoadImage("syoppo2"));
        kuvat.Add(LoadImage("syoppo3"));
        Image arvottuKuva = RandomGen.SelectOne<Image>(kuvat);
        PlatformCharacter vastustaja = new PlatformCharacter(RUUDUN_LEVEYS * 0.65, RUUDUN_KORKEUS * 0.85);

        IntMeter vastustajaElama = new IntMeter(VELAMAMIN, ELAMAMIN, VELAMAMIN);
        vastustajaElama.LowerLimit += delegate { vastustaja.Destroy(); pelaajan1Pisteet.Value++;};
        ProgressBar vasElamaPalkki = new ProgressBar(RUUDUN_LEVEYS, RUUDUN_KORKEUS * 0.1);
        vasElamaPalkki.Y = vasElamaPalkki.Y + RUUDUN_KORKEUS * 0.85;
        vasElamaPalkki.BindTo(vastustajaElama);
        vasElamaPalkki.Color = Color.Black;
        vasElamaPalkki.BorderColor = Color.DarkRed;
        vastustaja.Add(vasElamaPalkki);

        vastustaja.Position = paikka;
        vastustaja.Image = arvottuKuva;
        PlatformWandererBrain vihuAly = new PlatformWandererBrain();
        vihuAly.Speed = NOPEUS*0.7;
        vastustaja.Brain = vihuAly;
        vastustaja.Tag = "vastustaja";
        AddCollisionHandler(vastustaja, "lautanen", delegate (PhysicsObject tormaaja, PhysicsObject lautanen) { VastTormaaLautanen(tormaaja, lautanen, vastustajaElama); });

        Add(vastustaja);
    }


    /// <summary>
    /// Aliohjelma, joka soittaa jaws-tunnarin
    /// </summary>
    private void SoitaKauhua()
    {
        MediaPlayer.Play("jaws");
    }


    /// <summary>
    /// Aliohjelma luo lattian
    /// </summary>
    /// <param name="paikka">lattian paikka</param>
    /// <param name="leveys">lattian leveys</param>
    /// <param name="korkeus">lattian korkeus</param>
    private void LuoLattia(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject lattia = PhysicsObject.CreateStaticObject(leveys, korkeus);
        lattia.Position = paikka;
        lattia.Image = LoadImage("lattia");
        lattia.Tag = "lattia";
        Add(lattia);
    }


    /// <summary>
    /// Aliohjelma, joka käsittelee lautasen osumista lattiaan
    /// </summary>
    private void LautanenOsuu(PhysicsObject lautanen, PhysicsObject lattia)
    {
        lautanenrikki.Play();
        lautanen.Destroy();
    }


    /// <summary>
    /// Aliohjelma, joka luo laavan
    /// </summary>
    /// <param name="paikka">Laavan sijainti</param>
    void LuoLaava(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject laava = PhysicsObject.CreateStaticObject(leveys, korkeus);
        laava.Position = paikka;
        laava.Image = LoadImage("laava");
        laava.Tag = "laava";
        Add(laava);
    }


    /// <summary>
    /// Aliohjelma, joka luo tason, jolla on erikoisominaisuuksia, kuten animaatio ja oman musiikin aloittaminen (Easter-Egg)
    /// </summary>
    private void LuoTaso(Vector paikka, double leveys, double korkeus)
    {
        PlatformCharacter taso = new PlatformCharacter(leveys, korkeus);
        taso.Position = paikka;
        taso.Color = RandomGen.NextColor();
        taso.Tag = "taso";
        taso.Image = LoadImage("happy1");
        taso.AnimIdle = new Animation(disko);
        taso.AnimIdle.FPS = 8;
        Add(taso);
    }


    /// <summary>
    /// Aliohjelma käsittelee pelaajan törmäystä tasoon
    /// </summary>
    private void TormaaTaso(PhysicsObject pelaaja1, PhysicsObject taso)
    {
        MediaPlayer.Play("sos");
    }


    /// <summary>
    /// Aliohjelma, joka hoitaa kokin "heittämien" puukkojen luonnin ja heittämisen 
    /// </summary>
    private void HeitaPuukko(PlatformCharacter kokki)
    {
        PhysicsObject puukko = new PhysicsObject(RUUDUN_LEVEYS * 0.25, RUUDUN_KORKEUS * 0.25);
        puukko.IgnoresCollisionResponse = true;
        puukko.Image = LoadImage("puukko");
        puukko.Tag = "puukko";
        kokki.Throw(puukko, Angle.FromDegrees(30), (250));
        AddCollisionHandler(puukko, "lattia", PuukkoOsuu);
        AddCollisionHandler(puukko, "lautanen", PuukkoOsuuLautanen);
    }


    /// <summary>
    /// Aliohjelma, joka käsittelee puukon törmäystä lattiaan
    /// </summary>
    private void PuukkoOsuu(PhysicsObject puukko, PhysicsObject lattia)
    {
        puukko.Destroy();
    }


    /// <summary>
    /// Aliohjelma, joka käsittelee puukon osumista pelaajaan
    /// </summary>
    /// <param name="pelaaja1Elama">pelaajan elämät</param>
    private void PuukkoOsuuPelaaja1(PhysicsObject pelaaja1, PhysicsObject puukko, IntMeter pelaaja1Elama)
    {
        puukko.Destroy();
        kipu.Play();
        pelaaja1Elama.Value--;
    }


    /// <summary>
    /// Aliohjelma, joka käsittelee puukon osumista lautaseen
    /// </summary>
    private void PuukkoOsuuLautanen(PhysicsObject lautanen, PhysicsObject puukko)
    {
        lautanen.Destroy();
        puukko.Destroy();
        lautanenrikki.Play();
    }


    /// <summary>
    /// Aliohjelma, joka käsittelee puukon osumista kokkiin
    /// </summary>
    /// <param name="kokkiElama">pelaajan elämät</param>
    private void KokkiTormaaLautanen(PhysicsObject kokki, PhysicsObject lautanen, IntMeter kokkiElama)
    {
        kokki.Image = LoadImage("monster");
        peto.Play();
        kokki.Height = RUUDUN_KORKEUS * 1.25;
        kokki.Width = RUUDUN_LEVEYS * 1.1;
        lautanen.Destroy();
        kokkiElama.Value--;

    }

    /// <summary>
    /// Aliohjelma, joka luo pisteet maailmaan ja arpoo niille kuvan listasta
    /// </summary>
    private void LuoPiste(Vector paikka, double leveys, double korkeus)
    {
        List<Image> kuvalist = new List<Image>();
        kuvalist.Add(LoadImage("viina1"));
        kuvalist.Add(LoadImage("viina2"));
        kuvalist.Add(LoadImage("viina3"));
        Image arvottuPisteKuva = RandomGen.SelectOne<Image>(kuvalist);

        PhysicsObject Piste = PhysicsObject.CreateStaticObject(RUUDUN_LEVEYS * 0.25, RUUDUN_KORKEUS * 0.625);
        Piste.IgnoresCollisionResponse = true;
        Piste.Position = paikka;
        Piste.Image = arvottuPisteKuva;
        Piste.Tag = "Piste";
        Add(Piste);
    }
    private void LuoLautaskasa(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject LautasKasa = PhysicsObject.CreateStaticObject(RUUDUN_LEVEYS * 0.4, RUUDUN_KORKEUS * 0.15);
        LautasKasa.IgnoresCollisionResponse = true;
        LautasKasa.Position = paikka;
        LautasKasa.Image = LoadImage("lautanen");
        LautasKasa.Tag = "LautasKasa";
        Add(LautasKasa);
    }

    /// <summary>
    /// Aliohjelma, joka luo salaisuuden pelimaailmaan (Easter-Egg)
    /// </summary>
    /// <param name="paikka">paikka</param>
    /// <param name="leveys">Salaisuuden leveys</param>
    /// <param name="korkeus">Salaisuuden korkeus</param>
    private void LuoSalaisuus(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject Salaisuus = PhysicsObject.CreateStaticObject(RUUDUN_LEVEYS, RUUDUN_KORKEUS);
        Salaisuus.IgnoresCollisionResponse = true;
        Salaisuus.Position = paikka;
        Salaisuus.Image = LoadImage("salaisuus");
        Salaisuus.Tag = "Salaisuus";
        Add(Salaisuus);
        MediaPlayer.Play("tausta1");
    }


    /// <summary>
    /// Aliohjelma, joka luo Hampurilaisia maailmaan
    /// </summary>
    private void LuoBurger(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject Burge = PhysicsObject.CreateStaticObject(RUUDUN_LEVEYS * 0.5, RUUDUN_KORKEUS * 0.5);
        Burge.IgnoresCollisionResponse = true;
        Burge.Position = paikka;
        Burge.Image = LoadImage("burge");
        Burge.Tag = "Burge";
        Add(Burge);
    }


    /// <summary>
    /// Aliohjelma, joka reagoi Pelaajaa liikuttelevia näppäimiä painamalla ja liikuttaa pelaajaa nopeudella "NOPEUS"
    /// </summary>
    /// <param name="nopeus">Nopeus, jolla pelaajaa liikutetaan</param>
    private void Liikuta(PlatformCharacter pelaaja1, double nopeus)
    {
        pelaaja1.Walk(nopeus);
    }


    /// <summary>
    /// Aliohjelma, joka reagoi Pelaajaa hypyttävää näppäintä painamalla ja hypyttää pelaajaa nopeudella "HYPPY"
    /// </summary>
    /// <param name="HYPPY">Nopeus jolla pelaaja hyppää</param>
    private void Hyppaa(PlatformCharacter pelaaja1, double hyppy)
    {
        pelaaja1.Jump(hyppy);
        hyppyaani.Play();
    }


    /// <summary>
    /// Aliohjelma, joka lisää pelaajaa liikuttelevat näppäimet
    /// </summary>
    private void LisaaNappaimet(PlatformCharacter Pelaaja1)
    {
        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Up, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", Pelaaja1, HYPPY);
        Keyboard.Listen(Key.Left, ButtonState.Down, Liikuta, "Liikkuu vasemmalle", Pelaaja1, (-NOPEUS));
        Keyboard.Listen(Key.Right, ButtonState.Down, Liikuta, "Liikkuu oikealle", Pelaaja1, (NOPEUS));
        Keyboard.Listen(Key.Space, ButtonState.Pressed, HeitaLautanen, "Heita ammus", Pelaaja1, "pelaajanAmmus");
    }


    /// <summary>
    /// Aliohjelma, joka lisää laskurit peliin
    /// </summary>
    private void LisaaLaskurit()
    {
        pelaajan1Pisteet = LuoPisteLaskuri(Screen.Right - 100.0, Screen.Top - 100.0, MAXPISTEET);
        lautastenMaara = LuoLautasLaskuri(Screen.Left + 100.0, Screen.Top - 100.0, LMAX);
    }


    /// <summary>
    /// Aliohjelma, joka luo pistelaskurit
    /// </summary>
    /// <param name="x">x-koordinaatin piste</param>
    /// <param name="y">y-koordinaatin piste</param>
    /// <param name="maxPisteet">Maksimi pistemäärä</param>
    /// <returns></returns>
    private IntMeter LuoPisteLaskuri(double x, double y, int maxPisteet)
    {
        IntMeter pisteLaskuri = new IntMeter(0);
        pisteLaskuri.MaxValue = maxPisteet;
        Widget pisteruutu = new Widget(RUUDUN_LEVEYS * 3, RUUDUN_KORKEUS * 3);
        pisteruutu.X = x;
        pisteruutu.Y = y;
        pisteruutu.Image = LoadImage("aurinko");
        Add(pisteruutu);
        Label pistenaytto = new Label();
        pistenaytto.BindTo(pisteLaskuri);
        pisteruutu.Add(pistenaytto);
        return pisteLaskuri;
    }
    private IntMeter LuoLautasLaskuri(double x, double y, int maxPisteet)
    {
        IntMeter lautasLaskuri = new IntMeter(0);
        lautasLaskuri.MaxValue = maxPisteet;
        Widget pisteruutu = new Widget(RUUDUN_LEVEYS * 3, RUUDUN_KORKEUS * 3);
        pisteruutu.X = x;
        pisteruutu.Y = y;
        pisteruutu.Image = LoadImage("lautanen");
        Add(pisteruutu);
        Label pistenaytto = new Label();
        pistenaytto.BindTo(lautasLaskuri);
        pisteruutu.Add(pistenaytto);
        return lautasLaskuri;
    }


    /// <summary>
    /// Aliohjelma, joka käsittelee pelaajan ja pisteen törmäystä
    /// </summary>
    private void TormaaPiste(PhysicsObject pelaaja1, PhysicsObject piste)
    {
        piste.Destroy();
        pelaajan1Pisteet.Value++;
        gulp1.Play();
    }
    private void TormaaLautasKasa(PhysicsObject pelaaja1, PhysicsObject LautasKasa)
    {
        LautasKasa.Destroy();
        lautastenMaara.Value++;
    }

    /// <summary>
    /// Aliohjelma, joka käsittelee pelaajan ja salaisuuden törmäystä
    /// </summary>
    private void TormaaSalaisuus(PhysicsObject pelaaja1, PhysicsObject salaisuus)
    {
        MessageDisplay.Add("Löysit salaisuuden");
        salaisuus.Destroy();
        pelaajan1Pisteet.Value = pelaajan1Pisteet.Value + 100;
        wheee.Play();
    }


    /// <summary>
    /// Aliohjelma, joka käsittelee pelaajan ja burgerin törmäystä
    /// </summary>
    private void TormaaBurgeriin(PhysicsObject pelaaja1, PhysicsObject burge, IntMeter pelaaja1Elama)
    {
        burge.Destroy();
        pelaaja1Elama.Value++;
        omnom.Play();
    }


    /// <summary>
    /// Aliohjelma, joka käsittelee pelaajan ja vastustajan törmäystä
    /// </summary>
    private void TormaaVastustajaan(PhysicsObject pelaaja1, PhysicsObject vastustaja, IntMeter pelaaja1Elama)
    {
        pelaaja1Elama.Value--;
        kipu.Play();
    }


    /// <summary>
    /// Aliohjelma, joka käsittelee pelaajan ja kokin törmäystä
    /// </summary>
    private void TormaaKokkiin(PhysicsObject pelaaja1, PhysicsObject kokki)
    {
        pelaaja1Elama.Value = 0;
        kuolema1.Play();
    }


    /// <summary>
    /// Aliohjelma, joka käsittelee pelaajan osumista laavaan
    /// </summary>
    private void TormaaLaava(PhysicsObject pelaaja1, PhysicsObject laava)
    {
        laava.IgnoresCollisionResponse = false;
        kuplinta.Play();
        pelaaja1Elama.Value--;
    }


    /// <summary>
    /// Aliohjelma, joka käsittelee vastustajan ja lautasen törmäystä
    /// </summary>
    private void VastTormaaLautanen(PhysicsObject vastustaja, PhysicsObject lautanen, IntMeter vastustajaElama)
    {
        lautanen.Destroy();
        vastustajaElama.Value--;
        lautanenrikki.Play();
        huuto.Play();
    }


    /// <summary>
    /// Aliohjelma, joka käsittelee pelaajan kuoleman ja antaa pelaajan laittaa nimensä top listalle
    /// </summary>
    private void PelaajaKuolee()
    {
        ///topLista.HighScoreWindow.NameInputWindow.Message.Text = "Ihan oikeesti hävisit näin helpossa pelissä!!!\nJa vaan " + pelaajan1Pisteet.Value + "pisteellä!!! Ei kannata kokeilla Dark Soulia...";
        GameObject gameOver = new GameObject(RUUDUN_LEVEYS * 6, RUUDUN_KORKEUS *3.8);
        gameOver.Image = LoadImage("gameOver");
        gameOver.Position = pelaaja1.Position;
        pelaaja1.Destroy();
        MediaPlayer.Play("havio");
        Add(gameOver, 1);
        LataaHighScore();
    }


    /// <summary>
    /// Aliohjelma, joka siirtää pistelaskurista valikkoon
    /// </summary>
    /// <param name="sender">Pakollinen juttu, joka kuuluu sinne</param>
    private void PisteLaskuristaValikkoon(Window sender)
    {
        ClearAll();
        AlkuValikko();
    }


    /// <summary>
    /// Aliohjelma, joka soittaa voittoviisuun ja antaa laittaa pisteet ylös
    /// </summary>
    private void VoititPelin()
    {
        MediaPlayer.Play("voittaja");
        topLista.HighScoreWindow.NameInputWindow.Message.Text = "Onneksi olkoon voitit peli! Sait " + pelaajan1Pisteet.Value+ " pistettä!\nMahtaa mutsis olla ylpeenä!";
        LataaHighScore();
    }


    /// <summary>
    /// Aliohjelma, joka Näyttää pistelaskurin ja palauttaa päävalikkoon
    /// </summary>
    private void LataaHighScore()
    {
        topLista.EnterAndShow(pelaajan1Pisteet.Value);
        topLista.HighScoreWindow.Closed += PisteLaskuristaValikkoon;
    }
}