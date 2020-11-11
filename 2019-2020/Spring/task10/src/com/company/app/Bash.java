package com.company.app;

import com.company.commands.*;
import java.io.*;
import java.util.Scanner;

public class Bash {
    public static void run(Scanner in, PrintStream out) {
        System.setOut(out);

        boolean fl = true;
        while (fl) {
            String line = in.nextLine().trim();
            if (line.isEmpty())
                continue;
            String[] pipelineCommands = line.split("\\|");
            String lastResult = "";
            for (String pipelineCommand : pipelineCommands) {
                String s = pipelineCommand.trim();
                if (s.isEmpty())
                    continue;

                String[] args = new String[0];
                try {
                    args = Parser.GetArguments(s + " " + lastResult);
                } catch (Exception exception) {
                    exception.printStackTrace();
                }
                if (args.length == 0)
                    continue;
                String command = args[0];

                if (command.equals("exit")) {
                    fl = false;
                    break;
                }

                try {
                    lastResult = execute(command, args);
                } catch (Exception exception) {
                    System.out.println("[Error] " + exception.getMessage());
                }
            }
            System.out.print(lastResult);
        }
    }

    private static String execute(String command, String[] args) throws Exception {
        return switch (command) {
            case "echo" -> Echo.execute(args);
            case "cat" -> Cat.execute(args);
            case "pwd" -> Pwd.execute(args);
            case "ls" -> Ls.execute(args);
            case "wc" -> Wc.execute(args);
            default -> Command.execute(args);
        };
    }
}

