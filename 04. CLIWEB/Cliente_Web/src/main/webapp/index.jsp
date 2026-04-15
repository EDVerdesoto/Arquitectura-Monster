<%-- 
    Document   : index
    Created on : 15 abr 2026, 14:17:06
    Author     : Soto
--%>

<%@page contentType="text/html" pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title>Login - Conversor GR03</title>
        <style>
            body {
font-family: sans-serif;
background: #222;
color: white;
padding: 50px;
text-align: center;
}
            .caja {
background: #333;
padding: 20px;
border-radius: 10px;
display: inline-block;
}
            input {
margin: 10px;
padding: 8px;
}
            button {
padding: 10px 20px;
background: #4CAF50;
color: white;
border: none;
cursor: pointer;
}
            .error {
color: #ff4d4d;
font-weight: bold;
}
        </style>
    </head>
    <body>
        <div class="caja">
            <img src="https://i.pinimg.com/236x/c4/b0/b4/c4b0b4404ae06550e0a549ce048763b8.jpg" alt="alt"/>
            <h2>Iniciar Sesión</h2>

            <form method="POST" action="LoginServlet">
                <input type="text" name="txtUsuario" placeholder="Usuario" required><br>
                <input type="password" name="txtClave" placeholder="Contraseña" required><br>
                <button type="submit">Entrar</button>
            </form>

            <% if (request.getAttribute("error") != null) {%>
            <p class="error"><%= request.getAttribute("error")%></p>
            <% }%>
        </div>
    </body>
</html>