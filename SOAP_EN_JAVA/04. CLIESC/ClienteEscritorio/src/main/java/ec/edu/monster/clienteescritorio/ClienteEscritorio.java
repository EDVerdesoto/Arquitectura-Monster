/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 */

package ec.edu.monster.clienteescritorio;

import ec.edu.monster.clienteescritorio.controlador.ControladorConversiones;
import ec.edu.monster.clienteescritorio.vista.LoginFrame;

/**
 *
 * @author joanc
 */
public class ClienteEscritorio {

    public static void main(String[] args) {
        ControladorConversiones controlador = new ControladorConversiones();
    
        LoginFrame login = new LoginFrame(controlador);
        login.setVisible(true);
    }
}
