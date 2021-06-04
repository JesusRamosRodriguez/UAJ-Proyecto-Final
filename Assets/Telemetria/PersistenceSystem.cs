﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#region STRUCTS
public struct processedLevelData
{
    public uint levelNumber;
    public uint totalSamples; // número total de partidas acumuladas hasta ahora

    #region Dificultad y equilibrado
    public float[,] mapaCalorMuertes;
    public float promedioMuertes;
    public uint porcentajeFlashes;
    #endregion

    #region Sistema puntuacion
    public float promedioPuntuacion;
    public uint porcentajeColeccionables; // Porcentaje promedio de coleccionables totales que recogen
    public Dictionary<int, uint> porcentajeColeccionablesConcretos;
    public float promedioTiempoNivel;
    #endregion

    #region IA Enemiga
    public float promedioDetecciones;
    public float[,] mapaCalorGuardias;
    public float promedioGuardiasFlasheados;
    #endregion

    #region Diseño nivel
    public float[,] mapaCalorNivel;
    public uint porcentajeCamarasDesactivadas;
    public float promedioDeteccionCamaras;
    public uint porcentajeCarretesRecogidos;
    public uint porcentajeFlashesRecogidos;
    public float promedioTiempoEnHallarObjetivo;
    #endregion

    #region Interfaz y usabilidad
    public float promedioFotosContraGuardias;
    public float promedioFallosMinijuego;
    #endregion

    public void Reset(uint level, uint sizeX, uint sizeY) // True si queremos resetear TODO
    {
        levelNumber = level;
        totalSamples = 0;

        mapaCalorMuertes = new float[sizeX, sizeY];
        mapaCalorGuardias = new float[sizeX, sizeY];
        mapaCalorNivel = new float[sizeX, sizeY];

        promedioMuertes = 0.0f;
        porcentajeFlashes = 0;
        promedioPuntuacion = 0.0f;
        porcentajeColeccionables = 0;
        porcentajeColeccionablesConcretos = new Dictionary<int, uint>();
        promedioTiempoNivel = 0.0f;
        promedioDetecciones = 0.0f;
        promedioGuardiasFlasheados = 0.0f;
        porcentajeCamarasDesactivadas = 0;
        promedioDeteccionCamaras = 0.0f;
        porcentajeCarretesRecogidos = 0;
        porcentajeFlashesRecogidos = 0;
        promedioTiempoEnHallarObjetivo = 0.0f;
        promedioFotosContraGuardias = 0.0f;
        promedioFallosMinijuego = 0.0f;
    }

}

public struct constLevelData
{
    public uint tCarretes;
    public uint tFlashes;
    public uint tColeccionables;
    public uint tCamaras;
}
#endregion


public class PersistenceSystem 
{
    const uint sizeX = 100;
    const uint sizeY = 100;
    const float heatMapIncrease = 0.01f;

    int currentLevel = 1;
    #region DataManagement

    //  Const data (total)
    constLevelData level1consts;
    constLevelData level2consts;
    constLevelData level3consts;

    constLevelData[] levelConsts;

    //  Session data
    uint clicksEnCinematica = 0;
    float promedioClicksEnCinematica = 0; // EL DATO ACUMULADO, habría que descodificarlo/cargarlo en el init

    //  Raw level data
    struct levelData
    {
        public uint numeroNivel;

        public uint fotos;
        public uint flashes;
        public uint fotosAGuardias;
        public uint flashesAGuardias;
        public uint coleccionablesRecogidos;
        public uint camarasDesactivadas;
        public uint flashesRecogidos;
        public uint fotosRecogidas;
        public uint fallosMinijuego;
        public uint muertes; // NO SE BORRA AL MORIR o reiniciar | SÍ AL ACABAR
        public uint deteccionesGuardia;
        public uint deteccionesCamara;
        public uint tiempoPartida;
        public int tiempoEnEncontrarObjetivo;
        public int puntuacionFinal;

        public float[,] posicionesJugador;
        public float[,] posicionesGuardias;
        public float[,] muertesJugador;  // NO SE BORRA AL MORIR o reiniciar | SÍ AL ACABAR

        public Stack<int> coleccionables;  // id coleccionable recogido

        public void Reset(uint level, uint sizeX, uint sizeY, bool flag = false) // True si queremos resetear TODO
        {
            numeroNivel = level;
            fotos = 0;
            flashes = 0;
            fotosAGuardias = 0;
            flashesAGuardias = 0;
            coleccionablesRecogidos = 0;
            camarasDesactivadas = 0;
            flashesRecogidos = 0;
            fotosRecogidas = 0;
            fallosMinijuego = 0;
            deteccionesGuardia = 0;
            tiempoPartida = 0;
            tiempoEnEncontrarObjetivo = 0;
            puntuacionFinal = 0;

            coleccionables = new Stack<int>();

            posicionesJugador = new float[sizeX, sizeY];
            posicionesGuardias = new float[sizeX, sizeY];

            if (flag)
            {
                muertes = 0;
                muertesJugador = new float[sizeX, sizeY];
            }
        }
    }
    levelData[] levelDatas;                     //  Array de estructuras de datos especificos de cada nivel
    processedLevelData[] processedLevelDatas;   //  Array de datos procesados de todas las partidas previas

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

    public bool Init () //  initializes the system by opening the filestream of the file of the current session and loading the general file if it exists
    {
        //  Set up status
        levelDatas = new levelData[3];
        processedLevelDatas = new processedLevelData[3];

        for (uint i = 0; i < 3; i++)
        {
            levelDatas[i] = new levelData();
            levelDatas[i].Reset(i+1, sizeX, sizeY, true);

            processedLevelDatas[i] = new processedLevelData();
            if (!DecodeLevel(i + 1)) processedLevelDatas[i].Reset(i + 1, sizeX, sizeY);
        }

        // Level constants
        initLevelConsts();

        //  Set up file
        if (OpenFile())
            return true;

        return false;
    }

    private void initLevelConsts()
    {
        level1consts.tCarretes = 0;
        level1consts.tFlashes = 0;
        level1consts.tCamaras = 0;
        level1consts.tColeccionables = 0;

        level2consts.tCarretes = 0;
        level2consts.tFlashes = 0;
        level2consts.tCamaras = 0;
        level2consts.tColeccionables = 0;

        level3consts.tCarretes = 0;
        level3consts.tFlashes = 0;
        level3consts.tCamaras = 0;
        level3consts.tColeccionables = 0;

        levelConsts = new constLevelData[] { level1consts, level2consts, level3consts };
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
        bool failed = false;
        switch (e.eventName)
        {
            case "FotoUsada":
                levelDatas[currentLevel - 1].fotos++;
                break;
            case "FlashUsado":
                levelDatas[currentLevel - 1].flashes++;
                break;
            case "FotoGuardia":
                levelDatas[currentLevel - 1].fotosAGuardias++;
                break;
            case "FlashGuardia":
                levelDatas[currentLevel - 1].flashesAGuardias++;
                break;
            case "FlashRecogido":
                levelDatas[currentLevel - 1].flashesRecogidos++;
                break;
            case "FotoRecogida":
                levelDatas[currentLevel - 1].fotosRecogidas++;
                break;
            case "Muerte":
                levelDatas[currentLevel - 1].muertes++;
                break;
            case "GuardiaDetectaJugador":
                levelDatas[currentLevel - 1].deteccionesGuardia++;
                break;
            case "CamaraDetectaJugador":
                levelDatas[currentLevel - 1].deteccionesCamara++;
                break;
            case "ClickCinematica":
                clicksEnCinematica++;
                break;
            default:
                failed = true;
                Debug.LogError("PersistenceSystem ha recibido un evento de tipo 'singleEvent' no reconocible. Id del evento: " + e.eventName);
                break;
        }
        return !failed;
    }
    public bool SendEvent(valueEvent e)
    {
        bool failed = false;
        switch (e.eventName)
        {
            case "TiempoFinalNivel":
                levelDatas[currentLevel - 1].tiempoPartida = (uint)e.value;
                break;
            case "TiempoFamosoObjetivo":
                levelDatas[currentLevel - 1].tiempoEnEncontrarObjetivo = e.value;
                break;
            case "ColeccionableConcreto":
                levelDatas[currentLevel - 1].coleccionablesRecogidos++;
                levelDatas[currentLevel - 1].coleccionables.Push(e.value);
                break;
            case "PuntuacionNivel":
                levelDatas[currentLevel - 1].puntuacionFinal = e.value;
                break;
            default:
                failed = true;
                Debug.LogError("PersistenceSystem ha recibido un evento de tipo 'valueEvent' no reconocible. Id del evento: " + e.eventName);
                break;
        }
        return !failed;
    }

    public bool SendEvent(positionEvent e)
    {
        bool failed = false;
        switch (e.eventName)
        {
            case "PlayerPosition":
                levelDatas[currentLevel - 1].posicionesJugador[Mathf.RoundToInt(e.x), Mathf.RoundToInt(e.y)] += heatMapIncrease;
                break;
            case "GuardiaPosition":
                levelDatas[currentLevel - 1].posicionesGuardias[Mathf.RoundToInt(e.x), Mathf.RoundToInt(e.y)] += heatMapIncrease;
                break;
            default:
                failed = true;
                Debug.LogError("PersistenceSystem ha recibido un evento de tipo 'positionEvent' no reconocible. Id del evento: " + e.eventName);
                break;
        }
        return !failed;
    }

    public bool SendEvent(levelEvent e)
    {
        bool failed = false;
        switch (e.eventName)
        {
            case "InicioNivel":
                currentLevel = e.nivel;
                break;
            case "FinNivel":
                break;
            case "Reinicio":
                break;
            case "AbandonoNivel":
                break;
            default:
                failed = true;
                Debug.LogError("PersistenceSystem ha recibido un evento de tipo 'positionEvent' no reconocible. Id del evento: " + e.eventName);
                break;
        }
        return !failed;
    }
    #endregion

    #region Processer
    private bool ProcessCurrentLevel ()
    {
        int currentIndex = currentLevel - 1;

        processedLevelDatas[currentIndex].totalSamples += 1;
        uint accumulatedSamples = processedLevelDatas[currentIndex].totalSamples;
        float acumulatedDataWeight = (accumulatedSamples - 1) / accumulatedSamples;

        #region Dificultad y equilibrado
        // Promedio muertes
        processedLevelDatas[currentIndex].promedioMuertes = (processedLevelDatas[currentIndex].promedioMuertes * acumulatedDataWeight) + 
            (levelDatas[currentIndex].muertes * (1.0f - acumulatedDataWeight));

        // Porcentaje flashes gastados
        processedLevelDatas[currentIndex].porcentajeFlashes = processPercentageMetric(processedLevelDatas[currentIndex].porcentajeFlashes, levelDatas[currentIndex].flashes,
            levelConsts[currentIndex].tFlashes, acumulatedDataWeight);

        // TODO: ACUMULAR MAPA CALOR MUERTE
        #endregion

        #region Sistema puntuacion
        // Promedio puntuacion
        processedLevelDatas[currentIndex].promedioPuntuacion = (processedLevelDatas[currentIndex].promedioPuntuacion * acumulatedDataWeight) +
            (levelDatas[currentIndex].puntuacionFinal * (1.0f - acumulatedDataWeight));

        // Porcentaje coleccionables
        processedLevelDatas[currentIndex].porcentajeColeccionables = processPercentageMetric(processedLevelDatas[currentIndex].porcentajeColeccionables,
            levelDatas[currentIndex].coleccionablesRecogidos, levelConsts[currentIndex].tColeccionables, acumulatedDataWeight);

        // Porcentaje coleccionables concretos
        foreach (int id in levelDatas[currentIndex].coleccionables)
        {
            uint storedValue;
            if (processedLevelDatas[currentIndex].porcentajeColeccionablesConcretos.TryGetValue(id, out storedValue))
            {
                uint newPercentage = (storedValue * accumulatedSamples) / (accumulatedSamples - 1);
            }
            else processedLevelDatas[currentIndex].porcentajeColeccionablesConcretos.Add(id, (uint)( 1 / accumulatedSamples));
        }

        // Promedio tiempo nivel
        processedLevelDatas[currentIndex].promedioTiempoNivel = (processedLevelDatas[currentIndex].promedioTiempoNivel * acumulatedDataWeight) +
            (levelDatas[currentIndex].tiempoPartida * (1.0f - acumulatedDataWeight));
        #endregion

        #region IA Enemiga
        // Promedio detecciones
        processedLevelDatas[currentIndex].promedioDetecciones = (processedLevelDatas[currentIndex].promedioDetecciones * acumulatedDataWeight) +
           (levelDatas[currentIndex].deteccionesGuardia * (1.0f - acumulatedDataWeight));

        // TODO: ACUMULAR MAPA CALOR GUARDIAS

        // Promedio guardias flasheados
        processedLevelDatas[currentIndex].promedioGuardiasFlasheados = (processedLevelDatas[currentIndex].promedioGuardiasFlasheados * acumulatedDataWeight) +
           (levelDatas[currentIndex].flashesAGuardias * (1.0f - acumulatedDataWeight));
        #endregion

        #region Diseño nivel
        // TODO: ACUMULAR MAPA DE CALOR DEL NIVEL

        // Porcentaje Camaras desactivadas
        processedLevelDatas[currentIndex].porcentajeCamarasDesactivadas = processPercentageMetric(processedLevelDatas[currentIndex].porcentajeCamarasDesactivadas,
            levelDatas[currentIndex].camarasDesactivadas, levelConsts[currentIndex].tCamaras, acumulatedDataWeight);

        // Promedio deteccion camaras
        processedLevelDatas[currentIndex].promedioDeteccionCamaras = (processedLevelDatas[currentIndex].promedioDeteccionCamaras * acumulatedDataWeight) +
           (levelDatas[currentIndex].deteccionesCamara * (1.0f - acumulatedDataWeight));

        // Porcentaje carretes recogidos
        processedLevelDatas[currentIndex].porcentajeCarretesRecogidos = processPercentageMetric(processedLevelDatas[currentIndex].porcentajeCarretesRecogidos,
            levelDatas[currentIndex].fotosRecogidas, levelConsts[currentIndex].tCarretes, acumulatedDataWeight);

        // Porcentaje flashes recogidos
        processedLevelDatas[currentIndex].porcentajeFlashesRecogidos = processPercentageMetric(processedLevelDatas[currentIndex].porcentajeFlashesRecogidos,
            levelDatas[currentIndex].flashesRecogidos, levelConsts[currentIndex].tFlashes, acumulatedDataWeight);

        // Promedio tiempo en hallar objetivo
        processedLevelDatas[currentIndex].promedioTiempoEnHallarObjetivo = (processedLevelDatas[currentIndex].promedioTiempoEnHallarObjetivo * acumulatedDataWeight) +
           (levelDatas[currentIndex].tiempoEnEncontrarObjetivo * (1.0f - acumulatedDataWeight));
        #endregion

        #region Interfaz y usabilidad
        // Promedio de fotos contra guardias
        processedLevelDatas[currentIndex].promedioFotosContraGuardias = (processedLevelDatas[currentIndex].promedioFotosContraGuardias * acumulatedDataWeight) +
           (levelDatas[currentIndex].fotosAGuardias * (1.0f - acumulatedDataWeight));

        // Promedio Fallos Minijuego
        processedLevelDatas[currentIndex].promedioFallosMinijuego = (processedLevelDatas[currentIndex].promedioFallosMinijuego * acumulatedDataWeight) +
           (levelDatas[currentIndex].fallosMinijuego * (1.0f - acumulatedDataWeight));
        #endregion

        return true;
    }

    private uint processPercentageMetric (uint accData, uint newData, uint levelConstData, float accWeight)
    {
        uint pctPartidaActual = newData / levelConstData;
        return (uint)Mathf.RoundToInt((accData * accWeight) +
            (pctPartidaActual * (1.0f - accWeight)));
    }
    #endregion

    #region Decoder

    //  Auxiliar readers
    private uint GetuintFromPos(int pos) { return uint.Parse(sr.ReadLine().Split(':')[pos]); }
    private float GetfloatFromPos(int pos) { return float.Parse(sr.ReadLine().Split(':')[pos]); }
    private bool DecodeLevel(uint level)
    {
        string s;
        
        try
        {
            //  Ensures that the stream pointer is pointing at the beginning of the file
            sr.DiscardBufferedData();
            sr.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            //  Checks if there actually is data for the asked level in the file
            s = sr.ReadLine();
            while (s != "Lvl " + level) s = sr.ReadLine();
            if (sr.EndOfStream) return false;

            //  Decodes..
            currentLevel = s[4];                                                                   //  Lvl x
            if(currentLevel == 1)promedioClicksEnCinematica = GetfloatFromPos(1);                  // (just in lvl 1) promedioClicksEnCinematica: x
            processedLevelDatas[level-1].totalSamples = GetuintFromPos(1);                         //  totalSamples:x
            processedLevelDatas[level-1].promedioMuertes = GetfloatFromPos(1);                     //  promedioMuertes:x
            processedLevelDatas[level-1].porcentajeFlashes = GetuintFromPos(1);                    //  porcentajeFlashes:x
            processedLevelDatas[level-1].promedioPuntuacion = GetfloatFromPos(1);                  //  promedioPuntuacion:x
            processedLevelDatas[level-1].porcentajeColeccionables = GetuintFromPos(1);             //  porcentajeColeccionables:x
            processedLevelDatas[level-1].promedioTiempoNivel = GetuintFromPos(1);                  //  promedioTiempoNivel:x
            processedLevelDatas[level-1].promedioDetecciones = GetfloatFromPos(1);                 //  promedioDetecciones:x
            processedLevelDatas[level-1].promedioGuardiasFlasheados = GetfloatFromPos(1);          //  promedioGuardiasFlasheados:x
            processedLevelDatas[level-1].porcentajeCamarasDesactivadas = GetuintFromPos(1);        //  porcentajeCamarasDesactivadas:x
            processedLevelDatas[level-1].promedioDeteccionCamaras = GetfloatFromPos(1);            //  promedioDeteccionCamaras:x
            processedLevelDatas[level-1].porcentajeCarretesRecogidos = GetuintFromPos(1);          //  porcentajeCarretesRecogidos:x
            processedLevelDatas[level-1].porcentajeFlashesRecogidos = GetuintFromPos(1);           //  porcentajeFlashesRecogidos:x
            processedLevelDatas[level-1].promedioTiempoEnHallarObjetivo = GetfloatFromPos(1);      //  promedioTiempoEnHallarObjetivo:x
            processedLevelDatas[level-1].promedioFotosContraGuardias = GetfloatFromPos(1);         //  promedioFotosContraGuardias:x
            processedLevelDatas[level-1].promedioFallosMinijuego = GetfloatFromPos(1);             //  promedioFallosMinijuego:x

            //  porcentajeColeccionablesConcretos: id1-% id2-% id3-% id4-%
            string[] subs;
            string[] subs2;
            subs = sr.ReadLine().Split(' ');    // id1-% / id2-% /id3-%
            for (int i = 1; i < subs.Length; i++)
            {
                subs2 = subs[i].Split('-');     // id1 / %
                processedLevelDatas[level-1].porcentajeColeccionablesConcretos[int.Parse(subs2[0])] = uint.Parse(subs2[1]);
            }


        }
        catch  (System.Exception e)
        {
            Debug.LogError("General telemetry file can't be read. It may be corrupted!  Details: ");
            Debug.LogError(e.Message);
        }
        return true;
    }
    #endregion

    #region Printers
    private void Print (string s )
    {
        sw.Write(s);
    }
    private void PrintL(string s)
    {
        sw.WriteLine(s);
    }
    #endregion

    #region Encoder
    private bool EncodeLevel(uint level)
    {
        try
        {
            // Check wich level are we encoding
            if(level == 1)
            PrintL("Lvl " + level);
            PrintL("promedioClicksEnCinematica: " + level);
        }
        catch (System.Exception e)
        {
            Debug.LogError("General telemetry file can't be read. It may be corrupted!  Details: ");
            Debug.LogError(e.Message);
        }
        return true;
    }
    #endregion
}
