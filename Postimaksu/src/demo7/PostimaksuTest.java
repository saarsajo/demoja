package demo7;
// Generated by ComTest BEGIN
import static org.junit.Assert.*;
import org.junit.*;
import static demo7.Postimaksu.*;
// Generated by ComTest END

/**
 * Test class made by ComTest
 * @version 2019.02.25 02:13:25 // Generated by ComTest
 *
 */
public class PostimaksuTest {



  // Generated by ComTest BEGIN
  /** testSuurinKirjeenPaino19 */
  @Test
  public void testSuurinKirjeenPaino19() {    // Postimaksu: 19
    assertEquals("From: Postimaksu line: 20", 0, suurinPaketinPaino(0)); 
    assertEquals("From: Postimaksu line: 21", 1000, suurinPaketinPaino(1)); 
    assertEquals("From: Postimaksu line: 22", 2500, suurinPaketinPaino(3)); 
    assertEquals("From: Postimaksu line: 23", 10000, suurinPaketinPaino(9)); 
  } // Generated by ComTest END
}