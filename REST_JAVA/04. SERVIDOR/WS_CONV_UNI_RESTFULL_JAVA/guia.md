# 📘 Guía de Implementación — Cliente para WS_CONV_UNI_RESTFULL_JAVA

> **Versión:** 1.0  
> **Servidor:** Jakarta EE 11 (JAX-RS) sobre Payara / GlassFish  
> **Formato de datos:** JSON (`application/json`)

---

## Tabla de Contenidos

1. [Descripción General](#1-descripción-general)
2. [URL Base del Servidor](#2-url-base-del-servidor)
3. [Modelo de Datos — `Conversion`](#3-modelo-de-datos--conversion)
4. [Flujo de Autenticación](#4-flujo-de-autenticación)
5. [Endpoints Disponibles](#5-endpoints-disponibles)
   - 5.1 [Login](#51-login---post-authlogin)
   - 5.2 [Conversión de Longitud](#52-conversión-de-longitud---post-conversorlongitud)
   - 5.3 [Conversión de Masa](#53-conversión-de-masa---post-conversormasa)
   - 5.4 [Conversión de Temperatura](#54-conversión-de-temperatura---post-conversortemperatura)
6. [Unidades Soportadas](#6-unidades-soportadas)
7. [Códigos de Respuesta HTTP](#7-códigos-de-respuesta-http)
8. [Ejemplos de Implementación por Plataforma](#8-ejemplos-de-implementación-por-plataforma)
   - 8.1 [Android (Java / Kotlin)](#81-android-java--kotlin)
   - 8.2 [JavaScript / Web (Fetch API)](#82-javascript--web-fetch-api)
   - 8.3 [Java Desktop (HttpClient)](#83-java-desktop-httpclient)
   - 8.4 [cURL (línea de comandos)](#84-curl-línea-de-comandos)
9. [Configuración CORS](#9-configuración-cors)
10. [Errores Comunes y Soluciones](#10-errores-comunes-y-soluciones)

---

## 1. Descripción General

El servidor **WS_CONV_UNI_RESTFULL_JAVA** es un Web Service RESTful construido con **Jakarta EE 11 (JAX-RS)** que expone servicios de **conversión de unidades** en tres categorías:

| Categoría      | Descripción                                  |
|----------------|----------------------------------------------|
| **Longitud**   | Centímetros, Metros, Pies, Yardas, Millas    |
| **Masa**       | Gramos, Kilogramos, Onzas, Libras, Quintales |
| **Temperatura**| Celsius, Fahrenheit, Kelvin, Newton, Réaumur  |

El servidor requiere **autenticación mediante token**: primero debes hacer login y luego enviar el token en cada solicitud de conversión.

---

## 2. URL Base del Servidor

```
http://<HOST>:<PUERTO>/<CONTEXT_PATH>/
```

### Valores típicos según el servidor de aplicaciones

| Servidor           | URL Base por defecto                                                    |
|--------------------|-------------------------------------------------------------------------|
| **Payara / GlassFish** | `http://localhost:8080/WS_CONV_UNI_RESTFULL_JAVA-1.0-SNAPSHOT/`     |
| **WildFly**        | `http://localhost:8080/WS_CONV_UNI_RESTFULL_JAVA-1.0-SNAPSHOT/`         |
| **TomEE**          | `http://localhost:8080/WS_CONV_UNI_RESTFULL_JAVA-1.0-SNAPSHOT/`         |

> **Nota:** El context path depende del nombre del archivo `.war` generado por Maven. Puedes verificarlo al desplegar la aplicación en tu servidor.

La raíz REST está mapeada a `/` (definida por `@ApplicationPath("/")`), por lo que los endpoints se montan directamente sobre el context path.

---

## 3. Modelo de Datos — `Conversion`

Todas las peticiones y respuestas utilizan el **mismo modelo JSON**:

### Estructura JSON completa

```json
{
  "from":    "string",
  "to":      "string",
  "value":   0.0,
  "token":   "string",
  "result":  0.0,
  "message": "string",
  "success": true
}
```

### Descripción de campos

| Campo     | Tipo      | Dirección       | Descripción                                           |
|-----------|-----------|-----------------|-------------------------------------------------------|
| `from`    | `String`  | Cliente → Server | Unidad de origen (o usuario en login)                 |
| `to`      | `String`  | Cliente → Server | Unidad de destino (o contraseña en login)             |
| `value`   | `double`  | Cliente → Server | Valor numérico a convertir                            |
| `token`   | `String`  | Bidireccional    | Token de sesión (lo devuelve el login, se envía después) |
| `result`  | `double`  | Server → Cliente | Resultado de la conversión                            |
| `message` | `String`  | Server → Cliente | Mensaje descriptivo del resultado                     |
| `success` | `boolean` | Server → Cliente | `true` si la operación fue exitosa                    |

---

## 4. Flujo de Autenticación

El servidor usa un sistema de autenticación basado en tokens en memoria. El flujo es el siguiente:

```
┌─────────┐                              ┌──────────┐
│ CLIENTE │                              │ SERVIDOR │
└────┬────┘                              └────┬─────┘
     │                                        │
     │  1. POST /auth/login                   │
     │  { "from":"monster","to":"monster9" }  │
     │ ─────────────────────────────────────► │
     │                                        │
     │  2. Respuesta con token                │
     │  { "token":"uuid...", "success":true } │
     │ ◄───────────────────────────────────── │
     │                                        │
     │  3. POST /conversor/longitud           │
     │  { "token":"uuid...", ... }            │
     │ ─────────────────────────────────────► │
     │                                        │
     │  4. Respuesta con resultado            │
     │  { "result":100.0, "success":true }    │
     │ ◄───────────────────────────────────── │
```

### Credenciales válidas

| Campo    | Valor       |
|----------|-------------|
| Usuario  | `monster`   |
| Contraseña | `monster9`|

> **⚠️ Importante:** El campo `from` se usa como usuario y el campo `to` como contraseña en el endpoint de login. El token debe almacenarse en el cliente y enviarse en **todas** las peticiones de conversión posteriores.

---

## 5. Endpoints Disponibles

### 5.1 Login — `POST /auth/login`

Autentica al usuario y devuelve un token de sesión.

**Request:**

```http
POST /auth/login
Content-Type: application/json

{
  "from": "monster",
  "to": "monster9"
}
```

**Response exitosa (200 OK):**

```json
{
  "from": "monster",
  "to": "monster9",
  "value": 0.0,
  "token": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "result": 0.0,
  "message": "Bienvenido al sistema.",
  "success": true
}
```

**Response fallida (401 Unauthorized):**

```json
{
  "from": "usuario_incorrecto",
  "to": "clave_incorrecta",
  "value": 0.0,
  "token": null,
  "result": 0.0,
  "message": "Usuario o clave fallidos, intenta de nuevo.",
  "success": false
}
```

---

### 5.2 Conversión de Longitud — `POST /conversor/longitud`

Convierte entre unidades de longitud.

**Request:**

```http
POST /conversor/longitud
Content-Type: application/json

{
  "from": "metros",
  "to": "centimetros",
  "value": 1.0,
  "token": "a1b2c3d4-e5f6-7890-abcd-ef1234567890"
}
```

**Response exitosa (200 OK):**

```json
{
  "from": "metros",
  "to": "centimetros",
  "value": 1.0,
  "token": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "result": 100.0,
  "message": "Conversión exitosa.",
  "success": true
}
```

**Unidades válidas:** `centimetros`, `metros`, `pies`, `yardas`, `millas`

---

### 5.3 Conversión de Masa — `POST /conversor/masa`

Convierte entre unidades de masa.

**Request:**

```http
POST /conversor/masa
Content-Type: application/json

{
  "from": "kilogramos",
  "to": "libras",
  "value": 5.0,
  "token": "a1b2c3d4-e5f6-7890-abcd-ef1234567890"
}
```

**Response exitosa (200 OK):**

```json
{
  "from": "kilogramos",
  "to": "libras",
  "value": 5.0,
  "token": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "result": 11.0231,
  "message": "Conversión exitosa.",
  "success": true
}
```

**Unidades válidas:** `gramos`, `kilogramos`, `onzas`, `libras`, `quintales`

---

### 5.4 Conversión de Temperatura — `POST /conversor/temperatura`

Convierte entre unidades de temperatura.

**Request:**

```http
POST /conversor/temperatura
Content-Type: application/json

{
  "from": "celsius",
  "to": "fahrenheit",
  "value": 100.0,
  "token": "a1b2c3d4-e5f6-7890-abcd-ef1234567890"
}
```

**Response exitosa (200 OK):**

```json
{
  "from": "celsius",
  "to": "fahrenheit",
  "value": 100.0,
  "token": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "result": 212.0,
  "message": "Conversión exitosa.",
  "success": true
}
```

**Unidades válidas:** `celsius`, `fahrenheit`, `kelvin`, `newton`, `reaumur`

---

## 6. Unidades Soportadas

### Longitud

| Valor (String) | Descripción        |
|-----------------|--------------------|
| `centimetros`   | Centímetros (cm)   |
| `metros`        | Metros (m)         |
| `pies`          | Pies (ft)          |
| `yardas`        | Yardas (yd)        |
| `millas`        | Millas (mi)        |

### Masa

| Valor (String) | Descripción        |
|-----------------|--------------------|
| `gramos`        | Gramos (g)         |
| `kilogramos`    | Kilogramos (kg)    |
| `onzas`         | Onzas (oz)         |
| `libras`        | Libras (lb)        |
| `quintales`     | Quintales (qq)     |

### Temperatura

| Valor (String) | Descripción        |
|-----------------|--------------------|
| `celsius`       | Celsius (°C)       |
| `fahrenheit`    | Fahrenheit (°F)    |
| `kelvin`        | Kelvin (K)         |
| `newton`        | Newton (°N)        |
| `reaumur`       | Réaumur (°Ré)      |

> **Nota:** Los valores de las unidades son **case-insensitive** (el servidor los convierte a minúsculas internamente). Sin embargo, se recomienda enviarlos siempre en minúsculas.

---

## 7. Códigos de Respuesta HTTP

| Código | Significado              | Cuándo ocurre                                       |
|--------|--------------------------|-----------------------------------------------------|
| `200`  | OK                       | Login exitoso o conversión exitosa                  |
| `400`  | Bad Request              | Unidad de origen o destino no reconocida             |
| `401`  | Unauthorized             | Credenciales incorrectas o token inválido/expirado   |

---

## 8. Ejemplos de Implementación por Plataforma

### 8.1 Android (Java / Kotlin)

#### Dependencia (Retrofit + Gson)

```gradle
// build.gradle (app)
dependencies {
    implementation 'com.squareup.retrofit2:retrofit:2.9.0'
    implementation 'com.squareup.retrofit2:converter-gson:2.9.0'
}
```

#### Modelo

```java
public class Conversion {
    private String from;
    private String to;
    private double value;
    private String token;
    private double result;
    private String message;
    private boolean success;

    // Constructor vacío
    public Conversion() {}

    // Constructor para login
    public Conversion(String from, String to) {
        this.from = from;
        this.to = to;
    }

    // Constructor para conversión
    public Conversion(String from, String to, double value, String token) {
        this.from = from;
        this.to = to;
        this.value = value;
        this.token = token;
    }

    // Getters y Setters...
    public String getFrom()       { return from; }
    public void setFrom(String f) { this.from = f; }

    public String getTo()         { return to; }
    public void setTo(String t)   { this.to = t; }

    public double getValue()      { return value; }
    public void setValue(double v) { this.value = v; }

    public String getToken()        { return token; }
    public void setToken(String t)  { this.token = t; }

    public double getResult()       { return result; }
    public void setResult(double r) { this.result = r; }

    public String getMessage()        { return message; }
    public void setMessage(String m)  { this.message = m; }

    public boolean isSuccess()          { return success; }
    public void setSuccess(boolean s)   { this.success = s; }
}
```

#### Interface del API

```java
import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.POST;

public interface ConversionApi {

    @POST("auth/login")
    Call<Conversion> login(@Body Conversion body);

    @POST("conversor/longitud")
    Call<Conversion> convertirLongitud(@Body Conversion body);

    @POST("conversor/masa")
    Call<Conversion> convertirMasa(@Body Conversion body);

    @POST("conversor/temperatura")
    Call<Conversion> convertirTemperatura(@Body Conversion body);
}
```

#### Configuración de Retrofit

```java
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class ApiClient {

    // ⚠️ Cambia esta URL por la dirección real de tu servidor
    private static final String BASE_URL =
        "http://10.0.2.2:8080/WS_CONV_UNI_RESTFULL_JAVA-1.0-SNAPSHOT/";

    private static Retrofit retrofit = null;

    public static ConversionApi getApi() {
        if (retrofit == null) {
            retrofit = new Retrofit.Builder()
                .baseUrl(BASE_URL)
                .addConverterFactory(GsonConverterFactory.create())
                .build();
        }
        return retrofit.create(ConversionApi.class);
    }
}
```

> **Nota para el emulador de Android:** Usa `10.0.2.2` en lugar de `localhost` para referirte a la máquina host desde el emulador.

#### Uso en una Activity

```java
// --- LOGIN ---
Conversion loginBody = new Conversion("monster", "monster9");

ApiClient.getApi().login(loginBody).enqueue(new Callback<Conversion>() {
    @Override
    public void onResponse(Call<Conversion> call, Response<Conversion> response) {
        if (response.isSuccessful() && response.body().isSuccess()) {
            String token = response.body().getToken();
            // Guardar el token para usarlo después
        }
    }

    @Override
    public void onFailure(Call<Conversion> call, Throwable t) {
        // Manejar error de red
    }
});

// --- CONVERSIÓN ---
Conversion convBody = new Conversion("metros", "centimetros", 5.0, token);

ApiClient.getApi().convertirLongitud(convBody).enqueue(new Callback<Conversion>() {
    @Override
    public void onResponse(Call<Conversion> call, Response<Conversion> response) {
        if (response.isSuccessful() && response.body().isSuccess()) {
            double resultado = response.body().getResult();
            // Mostrar resultado
        }
    }

    @Override
    public void onFailure(Call<Conversion> call, Throwable t) {
        // Manejar error de red
    }
});
```

---

### 8.2 JavaScript / Web (Fetch API)

```javascript
const BASE_URL = "http://localhost:8080/WS_CONV_UNI_RESTFULL_JAVA-1.0-SNAPSHOT";
let token = null;

// ─── LOGIN ───────────────────────────────────────────
async function login(usuario, clave) {
  const response = await fetch(`${BASE_URL}/auth/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ from: usuario, to: clave }),
  });

  const data = await response.json();

  if (data.success) {
    token = data.token;
    console.log("Login exitoso. Token:", token);
  } else {
    console.error("Login fallido:", data.message);
  }

  return data;
}

// ─── CONVERSIÓN GENÉRICA ─────────────────────────────
async function convertir(tipo, from, to, value) {
  // tipo puede ser: "longitud", "masa", "temperatura"
  const response = await fetch(`${BASE_URL}/conversor/${tipo}`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ from, to, value, token }),
  });

  const data = await response.json();

  if (data.success) {
    console.log(`Resultado: ${data.result}`);
  } else {
    console.error("Error:", data.message);
  }

  return data;
}

// ─── USO ─────────────────────────────────────────────
await login("monster", "monster9");
await convertir("longitud", "metros", "pies", 10);
await convertir("masa", "kilogramos", "libras", 5);
await convertir("temperatura", "celsius", "fahrenheit", 100);
```

---

### 8.3 Java Desktop (HttpClient)

Usando `java.net.http.HttpClient` (Java 11+):

```java
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;

public class ClienteRest {

    private static final String BASE_URL =
        "http://localhost:8080/WS_CONV_UNI_RESTFULL_JAVA-1.0-SNAPSHOT";

    private static final HttpClient client = HttpClient.newHttpClient();

    public static String hacerPost(String endpoint, String jsonBody) throws Exception {
        HttpRequest request = HttpRequest.newBuilder()
            .uri(URI.create(BASE_URL + endpoint))
            .header("Content-Type", "application/json")
            .POST(HttpRequest.BodyPublishers.ofString(jsonBody))
            .build();

        HttpResponse<String> response = client.send(request,
            HttpResponse.BodyHandlers.ofString());

        System.out.println("Status: " + response.statusCode());
        System.out.println("Body:   " + response.body());
        return response.body();
    }

    public static void main(String[] args) throws Exception {
        // 1. Login
        String loginJson = """
            {
              "from": "monster",
              "to": "monster9"
            }
            """;

        String loginResp = hacerPost("/auth/login", loginJson);
        // Extraer el token de loginResp (usar Gson, Jackson, o parseo manual)
        // Ejemplo simplificado:
        String token = loginResp.split("\"token\":\"")[1].split("\"")[0];

        // 2. Conversión
        String convJson = String.format("""
            {
              "from": "celsius",
              "to": "fahrenheit",
              "value": 100.0,
              "token": "%s"
            }
            """, token);

        hacerPost("/conversor/temperatura", convJson);
    }
}
```

---

### 8.4 cURL (línea de comandos)

#### Login

```bash
curl -X POST http://localhost:8080/WS_CONV_UNI_RESTFULL_JAVA-1.0-SNAPSHOT/auth/login \
  -H "Content-Type: application/json" \
  -d '{"from":"monster","to":"monster9"}'
```

#### Conversión de Longitud

```bash
curl -X POST http://localhost:8080/WS_CONV_UNI_RESTFULL_JAVA-1.0-SNAPSHOT/conversor/longitud \
  -H "Content-Type: application/json" \
  -d '{"from":"metros","to":"pies","value":10.0,"token":"TU_TOKEN_AQUI"}'
```

#### Conversión de Masa

```bash
curl -X POST http://localhost:8080/WS_CONV_UNI_RESTFULL_JAVA-1.0-SNAPSHOT/conversor/masa \
  -H "Content-Type: application/json" \
  -d '{"from":"kilogramos","to":"libras","value":5.0,"token":"TU_TOKEN_AQUI"}'
```

#### Conversión de Temperatura

```bash
curl -X POST http://localhost:8080/WS_CONV_UNI_RESTFULL_JAVA-1.0-SNAPSHOT/conversor/temperatura \
  -H "Content-Type: application/json" \
  -d '{"from":"celsius","to":"fahrenheit","value":100.0,"token":"TU_TOKEN_AQUI"}'
```

---

## 9. Configuración CORS

El servidor tiene habilitado CORS con la siguiente configuración:

| Parámetro                        | Valor                                      |
|----------------------------------|---------------------------------------------|
| `Access-Control-Allow-Origin`    | `http://localhost:3000`                     |
| `Access-Control-Allow-Methods`   | `GET, POST, PUT, DELETE, OPTIONS`           |
| `Access-Control-Allow-Headers`   | `Content-Type, X-Requested-With, Authorization` |
| `Access-Control-Allow-Credentials` | `true`                                    |

> **⚠️ Si tu cliente corre en un puerto diferente a `3000`**, necesitarás modificar el archivo `CORSFilter.java` en el servidor para agregar tu origen. Busca la línea:
>
> ```java
> httpResponse.setHeader("Access-Control-Allow-Origin", "http://localhost:3000");
> ```
>
> Y cámbiala por tu dirección, o usa `"*"` para permitir todos los orígenes (solo en desarrollo):
>
> ```java
> httpResponse.setHeader("Access-Control-Allow-Origin", "*");
> ```

---

## 10. Errores Comunes y Soluciones

### ❌ `401 Unauthorized` — "Tu sesión expiró o el token no vale."

| Causa posible                                  | Solución                                        |
|------------------------------------------------|-------------------------------------------------|
| No estás enviando el campo `token`             | Agrega el token recibido del login al JSON       |
| El token es incorrecto o fue tipografiado mal  | Usa exactamente el token devuelto por `/auth/login` |
| El servidor fue reiniciado                     | Vuelve a hacer login (los tokens se guardan en memoria) |

### ❌ `400 Bad Request` — "Unidad de origen/destino no reconocida"

| Causa posible                                  | Solución                                        |
|------------------------------------------------|-------------------------------------------------|
| Escribiste mal la unidad                       | Verifica que usas exactamente los valores de la [Sección 6](#6-unidades-soportadas) |
| Enviaste la unidad en otro idioma              | Las unidades deben estar **en español** (e.g., `metros`, no `meters`) |
| La temperatura usa un estándar diferente       | Para temperatura, usa los nombres estándar: `celsius`, `fahrenheit`, `kelvin`, `newton`, `reaumur` |

### ❌ Error de CORS en el navegador

| Causa posible                                  | Solución                                        |
|------------------------------------------------|-------------------------------------------------|
| Tu frontend corre en un puerto diferente a 3000 | Modifica `CORSFilter.java` según la [Sección 9](#9-configuración-cors) |
| Estás usando `file://` como origen             | Sirve tu HTML desde un servidor local (`http://`)  |

### ❌ `Connection refused` o error de red

| Causa posible                                  | Solución                                        |
|------------------------------------------------|-------------------------------------------------|
| El servidor no está corriendo                  | Despliega el `.war` en tu servidor de aplicaciones |
| Puerto o host incorrecto                       | Verifica que la URL base coincida con tu configuración |
| En Android, usas `localhost`                   | Usa `10.0.2.2` en el emulador o la IP de tu PC en dispositivo físico |

---

> **💡 Tip:** Para probar rápidamente todos los endpoints, puedes usar herramientas como [Postman](https://www.postman.com/) o [Insomnia](https://insomnia.rest/). Importa las URLs y JSONs de ejemplo de esta guía.
