using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RnadomMapGenerationExperiment {
    class BulletSpeedPickup : Pickup{
        private Color color = Color.DarkBlue;
        private Vector2 position;

        public BulletSpeedPickup(Vector2 _position) : base(_position) {
            position = _position;
        }

        override public void UsePickup(Character player) {
            player.BulletSpeed += 2;
            player.PickupColors.Add(color);
        }

        public override void Draw() {
            ShapeBatch.CircleOutline(position, 20, color);
        }
    }
}
