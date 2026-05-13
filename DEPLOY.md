# Configuracion de despliegue

## 1. Payara Server (unico dominio, puerto 8080)

Ambos WARs se despliegan en la misma instancia Payara en el puerto 8080:

```bash
asadmin start-domain
asadmin deploy --force=true SOAP_EN_JAVA/04.\ SERVIDOR/WS_ConUni_Java_GR03/target/WS_ConUni_Java_GR03-1.0-SNAPSHOT.war
asadmin deploy --force=true REST_JAVA/04.\ SERVIDOR/WS_CONV_UNI_RESTFULL_JAVA/target/WS_CONV_UNI_RESTFULL_JAVA-1.0-SNAPSHOT.war
```

Los `glassfish-web.xml` incluidos fijan los context roots a:
- SOAP: `/WS_ConUni_Java_GR03`
- REST: `/WS_CONV_UNI_RESTFULL_JAVA`

## 2. Cloudflare Tunnel - Zero Trust Dashboard

1. Ve a https://one.dash.cloudflare.com/ → Networks → Tunnels
2. Crea un tunel (o usa uno existente) e instala `cloudflared` en tu maquina
3. En el tunel → Public Hostname → Add a public hostname:

   **SOAP:**
   - Subdomain: `javasoap`
   - Domain: `dr00p3r.top`
   - Path: **(dejalo vacio)**
   - Type: `HTTP`
   - URL: `localhost:8080`

   **REST:**
   - Subdomain: `javarest`
   - Domain: `dr00p3r.top`
   - Path: **(dejalo vacio)**
   - Type: `HTTP`
   - URL: `localhost:8080`

4. Los DNS `javasoap.dr00p3r.top` y `javarest.dr00p3r.top` deben ser `CNAME → <tunnel-id>.cfargotunnel.com`

> **Importante:** Cloudflare NO reescribe paths. El Service URL solo acepta `host:port`.
> El context path (`/WS_ConUni_Java_GR03`, `/WS_CONV_UNI_RESTFULL_JAVA`) lo incluyen los clientes en sus URLs.

## 3. URLs finales de consumo

| Servicio | URL publica |
|----------|-------------|
| SOAP Login | `https://javasoap.dr00p3r.top/WS_ConUni_Java_GR03/WSLogin` |
| SOAP Longitud | `https://javasoap.dr00p3r.top/WS_ConUni_Java_GR03/WSLongitud` |
| SOAP Masa | `https://javasoap.dr00p3r.top/WS_ConUni_Java_GR03/WSMasa` |
| SOAP Temperatura | `https://javasoap.dr00p3r.top/WS_ConUni_Java_GR03/WSTemperatura` |
| REST Login | `https://javarest.dr00p3r.top/WS_CONV_UNI_RESTFULL_JAVA/auth/login` |
| REST Longitud | `https://javarest.dr00p3r.top/WS_CONV_UNI_RESTFULL_JAVA/conversor/longitud` |
| REST Masa | `https://javarest.dr00p3r.top/WS_CONV_UNI_RESTFULL_JAVA/conversor/masa` |
| REST Temperatura | `https://javarest.dr00p3r.top/WS_CONV_UNI_RESTFULL_JAVA/conversor/temperatura` |
