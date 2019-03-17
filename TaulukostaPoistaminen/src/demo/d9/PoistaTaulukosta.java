package demo.d9;

/**
 * @author Bulbasaur
 * @version 11.3.2019
 *Poistaa taulukosta halutut arvot
 */
public class PoistaTaulukosta {

    
 /**
 * 
 * @param taulukko on käsiteltävä taulukko
 * @param lkm on taulukon alkioiden määrä
 * @param n on taulukosta poistettava luku
 * @return maara 
 * @example
 * <pre name="test">
 * poista(new int[]{ 4, 7, 6, 3, 6, 2 }, 6, 6)   === 4;
 * poista(new int[]{ 2, 5, 1, 9}, 4, 5)          === 3;
 * poista(new int[]{ 1, 5, 2, 1, 6, 1, 7}, 7, 1) === 4;
 * poista(new int[]{ 2, 5, 2, 1, 9}, 5, 2)       === 3;
 * </pre>
 */
    static int poista(int taulukko[],int lkm,int n) {
        int maara = lkm;
        int apuluku;
        int apupaikka;
        for (int i=0; i < maara; i++) {
            if (taulukko[i]==n) {
                apupaikka = i;
                while(apupaikka+1<lkm) {
                    apuluku = taulukko[apupaikka+1];
                    taulukko[apupaikka+1]=taulukko[apupaikka];
                    taulukko[apupaikka]=apuluku;
                    apupaikka++;
                }
                maara--;
            }
        }
        return maara;
    }
    
    
    /**
     * Tulostetaan taulukosta lkm kappaletta lukuja
     * @param t   käsiteltävä taulukko
     * @param lkm käsitelteltävien alkioiden lkm
     */
    public static void tulosta(int t[], int lkm) {
        int tlkm = lkm;
        if (tlkm > t.length) tlkm = t.length;
        for (int i = 0; i < tlkm; i++)
            System.out.print(t[i] + " ");
        System.out.println();
    }

    
    /**
     * @param args ei käytössä
     */
    public static void main(String[] args) {
        int t[] = { 4, 7, 6, 3, 6, 2 };
        int lkm = 6;

        lkm = poista(t, lkm, 6); /* => t = {4,7,3,2}, lkm = 4 */
        tulosta(t, lkm);
    }
}