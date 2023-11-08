using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
    public struct Tile
    {
        public bool canHaveGem;
        public Point position;
        public Gem gem;
    }
}
