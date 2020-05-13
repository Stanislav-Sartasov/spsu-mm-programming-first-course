import com.company.pluginapp.plugin.*;

module SubstringSearchPlugin {
    exports com.company.pluginapp.plugin;

    uses ISubstringSearch;
    provides ISubstringSearch with BasicSubstringSearch, KnuthMorrisPrattAlgo;
}
