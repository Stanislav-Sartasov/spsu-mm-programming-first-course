package com.company.loader;


import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;

public class ModuleLoader extends ClassLoader {
    private final String pathToBin;

    public ModuleLoader(String path, ClassLoader parent) {
        super(parent);
        this.pathToBin = path;
    }

    @Override
    public Class<?> findClass(String className) throws ClassNotFoundException {
        try {
            byte[] b = fetchClassFromFS(pathToBin + className + ".class");
            return defineClass(null, b, 0, b.length);
        } catch (IOException ex) {
            return super.findClass(className);
        }
    }

    private byte[] fetchClassFromFS(String path) throws IOException {
        InputStream is = new FileInputStream(new File(path));
        long length = new File(path).length();

        byte[] bytes = new byte[(int)length];

        int offset = 0;
        int numRead;
        while (offset < bytes.length
                && (numRead = is.read(bytes, offset, bytes.length - offset)) >= 0) {
            offset += numRead;
        }

        if (offset < bytes.length) {
            throw new IOException("Could not completely read file " + path);
        }

        is.close();
        return bytes;
    }
}