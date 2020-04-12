package com.company;

public class MkV extends tank {
    private String modification;

    public String getModification() {
        return modification;
    }

    public MkV(int year, double w) throws Exception {
        super(year, 0, 8, w, "United Kingdom", "Mark V", "heavy tank");
        if (w == 28) {
            modification = "female";
        } else if (w == 30) {
            modification = "male";
        } else {
            throw new Exception("Wrong weight");
        }
    }

    public MkV(double w) throws Exception {
        super(1918, 0, 8, w, "United Kingdom", "Mark V", "heavy tank");
        if (w == 28) {
            modification = "female";
        } else if (w == 30) {
            modification = "male";
        } else {
            throw new Exception("Wrong weight");
        }
    }

    public String getTheCharacteristic() {
        return super.getTheCharacteristic() + "Modification: " + modification + "\n";
    }
}
