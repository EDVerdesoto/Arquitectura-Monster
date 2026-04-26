
package ec.edu.monster.clientesws;

import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlType;


/**
 * <p>Clase Java para convertirTemperatura complex type.
 * 
 * <p>El siguiente fragmento de esquema especifica el contenido que se espera que haya en esta clase.
 * 
 * <pre>{@code
 * <complexType name="convertirTemperatura">
 *   <complexContent>
 *     <restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       <sequence>
 *         <element name="valor" type="{http://www.w3.org/2001/XMLSchema}double"/>
 *         <element name="opcionOrigen" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *         <element name="opcionDestino" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *         <element name="token" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *       </sequence>
 *     </restriction>
 *   </complexContent>
 * </complexType>
 * }</pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "convertirTemperatura", propOrder = {
    "valor",
    "opcionOrigen",
    "opcionDestino",
    "token"
})
public class ConvertirTemperatura {

    protected double valor;
    protected String opcionOrigen;
    protected String opcionDestino;
    protected String token;

    /**
     * Obtiene el valor de la propiedad valor.
     * 
     */
    public double getValor() {
        return valor;
    }

    /**
     * Define el valor de la propiedad valor.
     * 
     */
    public void setValor(double value) {
        this.valor = value;
    }

    /**
     * Obtiene el valor de la propiedad opcionOrigen.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getOpcionOrigen() {
        return opcionOrigen;
    }

    /**
     * Define el valor de la propiedad opcionOrigen.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setOpcionOrigen(String value) {
        this.opcionOrigen = value;
    }

    /**
     * Obtiene el valor de la propiedad opcionDestino.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getOpcionDestino() {
        return opcionDestino;
    }

    /**
     * Define el valor de la propiedad opcionDestino.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setOpcionDestino(String value) {
        this.opcionDestino = value;
    }

    /**
     * Obtiene el valor de la propiedad token.
     * 
     * @return
     *     possible object is
     *     {@link String }
     *     
     */
    public String getToken() {
        return token;
    }

    /**
     * Define el valor de la propiedad token.
     * 
     * @param value
     *     allowed object is
     *     {@link String }
     *     
     */
    public void setToken(String value) {
        this.token = value;
    }

}
