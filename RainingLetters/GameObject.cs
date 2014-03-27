using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace RainingLetters
{
    class GameObject
    {
      
        public Vector2 position;  
        public Vector2 velocity;
        public bool alive;
        public char alphabet;

        public GameObject()
        {
            position = Vector2.Zero;
            velocity = Vector2.Zero;
            alive = false;
        }
    }
}
