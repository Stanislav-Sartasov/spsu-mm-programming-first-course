package com.company.app;

import java.io.*;
import java.util.Scanner;

public class Bash {
    private static String cat(String[] args) throws Exception {
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

    private static String echo(String[] args) {
        StringBuilder sb = new StringBuilder();
        for (int i = 1; i < args.length; i++)
            sb.append(args[i]).append(" ");
        return sb.append('\n').toString();
    }

    private static String ls(String[] args) throws Exception {
        if (args.length != 1)
            throw new Exception("Invalid number of arguments");
        File curDir = new File(".");

        StringBuilder sb = new StringBuilder();
        File[] filesList = curDir.listFiles();
        assert filesList != null;
        for(File f : filesList) {
            sb.append(f.getName()).append("\n");
        }
        return sb.toString();
    }

    private static String pwd(String[] args) throws Exception {
        if (args.length != 1)
            throw new Exception("Invalid number of arguments");
        File curDir = new File("");
        return curDir.getAbsolutePath() + "\n";
    }

    private static String wc(String[] args) throws Exception {
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

    private static void executeAnotherCommands(String[] arg) throws IOException {
        StringBuilder args = new StringBuilder();
        for (int i = 1; i < arg.length; i++)
            args.append(arg[i]).append(' ');
        ProcessBuilder pb = new ProcessBuilder(arg[0], args.toString());
        pb.inheritIO();
        Process process = pb.start();
        try {
            process.waitFor();
        } catch (InterruptedException e) {
            e.printStackTrace();
        }

    }


    public static void run(Scanner in, PrintStream out) {
        System.setOut(out);

        Boolean fl = true;
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

                switch (command) {
                    case "echo":
                        lastResult = echo(args);
                        break;
                    case "exit":
                        fl = false;
                        break;
                    case "cat":
                        try {
                            lastResult = cat(args);
                        } catch (Exception exception) {
                            exception.printStackTrace();
                        }
                        break;
                    case "pwd":
                        try {
                            lastResult = pwd(args);
                        } catch (Exception exception) {
                            exception.printStackTrace();
                        }
                        break;
                    case "ls":
                        try {
                            lastResult = ls(args);
                        } catch (Exception exception) {
                            exception.printStackTrace();
                        }
                        break;
                    case "wc":
                        try {
                            lastResult = wc(args);
                        } catch (Exception exception) {
                            exception.printStackTrace();
                        }
                        break;
                    default:
                        try {
                            executeAnotherCommands(args);
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                        break;
                }
            }

            System.out.print(lastResult);
        }
    }
}

