package com.company.commands;

import java.io.File;

public class Ls extends Command {
    public static String execute(String[] args) throws Exception {
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
}
