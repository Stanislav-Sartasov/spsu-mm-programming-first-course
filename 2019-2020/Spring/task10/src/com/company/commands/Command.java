package com.company.commands;

public class Command {
    public static String execute(String[] arg) throws Exception  {
        StringBuilder args = new StringBuilder();
        for (int i = 1; i < arg.length; i++)
            args.append(arg[i]).append(' ');
        ProcessBuilder pb = new ProcessBuilder(arg[0], args.toString());
        pb.inheritIO();
        Process process;
        try {
            process = pb.start();
            process.waitFor();
            return "";
        } catch (Exception e) {
            throw new Exception("Failed to execute command.");
        }
    }
}
