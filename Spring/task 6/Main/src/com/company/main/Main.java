package com.company.main;

import com.company.pluginapp.plugin.ISubstringSearch;
import java.util.List;

public class Main {

    public static void main(String[] args) {
        String s = "ababacaba";
        String t = "aba";

        List<ISubstringSearch> services = ISubstringSearch.getServices(ModuleLayer.boot());
        for (ISubstringSearch service : services) {
            System.out.println(service.getName() + " answer is "+ service.findSubstring(s, t));
        }
    }
}