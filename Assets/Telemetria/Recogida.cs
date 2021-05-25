using System;
using System.Collections.Generic;

public class RecogidaInfo
{
    private int coleccionables;
    private int totalColeccionables;
    private int carretes;
    private int totalCarretes;
    private int flashes;
    private int totalFlashes;


    public RecogidaInfo(int totCol, int totCarr, int totFlash)
    {
        this.coleccionables = 0;
        this.totalColeccionables = totCol;
        this.carretes = 0;
        this.totalCarretes = totCarr;
        this.flashes = 0;
        this.totalFlashes = totFlash;
    }
    

    public void recogidaColeccionable() {
        coleccionables++;
    }

    public void recogidaCarrete()
    {
        carretes++;
    }

    public void recogidaFlash()
    {
        flashes++;
    }

    public int Coleccionables
    {
        get { return this.coleccionables; }
    }

    public int Carretes
    {
        get { return this.carretes; }
    }

    public int Flashes
    {
        get { return this.flashes; }
    }


    public float porcentajeColeccionables() {
        return coleccionables * 100 / totalColeccionables;
    }

    public float porcentajeCarretes()
    {
        return carretes * 100 / totalCarretes;
    }

    public float porcentajeFlashes()
    {
        return flashes * 100 / totalFlashes;
    }

    public void reinicio() {
        coleccionables = 0;
        carretes = 0;
        flashes = 0;
    }
}