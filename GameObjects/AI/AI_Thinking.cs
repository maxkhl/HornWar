using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Horn_War_II.GameObjects.AI
{
    /// <summary>
    /// This partial class handles the thought-process of the AI.
    /// That contains target spotting, target prioritization and choosing a strategy
    /// </summary>
    partial class AI
    {
        /// <summary>
        /// Random generator
        /// </summary>
        private Random RandomGenerator { get; set; }

        /// <summary>
        /// Initializes AI-Thinking
        /// </summary>
        private void InitializeThinking()
        {
            this.RandomGenerator = new Random();
            this.Character.OnWeaponHit += Character_OnWeaponHit;
            this.Character.OnHit += Character_OnHit;
        }


        /// <summary>
        /// Processes the thinking of this AI
        /// </summary>
        /// <param name="gameTime"></param>
        private void ProcessThinking(GameTime gameTime)
        {
            var enemies = GetEnemies();

            // Enemies are close/visible
            if (enemies != null && enemies.Count > 0)
            {
                // Order enemies by possible threat
                enemies.OrderBy((enemy) =>
                {
                    var distance = (enemy.Position - this.Character.Position).Length();
                    var facing = Math.Abs(Angle(enemy.Body.LinearVelocity, enemy.Position - this.Character.Position));
                    var Speed = enemy.Body.LinearVelocity.Length();

                    return (int)((180 - facing) + (Speed * 100) + (distance * -1));
                });

                // High-value-target is first one in list (most likely to be a threat to us)
                var hvt = enemies[0];
                var hvt_distance = (hvt.Position - this.Character.Position).Length();

                if(this.State != AIState.Fallback && this.State != AIState.Charge)
                    this.State = AIState.Charge;

                //if (this.Options.Difficulty == AIOptions.DifficultyType.Wtf)
                if (this.State == AIState.Fallback && hvt_distance > 400)
                    this.State = AIState.Charge; //Attack again if we have enough distance
                //else if (this.State == AIState.Charge && hvt_distance < 140)
                //    this.State = AIState.Fallback; //Fallback when we're to close to get some momentum

                /*var forward = (hvt.Position - this.Character.Position);
                forward.Normalize();
                forward = new Vector2(forward.Y, -forward.X);*/

                //hvt +
                //   FarseerPhysics.ConvertUnits.ToDisplayUnits(hvt.Body.LinearVelocity - Vector2.Reflect(hvt.Body.LinearVelocity, forward))
                if (this.State == AIState.Charge)
                    Attack(
                        hvt,
                        200,
                        900,
                        30,
                        Tools.Easing.EaseFunction.CircEaseOut,
                        Tools.Easing.EaseFunction.BackEaseIn);
                else if (this.State == AIState.Fallback)
                    Fallback(hvt);

                //else
                //    this.GoTo = hvt.Position;

            }
            else if(State != AIState.Roam) // No enemies nearby - switch to roam mode
                Roam();
        }

        /// <summary>
        /// Lets the character roam around randomly
        /// </summary>
        private void Roam()
        {
            this.State = AIState.Roam;

            int Timeout = 10;
            bool clearPath = false;

            while (!clearPath)
            {
                if (Options.RoamArea != null)
                    Move(new Vector2(RandomGenerator.Next(Options.RoamArea.X, Options.RoamArea.Width), RandomGenerator.Next(Options.RoamArea.Y, Options.RoamArea.Height)));
                else
                    Move(new Vector2(this.Character.Position.X + RandomGenerator.Next(-300, 300), this.Character.Position.Y + RandomGenerator.Next(-300, 300)));

                var hits = Character.PhysicEngine.World.RayCast(FarseerPhysics.ConvertUnits.ToSimUnits(this.Character.Position), FarseerPhysics.ConvertUnits.ToSimUnits(this.GoTo));
                bool Obstacle = false;
                foreach (var fixture in hits)
                    if (fixture.Body.UserData != this && fixture.Body.UserData != this.Character.Weapon)
                        Obstacle = true;

                clearPath = !Obstacle;

                Timeout--;
                if (Timeout <= 0)
                    clearPath = true;
            }
        }



        /// <summary>
        /// Fired when char reached the goto point
        /// </summary>
        private void AI_GoToArrived(Vector2 GoToPoint)
        {
            switch(this.State)
            {
                // Keep roaming when already roaming and waypoint arrived
                case AIState.Roam:
                    Roam();
                    break;
            }
        }


        /// <summary>
        /// Returns Enemies in our visible range
        /// Can be a resource killer so dont use too often
        /// </summary>
        private List<Character> GetEnemies()
        {
            if (this.Options.Passive)
                return new List<Character>();
            else if (this.Options.DefaultHostile)
                return GetVisibleCharacters();
            else
            {
                var visibleChars = GetVisibleCharacters();

                return (from character in visibleChars
                        where
                            (character.Team != this.Character.Team || // Same team or
                            character.Team == "") && // - no team
                            !character.Dead //Ignore dead characters
                        select
                            character).ToList();
            }
        }

        /// <summary>
        /// Character got hit by something
        /// </summary>
        /// <param name="Contact">Object that hit us</param>
        /// <param name="DamagingImpact">Was impact damaging?</param>
        /// <param name="Damage">Damage we took</param>
        private void Character_OnHit(BodyObject Contact, bool DamagingImpact, float Damage)
        {
            if (DamagingImpact)
            {
                Options.Passive = false;
                // NPC got attacked by weapon
                if (typeof(Weapon).IsAssignableFrom(Contact.GetType()) && ((Weapon)Contact).Holder != null)
                {
                    //if (!Enemies.Contains(((Weapon)Contact).Holder))
                    //{
                    //    Character.SayRandom(new string[5] { "Fool!", "You'll regret this!", "Don't touch me!", "I'll fuck you up!", "Eh.." });
                    //}
                    //else
                    Character.SayRandom(new string[5] { "Ouch", "Shit", "Fuck", "Stop it", "Ah" });

                }
            }
        }

        /// <summary>
        /// Character hit another one with his weapon
        /// </summary>
        /// <param name="Target">Character, that got hit by us</param>
        private void Character_OnWeaponHit(Character Target)
        {
            this.State = AIState.Fallback;
            //if (Target == BiggestThreat)
            //    this.State = AIState.Fallback;
        }

        /// <summary>
        /// Returns all visible characters
        /// - Forced roam area makes characters outside of it invisible
        /// </summary>
        public List<Character> GetVisibleCharacters()
        {
            return (from component in Game.Components
                    where
                        component != this.Character &&
                        typeof(Character).IsAssignableFrom(component.GetType()) &&
                        (((Character)component).Position - this.Character.Position).Length() < this.Options.ViewDistance &&
                            !( 
                                this.Options.RoamArea != Rectangle.Empty && 
                                this.Options.ForceRoamArea && 
                                this.Options.RoamArea.Contains(((Character)component).Position)
                            )
                    select
                        (Character)component).ToList();
        }

        /// <summary>
        /// Disposes thinking-part of the AI
        /// </summary>
        private void DisposeThinking()
        {
            this.Character.OnHit -= Character_OnHit;
            this.Character.OnWeaponHit -= Character_OnWeaponHit;
        }
    }
}
