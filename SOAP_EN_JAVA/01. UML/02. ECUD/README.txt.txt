Arquitectura Monster – Patron MVC (Model-View-Controller)

Estructura del proyecto:
========================

com.example.conuniclientemovil
├── model/                          ← Capa Model (datos y logica de negocio)
│   ├── ServiceType.java            Enum con los tipos de servicio y sus unidades
│   ├── ConversionRequest.java      DTO con los datos de una solicitud de conversion
│   ├── ConversionResult.java       Resultado tipado de una conversion (exito/error)
│   ├── LoginResult.java            Resultado tipado de un login (exito/error)
│   ├── SoapClient.java             Cliente SOAP (fuente de datos remota)
│   ├── SoapErrorMapper.java        Mapeo de codigos de error del servidor
│   └── AppPreferences.java         Persistencia local (SharedPreferences)
│
├── view/                           ← Capa View (interfaz de usuario)
│   ├── LoginActivity.java          Pantalla de login (credenciales + imagen monstruo)
│   └── ConversionActivity.java     Pantalla de conversion de unidades
│
└── controller/                     ← Capa Controller (coordinacion)
    ├── LoginController.java        Controlador dedicado al login
    └── ConversionController.java   Controlador dedicado a la conversion

Pantallas:
==========
1. LoginActivity   → Formulario de login con imagen del monstruo a la derecha.
                     Tras login exitoso, navega a ConversionActivity.
                     Si ya hay token guardado, salta directo a conversion.

2. ConversionActivity → Seleccion de servicio, unidades y valor a convertir.
                        Muestra el token activo y permite cerrar sesion (SALIR).

Flujo MVC:
==========
1. El usuario interactua con la View (LoginActivity o ConversionActivity).
2. La View captura la accion y la delega al Controller correspondiente.
3. El Controller invoca al Model (SoapClient, SoapErrorMapper) y construye
   objetos de resultado (LoginResult, ConversionResult).
4. El Controller devuelve el resultado a la View mediante callbacks.
5. La View renderiza el resultado en pantalla.
