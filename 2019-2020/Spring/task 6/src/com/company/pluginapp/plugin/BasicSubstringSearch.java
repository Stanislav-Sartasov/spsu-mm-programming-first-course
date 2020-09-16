package com.company.pluginapp.plugin;

import com.company.pluginapp.core.ISubstringSearch;

public class BasicSubstringSearch implements ISubstringSearch {

    @Override
    public void load() {
        System.out.println("Module " + this.getClass() + " loading ...");
    }

    @Override
    public String getName() {
        return "Basic Substring Search";
    }

    @Override
    public int findSubstring(String s, String t) {
        int ans = 0;
        for (int i = 0; i < s.length() - t.length() + 1; i++) {
            boolean fl = true;
            for (int j = 0; j < t.length(); j++) {
                if (s.charAt(i + j) != t.charAt(j)) {
                    fl = false;
                    break;
                }
            }
            if (fl)
                ans++;
        }
        return ans;
    }
}