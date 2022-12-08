package com.example.demo1;

import javafx.animation.AnimationTimer;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.chart.LineChart;
import javafx.scene.chart.XYChart;
import javafx.scene.control.Button;
import com.fazecast.jSerialComm.SerialPort;
import javafx.scene.layout.AnchorPane;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;


public class HelloController {
    @FXML
    private AnchorPane box;

    @FXML
    private Button start_button;

    @FXML
    private Button initB;

    @FXML
    private Button btnc;

    @FXML
    private Button btnk;

    @FXML
    private Button btnf;

    @FXML
    private Button btnnapr;


    @FXML
    private LineChart<Number, Number> line_chart;

    InputStream in;
    OutputStream out;
    float temp;
    boolean flagC = false;

    int ix = 0;

    static SerialPort comPort;
    XYChart.Series<Number, Number> series = new XYChart.Series<Number, Number>();

    @FXML
    void onInit(ActionEvent event) {
        line_chart.getData().add(series);
        line_chart.setAnimated(false);

        comPort = SerialPort.getCommPorts()[0];
        comPort.setComPortParameters(
                115200, 8, SerialPort.ONE_STOP_BIT, SerialPort.NO_PARITY
        );
        comPort.openPort();

        System.out.println("COM port open: " + comPort.getDescriptivePortName());
        System.out.println("Event Listener open.");

        in = comPort.getInputStream();
        out = comPort.getOutputStream();
    }

    @FXML
    void onBtnC(ActionEvent event) throws IOException {
        out.write(1);
        series.getData().clear();
        ix = 0;
        System.out.println("Send C");
        boolean flagC = true;
    }

    @FXML
    void onBtnK(ActionEvent event) throws IOException {
        out.write(2);
        series.getData().clear();
        ix = 0;
        System.out.println("Send K");
        boolean flagC = false;
    }

    @FXML
    void onBtnF(ActionEvent event) throws IOException {
        out.write(4);
        series.getData().clear();
        ix = 0;
        System.out.println("Send F");
        boolean flagC = false;
    }

    @FXML
    void onBtnNapr(ActionEvent event) throws IOException {
        out.write(8);
        series.getData().clear();
        ix = 0;
        System.out.println("Send N");
        boolean flagC = false;
    }


    @FXML
    void onBtnP(ActionEvent event) throws IOException {
        out.write(16);
        series.getData().clear();
        ix = 0;
        System.out.println("Send P");
        boolean flagC = false;
    }

    void onCatch() {
        try {
            char[] newData = new char[10];
            for (int i = 0; i < 10; i++) {
                newData[i] = (char) in.read();
            }
            int firstNonSpace = 0;
            for (int i = 0; i < 10; i++) {
                if (!String.valueOf(newData[i]).equalsIgnoreCase(" ")) {
                    firstNonSpace = i;
                    break;
                }
            }

            System.out.print(" {" + String.valueOf(newData) + "|");
            String tempString = String.valueOf(newData).substring(firstNonSpace, 10);

            try {
                temp = Float.parseFloat(tempString);
                System.out.print(" |" + temp + "} ");
                series.getData().add(new XYChart.Data<Number, Number>(ix, temp));
                ix++;
                if (ix == 150) {
                    ix = 0;
                    series.getData().clear();
                }

            } catch (Exception e) {
                return;
            }

        } catch (Exception e) {
        }
    }

    void onColorChange() {
        if (flagC) {
            try {
                int color = (int) (((temp - 10) / 30) * 220);
                System.out.print("|" + color + "|");
                box.setStyle("-fx-background-color: hsb(" + color + ",100%,100%);");
            } catch (Exception e) {
            }
        }
    }

    @FXML
    void onStart() {
        line_chart.getData().clear();
        series.getData().clear();
        line_chart.getData().add(series);
        inputTimer.start();
        //colorTimer.start();
    }


    protected AnimationTimer inputTimer = new AnimationTimer() {
        @Override
        public void handle(long now) {
            onCatch();
        }
    };

    protected AnimationTimer colorTimer = new AnimationTimer() {
        @Override
        public void handle(long now) {
            onColorChange();
        }
    };


}
