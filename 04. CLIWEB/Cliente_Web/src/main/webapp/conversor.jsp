<%-- 
    Document   : conversor
    Created on : 15 abr 2026, 14:33:20
    Author     : Soto
--%>

<%@page contentType="text/html" pageEncoding="UTF-8"%>
<%
    // Verificamos que haya un token en la sesion
    String token = (String) session.getAttribute("miToken");
    if (token == null) {
        response.sendRedirect("index.jsp");
        return;
    }
%>
<!DOCTYPE html>
<html>
    <head>
        <title>Panel de Conversiones - ESPE</title>
        <style>
            body {
    font-family: sans-serif;
    background: #1e1e1e;
    color: white;
    text-align: center;
    }
            .panel {
    background: #2d2d2d;
    padding: 20px;
    display: inline-block;
    border-radius: 10px;
    margin-top: 50px;
    }
            select, input, button {
    margin: 10px;
    padding: 10px;
    border-radius: 5px;
    border: none;
    }
            button {
    background: #4CAF50;
    color: white;
    cursor: pointer;
    font-weight: bold;
    }
            .resultado {
    font-size: 1.5em;
    color: #4CAF50;
    margin-top: 20px;
    }
        </style>

        <script>
            const dicUnidades = {
                longitud: ["centimetros", "metros", "pies", "yardas", "millas"],
                masa: ["gramos", "kilogramos", "libras", "quintales", "onzas"],
                temperatura: ["fahrenheit", "kelvin", "celsius", "newton", "reaumur"]
            };

            function actualizarListas() {
            const magnitud = document.getElementById("cmbTipo").value;
            const listaOrigen = document.getElementById("listaOrigen");
            const listaDestino = document.getElementById("listaDestino");
            
            listaOrigen.innerHTML = "";
            listaDestino.innerHTML = "";

            dicUnidades[magnitud].forEach(unidad => {
                listaOrigen.add(new Option(unidad, unidad));
                listaDestino.add(new Option(unidad, unidad));
            });
        }

        window.onload = function() {
            actualizarListas();

            document.getElementById("formConversor").addEventListener("submit", async function(e) {
                e.preventDefault();

                const formData = new URLSearchParams(new FormData(this));

                try {
                    const response = await fetch('ConversorServlet', {
                        method: 'POST',
                        body: formData
                            });

                            const texto = await response.text();

                            document.getElementById("resTxt").innerText = "";
                            document.getElementById("errorTxt").innerText = "";

                            if (texto === "ERROR_SESSION") {
                                window.location.href = "index.jsp"; 
                            } else if (texto.startsWith("ERROR")) {
                                document.getElementById("errorTxt").innerText = texto.replace("ERROR:", "");
                            } else {
                                document.getElementById("resTxt").innerText = "Resultado: " + texto;
                            }

                        } catch (error) {
                            document.getElementById("errorTxt").innerText = "Error conectando al servidor.";
                        }
                    });
                };
        </script>
    </head>
    <body>
        <div class="panel">
            <h2>Bienvenido al Conversor</h2>

            <form id="formConversor">
                <input type="number" step="any" name="txtValor" placeholder="Valor" required>

                <select name="cmbTipo" id="cmbTipo" onchange="actualizarListas()">
                    <option value="longitud">Longitud</option>
                    <option value="masa">Masa</option>
                    <option value="temperatura">Temperatura</option>
                </select>

                <select name="txtOrigen" id="listaOrigen"></select>
                <select name="txtDestino" id="listaDestino"></select>

                <button type="submit">Convertir</button>
            </form>

            <div id="resTxt" class="resultado"></div>
            <div id="errorTxt" class="error"></div>

            <br><a href="index.jsp" style="color: #aaa;">Cerrar Sesión</a>
        </div>
    </body>
</html>