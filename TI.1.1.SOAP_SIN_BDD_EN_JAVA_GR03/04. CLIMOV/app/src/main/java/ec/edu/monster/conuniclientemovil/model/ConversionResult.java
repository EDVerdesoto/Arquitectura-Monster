package ec.edu.monster.conuniclientemovil.model;

/**
 * Modelo que encapsula el resultado de una conversión.
 * Puede indicar éxito, error de servicio, o error general.
 */
public class ConversionResult {

    public enum Status {
        SUCCESS,
        SERVICE_ERROR,
        ERROR
    }

    private final Status status;
    private final double value;
    private final String message;

    private ConversionResult(Status status, double value, String message) {
        this.status = status;
        this.value = value;
        this.message = message;
    }

    public static ConversionResult success(double value) {
        return new ConversionResult(Status.SUCCESS, value, null);
    }

    public static ConversionResult serviceError(String message, double rawValue) {
        return new ConversionResult(Status.SERVICE_ERROR, rawValue, message);
    }

    public static ConversionResult error(String message) {
        return new ConversionResult(Status.ERROR, 0, message);
    }

    public Status getStatus() {
        return status;
    }

    public double getValue() {
        return value;
    }

    public String getMessage() {
        return message;
    }

    public boolean isSuccess() {
        return status == Status.SUCCESS;
    }

    public boolean isTokenError() {
        return message != null && message.contains("Token invalido");
    }
}
