package demo.d8;

import java.util.ArrayList;
import java.util.Random;
/**
 * @author Bulbasaur
 * @version 4.3.2019
 * Ohjelma joka muuttaa matriisin listaksi ja summaa jäsenet
 */
public class Taulukot2 {
    static Random rand = new Random();
    
    /**
     * @param args ei käytössä
     */
    public static void main(String[] args) {
        int [][] matriisi1 = matriisi();
        System.out.println(" Summa on " + summa(listaksi(matriisi1)));

        int [][] matriisi2 = matriisi();
        System.out.println(" Summa on " + summa(listaksi(matriisi2)));

        int [][] matriisi3 = matriisi();
        System.out.println(" Summa on " + summa(listaksi(matriisi3)));
    }
    
    
    /**
     * @return matriisi jossa on satunnaisesti arvotut luvut väliltä 0-50
     * 
     */
    public static int [][] matriisi () {
        int q = rand.nextInt(50);
        int w = rand.nextInt(50);
        int e = rand.nextInt(50);
        int r = rand.nextInt(50);
        int t = rand.nextInt(50);
        int y = rand.nextInt(50);
        int u = rand.nextInt(50);
        int i = rand.nextInt(50);
        int o = rand.nextInt(50);
        int p = rand.nextInt(50);
        int a = rand.nextInt(50);
        
        int[][] matriisi = {    { q, w, e, r }, 
                                { r, t, y, u }, 
                                { i, o, p, a } };
        return matriisi;
    }
    
    
    /**
     * @param lista laskettavat luvut
     * @return summa
     */
    public static double summa(final int[] lista) {
        if (lista == null) return 0;
        int summa = 0;
        int i;
        for (i = 0; i < lista.length; i++)
            summa += lista[i];
        // for (double luku:luvut) summa += luku;
        return summa;
    }
    
    
    private static int[] listaksi(int[][] matriisi){
        ArrayList<Integer> lista = new ArrayList<Integer>();
        System.out.print("Lukujen: ");
        for(int i=0;i<matriisi.length;i++) {
            for(int j=0;j<matriisi.length;j++){
                lista.add(matriisi[i][j]);
                System.out.print(matriisi[i][j] + " ");
            }
        }
        int[] list = lista.stream().mapToInt(i -> i).toArray();
        return list;      
    }
}