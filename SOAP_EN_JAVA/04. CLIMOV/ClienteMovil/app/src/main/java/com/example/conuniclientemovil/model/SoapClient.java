package com.example.conuniclientemovil.model;

import org.ksoap2.SoapEnvelope;
import org.ksoap2.SoapFault;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapPrimitive;
import org.ksoap2.serialization.SoapSerializationEnvelope;
import org.ksoap2.transport.HttpTransportSE;

import java.io.IOException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

/**
 * Cliente SOAP que ejecuta llamadas a los servicios web de ConUni.
 *
 * Forma parte de la capa Model: es la fuente de datos remota.
 * No contiene lógica de presentación ni de coordinación de UI.
 */
public class SoapClient {

    private static final String NAMESPACE = "http://WebServices.monster.edu.ec/";
    private static final String SERVICE_LOGIN = "WSLogin";
    private static final String SERVICE_LONGITUD = "WSLongitud";
    private static final String SERVICE_MASA = "WSMasa";
    private static final String SERVICE_TEMPERATURA = "WSTemperatura";

    private static final String METHOD_LOGIN = "login";
    private static final String METHOD_CONVERTIR_LONGITUD = "convertirLongitud";
    private static final String METHOD_CONVERTIR_MASA = "convertirMasa";
    private static final String METHOD_CONVERTIR_TEMPERATURA = "convertirTemperatura";

    private static final int TIMEOUT_MS = 15000;

    private final ExecutorService executorService;
    private volatile String sessionToken;

    public SoapClient() {
        this(Executors.newFixedThreadPool(2));
    }

    public SoapClient(ExecutorService executorService) {
        this.executorService = executorService;
    }

    public Future<String> login(String baseUrl, String usuario, String clave) {
        return executorService.submit(() -> {
            SoapObject request = new SoapObject(NAMESPACE, METHOD_LOGIN);
            request.addProperty("usuario", usuario);
            request.addProperty("clave", clave);

            String endpoint = buildEndpoint(baseUrl, SERVICE_LOGIN);
            String token = callForString(endpoint, METHOD_LOGIN, request);
            sessionToken = token;
            return token;
        });
    }

    public Future<Double> convertirLongitud(
            String baseUrl,
            double valor,
            String unidadOrigen,
            String unidadDestino,
            String token
    ) {
        return executorService.submit(() -> {
            SoapObject request = new SoapObject(NAMESPACE, METHOD_CONVERTIR_LONGITUD);
            request.addProperty("valor", String.valueOf(valor));
            request.addProperty("unidadOrigen", unidadOrigen);
            request.addProperty("unidadDestino", unidadDestino);
            request.addProperty("token", token);

            String endpoint = buildEndpoint(baseUrl, SERVICE_LONGITUD);
            return callForDouble(endpoint, METHOD_CONVERTIR_LONGITUD, request);
        });
    }

    public Future<Double> convertirMasa(
            String baseUrl,
            double valor,
            String unidadOrigen,
            String unidadDestino,
            String token
    ) {
        return executorService.submit(() -> {
            SoapObject request = new SoapObject(NAMESPACE, METHOD_CONVERTIR_MASA);
            request.addProperty("valor", String.valueOf(valor));
            request.addProperty("unidadOrigen", unidadOrigen);
            request.addProperty("unidadDestino", unidadDestino);
            request.addProperty("token", token);

            String endpoint = buildEndpoint(baseUrl, SERVICE_MASA);
            return callForDouble(endpoint, METHOD_CONVERTIR_MASA, request);
        });
    }

    public Future<Double> convertirTemperatura(
            String baseUrl,
            double valor,
            String opcionOrigen,
            String opcionDestino,
            String token
    ) {
        return executorService.submit(() -> {
            SoapObject request = new SoapObject(NAMESPACE, METHOD_CONVERTIR_TEMPERATURA);
            request.addProperty("valor", String.valueOf(valor));
            request.addProperty("opcionOrigen", opcionOrigen);
            request.addProperty("opcionDestino", opcionDestino);
            request.addProperty("token", token);

            String endpoint = buildEndpoint(baseUrl, SERVICE_TEMPERATURA);
            return callForDouble(endpoint, METHOD_CONVERTIR_TEMPERATURA, request);
        });
    }

    public String getSessionToken() {
        return sessionToken;
    }

    public void clearSessionToken() {
        sessionToken = null;
    }

    public void shutdown() {
        executorService.shutdown();
    }

    private String buildEndpoint(String baseUrl, String servicePath) {
        if (baseUrl == null || baseUrl.trim().isEmpty()) {
            throw new IllegalArgumentException("baseUrl no puede estar vacia");
        }

        String normalizedBaseUrl = baseUrl.trim();
        if (!normalizedBaseUrl.endsWith("/")) {
            normalizedBaseUrl = normalizedBaseUrl + "/";
        }

        return normalizedBaseUrl + servicePath;
    }

    private double callForDouble(String endpoint, String methodName, SoapObject request) throws Exception {
        Object response = call(endpoint, methodName, request);
        if (response == null) {
            throw new IOException("Respuesta SOAP vacia para: " + methodName);
        }

        return Double.parseDouble(response.toString());
    }

    private String callForString(String endpoint, String methodName, SoapObject request) throws Exception {
        Object response = call(endpoint, methodName, request);
        if (response == null) {
            throw new IOException("Respuesta SOAP vacia para: " + methodName);
        }

        return response.toString();
    }

    private Object call(String endpoint, String methodName, SoapObject request) throws Exception {
        SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(SoapEnvelope.VER11);
        envelope.dotNet = false;
        envelope.implicitTypes = true;
        envelope.setOutputSoapObject(request);

        HttpTransportSE httpTransport = new HttpTransportSE(endpoint, TIMEOUT_MS);
        String soapAction = NAMESPACE + methodName;
        httpTransport.call(soapAction, envelope);

        if (envelope.bodyIn instanceof SoapFault) {
            SoapFault fault = (SoapFault) envelope.bodyIn;
            throw new IOException("SOAP Fault: " + fault.faultstring);
        }

        Object response = envelope.getResponse();
        if (response instanceof SoapPrimitive) {
            return ((SoapPrimitive) response).toString();
        }

        return response;
    }
}
