package com.company.pluginapp.plugin;

import java.util.List;
import java.util.ServiceLoader;
import java.util.stream.Collectors;

public interface ISubstringSearch {
    int findSubstring(String s, String t);

    String getName();

    static List<ISubstringSearch> getServices(ModuleLayer layer) {
        return ServiceLoader
                .load(layer, ISubstringSearch.class)
                .stream()
                .map(ServiceLoader.Provider::get)
                .collect(Collectors.toList());
    }

}
