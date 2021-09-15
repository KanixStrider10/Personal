using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RnadomMapGenerationExperiment {
    class SpeedPickup : Pickup {
        private Color color = Color.Yellow;
        private Vector2 position;

        public SpeedPickup(Vector2 _position) : base(_position) {
            position = _position;
        }

        override public void UsePickup(Character player) {
            player.Speed += 2;
            player.PickupColors.Add(color);
        }

        public override void Draw() {
            ShapeBatch.CircleOutline(position, 20, color);
        }
    }
}
