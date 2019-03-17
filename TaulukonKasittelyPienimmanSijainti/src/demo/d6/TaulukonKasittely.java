package demo.d6;
import java.util.Random;

/**
 * @author Bulbasaur
 * @version 18.2.2019
 *
 */
class TaulukonKasittely {

    static int[] taulukko;
    static int paikka = 0;

    
    public static int[] taulukonLuoja() {
        taulukko = new int[5];
        System.out.println("Taulukossa on luvut: ");
        for(int i=0;i<taulukko.length;i++)
        {
            taulukko[i] = randomLuku();
            System.out.println(taulukko[i]);
        }
        return taulukko;
    }
    

    public static int randomLuku(){
        Random rand = new Random();
        int randomLuku = rand.nextInt(100);
        return randomLuku;
    }
    
    
    /**
     * Palautetaan tieto siitä onko tutkittava vuosi karkausvuosi vai ei
     * @param taulu taulukko
     * @return pienimmän paikka
     * @example
     * <pre name="test">
     *   taulu =[59,32,47,75,64]
     *   pienimmanPaikka(int[] taulu) === Pienin numero on paikassa 1
     *   taulu =[1,50,2,99,14]
     *   pienimmanPaikka(int[] taulu) === Pienin numero on paikassa 0
     *   taulu =[19,15,20,50,10]
     *   pienimmanPaikka(int[] taulu) === Pienin numero on paikassa 4
     * </pre>
     */
    public static int pienimmanPaikka (int[] taulu){
        int j = 0;
        for(int i=0;i<taulu.length;i++)
        {
            if (taulu[i] < taulu[j]){
                paikka = i;
                j = i;
            }
        }
        System.out.println("Pienin numero on paikassa " + paikka);
        return paikka;
    }
    
    /**
     * Palautetaan tieto siitä onko tutkittava vuosi karkausvuosi vai ei
     * @param taulu randomilla luotu taulukko
     * @param pienimmanpaikka paikka josta pienin luku löytyy
     * @example
     * <pre name="test">
     *   taulu =[59,32,47,75,64]
     *   paikka = 1
     *   pienin(int[] taulu, int pienimmanpaikka) === Pienin numero on paikassa 1
     *   taulu =[1,50,2,99,14]
     *   paikka = 0
     *   pienin(int[] taulu, int pienimmanpaikka) === Pienin numero on paikassa 0
     *   taulu =[19,15,20,50,10]
     *   paikka = 4
     *   pienin(int[] taulu, int pienimmanpaikka) === Pienin numero on paikassa 4
     * </pre>
     */
    public static void pienin (int[] taulu, int pienimmanpaikka){
        int pienin = taulu[pienimmanpaikka];
        System.out.println("Pienin numero on arvoltaan " + pienin);

    }
    
    
    public static void main(String[] args) {
        taulukonLuoja();
        pienimmanPaikka(taulukko);
        pienin(taulukko, paikka);
        
    }

}
