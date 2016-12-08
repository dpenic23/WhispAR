package com.superpowered.simpleusb;

import android.os.Handler;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.widget.TextView;

import java.util.Locale;

public class MainActivity extends AppCompatActivity implements SuperpoweredUSBAudioHandler {
    private Handler handler;
    private TextView textView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        textView = (TextView)findViewById(R.id.text);

        SuperpoweredUSBAudio usbAudio = new SuperpoweredUSBAudio(getApplicationContext(), this);
        usbAudio.check();

        // Update UI every 40 ms.
        Runnable runnable = new Runnable() {
            @Override
            public void run() {
                int[] midi = getLatestMidiMessage();
                switch (midi[0]) {
                    case 8: textView.setText(String.format(Locale.ENGLISH, "Note Off, CH %d, %d, %d", midi[1] + 1, midi[2], midi[3])); break;
                    case 9: textView.setText(String.format(Locale.ENGLISH, "Note On, CH %d, %d, %d", midi[1] + 1, midi[2], midi[3])); break;
                    case 11: textView.setText(String.format(Locale.ENGLISH, "Control Change, CH %d, %d, %d", midi[1] + 1, midi[2], midi[3])); break;
                }
                handler.postDelayed(this, 40);
            }
        };
        handler = new Handler();
        handler.postDelayed(runnable, 40);
    }

    public void onUSBAudioDeviceAttached(int deviceIdentifier) {

    }

    public void onUSBMIDIDeviceAttached(int deviceIdentifier) {

    }

    public void onUSBDeviceDetached(int deviceIdentifier) {

    }

    private native int[]getLatestMidiMessage();

    static {
        System.loadLibrary("SuperpoweredExample");
    }
}
