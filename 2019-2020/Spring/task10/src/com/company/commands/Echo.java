package com.company.commands;

public class Echo extends Command {
    public static String execute(String[] args) throws Exception {
        StringBuilder sb = new StringBuilder();
        for (int i = 1; i < args.length; i++)
            sb.append(args[i]).append(" ");
        return sb.append('\n').toString();
    }
}
