module task_8 {
    requires javafx.controls;
    requires javafx.fxml;

    opens com.company.app to javafx.fxml;
    exports com.company.app;

    opens com.company.math;
    exports com.company.math;
}
