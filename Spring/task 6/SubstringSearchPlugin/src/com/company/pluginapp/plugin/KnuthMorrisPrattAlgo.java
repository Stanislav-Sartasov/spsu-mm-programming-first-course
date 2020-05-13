package com.company.pluginapp.plugin;

import static java.lang.Integer.min;

public class KnuthMorrisPrattAlgo implements ISubstringSearch {
    @Override
    public String getName() {
        return "Knuth-Morris-Pratt Algorithm";
    }

    @Override
    public int findSubstring(String s, String t) {
        s = t + s;
        int n = s.length();
        int[] z = new int[n];
        for (int i = 0; i < n; i++)
            z[i] = 0;
        int l = 0;
        int r = 0;
        for (int i = 1; i < n; i++) {
            if (i <= r)
                z[i] = min(r - i + 1, z[i - l]);
            while (i + z[i] < n && s.charAt(z[i]) == s.charAt(i + z[i]))
                z[i]++;
            if (i + z[i] - 1 > r){
                l = i;
                r = i + z[i] - 1;
            }
        }
        int ans = 0;
        for (int i = t.length(); i < n; i++)
            if (z[i] >= t.length())
                ans++;
        return ans;
    }
}