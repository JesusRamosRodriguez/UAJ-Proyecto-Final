using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
//using UnityEngine.SystemInfo.deviceUniqueIdentifier;

public enum SerializeType { JSON, PLAIN };

//public enum TipoEvento
//{
//    //Gestion de nivel :
//    InicioSesion,
//    InicioNivel,
//    FinSesion,
//    FinNivel,
//    AbandonoNivel,
//    Reinicio,
//    Pausa,
//    //Recogida :
//    Coleccionable,
//    FotoRecogida,
//    FlashRecogida,
//    //Uso de :
//    BotonCamara,
//    FotoUso,
//    FlashUso,
//    FotoGuardia,
//    //Default
//    Null
//}

// Eventos que solo pasan el propio evento como información
public struct singleEvent
{
    public string eventName;
    public DateTime time;
    public int nivel;

    public void setNull() { time = DateTime.UtcNow; }
}

// Eventos que pasan un valor numérico como información
public struct valueEvent
{
    public string eventName;
    public DateTime time;
    public int nivel;

    public int value;

    public void setNull() { time = DateTime.UtcNow; }
}

// Eventos que pasan una coordenada como información
public struct positionEvent
{
    public string eventName;
    public DateTime time;
    public int nivel;

    public float x;
    public float y;

    public void setNull() { time = DateTime.UtcNow; }
}

// Eventos de inicio, reinicio y fin de nivel
public struct levelEvent
{
    public string eventName;
    public DateTime time;
    public int nivel;

    public void setNull() { time = DateTime.UtcNow; }
}


public class TelemetrySystem{

    #region SINGLETON

    private static TelemetrySystem instance = null;

    private TelemetrySystem()
    {
        DateTime act = DateTime.UtcNow;
        machineID = SystemInfo.deviceUniqueIdentifier;
        sessionID = machineID + act.Hour.ToString() + act.Minute.ToString() + act.Second.ToString() + act.Day.ToString() + act.Month.ToString() + act.Year.ToString();

        timeElapsed = 0.0f;
        saveFrequency = 30000; // DEFAULT: Actualizamos datos cada 30 segundos
        encoding = SerializeType.JSON;

        threadIsStopped = true;

        //eventQueue = new Queue<TelemetryEvent>();

        persistence = new PersistenceSystem();
        persistence.Init(machineID, sessionID);
    }

    public static TelemetrySystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TelemetrySystem();
            }
            return instance;
        }
    }
    #endregion

    // Módulos
    private PersistenceSystem persistence;

    // Lista de eventos y codificación
    private SerializeType encoding;

    // Frecuencia de guardado
    private float saveFrequency; // La frecuencia en ms con la que el sistema serializa y graba
    internal float timeElapsed; // Tiempo transcurrido desde última actualización

    // IDs de sesión y máquina
    private readonly string sessionID;
    private readonly string machineID;

    // Hilo para serialización y guardado
    Thread telemetryThread;
    bool threadIsStopped;


    public void shutdown()
    {
        if (!threadIsStopped)
        {
            telemetryThread.Join();
        }
        //addEvent("FinSesion");
        ForcedUpdate();
        persistence.ShutDown();
    }

    public void Update () {
        timeElapsed += Time.deltaTime * 1000;
        //if(timeElapsed > saveFrequency && threadIsStopped)
        //{
        //    //telemetryThread = new Thread(SerializeAndSave);
        //    //telemetryThread.Start();
        //}
	}


    private void SerializeAndSave()
    {
        threadIsStopped = false;
        //while (eventQueue.Count > 0)
        //{
        //    persistence.toJson(eventQueue.Peek());
        //    eventQueue.Dequeue();
        //}

        timeElapsed = 0;
        threadIsStopped = true;
    }

    public bool telemetryThreadFinished()
    {
        return threadIsStopped;
    }

    /// <summary>
    /// Fuerza al sistema a serializar y guardar todos los eventos en la cola
    /// independientemente del tiempo transcurrido. Reinicia contador.
    /// </summary>
    public void ForcedUpdate()
    {
        //while (eventQueue.Count > 0)
        //{
        //    persistence.toJson(eventQueue.Peek());
        //    eventQueue.Dequeue();
        //}

        timeElapsed = 0;
    }

    /// <summary>
    /// Tipo de codificación para la serialización
    /// </summary>
    /// <param name="s"></param>
    public void SetEncoding(SerializeType s)
    {
        encoding = s;
    }

    /// <summary>
    /// Frecuencia con la que se serializa y guarda la telemetría
    /// </summary>
    /// <param name="time"> Tiempo en milisegundos </param>
    public void SetSaveFrequency(float time)
    {
        saveFrequency = time;
    }

    #region RECEPCION DE EVENTOS
    public void singleEvent(string eventName, int level)
    {
        singleEvent e;
        e.eventName = eventName;
        e.nivel = level;
        e.time = DateTime.UtcNow;

        // ENVIAR AL PROCESADOR
    }

    public void valueEvent(string eventName, int value, int level)
    {
        valueEvent e;
        e.eventName = eventName;
        e.nivel = level;
        e.time = DateTime.UtcNow;
        e.value = value;

        // ENVIAR AL PROCESADOR
    }

    public void postitionEvent(string eventName, float x, float y, int level)
    {
        positionEvent e;
        e.eventName = eventName;
        e.x = x;
        e.y = y;
        e.nivel = level;
        e.time = DateTime.UtcNow;

        // ENVIAR AL PROCESADOR
    }

    public void levelEvent(string eventName, int level = 0)
    {
        levelEvent e;
        e.eventName = eventName;
        e.nivel = level;
        e.time = DateTime.UtcNow;

        // ENVIAR AL PROCESADOR
    }


    #endregion
}
