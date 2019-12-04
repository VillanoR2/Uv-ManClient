﻿using LogicaDelNegocio.Modelo;
using MessageService.Dominio.Enum;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonsRegister : MonoBehaviour
{
    public InputField TFNombreUsuario;
    public InputField TFContrasena;
    public InputField TFCorreo;
    public InputField TFConfirmacionContrasena;


    private Boolean ValidarCampoNombreDeUsuario(string textoDelCampo)
    {
        if(textoDelCampo != "")
        {
            foreach(char caracter in textoDelCampo)
            {
                if(caracter.Equals(" "))
                {
                    return false;
                }
            }
            if(textoDelCampo.Length > 50)
            {
                return false;
            }
            return true;
        }
        return false;
    }

    private Boolean ValidarContrasena(String contrasena)
    {
        if (contrasena != "")
        {
            foreach (char caracter in contrasena)
            {
                if (caracter.Equals(" "))
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    private Boolean ValidarContrasenasConinciden(String contrasena, String confirmacion)
    {
        if(confirmacion != "")
        {
            return contrasena == confirmacion;
        }
        return false;
    }

    private Boolean ValidarCorreoElectronico(String correo)
    {
        String expresion;
        expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        if (Regex.IsMatch(correo, expresion))
        {
            if (Regex.Replace(correo, expresion, String.Empty).Length == 0)
            {
                return true;
            }
        }
        return false;
    }

    private Boolean ValidarCamposCorrectos(String nombreDeUsuario, String contrasena, String confirmacion, String correo)
    {
        Boolean todoValido = true;
        if (!ValidarCampoNombreDeUsuario(nombreDeUsuario))
        {
            //PonerCamposEnRojo
            Debug.LogWarning("Usuario invalido");
            todoValido = false;
        }
        if (!ValidarContrasena(contrasena))
        {
            //PonerCamposEnRojo
            Debug.LogWarning("Contraseña invalida");
            todoValido = false;
        }
        if (!ValidarContrasenasConinciden(contrasena, confirmacion))
        {
            //PonerCamposEnRojo
            Debug.LogWarning("Contraseñas no coinciden");
            todoValido = false;
        }
        if (!ValidarCorreoElectronico(correo))
        {
            //PonerCamposEnRojo
            Debug.LogWarning("Correo invalido");
            todoValido = false;
        }
        return todoValido;
    }

    public void Registrarse()
    {
        String nombreDeusuario = TFNombreUsuario.text;
        String contrasena = TFContrasena.text;
        String confirmacion = TFConfirmacionContrasena.text;
        String correoElectronico = TFCorreo.text;
        if (ValidarCamposCorrectos(nombreDeusuario, contrasena, confirmacion, correoElectronico))
        {
            CuentaModel cuentaARegistrar = CrearCuentaARegistrar();
            CuentaCliente.clienteDeCuenta.ReiniciarServicio();
            EnumEstadoRegistro estadoDelRegistro = CuentaCliente.clienteDeCuenta.servicioDeCuenta.Registrarse(cuentaARegistrar);
            Debug.Log(estadoDelRegistro);
            switch (estadoDelRegistro)
            {
                case EnumEstadoRegistro.RegistroCorrecto:
                    RegistroExitoso(cuentaARegistrar);
                    break;
                case EnumEstadoRegistro.UsuarioExistente:
                    break;
                case EnumEstadoRegistro.ErrorEnBaseDatos:
                    break;
            }
        }
    }

    private void RegistroExitoso(CuentaModel cuentaRegistrada)
    {
        CuentaCliente.clienteDeCuenta.cuentaAVerificar = cuentaRegistrada;
        SceneManager.LoadScene("VerificacionScreen");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private CuentaModel CrearCuentaARegistrar()
    {
        CuentaModel cuentaARegistrar = new CuentaModel();
        cuentaARegistrar.NombreUsuario = TFNombreUsuario.text;
        cuentaARegistrar.Contrasena = TFContrasena.text;
        cuentaARegistrar.CorreoElectronico = TFCorreo.text;
        return cuentaARegistrar;
    }

    public void RegresarAlLogin()
    {
        SceneManager.LoadScene("LoginScreen");
    }
}
