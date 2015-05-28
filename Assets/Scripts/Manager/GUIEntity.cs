using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Manager
{
    public class GUIEntity
    {
        public int currentInputType
        {
            get;
            set;
        }
        public bool displayHelp
        {
            get;
            set;
        }

        public Text[] textGui
        {
            get;
            set;
        }
        new public string name
        {
            get;
            set;
        }
        public Slider healthBar
        {
            get;
            set;
        }
        public float endTime
        {
            get;
            set;
        }

    }
}
