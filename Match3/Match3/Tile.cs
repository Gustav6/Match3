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
        public bool removed;
        public bool canHaveGem;
        public Gem gem;
    }
}
