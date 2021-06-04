using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// Eventos que solo pasan el propio evento como información
public struct singleEvent
{
    public string eventName;
    public DateTime time;
    public int nivel;
}

/// Eventos que pasan un valor numérico como información
public struct valueEvent
{
    public string eventName;
    public DateTime time;
    public int nivel;

    public int value;
}

/// Eventos que pasan una coordenada como información
public struct positionEvent
{
    public string eventName;
    public DateTime time;
    public int nivel;

    public float x;
    public float y;
}

/// Eventos de inicio, reinicio y fin de nivel
public struct levelEvent
{
    public string eventName;
    public DateTime time;
    public int nivel;
}


public class TelemetrySystem{

    #region SINGLETON

    private static TelemetrySystem instance = null;

    private TelemetrySystem()
    {
        timeElapsed = 0.0f;
        updateFrequency = 1000 / 30;

        canUpdate = false;
        threadIsStopped = true;

        //eventQueue = new Queue<TelemetryEvent>();

        persistence = new PersistenceSystem();
        persistence.Init();
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

    // Frecuencia de guardado
    private float saveFrequency; // La frecuencia en ms con la que el sistema serializa y graba
    internal float timeElapsed; // Tiempo transcurrido desde última actualización

    // Frecuencia de actualización de posiciones
    private float updateFrequency;
    private bool canUpdate;

    // Hilo para serialización y guardado
    Thread telemetryThread;
    bool threadIsStopped;


    public void shutdown()
    {
        if (!threadIsStopped)
        {
            telemetryThread.Join();
        }
        levelEvent("FinSesion");
        ForcedUpdate();
        persistence.ShutDown();
    }

    public void Update () {

        if (canUpdate) canUpdate = false;

        timeElapsed += Time.deltaTime * 1000;
        if (timeElapsed > updateFrequency /*&& threadIsStopped*/)
        {
            //telemetryThread = new Thread(SerializeAndSave);
            //telemetryThread.Start();
            canUpdate = true;
            timeElapsed = 0;
        }
    }

    public float getUpdateFrequency() { return updateFrequency; }

    public bool updateFrame() { return canUpdate; }

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
    /// Fuerza al sistema a serializar y guardar todos los eventos de la cola
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

        persistence.SendEvent(e);
    }

    public void valueEvent(string eventName, int value, int level)
    {
        valueEvent e;
        e.eventName = eventName;
        e.nivel = level;
        e.time = DateTime.UtcNow;
        e.value = value;

        persistence.SendEvent(e);
    }

    public void positionEvent(string eventName, float x, float y, int level)
    {
        positionEvent e;
        e.eventName = eventName;
        e.x = x;
        e.y = y;
        e.nivel = level;
        e.time = DateTime.UtcNow;

        persistence.SendEvent(e);
    }

    public void levelEvent(string eventName, int level = 0)
    {
        levelEvent e;
        e.eventName = eventName;
        e.nivel = level;
        e.time = DateTime.UtcNow;

        persistence.SendEvent(e);
    }


    #endregion
}
