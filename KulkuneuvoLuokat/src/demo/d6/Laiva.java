package demo.d6;

/**
 * @author Bulbasaur
 * @version 17.3.2019
 * Kulkuneuvot luokan alainen Laiva luokka
 */
public class Laiva implements Kulkuneuvo{
    
 
    int nopeus = 0;
    int matkustajamaara = 0;
    int mastonkorkeus = 0;
    
    
    /**
     * @param args ei käytössä
     */
    public static void main(String[] args) {
        Laiva laiva = new Laiva();
        laiva.nopeus(20);
        laiva.matkustajamaara(300);
        laiva.mastonkorkeus(15);
        
        Laiva paatti = new Laiva();
        paatti.nopeus(10);
        paatti.matkustajamaara(5);
        paatti.mastonkorkeus(3);
       
    }


    @Override
    public void nopeus(int uusiarvo) {
        nopeus = uusiarvo;
        System.out.println("Laivan nopeus on " + nopeus + " solmua"); 
    }


    @Override
    public void matkustajamaara(int uusiarvo) {
        matkustajamaara = uusiarvo;
        System.out.println("Laivassa on " + matkustajamaara + " matkustajaa");
    }
    
    
    /**
     * @param uusiarvo mainissa määritetyt uudet arvot
     */
    public void mastonkorkeus(int uusiarvo) {
        mastonkorkeus = uusiarvo;
        System.out.println("Laivan mastonkorkeus on " + mastonkorkeus + " metriä");
    }
}
