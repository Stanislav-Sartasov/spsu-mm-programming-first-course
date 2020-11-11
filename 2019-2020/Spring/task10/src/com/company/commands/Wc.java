package com.company.commands;

import java.io.FileReader;
import java.io.IOException;

public class Wc extends Command {
    public static String execute(String[] args) throws Exception {
        if (args.length < 2)
            throw new Exception("Invalid number of arguments");

        StringBuilder sb = new StringBuilder();
        for (int i = 1; i < args.length; i++) {
            String arg = args[i];
            try (FileReader reader = new FileReader(arg)) {
                int lines = 1;
                int words = 0;
                int bytes = 0;
                int c;
                char prev = ' ';
                while ((c = reader.read()) != -1) {
                    bytes++;
                    if (((char) c == ' ' || (char) c == '\t' || (char) c == '\n')
                            && (prev != ' ' && prev != '\t' && prev != '\n'))
                        words++;
                    if ((char) c == '\n')
                        lines++;
                    prev = (char) c;
                }
                if (prev != ' ' && prev != '\t' && prev != '\n')
                    words++;
                sb.append(lines).append(" ");
                sb.append(words).append(" ");
                sb.append(bytes).append(" ");
                sb.append(arg);
            } catch (IOException ex) {
                System.out.println(ex.getMessage());
                sb.append("0 0 0 ").append(arg);
            }
            sb.append('\n');
        }
        return sb.toString();
    }
}
