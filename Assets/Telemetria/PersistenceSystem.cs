using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PersistenceSystem 
{
    #region FileManager (Opening and closing I/O)

    static string FILEPATH = @".\Telemetria\";

    private StreamWriter sw = null;

    // IDs
    string machineID;
    string sessionID;

    private bool OpenWriteFile (string file_path) //Opens output file (writting) if it isn't already openned
    {
        if(!Directory.Exists(@".\Telemetria"))
            Directory.CreateDirectory(@".\Telemetria");
        
        if (!File.Exists(file_path))
        {
            sw = File.CreateText(file_path);

            if(File.Exists(file_path)) return true;
        }
        return false;
    }

    public bool Init (string machineID_, string sessionID_)
    {
        machineID = machineID_;
        sessionID = sessionID_;


        if (OpenWriteFile(FILEPATH + sessionID + ".txt"))
        {
            sw.Write("{\n");
            sw.WriteLine("\"Session ID\": " + '\"' + sessionID + '\"');
            sw.WriteLine("\"Machine ID\": " + '\"' + machineID + '\"');
            sw.Write("}\n");
            sw.Write("{\n");
            return true;
        }
        return false;
    }

    public bool ShutDown()
    {
        sw.Write("}");

        sw.Close();
        return true;
    }
    #endregion

    #region Printer
    private bool PrintOnDefault (ref string data)
    {
        sw.Write(data);
        return true;
    }
    #endregion

    #region Encoder
    public bool toJson(TelemetryEvent e)
    {
        string result = " ";
        switch (e.type)
        {
            case TipoEvento.InicioSesion:
                result = "\"Inicio sesion\": " + '\"' + "lvl " + e.nivel.ToString() + ' ' + e.time.ToString() + "\",\n";
                break;
            case TipoEvento.InicioNivel:
                result = "\"Inicio nivel\": " + '\"' + "lvl " + e.nivel.ToString() + ' ' + e.time.ToString() + "\",\n";
                break;
            case TipoEvento.FinSesion:
                result = "\"Fin sesion\": " + '\"' + "lvl " + e.nivel.ToString() + ' ' + e.time.ToString() + "\",\n";
                break;
            case TipoEvento.FinNivel:
                result = "\"Fin nivel\": " + '\"' + "lvl " + e.nivel.ToString() + ' ' + e.time.ToString() + "\",\n";
                break;
            case TipoEvento.Reinicio:
                result = "\"Reinicio de nivel\": " + '\"' + "lvl " + e.nivel.ToString() + ' ' + e.time.ToString() + "\",\n";
                break;
            case TipoEvento.Pausa:
                result = "\"Pausa\": " + '\"' + "lvl " + e.nivel.ToString() + ' ' + e.time.ToString() + "\",\n";
                break;
            case TipoEvento.Coleccionable:
                result = "\"Coleccionable recogido\": " + '\"' + "lvl " + e.nivel.ToString() + ' ' + e.time.ToString() + "\",\n";
                break;
            case TipoEvento.FotoRecogida:
                result = "\"Fotografia recogida\": " + '\"' + "lvl " + e.nivel.ToString() + ' ' + e.time.ToString() + "\",\n";
                break;
            case TipoEvento.FlashRecogida:
                result = "\"Flash recogido\": " + '\"' + "lvl " + e.nivel.ToString() + ' ' + e.time.ToString() + "\",\n";
                break;
            case TipoEvento.BotonCamara:
                result = "\"Boton camara pulsado\": " + '\"' + "lvl " + e.nivel.ToString() + ' ' + e.time.ToString() + "\",\n";
                break;
            case TipoEvento.FotoUso:
                result = "\"Fotografia usada\": " + '\"' + "lvl " + e.nivel.ToString() + ' ' + e.time.ToString() + "\",\n";
                break;
            case TipoEvento.FlashUso:
                result = "\"Flash usado\": " + '\"' + "lvl " + e.nivel.ToString() + ' ' + e.time.ToString() + "\",\n";
                break;
            case TipoEvento.FotoGuardia:
                result = "\"Guardia fotografiado\": " + '\"' + "lvl " + e.nivel.ToString() + ' ' + e.time.ToString() + "\",\n";
                break;
            default:
                break;
        }

        if (result != " ")  //Print
        {
            return PrintOnDefault(ref result);
        }

        else return false;
    }
    #endregion
}
