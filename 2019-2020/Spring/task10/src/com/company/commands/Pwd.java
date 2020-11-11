package com.company.commands;

import java.io.File;

public class Pwd extends Command {

    public static String execute(String[] args) throws Exception {
        if (args.length != 1)
            throw new Exception("Invalid number of arguments");
        File curDir = new File("");
        return curDir.getAbsolutePath() + "\n";
    }

}
