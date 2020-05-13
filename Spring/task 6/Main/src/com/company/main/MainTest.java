package com.company.main;

import com.company.pluginapp.plugin.ISubstringSearch;
import org.junit.Assert;
import org.junit.jupiter.api.Test;
import java.util.List;

public class MainTest {

    @Test
    public void main() {
        String s = "abbaabbaabbbabaabaabbabbaabbbaabbabbbbbbaaaabbaaabbaaaaaabb";
        String t = "aabba";
        String ans = "";
        List<ISubstringSearch> services = ISubstringSearch.getServices(ModuleLayer.boot());
        for (ISubstringSearch service : services) {
            ans += service.getName() + " answer is "+ service.findSubstring(s, t) + "\n";
        }

        Assert.assertEquals(ans, "Basic Substring Search answer is 5\n" +
                                        "Knuth-Morris-Pratt Algorithm answer is 5\n");
    }
}