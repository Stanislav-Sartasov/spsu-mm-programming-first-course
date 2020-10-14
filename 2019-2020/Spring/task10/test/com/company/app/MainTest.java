package com.company.app;

import org.junit.jupiter.api.Test;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.PrintStream;
import java.util.Scanner;

import static org.junit.Assert.assertEquals;


class MainTest {

    @Test
    void testEcho() {
        ByteArrayOutputStream outContent = new ByteArrayOutputStream();

        String script = "echo hi\n" +
                         "exit";

        String ans = "hi \n";

        Bash.run(new Scanner(new ByteArrayInputStream(script.getBytes())), new PrintStream(outContent));
        assertEquals(ans, outContent.toString());
    }

    @Test
    void testLs() {
        ByteArrayOutputStream outContent = new ByteArrayOutputStream();

        String script = "ls\n" +
                         "exit";

        String ans = "input.txt\n" +
                    ".idea\n" +
                    "out\n" +
                    "src\n" +
                    "task10.iml\n" +
                    "test\n";

        Bash.run(new Scanner(new ByteArrayInputStream(script.getBytes())), new PrintStream(outContent));
        assertEquals(ans, outContent.toString());
    }

    @Test
    void testPwd() {
        ByteArrayOutputStream outContent = new ByteArrayOutputStream();

        String script = "pwd\n" +
                         "exit";

        String ans = new File("").getAbsolutePath() + "\n";

        Bash.run(new Scanner(new ByteArrayInputStream(script.getBytes())), new PrintStream(outContent));
        assertEquals(ans, outContent.toString());
    }

    @Test
    void testWc() {
        ByteArrayOutputStream outContent = new ByteArrayOutputStream();

        String script = "wc input.txt\n" +
                        "exit";

        String ans = "1 2 13 input.txt\n";

        Bash.run(new Scanner(new ByteArrayInputStream(script.getBytes())), new PrintStream(outContent));
        assertEquals(ans, outContent.toString());
    }

    @Test
    void testExecuteAnotherCommands() {
        ByteArrayOutputStream outContent = new ByteArrayOutputStream();

        String script = "touch new_file.txt\n" +
                        "ls\n" +
                        "exit";

        String ans = "input.txt\n" +
                "new_file.txt \n" +
                ".idea\n" +
                "out\n" +
                "src\n" +
                "task10.iml\n" +
                "test\n";

        Bash.run(new Scanner(new ByteArrayInputStream(script.getBytes())), new PrintStream(outContent));
        assertEquals(ans, outContent.toString());

        script = "rm new_file.txt\n" +
                "ls\n" +
                "exit";

        ans = "input.txt\n" +
                ".idea\n" +
                "out\n" +
                "src\n" +
                "task10.iml\n" +
                "test\n";

        outContent = new ByteArrayOutputStream();
        Bash.run(new Scanner(new ByteArrayInputStream(script.getBytes())), new PrintStream(outContent));
        assertEquals(ans, outContent.toString());
    }

    @Test
    void testPipelines() {
        ByteArrayOutputStream outContent = new ByteArrayOutputStream();

        String script = "ls | wc\n" +
                        "exit";

        String ans = ".idea (Is a directory)\n" +
                "out (Is a directory)\n" +
                "src (Is a directory)\n" +
                "test (Is a directory)\n" +
                "1 2 13 input.txt\n" +
                "0 0 0 .idea\n" +
                "0 0 0 out\n" +
                "0 0 0 src\n" +
                "38 87 2053 task10.iml\n" +
                "0 0 0 test\n";

        Bash.run(new Scanner(new ByteArrayInputStream(script.getBytes())), new PrintStream(outContent));
        assertEquals(ans, outContent.toString());
    }

    @Test
    void testVariables() {
        ByteArrayOutputStream outContent = new ByteArrayOutputStream();

        String script = "$a=input.txt\n" +
                        "cat $a | echo\n" +
                        "exit";

        String ans = "Hello, World! \n";

        Bash.run(new Scanner(new ByteArrayInputStream(script.getBytes())), new PrintStream(outContent));
        assertEquals(ans, outContent.toString());
    }
}