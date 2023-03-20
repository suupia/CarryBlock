//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;

//using System.Collections.Generic;

//#nullable enable

//public class PlayerStateManager
//{
//    public PlayerState State => state;
//    public PlayerController.UnitType SelectedUnitType => selectedUnitType;
//    public PlayerController.UnitType selectedUnitType { get; private set; }
//    PlayerState state;

//    public enum  PlayerState
//    {
//       Init, SelectingUnit, BattlingUnit, ReturningUnit
//    }

//    public PlayerStateManager()
//    {
//        state = PlayerState.Init;
//    }

//    public void ToBattlingUnit(PlayerController.UnitType unitType)
//    {
//        this.selectedUnitType = unitType;
//        this.state = PlayerState.BattlingUnit;
//    }


//}


//namespace FSharpProject.PhaseManager
//{
//    using FSharpProject;

//    public enum Phase
//    {
//        Init,
//        Preparation,
//        Observation,
//        Result,
//    }

//    public interface IPhase
//    {
//        Phase Next { get; }
//    }

//    public class PhaseState : IPhase
//    {
//        private readonly Phase next;

//        public PhaseState(Phase next)
//        {
//            this.next = next;
//        }

//        public Phase Next => next;
//    }

//    public class PhaseManager : EventRegisterable<Phase>
//    {
//        private int wave = 0;
//        private Phase phase = Phase.Init;

//        // Set all phase state
//        private readonly Dictionary<Phase, IPhase> phases =
//            new Dictionary<Phase, IPhase>()
//            {
//                { Phase.Init, new PhaseState(Phase.Preparation) },
//                { Phase.Preparation, new PhaseState(Phase.Observation) },
//                { Phase.Observation, new PhaseState(Phase.Result) },
//                { Phase.Result, new PhaseState(Phase.Preparation) },
//            };

//        public PhaseManager()
//        {
//            this.Register(phase =>
//            {
//                switch (phase)
//                {
//                    case Phase.Init:
//                        wave = 0;
//                        this.phase = Phase.Init;
//                        break;
//                    case Phase.Preparation:
//                        wave++;
//                        break;
//                }
//            });
//        }

//        /// <summary>
//        /// Return current phase
//        /// </summary>
//        public Phase Phase => phase;

//        /// <summary>
//        /// Return wave
//        /// </summary>
//        public int Wave => wave;

//        /// <summary>
//        /// Set next phase
//        /// </summary>
//        public void Next()
//        {
//            phase = phases[phase].Next;
//            this.Trigger(phase);
//        }

//        /// <summary>
//        /// Reset status.
//        /// Phase to Init, wave to 0
//        /// </summary>
//        public void Reset() => this.Trigger(Phase.Init);
//    }


//    public class EventRegisterable<T>
//    {
//        private readonly Event<T> eventValue = new Event<T>();
//        private readonly Dictionary<int, Handler<T>> handlers = new Dictionary<int, Handler<T>>();

//        public event Action<T> Event
//        {
//            add => eventValue.AddHandler(new Handler<T>(e => value(e)));
//            remove => eventValue.RemoveHandler(new Handler<T>(e => value(e)));
//        }

//        /// <summary>
//        /// Register callback.
//        /// Returned id needs to Unregister callback.
//        /// </summary>
//        public int Register(Action<T> action)
//        {
//            var handler = new Handler<T>(e => action(e));
//            eventValue.AddHandler(handler);
//            int hash = handler.GetHashCode();
//            handlers.Add(hash, handler);
//            return hash;
//        }

//        /// <summary>
//        /// Unregister callback. You need id which can get form Register to call this.
//        /// </summary>
//        public void Unregister(int id)
//        {
//            eventValue.RemoveHandler(handlers[id]);
//            handlers.Remove(id);
//        }

//        public void Trigger(T arg) => eventValue.Trigger(arg);
//    }
//}