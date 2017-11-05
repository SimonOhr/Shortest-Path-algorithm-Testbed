﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortest_Path_Algorithm
{
    class Rectangle
    {
        Texture2D tex;
        Vector2 pos;
        Color color;
        private bool active;
        private bool inProgress;
        private bool isTarget;
        private bool isPath;
        public Rectangle(Texture2D tex, Vector2 pos)
        {
            this.pos = pos;
            this.tex = tex;
            color = Color.White;
        }
        public void Update()
        {
            if (active && !isPath)
            {
                color = Color.Blue;
                inProgress = false;
            }
            if (inProgress && !active && !isPath)
            {
                color = Color.Red;
            }
            if (isPath == true)
            {
                color = Color.Green;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, color);
        }

        public void SetActive()
        {
            color = Color.Blue;
            active = true;           
        }

        public void SetInProgress()
        {
            //color = Color.Red;
            inProgress = true;
        }
        public void SetTarget()
        {
            color = Color.Gold;
            isTarget = true;
        }
        public void SetPath()
        {
            color = Color.Green;
            isPath = true;      
        }
    }
}
