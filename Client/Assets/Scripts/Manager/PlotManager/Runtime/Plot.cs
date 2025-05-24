using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using LitJson;
using System;

namespace PlotDesigner.Runtime
{
    public class Plot
    {
        public List<Track> trackDataList;

        public void Init()
        {
            if (null == trackDataList)
            {
                trackDataList = new List<Track>();
            }

            for (int i = 0; i < trackDataList.Count; i++)
            {
                var track = trackDataList[i];
                track.Init();
            }
        }


        public void AddTrack(Track track)
        {
            trackDataList.Add(track);
        }

        public void DeleteTrack(Track t)
        {
            trackDataList.Remove(t);
        }


        public void AddTrackNode(Track trackData, TrackNode insObj)
        {
            trackData.AddTrackNode(insObj);
        }


        public void DeleteTrackNode(Track trackData, TrackNode insObj)
        {
            trackData.DeleteTrackNode(insObj);
        }

        //--------------will del 

        public void Load(string json)
        {
            //填充 track list

        }


        public void Start()
        {

        }

        public void Update(float currTime)
        {

        }

        public void End()
        {

        }

    }

}
