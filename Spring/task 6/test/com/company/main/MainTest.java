package com.company.main;

import com.company.loader.ModuleLoader;
import com.company.pluginapp.core.ISubstringSearch;
import org.junit.Assert;
import org.junit.jupiter.api.Test;

import java.io.File;
import java.lang.reflect.InvocationTargetException;
import java.util.Random;

import static org.junit.jupiter.api.Assertions.*;

class MainTest {

    private String randomString(int len) {
        char[] chars = "abcdef".toCharArray();
        StringBuilder sb = new StringBuilder(20);
        Random random = new Random();
        for (int i = 0; i < len; i++) {
            char c = chars[random.nextInt(chars.length)];
            sb.append(c);
        }
        return sb.toString();
    }

    @Test
    void main() {
        for (int z = 0; z < 50; z++) {
            String s = randomString(100);
            String t = randomString(2);

            String modulePath = "out/production/task 6/com/company/pluginapp/plugin/";
            ModuleLoader loader = new ModuleLoader(modulePath, ClassLoader.getSystemClassLoader());

            File dir = new File(modulePath);
            String[] modules = dir.list();

            int[] answers = new int[0];
            if (modules != null) {
                answers = new int[modules.length];
            }
            int i = 0;

            for (String module : modules) {
                String moduleName = module.split(".class")[0];
                ISubstringSearch execute = null;
                try {
                    execute = (ISubstringSearch) loader.loadClass(moduleName).getDeclaredConstructor().newInstance();
                } catch (InstantiationException | IllegalAccessException | InvocationTargetException | NoSuchMethodException | ClassNotFoundException e) {
                    e.printStackTrace();
                }
                answers[i] = execute.findSubstring(s, t);
                i++;
            }

            //System.out.println(s + "    " + t + "   " + answers[0]);

            for (int j = 0; j < i - 1; j++) {
                Assert.assertEquals(answers[j], answers[j + 1]);
            }
        }
    }
}