using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#region STRUCTS

public struct graphicsData
{
    public uint valueX;
    public uint valueY;
}
public struct processedLevelData
{
    public uint levelNumber;
    public uint totalSamples; // número total de partidas acumuladas hasta ahora

    #region Dificultad y equilibrado
    public float[,] mapaCalorMuertes;
    public float promedioMuertes;
    public uint porcentajeFlashes;
    public List<graphicsData> graficaFlashesMuertes;
    #endregion

    #region Sistema puntuacion
    public float promedioPuntuacion;
    public uint porcentajeColeccionables; // Porcentaje promedio de coleccionables totales que recogen
    public Dictionary<int, uint> porcentajeColeccionablesConcretos;
    public float promedioTiempoNivel;
    public List<graphicsData> graficaPuntuacionTiempo;
    public List<graphicsData> graficaColeccionablesPuntuacion;
    #endregion

    #region IA Enemiga
    public float promedioDetecciones;
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

    public void InitStructures(uint sizeX, uint sizeY)
    {
        mapaCalorMuertes = new float[sizeX, sizeY];
        mapaCalorNivel = new float[sizeX, sizeY];

        porcentajeColeccionablesConcretos = new Dictionary<int, uint>();

        graficaFlashesMuertes = new List<graphicsData>();
        graficaPuntuacionTiempo = new List<graphicsData>();
        graficaColeccionablesPuntuacion = new List<graphicsData>();
    }
    public void Reset(uint level, uint sizeX, uint sizeY, bool fileExisted)
    {
        levelNumber = level;
        if (fileExisted)
            totalSamples--;
        else totalSamples = 0;
        
        for(int i = 0; i < sizeX; i++)
        {
            for (int j = 0; i < sizeY; i++)
            {
                mapaCalorMuertes[i, j] = 0.0f;
                mapaCalorNivel[i, j] = 0.0f;
            }
        }

        promedioMuertes = 0.0f;
        porcentajeFlashes = 0;
        promedioPuntuacion = 0.0f;
        porcentajeColeccionables = 0;
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
    const float heatMapIncrease = 1.00f;

    int currentLevel = 1;

    bool FileExisted = false;

    #region DataManagement

    //  Const data (total)
    constLevelData level1consts;
    constLevelData level2consts;
    constLevelData level3consts;

    constLevelData[] levelConsts;

    // Heat Maps
    float[] playerMapsMaxValue;
    float[] deathMapsMaxValue;

    //  Session data
    uint clicksEnCinematica = 0;
    float promedioClicksEnCinematica = 0;

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
        public uint muertes;
        public uint deteccionesGuardia;
        public uint deteccionesCamara;
        public uint tiempoPartida;
        public int tiempoEnEncontrarObjetivo;
        public int puntuacionFinal;

        public float[,] posicionesJugador;
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

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; i < sizeY; i++)
                {
                    posicionesJugador[i, j] = 0.0f;
                }
            }

            if (flag)
            {
                muertes = 0;
                muertesJugador = new float[sizeX, sizeY];

                for (int i = 0; i < sizeX; i++)
                {
                    for (int j = 0; i < sizeY; i++)
                    {
                        muertesJugador[i, j] = 0.0f;
                    }
                }
            }
        }
    }
    levelData[] levelDatas;                     //  Array de estructuras de datos especificos de cada nivel
    processedLevelData[] processedLevelDatas;   //  Array de datos procesados de todas las partidas previas

    private void initLevelConsts()
    {
        level1consts.tCarretes = 3;
        level1consts.tFlashes = 4;
        level1consts.tCamaras = 4;
        level1consts.tColeccionables = 5;

        level2consts.tCarretes = 3;
        level2consts.tFlashes = 4;
        level2consts.tCamaras = 3;
        level2consts.tColeccionables = 5;

        level3consts.tCarretes = 2;
        level3consts.tFlashes = 4;
        level3consts.tCamaras = 5;
        level3consts.tColeccionables = 5;

        levelConsts = new constLevelData[] { level1consts, level2consts, level3consts };
    }
    #endregion

    #region FileManager (Opening and closing I/O)

    //  Paths
    static string FILEPATH = @".\Telemetria\";
    static string FILEGENERAL = @".\Telemetria\general.txt";

    private bool OpenFile ()                        //  Opens input/output file (reading/writting) if it isn't already openned 
    {                                               //  ¡After I/O overhaul it just creates the folder and general file if they don't exist
                                                    //  and updates a flag

        if (!Directory.Exists(FILEPATH))            //  Creates upper folder 
            Directory.CreateDirectory(FILEPATH);   

        try
        {
            if (File.Exists(FILEPATH)) FileExisted = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
        return true;
    }

    public bool Init () //  initializes the system by opening the filestream of the file of the current session and loading the general file if it exists
    {
        //  Set up file
        if (!OpenFile()) return false;

        //  initializes memory structures
        levelDatas = new levelData[3];
        processedLevelDatas = new processedLevelData[3];
        for (uint i = 0; i < 3; i++)
        {
            levelDatas[i] = new levelData();
            levelDatas[i].Reset(i+1, sizeX, sizeY, true);

            processedLevelDatas[i] = new processedLevelData();

            processedLevelDatas[i].InitStructures(sizeX, sizeY);
            if (!DecodeLevel(i + 1)) processedLevelDatas[i].Reset(i + 1, sizeX, sizeY, FileExisted);
        }
        // Level constants
        initLevelConsts();

        // Heat Maps
        playerMapsMaxValue = new float[3];
        deathMapsMaxValue = new float[3];

        for (int i = 0; i < 3; i++)
        {
            playerMapsMaxValue[i] = 0.0f;
            deathMapsMaxValue[i] = 0.0f;
        }

        return true;
    }

    public bool ShutDown()  //  Shuts down the system
    {
        Debug.Log("ShutDown...");
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
            case "CamaraDesactivada":
                levelDatas[currentLevel - 1].camarasDesactivadas++;
                break;
            case "ClickCinematica":
                clicksEnCinematica++;
                break;
            case "FalloMinijuego":
                levelDatas[currentLevel - 1].fallosMinijuego++;
                break;
            case "CamaraActivada":
                levelDatas[currentLevel - 1].camarasDesactivadas--;
                break;
            default:
                Debug.LogError("PersistenceSystem ha recibido un evento de tipo 'singleEvent' no reconocible. Id del evento: " + e.eventName);
                return false;
        }
        return true;
    }
    public bool SendEvent(valueEvent e)
    {
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
                Debug.LogError("PersistenceSystem ha recibido un evento de tipo 'valueEvent' no reconocible. Id del evento: " + e.eventName);
                return false;
        }
        return true;
    }

    public bool SendEvent(positionEvent e)
    {
        switch (e.eventName)
        {
            case "PlayerPosition":
                processPositions(ref levelDatas[currentLevel - 1].posicionesJugador, e.x, e.y, 1);
                break;
            case "MuertePosition":
                processPositions(ref levelDatas[currentLevel - 1].muertesJugador, e.x, e.y, 2);
                break;
            default:            
                Debug.LogError("PersistenceSystem ha recibido un evento de tipo 'positionEvent' no reconocible. Id del evento: " + e.eventName);
                return false;
        }
        return true;
    }

    public bool SendEvent(levelEvent e)
    {
        switch (e.eventName)
        {
            case "InicioNivel":
                currentLevel = e.nivel;
                break;
            case "FinNivel":
                if(ProcessCurrentLevel()) return Encode();
                break;
            case "Reinicio":
                levelDatas[currentLevel - 1].Reset((uint)currentLevel, sizeX, sizeY);
                break;
            case "AbandonoNivel":
                levelDatas[currentLevel - 1].Reset((uint)currentLevel, sizeX, sizeY);
                break;
            case "InicioSesion":
                break;
            case "FinSesion":
                break;
            default:
                Debug.LogError("PersistenceSystem ha recibido un evento de tipo 'levelEvent' no reconocible. Id del evento: " + e.eventName);
                return false;
        }
        return true;
    }
    #endregion

    #region Processer
    private bool ProcessCurrentLevel()
    {

        try
        {
            int currentIndex = currentLevel - 1;

            processedLevelDatas[currentIndex].totalSamples += 1;
            uint accumulatedSamples = processedLevelDatas[currentIndex].totalSamples;
            float accumulatedDataWeight = (float)(accumulatedSamples - 1) / accumulatedSamples;


            processDificultyMetrics(currentIndex, accumulatedSamples, accumulatedDataWeight);
            processScoreMetrics(currentIndex, accumulatedSamples, accumulatedDataWeight);
            processIAMetrics(currentIndex, accumulatedSamples, accumulatedDataWeight);
            processDesignMetrics(currentIndex, accumulatedSamples, accumulatedDataWeight);
            processInterfaceMetrics(currentIndex, accumulatedSamples, accumulatedDataWeight);

            if(currentLevel == 1)
            {
                promedioClicksEnCinematica = (promedioClicksEnCinematica * accumulatedDataWeight) +
                (clicksEnCinematica * (1.0f - accumulatedDataWeight));
            }

         }
         catch (System.Exception e)
         {
             Debug.LogError("Fail processing current level!  Details: ");
             Debug.LogError(e.Message + " / " + e.Data);
             return false;
         }
        return true;
    }

    private void processDificultyMetrics(int currentIndex, uint accumulatedSamples, float accumulatedDataWeight)
    {
        // Promedio muertes
        processedLevelDatas[currentIndex].promedioMuertes = (processedLevelDatas[currentIndex].promedioMuertes * accumulatedDataWeight) +
            (levelDatas[currentIndex].muertes * (1.0f - accumulatedDataWeight));

        // Porcentaje flashes gastados
        processedLevelDatas[currentIndex].porcentajeFlashes = processPercentageMetric(processedLevelDatas[currentIndex].porcentajeFlashes, levelDatas[currentIndex].flashes,
            (levelConsts[currentIndex].tFlashes + 3), accumulatedDataWeight);


        // ACUMULAR MAPA CALOR MUERTE
        accumulateHeatMap(levelDatas[currentIndex].muertesJugador, accumulatedDataWeight, 2);

        // Gráfica Flashes-Muertes
        graphicsData flashesMuertes; flashesMuertes.valueX = levelDatas[currentIndex].flashes; flashesMuertes.valueY = levelDatas[currentIndex].muertes;
        if (processedLevelDatas[currentIndex].graficaFlashesMuertes.Count > 10)
        {
            processedLevelDatas[currentIndex].graficaFlashesMuertes.RemoveAt(0);
            processedLevelDatas[currentIndex].graficaFlashesMuertes.Add(flashesMuertes);
        }
        else processedLevelDatas[currentIndex].graficaFlashesMuertes.Add(flashesMuertes);
    }

    private void processScoreMetrics(int currentIndex, uint accumulatedSamples, float accumulatedDataWeight)
    {
        // Promedio puntuacion
        processedLevelDatas[currentIndex].promedioPuntuacion = (processedLevelDatas[currentIndex].promedioPuntuacion * accumulatedDataWeight) +
            (levelDatas[currentIndex].puntuacionFinal * (1.0f - accumulatedDataWeight));

        // Porcentaje coleccionables
        processedLevelDatas[currentIndex].porcentajeColeccionables = processPercentageMetric(processedLevelDatas[currentIndex].porcentajeColeccionables,
            levelDatas[currentIndex].coleccionablesRecogidos, levelConsts[currentIndex].tColeccionables, accumulatedDataWeight);

        // Porcentaje coleccionables concretos
        foreach (int id in levelDatas[currentIndex].coleccionables)
        {
            uint storedValue;
            if (processedLevelDatas[currentIndex].porcentajeColeccionablesConcretos.TryGetValue(id, out storedValue))
            {
                uint newPercentage = (uint)Mathf.RoundToInt(((float)storedValue * accumulatedSamples) / (accumulatedSamples - 1));
            }
            else processedLevelDatas[currentIndex].porcentajeColeccionablesConcretos.Add(id, (uint)Mathf.RoundToInt(1 / (float)accumulatedSamples));
        }

        // Promedio tiempo nivel
        processedLevelDatas[currentIndex].promedioTiempoNivel = (processedLevelDatas[currentIndex].promedioTiempoNivel * accumulatedDataWeight) +
            (levelDatas[currentIndex].tiempoPartida * (1.0f - accumulatedDataWeight));

        // Gráfica Puntuacion-Tiempo
        graphicsData puntuacionTiempo; puntuacionTiempo.valueX = (uint)levelDatas[currentIndex].puntuacionFinal; puntuacionTiempo.valueY = levelDatas[currentIndex].tiempoPartida;
        if (processedLevelDatas[currentIndex].graficaPuntuacionTiempo.Count > 10)
        {
            processedLevelDatas[currentIndex].graficaPuntuacionTiempo.RemoveAt(0);
            processedLevelDatas[currentIndex].graficaPuntuacionTiempo.Add(puntuacionTiempo);
        }
        else processedLevelDatas[currentIndex].graficaPuntuacionTiempo.Add(puntuacionTiempo);

        // Gráfica Coleccionables-Puntuacion
        graphicsData coleccPuntuacion; coleccPuntuacion.valueX = levelDatas[currentIndex].coleccionablesRecogidos; coleccPuntuacion.valueY = (uint)levelDatas[currentIndex].puntuacionFinal;
        if (processedLevelDatas[currentIndex].graficaColeccionablesPuntuacion.Count > 10)
        {
            processedLevelDatas[currentIndex].graficaColeccionablesPuntuacion.RemoveAt(0);
            processedLevelDatas[currentIndex].graficaColeccionablesPuntuacion.Add(coleccPuntuacion);
        }
        else processedLevelDatas[currentIndex].graficaColeccionablesPuntuacion.Add(coleccPuntuacion);
    }

    private void processIAMetrics(int currentIndex, uint accumulatedSamples, float accumulatedDataWeight)
    {
        // Promedio detecciones
        processedLevelDatas[currentIndex].promedioDetecciones = (processedLevelDatas[currentIndex].promedioDetecciones * accumulatedDataWeight) +
               (levelDatas[currentIndex].deteccionesGuardia * (1.0f - accumulatedDataWeight));

        // Promedio guardias flasheados
        processedLevelDatas[currentIndex].promedioGuardiasFlasheados = (processedLevelDatas[currentIndex].promedioGuardiasFlasheados * accumulatedDataWeight) +
           (levelDatas[currentIndex].flashesAGuardias * (1.0f - accumulatedDataWeight));
    }

    private void processDesignMetrics(int currentIndex, uint accumulatedSamples, float accumulatedDataWeight)
    {
        // ACUMULAR MAPA DE CALOR DEL NIVEL
        accumulateHeatMap(levelDatas[currentIndex].posicionesJugador, accumulatedDataWeight, 1);

        // Porcentaje Camaras desactivadas
        processedLevelDatas[currentIndex].porcentajeCamarasDesactivadas = processPercentageMetric(processedLevelDatas[currentIndex].porcentajeCamarasDesactivadas,
            levelDatas[currentIndex].camarasDesactivadas, levelConsts[currentIndex].tCamaras, accumulatedDataWeight);

        // Promedio deteccion camaras
        processedLevelDatas[currentIndex].promedioDeteccionCamaras = (processedLevelDatas[currentIndex].promedioDeteccionCamaras * accumulatedDataWeight) +
           (levelDatas[currentIndex].deteccionesCamara * (1.0f - accumulatedDataWeight));

        // Porcentaje carretes recogidos
        processedLevelDatas[currentIndex].porcentajeCarretesRecogidos = processPercentageMetric(processedLevelDatas[currentIndex].porcentajeCarretesRecogidos,
            levelDatas[currentIndex].fotosRecogidas, levelConsts[currentIndex].tCarretes, accumulatedDataWeight);

        // Porcentaje flashes recogidos
        processedLevelDatas[currentIndex].porcentajeFlashesRecogidos = processPercentageMetric(processedLevelDatas[currentIndex].porcentajeFlashesRecogidos,
            levelDatas[currentIndex].flashesRecogidos, levelConsts[currentIndex].tFlashes, accumulatedDataWeight);

        // Promedio tiempo en hallar objetivo
        processedLevelDatas[currentIndex].promedioTiempoEnHallarObjetivo = (processedLevelDatas[currentIndex].promedioTiempoEnHallarObjetivo * accumulatedDataWeight) +
           (levelDatas[currentIndex].tiempoEnEncontrarObjetivo * (1.0f - accumulatedDataWeight));
    }

    private void processInterfaceMetrics(int currentIndex, uint accumulatedSamples, float accumulatedDataWeight)
    {
        // Promedio de fotos contra guardias
        processedLevelDatas[currentIndex].promedioFotosContraGuardias = (processedLevelDatas[currentIndex].promedioFotosContraGuardias * accumulatedDataWeight) +
               (levelDatas[currentIndex].fotosAGuardias * (1.0f - accumulatedDataWeight));

        // Promedio Fallos Minijuego
        processedLevelDatas[currentIndex].promedioFallosMinijuego = (processedLevelDatas[currentIndex].promedioFallosMinijuego * accumulatedDataWeight) +
           (levelDatas[currentIndex].fallosMinijuego * (1.0f - accumulatedDataWeight));
    }

    private uint processPercentageMetric (uint accData, uint newData, uint levelConstData, float accWeight)
    {
        uint pctPartidaActual = (newData*100) / levelConstData;
        return (uint)Mathf.RoundToInt((accData * accWeight) +
            (pctPartidaActual * (1.0f - accWeight)));
    }

    
    #region Mapas de calor
    private bool processPositions(ref float [,] heatMap, float x, float y, int flag)
    {
        float oldMinX; float oldMaxX;
        float oldMinY; float oldMaxY;

        float newMinX; float newMaxX;
        float newMinY; float newMaxY;

        switch (currentLevel)
        {
            case 1:
                oldMinX = -85; oldMaxX = 18;
                oldMinY = -41; oldMaxY = 61;

                newMinX = 0; newMaxX = 100;
                newMinY = 0; newMaxY = 100;

                break;
            case 2:
                oldMinX = -29; oldMaxX = 87;
                oldMinY = -79; oldMaxY = 17;

                newMinX = 12; newMaxX = 88;
                newMinY = 0; newMaxY = 100;
                break;
            case 3:
                oldMinX = -23; oldMaxX = 96;
                oldMinY = -11; oldMaxY = 56;

                newMinX = 0; newMaxX = 100;
                newMinY = 23; newMaxY = 77;
                break;
            default:
                oldMinX = 0; oldMaxX = 0;
                oldMinY = 0; oldMaxY = 0;

                newMinX = 0; newMaxX = 100;
                newMinY = 0; newMaxY = 100;
                break;
        }
        if (oldMinX == 0) return false;

        int adjustedX = Mathf.RoundToInt(rangeChange(x, oldMinX, oldMaxX, newMinX, newMaxX));
        int adjustedY = Mathf.RoundToInt(rangeChange(y, oldMinY, oldMaxY, newMinY, newMaxY));

        heatMap[adjustedX, adjustedY] += heatMapIncrease;

        //if(flag == 1)Debug.LogError("Punto X: " + adjustedX + " Punto Y: " + adjustedY + " VALOR DE LA CASILLA ACTUAL: " + heatMap[adjustedX, adjustedY]);

        switch (flag)
        {
            case 1:
                if (heatMap[adjustedX, adjustedY] > playerMapsMaxValue[currentLevel - 1])
                    playerMapsMaxValue[currentLevel - 1] = heatMap[adjustedX, adjustedY];
                    break;
            case 2:
                if (heatMap[adjustedX, adjustedY] > deathMapsMaxValue[currentLevel - 1])
                    deathMapsMaxValue[currentLevel - 1] = heatMap[adjustedX, adjustedY];
                break;
            default:
                break;
        }

        return true;
    }

    private bool accumulateHeatMap (float [,] newMap, float accDataWeight, int flag)
    {
        // Igualamos todos los valores del mapa nuevo al rango [0, 100]
        float matrixMaxValue = 0.0f;
        switch (flag)
        {
            case 1:
                matrixMaxValue = playerMapsMaxValue[currentLevel - 1];
                break;
            case 2:
                matrixMaxValue = deathMapsMaxValue[currentLevel - 1];
                break;
            default:
                break;
        }

        try
        {
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    if (newMap[i, j] > 0.0f) newMap[i, j] = rangeChange(newMap[i, j], 0, sizeX, 0, matrixMaxValue);

                    switch (flag)
                    { 
                        case 1:
                            processedLevelDatas[currentLevel - 1].mapaCalorNivel[i, j] = (processedLevelDatas[currentLevel - 1].mapaCalorNivel[i, j]
                                * accDataWeight) + (newMap[i, j] * (1.0f - accDataWeight));
                            break;
                        case 2:
                            processedLevelDatas[currentLevel - 1].mapaCalorMuertes[i, j] = (processedLevelDatas[currentLevel - 1].mapaCalorMuertes[i, j]
                                * accDataWeight) + (newMap[i, j] * (1.0f - accDataWeight));
                            break;
                        default:
                            break;
                    }
                }
            }
        } catch(System.IndexOutOfRangeException e)
        {
            Debug.LogError("Out of Range al procesar mapa de calor con el acumulado");
            return false;
        }

        return true;
    }

    private float rangeChange (float x, float oMin, float oMax, float nMin, float nMax)
    {
        // Range check
        if(oMin == oMax)
        {
            return 0;
        }
        if (nMin == nMax)
        {
            return 0;
        }

        float result = 0;

        // Check reversed input range
        bool reverseInput = false;
        float oldMin = Mathf.Min(oMin, oMax);
        float oldMax = Mathf.Max(oMin, oMax);
        if (oldMin != oMin) reverseInput = true;

        // Check reversed output range
        bool reverseOutput = false;
        float newMin = Mathf.Min(nMin, nMax);
        float newMax = Mathf.Max(nMin, nMax);
        if (newMin != nMin) reverseOutput = true;

        float portion = (x - oldMin) * (newMax - newMin) / (oldMax - oldMin);

        if (reverseInput) {
            portion = (oldMax - x) * (newMax - newMin) / (oldMax - oldMin);
        }
        if (!reverseOutput) result = portion + newMin;
        else result = newMax - portion;
        return result;
    }
    #endregion

    #endregion

    #region Decoder

    //  Auxiliar readers
    private uint GetuintFromPos(StreamReader sr, int pos) { return uint.Parse(sr.ReadLine().Split(':')[pos]); }
    private float GetfloatFromPos(StreamReader sr, int pos) { return float.Parse(sr.ReadLine().Split(':')[pos]); }
    private bool DecodeLevel(uint level)
    {
        string s;

        try
        {
            using (StreamReader sr = new StreamReader(FILEGENERAL))
            {
                if (sr.EndOfStream) return false;

                s = sr.ReadLine();

                if (s == null)
                {
                    Debug.Log("First TelemetrySystem execution! End at least 1 level to output data...");
                    return false;
                }


                while (s != ("Lvl " + level))
                {
                    s = sr.ReadLine();
                }

                //  Decodes..
                uint lvl = level - 1;
                currentLevel = int.Parse(s.Split(' ')[1]);                                         //  Lvl x
                if (currentLevel == 1) promedioClicksEnCinematica = GetfloatFromPos(sr, 1);            // (just in lvl 1) promedioClicksEnCinematica: x
                processedLevelDatas[lvl].totalSamples = GetuintFromPos(sr, 1);                         //  totalSamples:x
                processedLevelDatas[lvl].promedioMuertes = GetfloatFromPos(sr, 1);                     //  promedioMuertes:x
                processedLevelDatas[lvl].porcentajeFlashes = GetuintFromPos(sr, 1);                    //  porcentajeFlashes:x
                processedLevelDatas[lvl].promedioPuntuacion = GetfloatFromPos(sr, 1);                  //  promedioPuntuacion:x
                processedLevelDatas[lvl].porcentajeColeccionables = GetuintFromPos(sr, 1);             //  porcentajeColeccionables:x
                processedLevelDatas[lvl].promedioTiempoNivel = GetfloatFromPos(sr, 1);                  //  promedioTiempoNivel:x
                processedLevelDatas[lvl].promedioDetecciones = GetfloatFromPos(sr, 1);                 //  promedioDetecciones:x
                processedLevelDatas[lvl].promedioGuardiasFlasheados = GetfloatFromPos(sr, 1);          //  promedioGuardiasFlasheados:x
                processedLevelDatas[lvl].porcentajeCamarasDesactivadas = GetuintFromPos(sr, 1);        //  porcentajeCamarasDesactivadas:x
                processedLevelDatas[lvl].promedioDeteccionCamaras = GetfloatFromPos(sr, 1);            //  promedioDeteccionCamaras:x
                processedLevelDatas[lvl].porcentajeCarretesRecogidos = GetuintFromPos(sr, 1);          //  porcentajeCarretesRecogidos:x
                processedLevelDatas[lvl].porcentajeFlashesRecogidos = GetuintFromPos(sr, 1);           //  porcentajeFlashesRecogidos:x
                processedLevelDatas[lvl].promedioTiempoEnHallarObjetivo = GetfloatFromPos(sr, 1);      //  promedioTiempoEnHallarObjetivo:x
                processedLevelDatas[lvl].promedioFotosContraGuardias = GetfloatFromPos(sr, 1);         //  promedioFotosContraGuardias:x
                processedLevelDatas[lvl].promedioFallosMinijuego = GetfloatFromPos(sr, 1);             //  promedioFallosMinijuego:x
                                                                                                   //  porcentajeColeccionablesConcretos: id1/% id2/% id3/% id4/%
                string[] subs;
                string[] subs2;
                subs = sr.ReadLine().Split(' ');    // id1/% / id2/% /id3/%
                if (subs[1] == null)
                {
                    for (int i = 1; i < subs.Length; i++)
                    {
                        subs2 = subs[i].Split('/');     // id1 / %
                        processedLevelDatas[lvl].porcentajeColeccionablesConcretos[int.Parse(subs2[0])] = uint.Parse(subs2[1]);
                    }
                }

                string [] xData;
                xData = sr.ReadLine().Split(' ')[1].Split('|');
                for (int x = 0; x < sizeX; x++)
                {
                    string[] yData = xData[x].Split('/');
                    for (int y = 0; y < sizeY; y++)
                    {
                      processedLevelDatas[lvl].mapaCalorNivel[x, y] = float.Parse(yData[y]);
                    }
                }

                string[] xData2;
                xData2 = sr.ReadLine().Split(' ')[1].Split('|');
                for (int x = 0; x < sizeX; x++)
                {
                    string[] yData = xData2[x].Split('/');
                    for (int y = 0; y < sizeY; y++)
                    {
                        processedLevelDatas[lvl].mapaCalorMuertes[x, y] = float.Parse(yData[y]);
                    }
                }

                Debug.Log("(Decoder) succesfully decoded lvl " + (lvl + 1));
            }   
        }
        catch  (System.Exception e)
        {
            Debug.LogWarning("(Decoder) General telemetry file can't be read. It may be corrupted or this is the first execution :D !  Details: ");
            Debug.LogWarning(e.Message + " / " + e.Data);
            return false;
        }
        
        return true;
    }
    #endregion

    #region Encoder
    private bool Encode()
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(FILEGENERAL, false))
            {
                for (int i = 0; i < 3; i++)   // Rewrites the 3 lvls in general.txt
                {
                    sw.WriteLine("Lvl " + (i + 1));
                    if (i == 0) sw.WriteLine("promedioClicksEnCinematica: " + promedioClicksEnCinematica);  // Exclusive of lvl 1 (index 0)
                    sw.WriteLine("totalSamples: " + processedLevelDatas[i].totalSamples);
                    sw.WriteLine("promedioMuertes: " + processedLevelDatas[i].promedioMuertes);
                    sw.WriteLine("porcentajeFlashes: " + processedLevelDatas[i].porcentajeFlashes);
                    sw.WriteLine("promedioPuntuacion: " + processedLevelDatas[i].promedioPuntuacion);
                    sw.WriteLine("porcentajeColeccionables: " + processedLevelDatas[i].porcentajeColeccionables);
                    sw.WriteLine("promedioTiempoNivel: " + processedLevelDatas[i].promedioTiempoNivel);
                    sw.WriteLine("promedioDetecciones: " + processedLevelDatas[i].promedioDetecciones);
                    sw.WriteLine("promedioGuardiasFlasheados: " + processedLevelDatas[i].promedioGuardiasFlasheados);
                    sw.WriteLine("porcentajeCamarasDesactivadas: " + processedLevelDatas[i].porcentajeCamarasDesactivadas);
                    sw.WriteLine("promedioDeteccionCamaras: " + processedLevelDatas[i].promedioDeteccionCamaras);
                    sw.WriteLine("porcentajeCarretesRecogidos: " + processedLevelDatas[i].porcentajeCarretesRecogidos);
                    sw.WriteLine("porcentajeFlashesRecogidos: " + processedLevelDatas[i].porcentajeFlashesRecogidos);
                    sw.WriteLine("promedioTiempoEnHallarObjetivo: " + processedLevelDatas[i].promedioTiempoEnHallarObjetivo);
                    sw.WriteLine("promedioFotosContraGuardias: " + processedLevelDatas[i].promedioFotosContraGuardias);
                    sw.WriteLine("promedioFallosMinijuego: " + processedLevelDatas[i].promedioFallosMinijuego);

                    sw.Write("porcentajeColeccionablesConcretos: ");
                    if (processedLevelDatas[i].porcentajeColeccionablesConcretos.Count > 0)
                        foreach (KeyValuePair<int, uint> collectable in processedLevelDatas[i].porcentajeColeccionablesConcretos)
                        {
                            sw.Write(collectable.Key.ToString() + '/' + collectable.Value.ToString() + ' ');
                        }
                    sw.Write('\n');

                    sw.Write("PlayerPositions: ");
                    if(processedLevelDatas[i].mapaCalorNivel.Length > 0)
                    {
                        bool ctrl = true;
                        for (int x = 0; x < sizeX; x++)
                        {
                            for (int y = 0; y < sizeY; y++)
                            {
                                sw.Write(processedLevelDatas[i].mapaCalorNivel[x, y]);
                                if ((y == (sizeY - 1)) && (x == (sizeX - 1)))
                                {
                                    ctrl = false;
                                }
                                else if (!(y == (sizeY - 1))) sw.Write("/");
                            }
                            if (ctrl)
                            {
                                sw.Write("|");
                            }
                        }
                        sw.Write("\n");
                    }

                        

                    sw.Write("DeathPositions: ");
                    if (processedLevelDatas[i].mapaCalorMuertes.Length > 0)
                    {
                        bool ctrl = true;
                        for (int x = 0; x < sizeX; x++)
                        {
                            for (int y = 0; y < sizeY; y++)
                            {
                                sw.Write(processedLevelDatas[i].mapaCalorMuertes[x, y]);
                                if ((y == (sizeY - 1)) && (x == (sizeX - 1)))
                                {
                                    ctrl = false;                                        
                                }
                                else if(!(y == (sizeY - 1))) sw.Write("/");
                            }
                            if (ctrl)
                            {
                                sw.Write("|");
                            }
                        }
                        sw.Write("\n");
                    }
                    Debug.Log("Successfull encoding! lvl: " + (i + 1));
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("(Encoder) General telemetry file can't be written.  Details: ");
            Debug.LogError(e.Message);
            return false;
        }
        return true;
    }
    #endregion
}
