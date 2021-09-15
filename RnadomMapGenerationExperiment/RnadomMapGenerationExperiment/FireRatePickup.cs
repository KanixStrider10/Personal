using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RnadomMapGenerationExperiment {
    class FireRatePickup : Pickup{
        private Color color = Color.Ivory;
        private Vector2 position;

        public FireRatePickup(Vector2 _position) : base(_position) {
            position = _position;
        }

        override public void UsePickup(Character player) {
            player.FireRate -= 3;
            if(player.FireRate < 3) {
                player.FireRate = 3;
            }
            player.PickupColors.Add(color);
        }

        public override void Draw() {
            ShapeBatch.CircleOutline(position, 20, color);
        }
    }
}
