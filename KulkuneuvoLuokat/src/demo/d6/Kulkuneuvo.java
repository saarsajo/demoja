package demo.d6;


/**
 * @author Bulbasaur
 * @version 17.3.2019
 *Demo jonka tarkoituksena on periä luokan atribuutteja
 */
public interface Kulkuneuvo {
    
    
    /**
     * @param nopeus kuvastaa kulkuneuvon nopeutta
     */
    void nopeus(int nopeus);
    
    
    /**
     * @param maara kuvastaa matkustajien määrää
     */
    void matkustajamaara(int maara);    
    
    
    /**
     * @param a ei käytössä
     */
    public static void main(String[] a){    
        System.out.println("Laivat ja lentokoneet ovat kulkuneuvoja");
    }
    }