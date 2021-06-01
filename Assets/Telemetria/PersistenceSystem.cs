using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#region STRUCTS
public struct processedLevelData
{
    int levelNumber;
    uint totalSamples; // número total de partidas acumuladas hasta ahora

    #region Dificultad y equilibrado
    float [,] deathHeatMap;
    float promedioMuertes;
    int porcentajeFlashes;
    int porcentajeAbandonos;
    #endregion

    #region Sistema puntuacion
    float promedioPuntuacion;
    int porcentajeColeccionables; // Porcentaje promedio de coleccionables totales que recogen
    Dictionary<uint, uint> porcentajeColeccionablesConcretos;
    int promedioTiempoNivel;
    #endregion

    #region IA Enemiga
    float promedioDetecciones;
    float[,] guardsHeatMap;
    float promedioMuertesDormidos; // Muertes causadas por guardias que dormían
    float promedioGuardiasFlasheados;
    #endregion

    #region Diseño nivel
    float[,] levelHeatMap;
    int porcentajeCamarasDesactivadas;
    float promedioDeteccionCamaras;
    int porcentajeCarretesRecogidos;
    int porcentajeFlashesRecogidos;
    float promedioTiempoEnHallarObjetivo;
    #endregion

    #region Interfaz y usabilidad
    float promedioFotosContraGuardias;
    float promedioFallosMinijuego;
    float promedioClicksEnCinematica;
    #endregion
}
#endregion


public class PersistenceSystem 
{
    const uint sizeX = 100;
    const uint sizeY = 100;

    #region DataManagement

    //  Const data (total)
    const uint tCarretes1 = 0;
    const uint tFlashes1 = 0;
    const uint tColeccionables1 = 0;
    const uint tCamaras1 = 0;

    const uint tCarretes2 = 0;
    const uint tFlashes2 = 0;
    const uint tColeccionables2 = 0;
    const uint tCamaras2 = 0;

    const uint tCarretes3 = 0;
    const uint tFlashes3 = 0;
    const uint tColeccionables3 = 0;
    const uint tCamaras3 = 0;

    //  Session data
    uint clicksEnCinematica;

    //  Raw level data
    struct levelData
    {
        uint numeroNivel;

        uint fotos;
        uint flashes;
        uint fotosAGuardias;
        uint coleccionablesRecogidos;
        uint camarasDesactivadas;
        uint flashesRecogidos;
        uint fallosMinijuego;
        uint muertes; // NO SE BORRA AL MORIR o reiniciar | SÍ AL ACABAR
        uint muertesPorGuardiaDormido; // NO SE BORRA AL MORIR o reiniciar | SÍ AL ACABAR
        uint tiempoPartida;
        uint tiempoEnEncontrarObjetivo;
        uint puntuacionFinal;

        float[,] posicionesJugador;
        float[,] posicionesGuardias;
        float[,] muertesJugador;  // NO SE BORRA AL MORIR o reiniciar | SÍ AL ACABAR

        Dictionary<uint, uint> coleccionables;  // id coleccionable -> veces recogido

        private void Reset(uint level, bool flag = false) // True si queremos resetear TODO
        {
            numeroNivel = level;
            fotos = 0;
            flashes = 0;
            fotosAGuardias = 0;
            coleccionablesRecogidos = 0;
            camarasDesactivadas = 0;
            flashesRecogidos = 0;
            fallosMinijuego = 0;
            tiempoPartida = 0;
            tiempoEnEncontrarObjetivo = 0;
            puntuacionFinal = 0;

            posicionesJugador = new float[posicionesJugador.GetLength(0), posicionesJugador.GetLength(1)];
            posicionesGuardias = new float[posicionesGuardias.GetLength(0), posicionesGuardias.GetLength(1)];

            if (flag)
            {
                muertes = 0;
                muertesPorGuardiaDormido = 0;
                muertesJugador = new float[muertesJugador.GetLength(0), muertesJugador.GetLength(1)];
            }
        }
    }
    levelData[] levelDatas; //Array de estructuras de datos especificos de cada nivel

    #endregion

    #region FileManager (Opening and closing I/O)

    //  Paths
    static string FILEPATH = @".\Telemetria\";
    static string FILEGENERAL = @".\Telemetria\general.txt";

    //  Streams
    private FileStream fs;
    private StreamReader sr;
    private StreamWriter sw;

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

    public bool Init () //  initializes the system by opening the filestream of the file of the current session
    {
        //  Set up status
        levelDatas = new levelData[3];

        for (int i = 0; i < 3; i++)
        {

        }

        //  Set up file
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

    #region Receiver
    //  Receive the events and store the data on the memory structures
    public bool SendEvent (singleEvent e)
    {
        switch (e.eventName)
        {
            case "FotoUsada":               
                break;
            case "FlashUsado":
                break;
            case "FotoGuardia":
                break;
            case "Coleccionable":
                break;
            case "FlashRecogido":
                break;
            case "FotoRecogida":
                break;
            case "Muerte":
                break;
            default:
                break;
        }
        return true;
    }
    public bool SendEvent(valueEvent e)
    {
        return true;
    }

    public bool SendEvent(positionEvent e)
    {
        return true;
    }

    public bool SendEvent(levelEvent e)
    {
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

    #endregion
}
