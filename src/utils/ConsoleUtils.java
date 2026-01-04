package utils;

public class ConsoleUtils {
    public static void clear() {
        try {
            new ProcessBuilder("cmd", "/c", "cls").inheritIO().start().waitFor();
        } catch (Exception e) {
            System.out.print("\n\n\n");
        }
    }

    public static void printLine() {
    }

    public static void printHeader(String title) {
        System.out.println("\n" + title + "\n");
    }

    private static String centerText(String text, int width) {
        int padding = (width - text.length()) / 2;
        return " ".repeat(padding) + text;
    }
}
