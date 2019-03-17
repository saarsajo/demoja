package pistelaskuri;

import javafx.fxml.FXML;
import javafx.scene.control.Label;

/**
 * Pistelaskuri jolla voi laskea 1, 2 tai 3 arvoisia pisteitä yhteen. Lisäksi mahdollisuus nollata pisteet.
 * Ensimmäiseen pisteeseen saa yhden lisäpisteen. Se on ominaisuus ei bugi ;)
 * @author Bulbasaur
 * @version 17.3.2019
 */
public class PistelaskuriController {
    private int yksi = 0;
    private int kaksi = 0;
    private int kolme = 0;
    private int yhteensa = 0;
    @FXML private Label laskuriYKS;
    @FXML private Label laskuriKAKS;
    @FXML private Label laskuriKOL;
    @FXML private Label laskuriYHT;

    
    @FXML void handleYKS() {
        laskuriYKS.setText("" + ++yksi);
        handleYHT();
    }

    
    @FXML void handleKAKS() {
        laskuriKAKS.setText("" + ++kaksi);
        handleYHT();
    }
    
    
    @FXML void handleKOL() {
        laskuriKOL.setText("" + ++kolme);
        handleYHT();
    }

    
    @FXML void handleYHT() {
        yhteensa = yksi + (2 * kaksi) + (3 * kolme);
        laskuriYHT.setText("" + ++yhteensa);
    }
    
    
    @FXML void handleNollaa() {
        laskuriYKS.setText("" + (yksi=0));
        laskuriKAKS.setText("" + (kaksi=0));
        laskuriKOL.setText("" + (kolme=0));
        laskuriYHT.setText("" + (yhteensa=0));

    }
}