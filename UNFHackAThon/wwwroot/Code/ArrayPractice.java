
package arraypractice;

import java.lang.reflect.Array;
import java.util.Arrays;

public class ArrayPractice {
    
    public static void printArray(int[] array) {
        System.out.print("[");
        for (int i = 0; i < array.length; i++) {
            int item = array[i];
            System.out.print(item);
            if (i < array.length -1) {
                System.out.print(", ");
            }
        }
        System.out.println("]");
    }
    
     public static void printArray(String[] array) {
        System.out.print("[");
        for (int i = 0; i < array.length; i++) {
            String item = array[i];
            System.out.print(item);
            if (i < array.length -1) {
                System.out.print(", ");
            }
        }
        System.out.println("]");
    }


    public static void main(String[] args) {
        // TODO code application logic here
        
        //  Index: 0  1  2  3  4  
        //  Array: [0, 5, 3, 2, 1] --> lenght 5
        
        // Random Objects from Array
        // Math.abs(rand.nextInt) % lenght;
        
        // Index:   0       1       2 
        // Array: ["bob", "sally", "max"] --> lenght 3
        
        // Declaring, Allocating, and Initalizing 
        int[] intArray1;
        int[] intArray2 = new int[4];
        printArray(intArray2);
        int[] intArray3 = {5, 2, 9, 1, 3};
        
        String[] shoppingList = {"bannans", "apples", "pears"};
        
        //  Index: 0    1  2  3   
        //  Array: [10, 8, 5, 10] --> lenght 4
        
        
        intArray2[0] = 10;
        intArray2[1] = 8;
        intArray2[2] = 5;
        intArray2[3] = 10;
        
        // Print out Arrays
        System.out.println(Arrays.toString(intArray2));
        System.out.println(Arrays.toString(intArray3));
        System.out.println();
        
        // Custom print out arrays
        printArray(intArray2);
        printArray(intArray3);
        System.out.println();
    
       // Retrieve objects 
       System.out.println(intArray2[3]);
       System.out.println(Array.get(intArray2, 3));
       System.out.println();
       
       //Given functions
       Arrays.sort(intArray3);
       printArray(intArray3);
       
       //Print string array
       printArray(shoppingList);
       
       System.out.println("Special For Loop:");
       //Special for loop: foreach
       for (String s : shoppingList){
           System.out.println(s);
          
       }
    }
    
}
