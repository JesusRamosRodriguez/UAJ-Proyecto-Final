using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Eventos

/*----------------------------------------Eventos ----------------------------------------*/

//public enum TipoEvento
//{
//    //Gestion de nivel :
//    Inicio,
//    Fin,
//    Reinicio,
//    Pausa,
//    Salida,
//    //Recogida :
//    Coleccionable,
//    FotoRecogida,
//    FlashRecogida,
//    //Uso de :
//    BotonCamara,
//    FotoUso,
//    FlashUso,
//    FotoGuardia
//}


////Gestion de Nivel
//public struct Evento
//{
//    public TipoEvento type;
//    public float time;

//    public void setNull() { time = -1.0f; }
//}

//#endregion

//#region Codificacion
//public class Codificacion
//{
//    public string CodeRecogidaInicio(int totalColeccionables, int totalCarretes, int totalFlashes)
//    {
//        string dataCodificada = totalColeccionables.ToString() + '/' + totalCarretes.ToString() + '/' + totalFlashes.ToString();
//        return dataCodificada;
//    }

//    public List<int> DecodeRecogidaInicio(string dataCodificada)
//    {
//        List<int> data = new List<int>();
//        string[] subs = dataCodificada.Split('/');
//        for (int i = 0; i < 3; i++)
//            data.Add(int.Parse(subs[i]));
//        return data;
//    }
//}
//#endregion
//#region Observer
//public class Observer
//{
//    private Codificacion codificador;
//    private RecogidaInfo recogidaInfo;
//    public void receiveEvent (Evento event_)
//    {
//        switch (event_.type)
//        {
//            case TipoEvento.Inicio:
//                List<int> initialValues = codificador.DecodeRecogidaInicio(event_.data);
//                recogidaInfo = new RecogidaInfo(initialValues[0], initialValues[1], initialValues[2]);
//                break;
//            case TipoEvento.Fin:
//                break;
//            case TipoEvento.Reinicio:
//                break;
//            case TipoEvento.Pausa:
//                break;
//            case TipoEvento.Salida:
//                break;
//            case TipoEvento.Coleccionable:
//                break;
//            case TipoEvento.FotoRecogida:
//                break;
//            case TipoEvento.FlashRecogida:
//                break;
//            case TipoEvento.BotonCamara:
//                break;
//            case TipoEvento.FotoUso:
//                break;
//            case TipoEvento.FlashUso:
//                break;
//            case TipoEvento.FotoGuardia:
//                break;
//            default:
//                break;
//        }
//    }
//}

//#endregion

//#region Observable
//public class Observable
//{
//    Observer EventHandler;

//    public void AttachEventHandler ( ref Observer eventHandler_) { EventHandler = eventHandler_; }
//    public virtual void SendEvent (Evento event_)
//    {
//        EventHandler.receiveEvent(event_);
//    }
//}
#endregion