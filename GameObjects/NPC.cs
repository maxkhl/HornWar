using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Horn_War_II.GameObjects
{
    /// <summary>
    /// Non-player-character this is a AI controlled character
    /// </summary>
    class NPC : Character
    {
        /// <summary>
        /// Gets or sets a value indicating whether [this npc is hostile towards everybody].
        /// </summary>
        public bool DefaultHostile { get; set; }

        /// <summary>
        /// Gets or sets the view distance.
        /// </summary>
        public float ViewDistance { get; set; }

        /// <summary>
        /// State of this NPC
        /// </summary>
        public MovementState State { get; private set; }

        /// <summary>
        /// Target position the npc is moving towards
        /// </summary>
        public Vector2 GoTo 
        { 
            get
            {
                return _GoTo;
            }
            set
            {
                _GoTo = value;
                LookAt = value;
            }
        }
        private Vector2 _GoTo;

        /// <summary>
        /// Roam-mode will pick random points only inside this area
        /// </summary>
        public Rectangle RoamArea { get; set; }

        /// <summary>
        /// Will enforce roamarea behaviour on any situation - even if in combat, targets will not be chased if outside of the area
        /// </summary>
        public bool ForceRoamArea { get; set; }

        /// <summary>
        /// Defines NPC movement behaviour
        /// </summary>
        public enum MovementState
        {
            Wait,
            Roam,
            Move,
            Charge,
            Fallback,
        }

        /// <summary>
        /// Equipted weapon
        /// </summary>
        public override Weapon Weapon
        {
            get
            {
                return base.Weapon;
            }
            set
            {
                if (base.Weapon != null)
                    base.Weapon.onHitCharacter -= Weapon_onHitCharacter;

                base.Weapon = value;

                base.Weapon.onHitCharacter += Weapon_onHitCharacter;
            }
        }

        /// <summary>
        /// Defines NPCs difficulty
        /// </summary>
        public DifficultyType Difficulty { get; set; }

        /// <summary>
        /// NPC difficulty
        /// </summary>
        public enum DifficultyType
        {
            Ez,
            Wtf,
        }

        private List<Character> Enemies = new List<Character>();
        private Character BiggestThreat;


        public NPC(Scenes.GameScene GameScene, PhysicEngine PhysicEngine, SkinType Skin)
            : base(GameScene, PhysicEngine, Skin)
        {
            this.ViewDistance = 500;
            this.Difficulty = DifficultyType.Wtf;
            this.State = MovementState.Roam;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var closeCharacters = GetVisibleCharacters();
            if(DefaultHostile)
            {
                foreach(var character in closeCharacters)
                {
                    if (!Enemies.Contains(character))
                        Enemies.Add(character);
                }
            }

            float[] ThreatLevel = new float[Enemies.Count];
            int count = 0;
            foreach(var enemy in Enemies)
            {
                if(closeCharacters.Contains(enemy))
                {
                    var distance = (enemy.Position - this.Position).Length();
                    var facing = Math.Abs(Angle(enemy.Body.LinearVelocity, enemy.Position - this.Position));
                    var Speed = enemy.Body.LinearVelocity.Length();
                    


                    ThreatLevel[count] = (180 - facing) + (Speed * 100) + (distance * -1);

                    // No threat if roamarea forced and target outside of it
                    // This will ensure the npc stays inside its box if required
                    if (ForceRoamArea && RoamArea != null &&
                        !RoamArea.Contains(enemy.Position))
                        ThreatLevel[count] = -999;
                }
                count++;
            }

            float BiggestThreatLevel = -1;
            for(int i = 0; i < ThreatLevel.Length; i++)
            {
                if(ThreatLevel[i] > BiggestThreatLevel &&
                    ThreatLevel[i] >= 0)
                {
                    BiggestThreatLevel = ThreatLevel[i];
                    BiggestThreat = Enemies[i];
                }
            }

            if (BiggestThreat != null)
            {
                this.Speed = WalkSpeed.Full;
                float Distance = (BiggestThreat.Position - this.Position).Length();

                if (this.State != MovementState.Fallback)
                {
                    if (Distance > 300)
                        this.State = MovementState.Move;
                    else if (Distance < 300 && Distance > 80)
                        this.State = MovementState.Charge;
                }
                else if (Distance > 400)
                    this.State = MovementState.Move;

                if (this.State == MovementState.Fallback)
                {
                    this.GoTo = (BiggestThreat.Position - this.Position) * -1 + this.Position;
                }
                else
                {
                    if (this.Difficulty == DifficultyType.Wtf)
                        this.GoTo = BiggestThreat.Position +
                                    FarseerPhysics.ConvertUnits.ToDisplayUnits(BiggestThreat.Body.LinearVelocity);
                    else
                        this.GoTo = BiggestThreat.Position;
                }
            }
            else if (this.State == MovementState.Roam && (this.Position - this.GoTo).Length() < 50)
            {
                this.Speed = WalkSpeed.Slow;

                var rnd = new Random();

                bool clearPath = false;

                int Timeout = 10;
                while (!clearPath)
                {
                    if (RoamArea != null)
                        this.GoTo = new Vector2(rnd.Next(RoamArea.X, RoamArea.Width), rnd.Next(RoamArea.Y, RoamArea.Height));
                    else
                        this.GoTo = new Vector2(this.Position.X + rnd.Next(-300, 300), this.Position.Y + rnd.Next(-300, 300));

                    var hits = PhysicEngine.World.RayCast(FarseerPhysics.ConvertUnits.ToSimUnits(this.Position), FarseerPhysics.ConvertUnits.ToSimUnits(this.GoTo));
                    bool Obstacle = false;
                    foreach (var fixture in hits)
                        if (fixture.Body.UserData != this && fixture.Body.UserData != this.Weapon)
                            Obstacle = true;

                    clearPath = !Obstacle;

                    Timeout--;
                    if (Timeout <= 0)
                        clearPath = true;
                }
                this.Color = Color.White;
            }
            else
                this.State = MovementState.Roam; //Roam, if nothing todo

            switch (this.State)
            {
                case MovementState.Move:
                    this.Color = new Color(255, 180, 180);
                    break;
                case MovementState.Fallback:

                    break;
                case MovementState.Charge:
                    this.Color = new Color(255, 150, 150);
                    break;
                case MovementState.Roam:

                    break;
            }

            if (this.State == MovementState.Charge)
                this.Boost = true;
            else
                this.Boost = false;

            if(this.State != MovementState.Wait)
                this.Move(Difficulty == DifficultyType.Ez ? this.Forward : this.GoTo - this.Position +
                                    (FarseerPhysics.ConvertUnits.ToDisplayUnits(this.Body.LinearVelocity) * -1));

            base.Update(gameTime);
        }


        void Weapon_onHitCharacter(Character Target)
        {
            if (Target == BiggestThreat)
                this.State = MovementState.Fallback;
        }


        public override void Hit(BodyObject Contact, bool DamagingImpact, float Damage)
        {
            if (DamagingImpact)
            {
                // NPC got attacked by weapon
                if (typeof(Weapon).IsAssignableFrom(Contact.GetType()) && ((Weapon)Contact).Holder != null)
                {
                    if (!Enemies.Contains(((Weapon)Contact).Holder))
                    {
                        Enemies.Add(((Weapon)Contact).Holder);
                        SayRandom(new string[5] { "Fool!", "You'll regret this!", "Don't touch me!", "I'll fuck yo shit up!", "Eh.." });
                    }
                    else
                        SayRandom(new string[5] { "Ouch", "Shit", "Fuck", "Stop it", "Ah" });

                }
            }

            base.Hit(Contact, DamagingImpact, Damage);
        }

        private float Angle(Vector2 Forward, Vector2 Position)
        {
            return (float)Math.Atan2(Forward.Y - Position.Y, Forward.X - Position.X);
        }

        /// <summary>
        /// Returns all visible characters
        /// </summary>
        public List<Character> GetVisibleCharacters()
        {
            return (from component in Game.Components
                    where
                        component != this &&
                        typeof(Character).IsAssignableFrom(component.GetType()) &&
                        (((Character)component).Position - this.Position).Length() < this.ViewDistance
                    select
                        (Character)component).ToList();
        }
    }
}
