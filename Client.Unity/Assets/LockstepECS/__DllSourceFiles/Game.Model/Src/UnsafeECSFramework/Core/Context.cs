using System.Collections.Generic;
using Lockstep.Game;
using Lockstep.Logging;
using Lockstep.Math;
using Lockstep.Serialization;
using Lockstep.UnsafeECS;
using NetMsg.Common;

namespace Lockstep.UnsafeECS.Game {
    public unsafe partial class Context {
        public BoidObstacle PlayerInfo =>
            _entities._BoidObstacleAry._EntityAry[GetService<IGlobalStateService>().LocalActorId];

        public BoidObstacle GetPlayerInfo(int localActorId){
            return _entities._BoidObstacleAry._EntityAry[localActorId];
        }

        protected override void OnAwake(){ }

        protected override void OnDestroy(){
            Debug.LogError("Destoryed !");
        }

        protected override void OnProcessInputQueue(byte actorId, InputCmd cmd){
            TempFields.InputCmds[actorId] = cmd;
        }
    }
}