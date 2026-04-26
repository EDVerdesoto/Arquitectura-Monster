package com.example.conuniclientemovil.model;

/**
 * Modelo que encapsula el resultado de un intento de login.
 */
public class LoginResult {

    private final boolean success;
    private final String token;
    private final String errorMessage;

    private LoginResult(boolean success, String token, String errorMessage) {
        this.success = success;
        this.token = token;
        this.errorMessage = errorMessage;
    }

    public static LoginResult success(String token) {
        return new LoginResult(true, token, null);
    }

    public static LoginResult error(String message) {
        return new LoginResult(false, null, message);
    }

    public boolean isSuccess() {
        return success;
    }

    public String getToken() {
        return token;
    }

    public String getErrorMessage() {
        return errorMessage;
    }
}
