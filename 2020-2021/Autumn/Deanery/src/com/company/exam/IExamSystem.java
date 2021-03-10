package com.company.exam;

public interface IExamSystem {
    void add(long studentId, long courseId);
    void remove(long studentId, long courseId);
    boolean contains(long studentId, long courseId);
}
