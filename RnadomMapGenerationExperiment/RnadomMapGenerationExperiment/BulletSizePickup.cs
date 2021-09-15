using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace RnadomMapGenerationExperiment {
    class BulletSizePickup : Pickup{
        private Color color = Color.HotPink;
        private Vector2 position;

        public BulletSizePickup(Vector2 _position) : base(_position) {
            position = _position;
        }

        override public void UsePickup(Character player) {
            player.BulletSize += 2;
            player.PickupColors.Add(color);
        }

        public override void Draw() {
            ShapeBatch.CircleOutline(position, 20, color);
        }
    }
}
