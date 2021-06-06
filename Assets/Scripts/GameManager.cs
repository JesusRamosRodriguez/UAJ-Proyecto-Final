using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


class Nivel
{
    // Variables para guardar de dónde viene la puntuación, de cara a la pantalla de puntuación
    public int puntos,
                ptsMinijuego, // puntos para el nivel conseguidos en el minijuego
                ptsFotoOp, // puntos para el nivel conseguidos por fotos a famosos opcionales
                ptsBombillas, // puntos para el nivel conseguidos por bombillas extra
                ptsCarretes, // puntos para el nivel conseguidos por carretes extra
                ptsLoot, // puntos para el nivel conseguidos por coleccionables cogidos
                penalTiempo, // penalización por tiempo
                puntuacionMaxima;

    public bool minijuego; // Es true si se ha entrado en el minijuego de nivel
    public bool terminado; // Es true si el nivel se ha terminado

	// Constructor
	public Nivel(string _nombre)
	{
		string nombre = _nombre; // Nombre del nivel, ejemplo: Nivel1

		// Inicialización
		puntos = 0;
		ptsMinijuego = 0;
		ptsFotoOp = 0;
		ptsBombillas = 0;
		ptsCarretes = 0;
		ptsLoot = 0;
		penalTiempo = 0;
        puntuacionMaxima = 0;

        minijuego = false;
        terminado = false;
	}

	// Devolver la puntuación total obtenida
	public int PuntuacionTotal()
	{
		return(ptsLoot + ptsMinijuego + ptsFotoOp + ptsCarretes + ptsBombillas + penalTiempo); //penalTiempo pasa como NEGATIVO
	}

	// Devolver el texto con la puntuación del nivel
	public string PuntuacionFinalText()
	{
		return("Coleccionables recogidos: " + ptsLoot + " puntos\n" + 
			"Bombillas y carretes sin usar: " + (ptsBombillas + ptsCarretes) + " puntos\n" +
			"Fotos a famosos secundarios: " + ptsFotoOp + " puntos\n" +
			"Fotos al famoso principal: " + ptsMinijuego + " puntos\n" + 
			"Penalización por tiempo: " + penalTiempo + " puntos\n" +
			"TOTAL: " + PuntuacionTotal() + " puntos\n");
	}



}

public class GameManager : MonoBehaviour {

    #region TELEMETRIA
    TelemetrySystem telemetria;

    public int getLevelNumber()
    {
        return int.Parse(actual[actual.Length - 1].ToString());
    }

    void Update()
    {
        telemetria.Update();
    }

    void OnApplicationQuit()
    {
        telemetria.shutdown();
    }

    #endregion

    Nivel nivel1;
    Nivel nivel2;
    Nivel nivel3;
    public int bombillas, carretes, loot,primeravez; //Las bombillas se usan para stunnear y los carretes para las fotos
                                          //puntuacionMinijuego,
                                          //puntos;

    public int carreteEspecial = 1;

    private bool Nivel1S = false, Nivel2S = false, Nivel3S = false,
                tutoVisto = false, allowPausa = true, puedeFlash = true;

    public static GameManager instance = null;

    string actual;
    float tiempo;
	bool gameIsPaused = false;

    //VARIABLES DE PUNTOS
    public int modPenalTiempo = 10, carretesExtraPts = 100, bombillasExtraPts = 250;

    GameObject crono,camara;

    void Awake()
    {
		if (instance == null)
        {
			instance = this;
			DontDestroyOnLoad(this.gameObject);
            
        }
		else Destroy(this.gameObject);

        telemetria = TelemetrySystem.Instance;

        nivel1 = new Nivel ("nivel1");
        nivel2 = new Nivel("nivel2");
        nivel3 = new Nivel("nivel3");
        actual = SceneManager.GetActiveScene().name;
        Partida();
        crono = GameObject.FindWithTag("Crono");
        primeravez = 0;

        telemetria.levelEvent("InicioSesion");
    }

    public int Bombillas()
    {
		return bombillas;
    }

    public int Carretes()
    {
        return carretes;
    }
    
    public int CarreteEspecial()
    {
        return carreteEspecial;
    }

    public int Loot()
    {
        return loot;
    }


	public void PickBombilla ()
	{
		bombillas++;
        telemetria.singleEvent("FlashRecogido", getLevelNumber());
    }

	public void PickCarrete ()
	{
        carretes++;
        telemetria.singleEvent("FotoRecogida", getLevelNumber());
    }

	// Aumenta la puntuación en una cantidad determinada y guarda de dónde viene la puntuación
	public void SumaPuntos(int cantidad, string motivo)
	{
		nivel1.puntos += cantidad;
        nivel2.puntos += cantidad;
        nivel3.puntos += cantidad;

        /*	"minijuego"	-> ptosMinijuego
		 * 	"opcional"	-> ptosFotoOp
		 * 	"bombillas"	-> ptosBombillas
		 * 	"carretes"	-> ptosCarretes
		 * 	"loot"	-> ptosLoot
         * 	"tiempo"-> penalTiempo
		 */
        int indiceNivel = int.Parse(actual[actual.Length-1].ToString());
        
        switch (indiceNivel)
        {
            case 1:
                // Nivel 1
                switch (motivo)
                {
                    case "minijuego":
                        nivel1.ptsMinijuego += cantidad;
                        break;
                    case "opcional":
                        nivel1.ptsFotoOp += cantidad;
                        break;
                    case "bombillas":
                        nivel1.ptsBombillas += cantidad;
                        break;
                    case "carretes":
                        nivel1.ptsCarretes += cantidad;
                        break;
                    case "loot":
                        nivel1.ptsLoot += cantidad;
                        break;
                    case "penalTiempo":
                        nivel1.penalTiempo += cantidad;
                        break;
                }
                break;
            case 2:
                // Nivel 2
                switch (motivo)
                {
                    case "minijuego":
                        nivel2.ptsMinijuego += cantidad;
                        break;
                    case "opcional":
                        nivel2.ptsFotoOp += cantidad;
                        break;
                    case "bombillas":
                        nivel2.ptsBombillas += cantidad;
                        break;
                    case "carretes":
                        nivel2.ptsCarretes += cantidad;
                        break;
                    case "loot":
                        nivel2.ptsLoot += cantidad;
                        break;
                    case "penalTiempo":
                        nivel2.penalTiempo += cantidad;
                        break;
                }
                break;
            case 3:
                // Nivel 3
                switch (motivo)
                {
                    case "minijuego":
                        nivel3.ptsMinijuego += cantidad;
                        break;
                    case "opcional":
                        nivel3.ptsFotoOp += cantidad;
                        break;
                    case "bombillas":
                        nivel3.ptsBombillas += cantidad;
                        break;
                    case "carretes":
                        nivel3.ptsCarretes += cantidad;
                        break;
                    case "loot":
                        nivel3.ptsLoot += cantidad;
                        break;
                    case "penalTiempo":
                        nivel3.penalTiempo += cantidad;
                        break;
                }
                break;
        }
       
	}

	public int GetPuntos()
	{
        int indiceNivel = int.Parse(actual[actual.Length - 1].ToString());
        int puntos = 0;
		switch (indiceNivel) {
		case 1:
			puntos = PuntosN1();
			break;
            case 2:
                puntos = PuntosN2();
                break;
            case 3:
                puntos = PuntosN3();
                break;
        }

		return puntos;
	}

	int PuntosN1()
	{
		return nivel1.puntos;
	}
    int PuntosN2()
    {
        return nivel2.puntos;
    }
    int PuntosN3()
    {
        return nivel3.puntos;
    }

    public void Minijuego()
    {
        if(actual == "Nivel1")
          nivel1.minijuego = true;
        else if(actual == "Nivel2")
            nivel2.minijuego = true;
        else if (actual == "Nivel3")
            nivel3.minijuego = true;
    }

    public void GoToMiniJuego()
    {
        int indiceNivel = int.Parse(actual[actual.Length - 1].ToString());
        switch (indiceNivel)
        {
            case 1:
                // Nivel 1
                SceneManager.LoadScene("Minijuego1");
                break;
            case 2:
                // Nivel 2
                SceneManager.LoadScene("Minijuego2");
                break;
            case 3:
                // Nivel 3
                SceneManager.LoadScene("Minijuego3");
                break;
        }
        camara =  GameObject.FindWithTag("CamarasLaseres");
        camara.SetActive(false);
        tiempo = GameObject.FindWithTag("Crono").GetComponent<Cronometro>().TiempoAntes();
        actual = "Minijuego" + indiceNivel;
    }
    public bool MinijuegoTerminado()
    {


        switch (actual)
        {

            case "Nivel1":
                return nivel1.minijuego;
                
            case "Nivel2":
                return nivel2.minijuego;
            case "Nivel3":
                return nivel3.minijuego;

            default:
                return false;
        }
        
    }




    public string NivelActual()
    {
        return actual;
    }
    public void Nivel1()
    {
        if (nivel1.terminado || primeravez == 1)
            SceneManager.LoadScene("Nivel1");
        else
        {
            SceneManager.LoadScene("Cinematica1");
            Invoke("Cinematica1", 85);
            primeravez = 1;
        }
        nivel1.minijuego = false;
        actual = "Nivel1";
        bombillas = 4;
        carretes = 3;
        loot = 0;
        carreteEspecial = 1;
        nivel1.puntos = 0;
    }
    void Cinematica1()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void Nivel2()
    {
        nivel2.minijuego = false;
        SceneManager.LoadScene("Nivel2");
        actual = "Nivel2";
        bombillas = 3;
        carretes = 3;
        loot = 0;
        carreteEspecial = 1;
        nivel2.puntos = 0;
    }
    public void Nivel3()
    {
        nivel3.minijuego = false;
        SceneManager.LoadScene("Nivel3");
        actual = "Nivel3";
        bombillas = 3;
        carretes = 2;
        loot = 0;
        carreteEspecial = 1;
        nivel3.puntos = 0;
    }

    public void Pierde()
    {
        SceneManager.LoadScene("FinPartida");
        Destroy(GameObject.FindWithTag("Objetos"));
        Destroy(GameObject.FindWithTag("CamarasLaseres"));
    }

        // FIN DEL NIVEL 1
        public void FinMinijuego()
	{
        int indiceNivel = int.Parse(actual[actual.Length - 1].ToString());
        // Puntuación por carretes extra
        for (int i = 0; i < carretes; i++)
			SumaPuntos (carretesExtraPts, "carretes");

		// Puntuación por bombillas extra
		for (int i = 0; i < bombillas; i++) {
			
			SumaPuntos (bombillasExtraPts, "bombillas");
			//Debug.Log ("Ptos Bombilla " + nivel1.ptsBombillas);
		}
        // Ir a la pantalla de puntuación de este nivel
        switch (indiceNivel)
        {  
            case 1:
                // Nivel 1         
                SceneManager.LoadScene("Nivel1");
                nivel1.minijuego = true;
                break;
            case 2:
                // Nivel 2
                SceneManager.LoadScene("Nivel2");
                nivel2.minijuego = true;
                break;
            case 3:
                // Nivel 3
                SceneManager.LoadScene("Nivel3");
                nivel3.minijuego = true;
                break;
        }
        
        actual = "Nivel" + indiceNivel;
        Invoke("ActualizaTiempo", 0.2f);

    }

    void ActualizaTiempo()
    {
        camara.SetActive(true);
        GameObject.FindWithTag("Crono").GetComponent<Cronometro>().CambiaTiempo(tiempo);
    }
    
	public void GoToPuntuacion() 
	{
        int indiceNivel = int.Parse(actual[actual.Length - 1].ToString());
        int penalTiempo = (int)System.Math.Round(GameObject.FindWithTag("Crono").GetComponent<Cronometro>().FinPartida());
        switch (indiceNivel) 
		{
		case 1:
                // Nivel 1
                SumaPuntos(-penalTiempo * modPenalTiempo, "penalTiempo");

                if (nivel1.PuntuacionTotal() > nivel1.puntuacionMaxima)
                    nivel1.puntuacionMaxima = nivel1.PuntuacionTotal();

                TelemetrySystem.Instance.valueEvent("PuntuacionNivel", nivel1.PuntuacionTotal(), 1);
                TelemetrySystem.Instance.levelEvent("FinNivel", 1);
                SceneManager.LoadScene("N1Puntuacion1");
                nivel1.terminado = true;
			break;
        case 2:
                // Nivel 2
                SumaPuntos(-penalTiempo * modPenalTiempo, "penalTiempo");
               
                if (nivel2.PuntuacionTotal() > nivel2.puntuacionMaxima)
                    nivel2.puntuacionMaxima = nivel2.PuntuacionTotal();

                TelemetrySystem.Instance.valueEvent("PuntuacionNivel", nivel2.PuntuacionTotal(), 2);
                TelemetrySystem.Instance.levelEvent("FinNivel", 2);
                SceneManager.LoadScene("N2Puntuacion2");
                nivel2.terminado = true;
                break;
            case 3:
                // Nivel 3
                SumaPuntos(-penalTiempo * modPenalTiempo, "penalTiempo");
                
                if (nivel3.PuntuacionTotal() > nivel3.puntuacionMaxima)
                    nivel3.puntuacionMaxima = nivel3.PuntuacionTotal();

                TelemetrySystem.Instance.valueEvent("PuntuacionNivel", nivel3.PuntuacionTotal(), 3);
                TelemetrySystem.Instance.levelEvent("FinNivel", 3);
                SceneManager.LoadScene("N3Puntuacion3");
                nivel3.terminado = true;
                break;
        }
        GuardaPartida();
        Destroy(GameObject.FindWithTag("Objetos"));
        Destroy(GameObject.FindWithTag("CamarasLaseres"));
        actual = "N" + indiceNivel + "Puntuacion" + indiceNivel;

    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
        actual = "Menu";
        Destroy(GameObject.FindWithTag("Objetos"));
        Destroy(GameObject.FindWithTag("CamarasLaseres"));
    }


	// Devolver la puntuación total obtenida
	public int PuntuacionTotalNivel()
	{
        int indiceNivel = int.Parse(actual[actual.Length - 1].ToString());
        int puntuacion = 0;
		switch (indiceNivel) 
		{
		case 1:
			puntuacion = nivel1.PuntuacionTotal();
			break;
        case 2:
            puntuacion = nivel2.PuntuacionTotal();
            break;
        case 3:
            puntuacion = nivel3.PuntuacionTotal();
            break;
        }

		return puntuacion;
	}

	public string TextoPuntuacion()
	{
        int indiceNivel = int.Parse(actual[actual.Length - 1].ToString());
        string texto = "";
		switch (indiceNivel) 
		{
		case 1:
			texto = nivel1.PuntuacionFinalText ();
			break;
        case 2:
            texto = nivel2.PuntuacionFinalText();
            break;
        case 3:
            texto = nivel3.PuntuacionFinalText();
            break;
        }

		return texto;
	}
    public int PuntuacionMaxima(int nivel)
    {
        switch (nivel)
        {
            case 1:
                return  nivel1.puntuacionMaxima;
            case 2:
                return nivel2.puntuacionMaxima;
                
            case 3:
                return nivel3.puntuacionMaxima;
                
        }
        return -1;
    }


    public void Continua()
    {
        if (actual == "N3Puntuacion3")
        {
            SceneManager.LoadScene("Cinematica2");
            Invoke("GoToMenu", 73);
        }

        else if (nivel1.terminado && nivel2.terminado)
            Nivel3();
        else if (nivel1.terminado)
            Nivel2();
        else
            Nivel1();
        
        if(GameObject.FindWithTag("Objetos") != null)
            Destroy(GameObject.FindWithTag("Objetos"));
        if (GameObject.FindWithTag("CamarasLaseres") != null)
            Destroy(GameObject.FindWithTag("CamarasLaseres"));
        tutoVisto = false;

        TelemetrySystem.Instance.levelEvent("InicioNivel", GameManager.instance.getLevelNumber());
    }
    public void Reintentar()
    {

        telemetria.levelEvent("Reinicio", getLevelNumber());

        if (actual == "Nivel1")
            Nivel1();
        else if (actual == "Nivel2")
            Nivel2();
        else
            Nivel3();

        if (GameObject.FindWithTag("Objetos") != null)
            Destroy(GameObject.FindWithTag("Objetos"));
        if (GameObject.FindWithTag("CamarasLaseres") != null)
            Destroy(GameObject.FindWithTag("CamarasLaseres"));
        tutoVisto = false;
    }
    public string EligeNivel()
    {
        if (nivel1.terminado && nivel2.terminado)
            return "123";
        else if (nivel1.terminado)
            return "12";
        else 
            return "1";
    }

    public void Partida()
    {
        if (File.Exists("PartidaGuardada"))
        {
            StreamReader entrada = new StreamReader("PartidaGuardada");
            string s;
            s = entrada.ReadLine();
            while (!entrada.EndOfStream)
            {
                if (s.Split(' ')[0] == "Nivel")
                {
                    if (s.Split(' ')[1] == "1")
                        nivel1.terminado = true;

                    else if (s.Split(' ')[1] == "2")
                        nivel2.terminado = true;
                    else if (s.Split(' ')[1] == "3")
                        nivel3.terminado = true;

                }
                else if (s.Split(' ')[0] == "Puntuacion")
                {
                    if (s.Split(' ')[1] == "1")
                        nivel1.puntuacionMaxima = int.Parse(s.Split(' ')[2]);

                    else if (s.Split(' ')[1] == "2")
                        nivel2.puntuacionMaxima = int.Parse(s.Split(' ')[2]);
                    else if (s.Split(' ')[1] == "3")
                        nivel3.puntuacionMaxima = int.Parse(s.Split(' ')[2]);

                }
                
                s = entrada.ReadLine();
                
            }

                if (s.Split(' ')[1] == "1")
                    nivel1.puntuacionMaxima = int.Parse(s.Split(' ')[2]);

                else if (s.Split(' ')[1] == "2")
                    nivel2.puntuacionMaxima = int.Parse(s.Split(' ')[2]);
            else if (s.Split(' ')[1] == "3")
                nivel3.puntuacionMaxima = int.Parse(s.Split(' ')[2]);


            entrada.Close(); 
        }
        
    }
    public void NuevaPartida()
    {
        if (File.Exists("PartidaGuardada"))
        {
            File.Delete("PartidaGuardada");
            nivel1.terminado = false;
            nivel2.terminado = false;
            nivel3.terminado = false;
            nivel1.puntuacionMaxima = 0;
            nivel2.puntuacionMaxima = 0;
            nivel3.puntuacionMaxima = 0;
        }
        primeravez = 0;
    }

    public void GuardaPartida()
    {
        int indiceNivel = int.Parse(actual[actual.Length - 1].ToString());
        StreamWriter salida;
        salida = new StreamWriter("PartidaGuardada");

        if (nivel1.terminado)
        {
            salida.WriteLine("Nivel 1");
            salida.WriteLine("Puntuacion 1 " + nivel1.puntuacionMaxima);
        }
        if (nivel2.terminado)
        {
            salida.WriteLine("Nivel 2");
            salida.WriteLine("Puntuacion 2 " + nivel2.puntuacionMaxima);
        }
        if (nivel3.terminado)
        {
            salida.WriteLine("Nivel 3");
            salida.WriteLine("Puntuacion 3 " + nivel3.puntuacionMaxima);
        }
        salida.Close();

    }
    
	public void SetPause(bool pause)
	{
		gameIsPaused = pause;
	}

	public bool GameIsPaused()
	{
		return gameIsPaused;
	}

    //Tutorial cámaras
    public void VistoTutorial()
    {
        tutoVisto = true;
    }

    //Devuelve true si ya se ha visto el tutorial
    public bool CheckTutorial()
    {
        return tutoVisto;
    }

    //Posibilitar hacer pausa
    public void SetAllowPausa(bool allow)
    {
        allowPausa = allow;
    }

    public bool AllowPausa()
    {
        return allowPausa;
    }

    //Posibilitar hacer flash
    public void SetPuedeFlash(bool allow)
    {
        puedeFlash = allow;
    }

    public bool PuedeFlash()
    {
        return puedeFlash;
    }

    public float tiempoMinijuego()
    {
        return GameObject.FindWithTag("Crono").GetComponent<Cronometro>().TiempoAntes();
    }
}