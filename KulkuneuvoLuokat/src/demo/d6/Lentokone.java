package demo.d6;

/**
 * @author Bulbasaur
 * @version 17.3.2019
 *Kulkuneuvot luokan alainen Lentokone luokka
 */
public class Lentokone implements Kulkuneuvo{

        int nopeus = 0;
        int matkustajamaara = 0;
        int siipienvali = 0;
        
        
        /**
         * @param args ei käytössä
         */
        public static void main(String[] args) {
            Lentokone lentokone = new Lentokone();
            lentokone.nopeus(200);
            lentokone.matkustajamaara(50);
            lentokone.siipienvali(50);
        }


        @Override
        public void nopeus(int uusiarvo) {
            nopeus = uusiarvo;
            System.out.println("Nopeus on " + nopeus + " km/h"); 
        }


        @Override
        public void matkustajamaara(int uusiarvo) {
            matkustajamaara = uusiarvo;
            System.out.println("Lentokoneessa on " + matkustajamaara + " matkustajaa");
        }
        
        
        /**
         * @param uusiarvo mainissa määritetty arvo siipienvälille
         */
        public void siipienvali(int uusiarvo) {
            siipienvali = uusiarvo;
            System.out.println("Lentokoneen siipienväli on " + siipienvali + " metriä");
        }
    }
