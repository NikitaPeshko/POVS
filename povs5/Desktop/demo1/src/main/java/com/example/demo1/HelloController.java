package com.example.demo1;


import javafx.animation.AnimationTimer;
import javafx.application.Platform;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.canvas.Canvas;
import javafx.scene.chart.LineChart;
import javafx.scene.chart.NumberAxis;
import javafx.scene.chart.XYChart;
import javafx.scene.control.Button;
import com.fazecast.jSerialComm.SerialPort;
import com.fazecast.jSerialComm.SerialPortDataListener;
import com.fazecast.jSerialComm.SerialPortEvent;
import javafx.scene.paint.Color;

import java.io.IOException;
import java.io.InputStream;
import java.io.UnsupportedEncodingException;
import java.net.URL;
import java.util.Arrays;
import java.util.ResourceBundle;
import java.util.concurrent.TimeUnit;

public class HelloController {
    @FXML
    private Button initB;

    @FXML
    private Button start_button;

    @FXML
    private LineChart<Number, Number> line_chart;

    //private static SerialPort serialPort;

    float napr = 1.34f;   // Напряжение (в вольтах) на датчике при температуре 25 °C.

    float naprPopr = 0.0043f;  // Изменение напряжения (в вольтах) при изменении температуры на градус.

    float naprObr = 3.3f;// Образцовое напряжение АЦП (в вольтах).
    InputStream in;
    float temp;

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

        //comPort.setComPortTimeouts(SerialPort.TIMEOUT_READ_SEMI_BLOCKING, 0, 0);
        System.out.println("Event Listener open.");

        in = comPort.getInputStream();

    }

    void onLong() {
        try {
            char[] newData = new char[11];
            for (int i = 0; i < 11; i++) {
                newData[i] = (char) in.read();
            }
            System.out.print(" {" + String.valueOf(newData) + "|");

            String potenzString = String.valueOf(newData).substring(0,String.valueOf(newData).indexOf('.'));

            try {
                temp = Float.parseFloat(potenzString);
                System.out.print(" |" + temp + "} ");
                series.getData().add(new XYChart.Data<Number, Number>(ix, temp));
                ix++;
                if (ix == 200) {
                    ix = 0;
                    series.getData().clear();
                }

            } catch (Exception e) {
                return;
            }



        } catch (Exception e) {
        }
    }

    @FXML
    void onStart() {
        line_chart.getData().clear();
        series.getData().clear();
        line_chart.getData().add(series);
        at.start();
    }


    protected AnimationTimer at = new AnimationTimer() {
        @Override
        public void handle(long now) {
            onLong();
        }
    };

}
