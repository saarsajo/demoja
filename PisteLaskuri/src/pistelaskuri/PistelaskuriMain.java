package pistelaskuri;

import javafx.application.Application;
import javafx.stage.Stage;
import javafx.scene.Scene;
import javafx.scene.layout.Pane;
import javafx.fxml.FXMLLoader;


/**
 * Pisteenlaskuri
 * @author Bulbasaur
 * @version 17.3.2019
 */
public class PistelaskuriMain extends Application {
    @Override
    public void start(Stage primaryStage) {
        try {
            FXMLLoader ldr = new FXMLLoader(getClass().getResource("PistelaskuriView.fxml"));
            final Pane root = ldr.load();
            Scene scene = new Scene(root);
            scene.getStylesheets().add(getClass().getResource("pistelaskuri.css").toExternalForm());
            primaryStage.setScene(scene);
            primaryStage.setTitle("Pistelaskuri");
            primaryStage.show();
        } catch(Exception e) {
            e.printStackTrace();
        }
    }

    /**
     * @param args Ei käytössä
     */
    public static void main(String[] args) {
        launch(args);
    }
}