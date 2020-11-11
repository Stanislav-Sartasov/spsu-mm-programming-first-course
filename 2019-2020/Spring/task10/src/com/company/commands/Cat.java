package com.company.commands;

import java.io.FileReader;
import java.io.IOException;

public class Cat extends Command{
    public static String execute(String[] args) throws Exception {
        if (args.length < 2)
            throw new Exception("Invalid number of arguments");
        StringBuilder sb = new StringBuilder();
        for (int i = 1; i < args.length; i++) {
            String arg = args[i];
            try (FileReader reader = new FileReader(arg)) {
                int c;
                while ((c = reader.read()) != -1) {
                    sb.append((char) c);
                }
            } catch (IOException ex) {
                System.out.println(ex.getMessage());
            }
            sb.append('\n');
        }
        return sb.toString();
    }
}
