package demo7;
import java.util.Random;

/**
 * @author Bulbasaur
 * @version 23.2.2019
 *Ohjelma joka arpoo satunnaisen rahasumman väliltä 0 - 20 ja laskee kuinka painavan paketin voi lähettää
 */
public class Postimaksu {
    
    static Random rand = new Random();
    private static int rahasumma = rand.nextInt(20);
    
    
    /**
     * @param raha on fygen määrä
     * @return kirjeenpaino
     * @example
     * <pre name="test">
     * suurinKirjeenPaino(0) === 0;
     * suurinKirjeenPaino(1) === 1000;
     * suurinKirjeenPaino(3) === 2500;
     * suurinKirjeenPaino(9) === 10000;
     * </pre>
     */
    public static int suurinPaketinPaino(int raha)   {
        if ( raha >= 16 )               return  30000;
        else if ( raha >= 12 )          return  18000;
        else if ( raha >= 8 )           return  10000;
        else if ( raha >= 4 )           return  6000;
        else if ( raha >= 2 )           return  2500;
        else if ( raha >= 1 )           return  1000; 
        else                            return  0;
      }

    
    /**
     * @param args ei käytössä
     */
    public static void main(String[] args) {
        suurinPaketinPaino(rahasumma);
        System.out.println("Summalla " + rahasumma + " saa lähettää ainakin " + suurinPaketinPaino(rahasumma) + " gramman painoisen kirjeen.");
    }

}