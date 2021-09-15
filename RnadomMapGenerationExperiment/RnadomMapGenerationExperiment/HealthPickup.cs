using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace RnadomMapGenerationExperiment {
    class HealthPickup : Pickup {
        private Color color = Color.OrangeRed;
        private Vector2 position;

        public HealthPickup(Vector2 _position) : base(_position) {
            position = _position;
        }

        override public void UsePickup(Character player) {
            player.Health += player.MaxHealth / 4;
            if(player.Health > player.MaxHealth) {
                player.Health = player.MaxHealth;
            }
            player.PickupColors.Add(color);
        }

        public override void Draw() {
            ShapeBatch.CircleOutline(position, 20, color);
        }
    }
}
