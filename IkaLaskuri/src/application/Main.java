package application;
import javafx.application.Application;
import javafx.scene.Scene;
import javafx.scene.layout.Pane;
import javafx.stage.Stage;
import javafx.fxml.FXMLLoader;

/**
 * @author Bulbasaur
 * @version 23.1.2019
 *Ohjelma joka laskee annettujen lukujen avulla huoneen pinta-alan ja tilavuuden
 */
public class Main extends Application {
    @Override
    public void start(Stage primaryStage) {
        try {
            FXMLLoader ldr = new FXMLLoader(getClass().getResource("MatkaGUIView.fxml"));
            final Pane root = ldr.load();
            Scene scene = new Scene(root);
            scene.getStylesheets().add(getClass().getResource("matka.css").toExternalForm());
            primaryStage.setScene(scene);
            primaryStage.setTitle("Matka");
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
