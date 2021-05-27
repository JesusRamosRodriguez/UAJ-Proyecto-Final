using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PersistenceSystem 
{
    #region FileManager (Opening and closing I/O)

    //  Paths
    static string FILEPATH = @".\Telemetria\";
    static string FILEGENERAL = @".\Telemetria\general.txt";

    //  Streams
    private FileStream fs;
    private StreamReader sr;
    private StreamWriter sw;

    //  IDs
    string machineID;
    string sessionID;

    private bool OpenFile ()    //Opens input/output file (reading/writting) if it isn't already openned
    {
        if(!Directory.Exists(FILEPATH))            //Creates upper folder 
            Directory.CreateDirectory(FILEPATH);   

        //  Opens the i/o stream
        try
        {
            fs = new FileStream(FILEGENERAL, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            sw = new StreamWriter(fs);
            sr = new StreamReader(fs);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }

    }

    public bool Init (string machineID_, string sessionID_) //  initializes the system by opening the filestream of the file of the current session
    {
        if (OpenFile())
            return true;

        return false;
    }

    public bool ShutDown()  //  Shuts down the system
    {
        fs.Close();
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
