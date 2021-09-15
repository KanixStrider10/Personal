using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RnadomMapGenerationExperiment {
    class DamagePickup : Pickup{
        private Color color = Color.DarkGreen;
        private Vector2 position;

        public DamagePickup(Vector2 _position) : base(_position) {
            position = _position;
        }

        override public void UsePickup(Character player) {
            player.Damage += 5;
            player.PickupColors.Add(color);
        }


        public override void Draw() {
            ShapeBatch.CircleOutline(position, 20, color);
        }
    }
}
