package demo.d9;
import java.util.Scanner;
import java.io.PrintWriter;
import java.io.FileNotFoundException;


/**
 * @author Bulbasaur
 * @version 11.3.2019
 *Ohjelma joka kirjoittaa tekstitiedostoon käyttäjän puolesta
 */
public class Kirjoittaminen {

    static String teksti;    
    static double maara;
    static double virheLuku = 0;   
    static Scanner syote = new Scanner(System.in);
    
    
    private static void SyoteSulku() {
        syote.close();        
    }


    /// Testeissä testataan minkä syötteen käyttäjä antaa. Jos väliltä 0-100 ohjelma jatkaa normaalia käyttöä.
    ///Jos 666 ohjelma päättyy. Ja jos jotain muuta pyydetään määrää uudestaan.
    private static void Tulosta(String tekst, double maar) {
        int i = 0;
        if (maar == 666) {
            System.out.println("Oot sää kyllä yks paholaasen poika");
            SyoteSulku();
        }
        else if (maar > 0 && maar <= 100) {
                try(PrintWriter kirjoittaja = new PrintWriter("d9KirjoittaminenTulostus.txt")) {
                    while (i<=maar) {
                        kirjoittaja.println(tekst);
                        i++;
                    }
                }
                catch (FileNotFoundException e) {
                    e.printStackTrace();
                }
            System.out.println("Tulostus tehty tiedostoon.");
            virheLuku = 0;
            SyoteSulku();
        }
        else {
            System.out.println("Väliltä 0-100 senkin tonttu!");
            virheLuku = 1;
            KysyMaara();
        }
    }


    private static void KysyTeksti() {
        System.out.print("Mitä haluat tulostaa?: ");
        teksti = syote.nextLine();
    }
    

    private static void KysyMaara() {
        if (virheLuku == 0) {
            System.out.print("Kuinka paljon tulostetaan väliltä 0-100?: ");
            maara = syote.nextDouble();
            Tulosta (teksti, maara);
        }
        else {
            System.out.print("Kuinka paljon tulostetaan väliltä 0-100? Jos ei onnistu näin vaikea asia niin kirjoita: 666: ");
            maara = syote.nextDouble();
            Tulosta (teksti, maara);
        }
    }

    
    /**
     * @param args ei käytössä
     */
    public static void main(String[] args) {
        KysyTeksti();
        KysyMaara();
    }
}
