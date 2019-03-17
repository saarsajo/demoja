using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;
/// <summary>
/// @author Markku Lehtonen Santeri Saarinen
/// @version 0.1
/// Pelin koodi pohjautuu Jypeli-nuorten Peliohjelmointikurssin 
/// ja Antti-Jussi Lakasen luentomateriaaleihin
/// Musiikki : SOS - 
/// 
/// </summary>
public class LaufKelner2_3 : PhysicsGame
{
    SoundEffect huuto = LoadSoundEffect("huuto");
    SoundEffect peto = LoadSoundEffect("peto");
    SoundEffect kuolema = LoadSoundEffect("kuolema");
    SoundEffect kupla = LoadSoundEffect("kupla");
    SoundEffect step = LoadSoundEffect("step");
    SoundEffect gulp1 = LoadSoundEffect("gulp1");
    SoundEffect gulp2 = LoadSoundEffect("gulp2");
    SoundEffect gulp3 = LoadSoundEffect("gulp3");
    SoundEffect hyppyaani = LoadSoundEffect("hyppy");

    const double KENTANLEVEYS = 800;
    const double KENTANKORKEUS = 600;

    const double RUUDUN_LEVEYS = 40;
    const double RUUDUN_KORKEUS = 40;

    const double hyppy = 500;
    const double nopeus = 150;

    PlatformCharacter pelaaja1;

    Image pelaajanKuva = LoadImage("pelaaja1");
    
    IntMeter pelaajan1Pisteet;

    int maxPisteet = 999999999;

    int pElamaMax = 5;

    int pElamaMin = 0;

    int kElamaMax = 50;

    int kElamaMin = 0;

    private Image[] klausKavely = LoadImages("klaus01", "klaus02", "klaus03", "klaus04");

    private Image[] klausPaikallaan = LoadImages("pelaaja1");

    private Image[] klausHyppy = LoadImages("klaus01");

    public override void Begin()
    {
        Gravity = new Vector(0, -1000);

        LuoKentta();

        lisaaNappaimet(pelaaja1);

        Camera.Follow(pelaaja1);
        Camera.ZoomFactor = 5;
        Camera.StayInLevel = true;

        lisaaLaskurit();
    }


    void LuoKentta()
    {
        TileMap ruudut = TileMap.FromLevelAsset("kentta1");
        ruudut.SetTileMethod('x', luoLattia);
        ruudut.SetTileMethod('Z', luoTaso);
        ruudut.SetTileMethod('X', luoLaava);
        ruudut.SetTileMethod('P', LuoPelaaja1);
        ruudut.SetTileMethod('V', luoVastustaja);
        ruudut.SetTileMethod('K', LuoKokki);
        ruudut.SetTileMethod('*', luoPiste);
        ruudut.SetTileMethod('S', luoSalaisuus);
        ruudut.SetTileMethod('#', luoBurger);
        ruudut.Execute(RUUDUN_LEVEYS, RUUDUN_KORKEUS);
        Level.CreateBorders();
        Level.Background.CreateGradient(Color.Yellow, Color.OrangeRed);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="paikka"></param>
    /// <param name="leveys"></param>
    /// <param name="korkeus"></param>
    void LuoPelaaja1(Vector paikka, double leveys, double korkeus)
    {
        pelaaja1 = new PlatformCharacter(21, 30);
        pelaaja1.AnimWalk = new Animation(klausKavely);
        pelaaja1.AnimWalk.FPS = 10;
        pelaaja1.AnimIdle = new Animation(klausPaikallaan);
        pelaaja1.AnimJump = new Animation(klausHyppy);
        pelaaja1.Image = pelaajanKuva;
        pelaaja1.Position = paikka;
        pelaaja1.Mass = 4.0;

        IntMeter pelaaja1Elama = new IntMeter(pElamaMax, pElamaMin, pElamaMax);
        pelaaja1Elama.LowerLimit += delegate { pelaaja1.Destroy(); MediaPlayer.Play("havio"); };

        ProgressBar pelaajaElamaPalkki = new ProgressBar(40, 5);

        pelaajaElamaPalkki.Y = pelaajaElamaPalkki.Y + 35;
        pelaajaElamaPalkki.BindTo(pelaaja1Elama);
        pelaajaElamaPalkki.Color = Color.OrangeRed;
        pelaajaElamaPalkki.BorderColor = Color.DarkRed;
        pelaaja1.Add(pelaajaElamaPalkki);
        while( pelaaja1Elama<3)
        {
            pelaajaElamaPalkki.Color = Color.Green;
        }

        AddCollisionHandler(pelaaja1, "Piste", tormaaPiste);
        AddCollisionHandler(pelaaja1, "Salaisuus", TormaaSalaisuus);
        AddCollisionHandler(pelaaja1, "puukko", delegate (PhysicsObject pelaaja1, PhysicsObject puukko) { puukkoOsuuPelaaja1(pelaaja1, puukko, pelaaja1Elama); });
        AddCollisionHandler(pelaaja1, "Burge", delegate (PhysicsObject pelaaja1, PhysicsObject Burge) { tormaaBurgeriin(pelaaja1, Burge, pelaaja1Elama); });
        AddCollisionHandler(pelaaja1, "vastustaja", delegate (PhysicsObject pelaaja1, PhysicsObject vastustaja) { tormaaVastustajaan(pelaaja1, vastustaja, pelaaja1Elama); });
        AddCollisionHandler(pelaaja1, "Kokki", tormaaKokkiin);
        AddCollisionHandler(pelaaja1, "laava", tormaaLaava);
        AddCollisionHandler(pelaaja1, "taso", tormaaTaso);
        Add(pelaaja1);


    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="paikka"></param>
    /// <param name="leveys"></param>
    /// <param name="korkeus"></param>
    void LuoKokki(Vector paikka, double leveys, double korkeus, PhysicsObject Piste)
    {

        PlatformCharacter kokki = new PlatformCharacter(RUUDUN_LEVEYS*0.75, RUUDUN_KORKEUS);
        
        kokki.Position = paikka;
        kokki.Image = LoadImage("kokki");
        kokki.Tag = "Kokki";

        FollowerBrain kokkiSeuraaAivot = new FollowerBrain(pelaaja1);
        kokkiSeuraaAivot.Speed = 150;
        kokkiSeuraaAivot.DistanceFar = 400;
        kokki.Brain = kokkiSeuraaAivot;
        kokkiSeuraaAivot.DistanceToTarget.AddTrigger(200, TriggerDirection.Down, soitaKauhua);

        IntMeter kokkiElama = new IntMeter(kElamaMax, kElamaMin, kElamaMax);
        kokkiElama.LowerLimit += delegate {kokki.Destroy(); pisteetPutoaa(Piste); };

        ProgressBar elamaPalkki = new ProgressBar(40, 5);
        
        elamaPalkki.Y = elamaPalkki.Y+35;
        elamaPalkki.BindTo(kokkiElama);
        elamaPalkki.Color = Color.Black;
        elamaPalkki.BorderColor = Color.DarkRed;
        kokki.Add(elamaPalkki);


        AddCollisionHandler(kokki, "lautanen", delegate(PhysicsObject tormaaja, PhysicsObject lautanen) { KokkiTormaaLautanen(tormaaja, lautanen, kokkiElama); });
        Timer heittoAjastin = new Timer();
        heittoAjastin.Interval = 1.0;
        heittoAjastin.Timeout += delegate
          {
              heitaPuukko(kokki);
          };
        heittoAjastin.Start();

        Add(kokki);
    }

    public void taustaMusiikki()
    {
        MediaPlayer.Play("sos");
        MediaPlayer.IsRepeating = true;
    }
    public void soitaKauhua()
    {
        MediaPlayer.Play("jaws");
    }
    public void luoLattia(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject lattia = PhysicsObject.CreateStaticObject(leveys, korkeus);
        lattia.Position = paikka;
        lattia.Image = LoadImage("lattia");
        lattia.Tag = "lattia";
        Add(lattia);
    }


    public void lautanenOsuu(PhysicsObject lautanen, PhysicsObject lattia)
    {
        lautanen.Destroy();
    }


    public void luoLaava(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject laava = PhysicsObject.CreateStaticObject(leveys, korkeus);
        laava.Position = paikka;
        laava.Image = LoadImage("laava");
        laava.Tag = "laava";
        Add(laava);
    }


    public void luoTaso(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject taso = PhysicsObject.CreateStaticObject(leveys, korkeus);
        taso.Position = paikka;
        taso.Color = RandomGen.NextColor();
        taso.Tag = "taso";
        ///taso.Image = LoadImage("");
        Add(taso);
    }


    public void tormaaTaso(PhysicsObject pelaaja1, PhysicsObject taso)
    {
            MediaPlayer.Play("sos");       
    }


    public void HeitaLautanen(PhysicsObject Pelaaja1, String tag)
    {
        PhysicsObject lautanen = new PhysicsObject(11, 9, Shape.Circle);
        lautanen.Position = Pelaaja1.Position*0.995;
        lautanen.IgnoresCollisionResponse = true;
        double xSuunta = 700;
        if (Pelaaja1.Velocity.X == 0)
            {
	            xSuunta *= -1;
                lautanen.Position = Pelaaja1.Position * 1.005;
            }


        lautanen.Image = LoadImage("lautanen");
        lautanen.Hit(new Vector(xSuunta, 100));
        lautanen.Tag = tag;
        lautanen.MaximumLifetime = TimeSpan.FromSeconds(1.0);
        lautanen.Tag = "lautanen";
        AddCollisionHandler(lautanen, "lattia", lautanenOsuu);
        AddCollisionHandler(lautanen, "puukko", puukkoOsuuLautanen);
        Add(lautanen);
	    }


    void luoVastustaja(Vector paikka, double leveys, double korkeus)
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
        PlatformCharacter vastustaja = new PlatformCharacter(25, 35);

        vastustaja.Position = paikka;
        vastustaja.Image = arvottuKuva;
        PlatformWandererBrain vihuAly = new PlatformWandererBrain();
        
        vastustaja.Brain = vihuAly;
        vastustaja.Tag = "vastustaja";

        AddCollisionHandler(vastustaja, "lautanen", vastTormaaLautanen);
        Add(vastustaja);
    }


    void heitaPuukko(PlatformCharacter kokki)
    {

        PhysicsObject puukko = new PhysicsObject(10,10);
        puukko.IgnoresCollisionResponse = true;
        puukko.Image = LoadImage("puukko");
        puukko.Tag = "puukko";
        kokki.Throw(puukko, Angle.FromDegrees(50), (200));
        AddCollisionHandler(puukko, "lattia", puukkoOsuu);
        AddCollisionHandler(puukko, "lautanen", puukkoOsuuLautanen);
    }


public void puukkoOsuu(PhysicsObject puukko, PhysicsObject lattia)
    {
        puukko.Destroy();
    }


    public void puukkoOsuuPelaaja1(PhysicsObject pelaaja1, PhysicsObject puukko, IntMeter pelaaja1Elama)
    {
        puukko.Destroy();
        pelaaja1Elama.Value--;
    }


    public void puukkoOsuuLautanen(PhysicsObject lautanen, PhysicsObject puukko)
    {
        lautanen.Destroy();
        puukko.Destroy();
    }


    public void KokkiTormaaLautanen(PhysicsObject kokki, PhysicsObject lautanen, IntMeter kokkiElama)
    {
        kokki.Image = LoadImage("monster");
        peto.Play();
        kokki.Height = 50;
        kokki.Width = 45;
        kokki.Mass = 5.0;
        lautanen.Destroy();
        kokkiElama.Value--;
    }


    void luoPiste(Vector paikka, double leveys, double korkeus)
    {
        List<Image> kuvalist = new List<Image>();
        kuvalist.Add(LoadImage("viina1"));
        kuvalist.Add(LoadImage("viina2"));
        kuvalist.Add(LoadImage("viina3"));
        Image arvottuPisteKuva = RandomGen.SelectOne<Image>(kuvalist);

        PhysicsObject Piste = PhysicsObject.CreateStaticObject(RUUDUN_LEVEYS/4, RUUDUN_KORKEUS/2);
        Piste.IgnoresCollisionResponse = true;
        Piste.Position = paikka;
        Piste.Image = arvottuPisteKuva;
        Piste.Tag = "Piste";
        Add(Piste);
    }


    void pisteetPutoaa(PhysicsObject Piste)
    {
        Piste.IgnoresGravity= false;
    }


    public void luoSalaisuus(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject Salaisuus = PhysicsObject.CreateStaticObject(RUUDUN_LEVEYS/2, RUUDUN_KORKEUS/2);
        Salaisuus.IgnoresCollisionResponse = true;
        Salaisuus.Position = paikka;
        Salaisuus.Image = LoadImage("salaisuus");
        Salaisuus.Tag = "Salaisuus";
        Add(Salaisuus);
    }


    public void luoBurger(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject Burge = PhysicsObject.CreateStaticObject(20, 20);
        Burge.IgnoresCollisionResponse = true;
        Burge.Position = paikka;
        Burge.Image = LoadImage("burge");
        Burge.Tag = "Burge";
        Add(Burge);
    }


    public void liikuta(PlatformCharacter pelaaja1, double nopeus)
    {
        pelaaja1.Walk(nopeus);
    }


    public void hyppaa(PlatformCharacter pelaaja1, double hyppy)
    {
        pelaaja1.Jump(hyppy);
        hyppyaani.Play();
    }


    public void lisaaNappaimet(PlatformCharacter Pelaaja1)
    {
        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Up, ButtonState.Pressed, hyppaa, "Pelaaja hyppää", Pelaaja1, hyppy);
        Keyboard.Listen(Key.Left, ButtonState.Down, liikuta,"Liikkuu vasemmalle" ,Pelaaja1, (-nopeus));
        Keyboard.Listen(Key.Right, ButtonState.Down, liikuta, "Liikkuu oikealle", Pelaaja1, (nopeus));
        Keyboard.Listen(Key.Space, ButtonState.Pressed, HeitaLautanen, "Heita ammus", Pelaaja1, "pelaajanAmmus");
    }
    

    void lisaaLaskurit()
    {
        pelaajan1Pisteet = luoPisteLaskuri(Screen.Right - 100.0, Screen.Top - 100.0, maxPisteet);
    }


    IntMeter luoPisteLaskuri(double x, double y, int maxPisteet)
    {
        IntMeter PisteLaskuri = new IntMeter(0);
        PisteLaskuri.MaxValue = maxPisteet;
        Label naytto = new Label();
        naytto.BindTo(PisteLaskuri);
        naytto.X = x;
        naytto.Y = y;
        naytto.TextColor = Color.DarkRed;
        naytto.BorderColor = Color.Black;
        naytto.Color = Color.Gray;
        Add(naytto);
        return PisteLaskuri;
    }


    public void tormaaPiste(PhysicsObject pelaaja1, PhysicsObject piste)
    {
        MessageDisplay.Add("Keräsit Viinaa!");
        piste.Destroy();
        pelaajan1Pisteet.Value++;
        gulp1.Play();
    }


    public void TormaaSalaisuus(PhysicsObject pelaaja1, PhysicsObject salaisuus)
    {
        MessageDisplay.Add("Keräsit SATA pistettä!");
        wheee.Play();
        salaisuus.Destroy();
        pelaajan1Pisteet.Value= pelaajan1Pisteet.Value + 100;
    }


    public void tormaaBurgeriin(PhysicsObject pelaaja1, PhysicsObject burge, IntMeter pelaaja1Elama)
    {
        MessageDisplay.Add("Keräsit burgerin!");
        burge.Destroy();
        pelaaja1Elama.Value++;
    }


    public void tormaaVastustajaan(PhysicsObject pelaaja1, PhysicsObject vastustaja, IntMeter pelaaja1Elama)
    {
        pelaaja1Elama.Value--;
    }


    public void tormaaKokkiin(PhysicsObject pelaaja1, PhysicsObject kokki)
    {
        pelaaja1.Destroy();
        MessageDisplay.Add("Kokki tappoi sinut");
    }


    public void tormaaLaava(PhysicsObject pelaaja1, PhysicsObject laava)
    {
        MessageDisplay.Add("Menit sitten laavaan uimaan!");
        laava.IgnoresCollisionResponse = true;
        pelaaja1.Destroy();
    }
    

    public void vastTormaaLautanen(PhysicsObject vastustaja, PhysicsObject lautanen)
    {
        lautanen.Destroy();
        vastustaja.Destroy();
    }


}