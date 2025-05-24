using NetProto;
using System;
using System.Collections.Generic;
using UnityEngine;

static public class EventDispatcher
{
    //custom event///////////////////////////////////////////////////////////////////
    static public void AddListener(EventIDs eventEnums, Callback handler)
    {
        var eventId = (int)eventEnums;
        EventManager.AddListener(eventId, handler);
    }

    static public void AddListener<T>(EventIDs eventEnums, Callback<T> handler)
    {
        var eventId = (int)eventEnums;
        //Debug.Log("add evenType : " + eventType);
        EventManager.AddListener(eventId, handler);
    }

    static public void AddListener<T,K>(EventIDs eventEnums, Callback<T,K> handler)
    {
        var eventId = (int)eventEnums;
        //Debug.Log("add evenType : " + eventType);
        EventManager.AddListener(eventId, handler);
    }


    static public void RemoveListener(EventIDs eventEnums, Callback handler)
    {
        var eventId = (int)eventEnums;
        EventManager.RemoveListener(eventId, handler);
    }

    static public void RemoveListener<T>(EventIDs eventEnums, Callback<T> handler)
    {
        var eventId = (int)eventEnums;
        EventManager.RemoveListener(eventId, handler);
    }

    static public void RemoveListener<T,K>(EventIDs eventEnums, Callback<T,K> handler)
    {
        var eventId = (int)eventEnums;
        EventManager.RemoveListener(eventId, handler);
    }

    static public void Broadcast(EventIDs eventEnums)
    {
        // if ((int)eventEnums >= 101001 && eventEnums <= EventIDs._Battle_Flag_End)
        // {
        //     Logx.Log(LogxType.Battle," Broadcast id : " + eventEnums);
        // }

        var eventId = (int)eventEnums;
        EventManager.Broadcast(eventId);
    }

    static public void Broadcast<T>(EventIDs eventEnums, T arg1)
    {
        // if ((int)eventEnums >= 101001 && eventEnums <= EventIDs._Battle_Flag_End)
        // {
        //     Logx.Log(LogxType.Battle," Broadcast id : " + eventEnums);
        // }
        
        var eventId = (int)eventEnums;
        EventManager.Broadcast(eventId, arg1);
    }

    static public void Broadcast<T,K>(EventIDs eventEnums, T arg1,K arg2)
    {
        // if ((int)eventEnums >= 101001 && eventEnums <= EventIDs._Battle_Flag_End)
        // {
        //     Logx.Log(LogxType.Battle," Broadcast id : " + eventEnums);
        // }
        
        var eventId = (int)eventEnums;
        EventManager.Broadcast(eventId, arg1, arg2);
    }


    //proto  event///////////////////////////////////////////////////////////////////
    static public void AddListener(ProtoIDs eventEnums, Callback handler)
    {
        var eventId = (int)eventEnums;
        EventManager.AddListener(eventId, handler);
    }

    static public void AddListener<T>(ProtoIDs eventEnums, Callback<T> handler)
    {
        var eventId = (int)eventEnums;
        //Debug.Log("add evenType : " + eventType);
        EventManager.AddListener(eventId, handler);
    }


    static public void RemoveListener(ProtoIDs eventEnums, Callback handler)
    {
        var eventId = (int)eventEnums;
        EventManager.RemoveListener(eventId, handler);
    }

    static public void RemoveListener<T>(ProtoIDs eventEnums, Callback<T> handler)
    {
        var eventId = (int)eventEnums;
        EventManager.RemoveListener(eventId, handler);
    }

    static public void Broadcast(ProtoIDs eventEnums)
    {
        var eventId = (int)eventEnums;
        EventManager.Broadcast(eventId);
    }

    static public void Broadcast<T>(ProtoIDs eventEnums, T arg1)
    {
        var eventId = (int)eventEnums;
        EventManager.Broadcast(eventId, arg1);
    }



}
