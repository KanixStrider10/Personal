using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ApollonianGasketVariation {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private Circle bigCircle;
        private Circle circle1;
        private Circle circle2;
        private Circle inversionCircle;

        private Line bigCircleInv;
        private Line circle1Inv;
        private Circle circle2Inv;

        bool showInversionCircle = false;

        KeyboardState pv;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 800;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 800;   // set this value to the desired height of your window
            _graphics.ApplyChanges();

            bigCircle = new Circle(new Vector2(400, 400), 400);
            circle1 = new Circle(new Vector2(400, 600), 200);
            circle2 = new Circle(new Vector2(400, 200), 200);
            inversionCircle = new Circle(new Vector2(0, 0), 0);

            bigCircleInv = new Line(new Vector2(0, 0), new Vector2(20, 10));
            circle1Inv = new Line(new Vector2(0, 0), new Vector2(10, 10));

            SetStartingCircles();
            SetInversionLines();
            SetFirstInvertedCircle();

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }
        
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed) {
                if(Math.Sqrt(Math.Pow(400 - mouseState.X, 2) + Math.Pow(400 - mouseState.Y, 2)) < 400) {
                    circle1.CenterX = mouseState.X;
                    circle1.CenterY = mouseState.Y;

                    SetStartingCircles();
                    SetInversionLines();
                    SetFirstInvertedCircle();
                }
            }

            if (KeyPressedThisFrame(Keys.I, keyboardState)) {
                if (showInversionCircle) {
                    showInversionCircle = false;
                }
                else {
                    showInversionCircle = true;
                }
            }
            
            pv = keyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            ShapeBatch.Begin(GraphicsDevice); 
            
            bigCircle.Draw(Color.White);

            circle1.Draw(Color.White);
            circle2.Draw(Color.White);

            if (showInversionCircle) {
                inversionCircle.Draw(Color.BlueViolet);

                bigCircleInv.Draw(Color.Green);
                circle1Inv.Draw(Color.Green);
                circle2Inv.Draw(Color.Green);
            }

            CreateStep(1);
            ShapeBatch.End();

            base.Draw(gameTime);
        }

        public bool KeyPressedThisFrame(Keys k, KeyboardState ks) {
            return ks.IsKeyDown(k) && !pv.IsKeyDown(k);
        }

        public void SetStartingCircles() {
            float C1distanceFromCenter = (float)Math.Sqrt(Math.Pow(400 - circle1.CenterX,2) + Math.Pow(400 - circle1.CenterY,2));
            circle1.Radius = 400 - C1distanceFromCenter;
            circle2.Radius = (800 - (2 * circle1.Radius))/2;
            float C2distanceFromCenter = (2 * circle1.Radius) + circle2.Radius - 400;

            //Find Circle2
            float ratioOfCenteralDistances = C2distanceFromCenter/C1distanceFromCenter;
            float C2X = 400 + (ratioOfCenteralDistances * (400 - circle1.CenterX));
            float C2Y = 400 + (ratioOfCenteralDistances * (400 - circle1.CenterY));

            circle2.Center = new Vector2(C2X, C2Y);

            //Find Circle of Inversion
            inversionCircle.Radius = circle1.Radius * 2;
            ratioOfCenteralDistances = 400/C1distanceFromCenter;
            float CIX = 400 - (ratioOfCenteralDistances * (400 - circle1.CenterX));
            float CIY = 400 - (ratioOfCenteralDistances * (400 - circle1.CenterY));
            inversionCircle.Center = new Vector2(CIX, CIY);
        }

        public void SetInversionLines() {
            //Finds points of intersection between big circle and inversion circle
            //Create line from these two points

            float x1 = bigCircle.CenterX;
            float y1 = bigCircle.CenterY;
            float x2 = inversionCircle.CenterX;
            float y2 = inversionCircle.CenterY;
            float r1 = bigCircle.Radius;
            float r2 = inversionCircle.Radius;

            float r1sq = (float)Math.Pow(r1, 2);
            float r2sq = (float)Math.Pow(r2, 2);
            float x1sq = (float)Math.Pow(x1, 2);
            float y1sq = (float)Math.Pow(y1, 2);
            float x2sq = (float)Math.Pow(x2, 2);
            float y2sq = (float)Math.Pow(y2, 2);

            float m = -(x1 - x2)/(y1 - y2);
            float yintercept = ((x1sq - x2sq) + (y1sq - y2sq) - (r1sq - r2sq))/(2 * (y1 - y2));

            bigCircleInv = new Line(new Vector2(0, yintercept), m);

            //Find point of intersection of the two inner circles
            //Create line through this point with the same slope as line1

            x2 = circle1.CenterX;
            y2 = circle1.CenterY;
            r2 = circle1.Radius;

            float distanceToCenter = (float)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
            float ratioDTCtoRadius = r2/distanceToCenter;
                        
            float xpoint = x2 + ratioDTCtoRadius * (x1 - x2);
            float ypoint = y2 + ratioDTCtoRadius * (y1 - y2);

            circle1Inv = new Line(new Vector2(xpoint, ypoint), bigCircleInv.Slope);
        }

        public void SetFirstInvertedCircle() {
            float b1 = circle1Inv.getYIntercept().Y;
            float b2 = bigCircleInv.getYIntercept().Y;
            float distanceBetweenLines = (float)(Math.Abs(b2 - b1)/Math.Sqrt((circle1Inv.Slope * circle1Inv.Slope) + 1));

            float distanceFromCircle2ToCenter = (float)Math.Sqrt(Math.Pow(circle2.CenterX - bigCircle.CenterX, 2) 
                + Math.Pow(circle2.CenterY - bigCircle.CenterY, 2));
            float distanceFromCircle2ToCircle2Inv = circle2.Radius + (distanceBetweenLines / 2);
            float distanceFromCircle2InvToCenter = distanceFromCircle2ToCenter - distanceFromCircle2ToCircle2Inv;
            float ratioOfDistances = distanceFromCircle2InvToCenter/distanceFromCircle2ToCenter;

            float xpoint = bigCircle.CenterX + (ratioOfDistances * (circle2.CenterX - bigCircle.CenterX));
            float ypoint = bigCircle.CenterY + (ratioOfDistances * (circle2.CenterY - bigCircle.CenterY));

            Vector2 circle2InvCenter = new Vector2(xpoint, ypoint);

            circle2Inv = new Circle(circle2InvCenter, distanceBetweenLines / 2);
        }

        public void CreateStep(int stepNum) {
            Circle firstDegreeInvertedCirclePos;
            Circle firstDegreeInvertedCircleNeg;
            float fdicRadius = circle2Inv.Radius;
            Vector2 fdicCenterPos;
            Vector2 fdicCenterNeg;
            float slopeOfTranslation = circle1Inv.Slope;

            float yinterceptOfRadialLine = circle2Inv.CenterY - slopeOfTranslation * (circle2Inv.CenterX);
            float distanceToYAxis = (float)Math.Sqrt(Math.Pow(circle2Inv.CenterX, 2) + Math.Pow(circle2Inv.CenterY - yinterceptOfRadialLine, 2));
            float ratioOfDistances = (2 * stepNum * fdicRadius)/distanceToYAxis;

            float xpoint = circle2Inv.CenterX + (ratioOfDistances * circle2Inv.CenterX);
            float ypoint = circle2Inv.CenterY + (ratioOfDistances * (circle2Inv.CenterY - yinterceptOfRadialLine));

            fdicCenterPos = new Vector2(xpoint, ypoint);

            xpoint = circle2Inv.CenterX - (ratioOfDistances * circle2Inv.CenterX);
            ypoint = circle2Inv.CenterY - (ratioOfDistances * (circle2Inv.CenterY - yinterceptOfRadialLine));

            fdicCenterNeg = new Vector2(xpoint, ypoint);

            firstDegreeInvertedCirclePos = new Circle(fdicCenterPos, fdicRadius);
            firstDegreeInvertedCircleNeg = new Circle(fdicCenterNeg, fdicRadius);
            if (showInversionCircle) {
                firstDegreeInvertedCirclePos.Draw(Color.Green);
                firstDegreeInvertedCircleNeg.Draw(Color.Green);
            }

            Circle firstDegreeCirclePos = InvertCircleOverCircle(firstDegreeInvertedCirclePos, inversionCircle);
            Circle firstDegreeCircleNeg = InvertCircleOverCircle(firstDegreeInvertedCircleNeg, inversionCircle);
            firstDegreeCirclePos.Draw(Color.MediumVioletRed);
            firstDegreeCircleNeg.Draw(Color.MediumVioletRed);

            if(stepNum == 50) {
                return;
            }
            CreateStep(stepNum + 1);
        }

        public Circle InvertCircleOverCircle(Circle c, Circle ic) {
            //Invert three points over the circle to create the new circle
            float radius;
            Vector2 center;

            Vector2 point1Inv = new Vector2(c.CenterX, c.CenterY + c.Radius);
            Vector2 point2Inv = new Vector2(c.CenterX, c.CenterY - c.Radius);
            Vector2 point3Inv = new Vector2(c.CenterX - c.Radius, c.CenterY);
            Vector2 point1 = InvertPointOverCircle(point1Inv, ic);
            Vector2 point2 = InvertPointOverCircle(point2Inv, ic);
            Vector2 point3 = InvertPointOverCircle(point3Inv, ic);
            
            Vector2 midpoint12 = new Vector2((point1.X + point2.X)/2, (point1.Y + point2.Y)/2);
            Vector2 midpoint13 = new Vector2((point1.X + point3.X)/2, (point1.Y + point3.Y)/2);

            float perpSlope12 = -(point2.X - point1.X)/(point2.Y - point1.Y);
            float perpSlope13 = -(point3.X - point1.X)/(point3.Y - point1.Y);

            float xpoint = (midpoint13.Y - midpoint12.Y - (perpSlope13 * midpoint13.X) + (perpSlope12 * midpoint12.X))/(perpSlope12 - perpSlope13);
            float ypoint = perpSlope12 * (xpoint - midpoint12.X) + midpoint12.Y;
            center = new Vector2(xpoint, ypoint);

            radius = (float)Math.Sqrt(Math.Pow(xpoint - point1.X, 2) + Math.Pow(ypoint - point1.Y, 2));

            return new Circle(center, radius);
        }

        public Vector2 InvertPointOverCircle(Vector2 p, Circle ic) {
            //(op) * (op') = r^2
            //op' = r^2 / op
            Vector2 invertedPoint = new Vector2();
            Vector2 opV = new Vector2(p.X - ic.CenterX, p.Y - ic.CenterY);

            float rSquared = ic.Radius * ic.Radius;
            float op = (float)Math.Sqrt(Math.Pow(opV.X, 2) + Math.Pow(opV.Y, 2));
            float opPrime = rSquared / op;

            Vector2 n = opV / op;

            invertedPoint.X = ic.CenterX + (opPrime * n.X);
            invertedPoint.Y = ic.CenterY + (opPrime * n.Y);

            return invertedPoint;
        }
    }
}
