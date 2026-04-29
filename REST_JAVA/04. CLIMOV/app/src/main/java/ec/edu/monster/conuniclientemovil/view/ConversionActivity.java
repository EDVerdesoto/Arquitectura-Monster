package ec.edu.monster.conuniclientemovil.view;

import android.content.Intent;
import android.os.Bundle;
import android.text.TextUtils;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;

import ec.edu.monster.conuniclientemovil.R;
import ec.edu.monster.conuniclientemovil.controller.ConversionController;
import ec.edu.monster.conuniclientemovil.model.AppPreferences;
import ec.edu.monster.conuniclientemovil.model.ConversionRequest;
import ec.edu.monster.conuniclientemovil.model.ConversionResult;
import ec.edu.monster.conuniclientemovil.model.ServiceType;
import ec.edu.monster.conuniclientemovil.model.RestClient;

import java.util.ArrayList;
import java.util.List;

/**
 * Pantalla de conversión de unidades — View del patrón MVC.
 *
 * Recibe el token y baseUrl del LoginActivity.
 * Delega las conversiones al ConversionController.
 * Permite cerrar sesión y volver al LoginActivity.
 */
public class ConversionActivity extends AppCompatActivity {

    /* ── Widgets ────────────────────────────────────────── */
    private EditText etValor;
    private EditText etDestinoPreview;

    private Spinner spServicio;
    private Spinner spUnidadOrigen;
    private Spinner spUnidadDestino;

    private Button btnConvertir;
    private Button btnLogout;

    private TextView tvEstado;

    /* ── Controller & Model ─────────────────────────────── */
    private ConversionController conversionController;
    private AppPreferences appPreferences;
    private RestClient restClient;

    private String currentToken;
    private String baseUrl;

    /* ══════════════════════════════════════════════════════
       Ciclo de vida
       ══════════════════════════════════════════════════════ */

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_conversion);

        extractIntentData();
        initDependencies();
        bindViews();
        setupServiceSpinner();
        setInitialValues();
        setupActions();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        conversionController.shutdown();
        restClient.shutdown();
    }

    /* ══════════════════════════════════════════════════════
       Inicialización
       ══════════════════════════════════════════════════════ */

    private void extractIntentData() {
        Intent intent = getIntent();
        currentToken = intent.getStringExtra(LoginActivity.EXTRA_TOKEN);
        baseUrl = intent.getStringExtra(LoginActivity.EXTRA_BASE_URL);
    }

    private void initDependencies() {
        appPreferences = new AppPreferences(this);
        restClient = new RestClient();
        conversionController = new ConversionController(restClient);
    }

    private void bindViews() {
        etValor = findViewById(R.id.etValor);
        etDestinoPreview = findViewById(R.id.etDestinoPreview);

        spServicio = findViewById(R.id.spServicio);
        spUnidadOrigen = findViewById(R.id.spUnidadOrigen);
        spUnidadDestino = findViewById(R.id.spUnidadDestino);

        btnConvertir = findViewById(R.id.btnConvertir);
        btnLogout = findViewById(R.id.btnLogout);

        tvEstado = findViewById(R.id.tvEstado);
    }

    private void setupServiceSpinner() {
        List<String> serviceNames = new ArrayList<>();
        for (ServiceType type : ServiceType.values()) {
            serviceNames.add(type.getDisplayName());
        }

        ArrayAdapter<String> serviceAdapter = new ArrayAdapter<>(
                this,
                R.layout.item_spinner_value,
                serviceNames
        );
        serviceAdapter.setDropDownViewResource(R.layout.item_spinner_dropdown_value);
        spServicio.setAdapter(serviceAdapter);

        spServicio.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, android.view.View view,
                                       int position, long id) {
                ServiceType selected = ServiceType.values()[position];
                updateUnitsForService(selected);
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
                updateUnitsForService(ServiceType.LONGITUD);
            }
        });

        updateUnitsForService(ServiceType.LONGITUD);
    }

    private void updateUnitsForService(ServiceType serviceType) {
        List<String> units = serviceType.getUnits();

        ArrayAdapter<String> originAdapter = new ArrayAdapter<>(
                this, R.layout.item_spinner_value, units);
        originAdapter.setDropDownViewResource(R.layout.item_spinner_dropdown_value);
        spUnidadOrigen.setAdapter(originAdapter);

        ArrayAdapter<String> destinationAdapter = new ArrayAdapter<>(
                this, R.layout.item_spinner_value, units);
        destinationAdapter.setDropDownViewResource(R.layout.item_spinner_dropdown_value);
        spUnidadDestino.setAdapter(destinationAdapter);

        if (units.size() > 1) {
            spUnidadDestino.setSelection(1);
        }
    }

    private void setInitialValues() {
        setStatusText("Listo", false);
    }

    private void setupActions() {
        btnConvertir.setOnClickListener(v -> doConvert());
        btnLogout.setOnClickListener(v -> doLogout());
    }

    /* ══════════════════════════════════════════════════════
       Acción de conversión → delegada al Controller
       ══════════════════════════════════════════════════════ */

    private void doConvert() {
        String valueText = etValor.getText().toString().trim();
        String selectedServiceName = (String) spServicio.getSelectedItem();
        String origin = (String) spUnidadOrigen.getSelectedItem();
        String destination = (String) spUnidadDestino.getSelectedItem();

        if (TextUtils.isEmpty(valueText)
                || TextUtils.isEmpty(selectedServiceName)
                || TextUtils.isEmpty(origin)
                || TextUtils.isEmpty(destination)) {
            showStatus("Complete todos los datos de conversion.", true);
            return;
        }

        if (origin.equals(destination)) {
            showStatus("Seleccione unidades diferentes para convertir.", true);
            return;
        }

        if (TextUtils.isEmpty(currentToken)) {
            showStatus("Token invalido. Vuelva a iniciar sesion.", true);
            doLogout();
            return;
        }

        final double inputValue;
        try {
            inputValue = Double.parseDouble(valueText.replace(',', '.'));
            if (Double.isNaN(inputValue) || Double.isInfinite(inputValue)) {
                showStatus("El valor ingresado no es valido.", true);
                return;
            }
        } catch (NumberFormatException ex) {
            showStatus("El valor debe ser numerico.", true);
            return;
        }

        ServiceType serviceType = ServiceType.fromDisplayName(selectedServiceName);
        ConversionRequest request = new ConversionRequest(
                serviceType, inputValue, origin, destination, currentToken);

        setBusy(true);
        setStatusText("Convirtiendo...", false);

        conversionController.convert(request, baseUrl, result ->
                runOnUiThread(() -> handleConversionResult(result))
        );
    }

    /* ══════════════════════════════════════════════════════
       Renderizado de resultado
       ══════════════════════════════════════════════════════ */

    private void handleConversionResult(ConversionResult result) {
        switch (result.getStatus()) {
            case SUCCESS:
                etDestinoPreview.setText(String.valueOf(result.getValue()));
                showStatus("Conversion exitosa.", false);
                break;

            case SERVICE_ERROR:
                etDestinoPreview.setText(String.valueOf(result.getValue()));
                if (result.isTokenError()) {
                    showStatus(result.getMessage(), true);
                    doLogout();
                    return;
                }
                showStatus(result.getMessage(), true);
                break;

            case ERROR:
                showStatus("Error en conversion: " + result.getMessage(), true);
                break;
        }
        setBusy(false);
    }

    /* ══════════════════════════════════════════════════════
       Logout → volver al login
       ══════════════════════════════════════════════════════ */

    private void doLogout() {
        currentToken = null;
        appPreferences.clearToken();
        restClient.clearSessionToken();

        Intent intent = new Intent(this, LoginActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK);
        startActivity(intent);
        finish();
    }

    /* ══════════════════════════════════════════════════════
       Helpers de UI
       ══════════════════════════════════════════════════════ */

    private void setStatusText(String message, boolean isError) {
        tvEstado.setText(message);
        int colorId = isError ? android.R.color.holo_red_dark : android.R.color.holo_green_dark;
        tvEstado.setTextColor(ContextCompat.getColor(this, colorId));
    }

    private void showStatus(String message, boolean isError) {
        setStatusText(message, isError);
        Toast.makeText(this, message, Toast.LENGTH_SHORT).show();
    }

    private void setBusy(boolean busy) {
        etValor.setEnabled(!busy);
        spServicio.setEnabled(!busy);
        spUnidadOrigen.setEnabled(!busy);
        spUnidadDestino.setEnabled(!busy);
        btnConvertir.setEnabled(!busy);
        btnLogout.setEnabled(!busy);
    }
}
