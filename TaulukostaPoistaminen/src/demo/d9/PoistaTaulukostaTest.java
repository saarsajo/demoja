package demo.d9;
// Generated by ComTest BEGIN
import static org.junit.Assert.*;
import org.junit.*;
import static demo.d9.PoistaTaulukosta.*;
// Generated by ComTest END

/**
 * Test class made by ComTest
 * @version 2019.03.11 01:39:16 // Generated by ComTest
 *
 */
public class PoistaTaulukostaTest {



  // Generated by ComTest BEGIN
  /** testPoista18 */
  @Test
  public void testPoista18() {    // PoistaTaulukosta: 18
    assertEquals("From: PoistaTaulukosta line: 19", 4, poista(new int[]{ 4, 7, 6, 3, 6, 2 }, 6, 6)); 
    assertEquals("From: PoistaTaulukosta line: 20", 3, poista(new int[]{ 2, 5, 1, 9}, 4, 5)); 
    assertEquals("From: PoistaTaulukosta line: 21", 3, poista(new int[]{ 1, 5, 1, 1, 69, 1, 7}, 7, 1)); 
    assertEquals("From: PoistaTaulukosta line: 22", 3, poista(new int[]{ 2, 5, 2, 1, 9}, 5, 2)); 
  } // Generated by ComTest END
}