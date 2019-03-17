package demo7;
/**
 * @author Bulbasaur
 * @version 25.2.2019
 * Ohjelma joka tutkii onko sille syötetty sana palindromi vai ei
 */
public class Palindromi {


   /**
    * @param sana on sana jota tutkitaan
 * @return joko true tai false
 * @example
    * <pre name="test">
     * palindromi("kala") === false;
     * palindromi("1221") === true;
     * palindromi("abba") === true;
     * palindromi("jeeja") === false;
    * </pre>
    */
   public static Boolean palindromi(String sana) {
       int pituus = sana.length();
       String kaannetty = "";
       
       for (int i = pituus - 1; i >= 0; i--) {
          kaannetty = kaannetty + sana.charAt(i);
       }
       if (sana.equals(kaannetty)) {
           System.out.println(sana + " on palindromi");
           return true;
       }
       System.out.println(sana + " ei ole palindromi");
       return false;
   }
   
   
   /**
 * @param args ei käytössä
 */
public static void main(String[] args) {
       palindromi("kala");
       palindromi("1221");
       palindromi("kaak");
       palindromi("abba");
       palindromi("jee");
   }
}