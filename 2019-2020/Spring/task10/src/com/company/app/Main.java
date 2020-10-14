package com.company.app;

import java.util.Scanner;

public class Main {

    public static void main(String[] args) {
        System.out.println("Starting Bash...\n" +
                "You can use pipelines(|), local variables ($e=echo), " +
                "commands: pwd, wc [FILENAME], ls, echo [args], exit, cat [FILENAME]");
        Bash.run(new Scanner(System.in), System.out);
    }
}

