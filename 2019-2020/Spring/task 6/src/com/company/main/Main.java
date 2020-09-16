package com.company.main;

import com.company.loader.ModuleLoader;
import com.company.pluginapp.core.ISubstringSearch;

import java.io.File;
import java.lang.reflect.InvocationTargetException;

public class Main {

    public static void main(String[] args) {
        String s = "ababacababa";
        String t = "aba";
        String modulePath = "out/production/task 6/com/company/pluginapp/plugin/";
        ModuleLoader loader = new ModuleLoader(modulePath, ClassLoader.getSystemClassLoader());

        File dir = new File(modulePath);
        String[] modules = dir.list();              // список доступных модулей

        if (modules != null) {
            for (String module: modules) {
                try {
                    String moduleName = module.split(".class")[0];
                    ISubstringSearch execute = (ISubstringSearch) loader.loadClass(moduleName).getDeclaredConstructor().newInstance();

                    execute.load();
                    System.out.println(execute.getName() + "'s answer is  " + execute.findSubstring(s, t) + "\n");
                } catch (NoSuchMethodException | InvocationTargetException | IllegalAccessException | InstantiationException | ClassNotFoundException e) {
                    e.printStackTrace();
                }
            }
        }
    }
}

