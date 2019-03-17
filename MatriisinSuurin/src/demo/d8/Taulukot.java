package demo.d8;
import java.io.*; 
import java.util.Random;

/**
 * @author Bulbasaur
 * @version 4.3.2019
 *
 */
public class Taulukot {
    static Random rand = new Random();
  
    
    /**
    * @param matriisi on tuotu matriisi
    */
    public static void matriisinSuurin(int matriisi[][]) 
    { 
        int k = 0;
        for (int i = 0; i < matriisi.length; i++) {
            if ( k < matriisi[i][0] ){
                k = matriisi[i][0];
            }
            for (int j = 0; j < matriisi[i].length; j++) {
                if ( k < matriisi[i][j] ){
                    k = matriisi[i][j];
                }
                System.out.print(matriisi[i][j] + " "); 
            }
        }
        System.out.print("Matriisin suurin luku on  " + k + "\n"); 
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
    * @param args ei käytössä
    * @throws IOException poikkeus
    */
    public static void main(String args[]) throws IOException 
    { 
        int [][] matriisi1 = matriisi();
        matriisinSuurin(matriisi1); 
        int [][] matriisi2 = matriisi();
        matriisinSuurin(matriisi2); 
        int [][] matriisi3 = matriisi();
        matriisinSuurin(matriisi3); 
    } 
}