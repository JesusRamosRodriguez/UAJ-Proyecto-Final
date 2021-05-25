using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
//using UnityEngine.SystemInfo.deviceUniqueIdentifier;

public enum SerializeType { JSON, PLAIN };

public enum TipoEvento
{
    //Gestion de nivel :
    InicioSesion,
    InicioNivel,
    FinSesion,
    FinNivel,
    AbandonoNivel,
    Reinicio,
    Pausa,
    //Recogida :
    Coleccionable,
    FotoRecogida,
    FlashRecogida,
    //Uso de :
    BotonCamara,
    FotoUso,
    FlashUso,
    FotoGuardia,
    //Default
    Null
}


//Gestion de Nivel
public struct TelemetryEvent
{
    public TipoEvento type;
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

        eventQueue = new Queue<TelemetryEvent>();

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
    private Queue<TelemetryEvent> eventQueue; // Lista de eventos recogidos a la espera de tratamiento
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
        addEvent("FinSesion");
        ForcedUpdate();
        persistence.ShutDown();
    }

    public void Update () {
        timeElapsed += Time.deltaTime * 1000;
        if(timeElapsed > saveFrequency && threadIsStopped)
        {
            telemetryThread = new Thread(SerializeAndSave);
            telemetryThread.Start();
        }
	}


    private void SerializeAndSave()
    {
        threadIsStopped = false;
        while (eventQueue.Count > 0)
        {
            persistence.toJson(eventQueue.Peek());
            eventQueue.Dequeue();
        }

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
        while (eventQueue.Count > 0)
        {
            persistence.toJson(eventQueue.Peek());
            eventQueue.Dequeue();
        }

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
    public void addEvent(TelemetryEvent e)
    {
        eventQueue.Enqueue(e);
    }

    public void addEvent(string eventName, int level = 0)
    {
        TelemetryEvent e;
        e.time = DateTime.UtcNow;
        e.nivel = level;

        switch (eventName){
            case "InicioSesion":
                e.type = TipoEvento.InicioSesion;
                break;
            case "InicioNivel":
                e.type = TipoEvento.InicioNivel;
                break;
            case "FinSesion":
                e.type = TipoEvento.FinSesion;
                break;
            case "FinNivel":
                e.type = TipoEvento.FinNivel;
                break;
            case "AbandonoNivel":
                e.type = TipoEvento.AbandonoNivel;
                break;
            case "Reinicio":
                e.type = TipoEvento.Reinicio;
                break;
            case "Pausa":
                e.type = TipoEvento.Pausa;
                break;
            case "Coleccionable":
                e.type = TipoEvento.Coleccionable;
                break;
            case "FotoRecogida":
                e.type = TipoEvento.FotoRecogida;
                break;
            case "FlashRecogida":
                e.type = TipoEvento.FlashRecogida;
                break;
            case "BotonCamara":
                e.type = TipoEvento.BotonCamara;
                break;
            case "FotoUso":
                e.type = TipoEvento.FotoUso;
                break;
            case "FlashUso":
                e.type = TipoEvento.FlashUso;
                break;
            case "FotoGuardia":
                e.type = TipoEvento.FotoGuardia;
                break;
            default:
                e.type = TipoEvento.Null;
                break;
        }

        eventQueue.Enqueue(e);
    }

    public void addEvent(TipoEvento eventType)
    {
        TelemetryEvent e;
        e.time = DateTime.UtcNow;
        e.type = eventType;
        e.nivel = 0;
        eventQueue.Enqueue(e);
    }

    public void addEvent(TipoEvento eventType, int level)
    {
        TelemetryEvent e;
        e.time = DateTime.UtcNow;
        e.type = eventType;
        e.nivel = level;
        eventQueue.Enqueue(e);
    }

    public void addEvent(TipoEvento eventType, int level, DateTime timestamp)
    {
        TelemetryEvent e;
        e.time = timestamp;
        e.type = eventType;
        e.nivel = level;
        eventQueue.Enqueue(e);
    }
    #endregion
}
