package com.company.app;

import java.util.ArrayList;
import java.util.HashMap;

public class Parser {
    private static final HashMap<String,String> variables = new HashMap<>();

    public static String[] GetArguments(String input) throws Exception {
        String[] args = input.replaceAll("[ ]+=[ ]+", "=").split("[ |\n]+");
        ArrayList<String> ans = new ArrayList<>();
        for (int j = 0; j < args.length; j++) {
            int i = 0;
            while (i < args[j].length()) {
                if (args[j].charAt(i) == '$') {
                    StringBuilder name = new StringBuilder();
                    i++;
                    while (i < args[j].length() && args[j].charAt(i) != ' ' && args[j].charAt(i) != '=') {
                        name.append(args[j].charAt(i));
                        i++;
                    }
                    if (i == args[j].length())
                        i--;

                    if (args[j].charAt(i) == '=') {
                        StringBuilder value = new StringBuilder();
                        i++;
                        while (i < args[j].length() && args[j].charAt(i) != ' ') {
                            value.append(args[j].charAt(i));
                            i++;
                        }
                        variables.put(name.toString(), value.toString());
                        args[j] = "";
                    } else {
                        if (variables.containsKey(name.toString())) {
                            args[j] = args[j].replace("$" + name.toString(), variables.get(name.toString()));
                        } else {
                            throw new Exception("Variable " + name.toString() + " not found");
                        }
                    }
                }
                i++;
            }
            if (!args[j].isEmpty())
                ans.add(args[j]);
        }
        args = new String[ans.size()];
        ans.toArray(args);
        return args;
    }
}

