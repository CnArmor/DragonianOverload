using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace Dragonian
{
    public class LordJob_WildDragonianTribe : LordJob
    {
        public LordJob_WildDragonianTribe()
        {
        }
        public LordJob_WildDragonianTribe(IntVec3 gatherSpot, int stayDuration)
        {
            this.gatherSpot = gatherSpot;
            this.stayDuration = stayDuration;
            if (lord.ownedPawns.Count >= 4)
                casualtyBeforeRun = 2;
            else
                casualtyBeforeRun = 1;
        }
        public override StateGraph CreateGraph()
        {
            StateGraph stateGraph = new StateGraph();
            //start by go to a spot
            LordToil startingToil = new LordToil_Travel(gatherSpot);
            stateGraph.StartingToil = startingToil;
            //wander around the spot
            LordToil wildDragonianGather = new LordToil_WildDragonianGather(gatherSpot);
            stateGraph.AddToil(wildDragonianGather);
            //exit map normally
            LordToil exitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
            stateGraph.AddToil(exitMap);
            //retaliate when harmed     Why you do this?
            LordToil assultColony = new LordToil_AssaultColony(false, true);
            stateGraph.AddToil(assultColony);
            //flee when killed          YOU MONSTER!
            LordToil exitMapFighting = new LordToil_ExitMapFighting(LocomotionUrgency.Sprint, false, true);
            stateGraph.AddToil(exitMapFighting);
            //break out and run when encaged
            LordToil letMeOut = new LordToil_ExitMap(LocomotionUrgency.Jog, true, true);
            stateGraph.AddToil(letMeOut);

            Transition arriveSpot = new Transition(startingToil, wildDragonianGather, false, true);
            arriveSpot.AddTrigger(new Trigger_Memo("TravelArrived"));
            stateGraph.AddTransition(arriveSpot);

            Transition leavePeacefully = new Transition(wildDragonianGather, exitMap, false, true);
            leavePeacefully.AddTrigger(new Trigger_TicksPassedWithoutHarm(stayDuration));
            leavePeacefully.AddTrigger(new Trigger_PawnExperiencingDangerousTemperatures());
            leavePeacefully.AddPreAction(new TransitionAction_Message("DragonianTribeLeaving".Translate()));
            leavePeacefully.AddPostAction(new TransitionAction_WakeAll());
            leavePeacefully.AddPostAction(new TransitionAction_EndAllJobs());
            stateGraph.AddTransition(leavePeacefully);

            Transition retaliateWhenHarmed = new Transition(wildDragonianGather, assultColony, false, true);
            retaliateWhenHarmed.AddSources(new LordToil[]
            {
                startingToil,
                letMeOut
            });
            retaliateWhenHarmed.AddTrigger(new Trigger_PawnHarmed(0.5f, true, Find.FactionManager.OfPlayer));
            retaliateWhenHarmed.AddTrigger(new Trigger_PawnLostViolently(true));
            retaliateWhenHarmed.AddPreAction(new TransitionAction_Message("DragonianTribeRetaliate".Translate(), MessageTypeDefOf.NegativeEvent));
            retaliateWhenHarmed.AddPostAction(new TransitionAction_WakeAll());
            retaliateWhenHarmed.AddPostAction(new TransitionAction_EndAllJobs());
            retaliateWhenHarmed.AddPostAction(new TransitionAction_GoMad());
            
            stateGraph.AddTransition(retaliateWhenHarmed);

            Transition runWhenKilled = new Transition(wildDragonianGather, exitMapFighting, false, true);
            runWhenKilled.AddSources(new LordToil[]
            {
                startingToil,
                assultColony,
                letMeOut
            });
            runWhenKilled.AddPreAction(new TransitionAction_Message("DragonianTribeFlee".Translate()));
            runWhenKilled.AddTrigger(new Trigger_PawnsLost(casualtyBeforeRun));
            runWhenKilled.AddPreAction(new TransitionAction_OutOfManhunter());
            runWhenKilled.AddPostAction(new TransitionAction_WakeAll());
            runWhenKilled.AddPostAction(new TransitionAction_EndAllJobs());
            stateGraph.AddTransition(runWhenKilled);

            Transition runWhenEncaged = new Transition(wildDragonianGather, letMeOut, false, true);
            runWhenEncaged.AddSource(startingToil);
            runWhenEncaged.AddPreAction(new TransitionAction_Message("DragonianTribeBreakOut".Translate()));
            runWhenEncaged.AddTrigger(new Trigger_PawnCannotReachMapEdge());
            stateGraph.AddTransition(runWhenEncaged);

            return stateGraph;
        }
        public override void ExposeData()
        {
            Scribe_Values.Look<IntVec3>(ref gatherSpot, "gatherSpot");
            Scribe_Values.Look<int>(ref stayDuration, "stayDuration");
        }
        private IntVec3 gatherSpot;
        private int stayDuration;
        private int casualtyBeforeRun;

    }
}