import java.util.ArrayList;
import java.util.List;

public class Main {
    public static void main(String[] args) {
        int n = 10;
        Process[] tasks = new Process[n];
        for (int i = 0; i < n; i++) {
            tasks[i] = new Process();
        }
        ProcessManager.runTasks(tasks);
    }
}
