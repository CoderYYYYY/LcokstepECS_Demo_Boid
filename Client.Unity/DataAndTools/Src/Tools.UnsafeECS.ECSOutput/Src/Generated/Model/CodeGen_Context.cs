
//------------------------------------------------------------------------------    
// <auto-generated>                                                                 
//     This code was generated by Tools.MacroExpansion, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null. 
//     https://github.com/JiepengTan/LockstepEngine                                          
//     Changes to this file may cause incorrect behavior and will be lost if        
//     the code is regenerated.                                                     
// </auto-generated>                                                                
//------------------------------------------------------------------------------  

//Power by ME //src: https://github.com/JiepengTan/ME  

//#define DONT_USE_GENERATE_CODE                                                                 
                                                                                                 
using System.Linq;                                                                               
using Lockstep.Serialization;                                                                    
using System.Runtime.InteropServices;                                                            
using System.Runtime.CompilerServices;                                                            
using System;                                                                                    
using Lockstep.InternalUnsafeECS;                                                               
using System.Collections;                                                                        
using Lockstep.Math;                                                                             
using System.Collections.Generic;                                                                
using Lockstep.Logging;                                                                          
using Lockstep.Util;                                                                          
namespace Lockstep.UnsafeECS.Game {  
    using Lockstep.Game;    
    using NetMsg.Common;

    public unsafe partial class TempFields {
        private Context _context;
        public Dictionary<int, InputCmd> InputCmds = new Dictionary<int, InputCmd>();

        public TempFields(Context context){
            this._context = context;
        }
    }

    public unsafe partial class Context : BaseContext {
        private static Context _instance;
        public static Context Instance {
            get => _instance ?? (_instance = new Context());
            set => _instance = value;
        }
        public bool HasInit = false;
        public TempFields TempFields{ get;private set;}

        public _EntityManager _entities = new _EntityManager();
        public IEntityService _entityService;
        private IServiceContainer _services;
        public T GetService<T>() where T : Lockstep.Game.IService{
            if (_services == null) return default(T);
            return _services.GetService<T>();
        }


    #region Rollback Implement
        private ClassBackupHelper<_EntityManager> _entitiesBackuper = new ClassBackupHelper<_EntityManager>();

        protected override void _DoBackup(int tick){
            _entitiesBackuper.Backup(tick, _entities.Clone());
        }

        protected override void _DoRollbackTo(int tick, int missFrameTick, bool isNeedClear){
            var clone = _entitiesBackuper.RollbackTo(tick, missFrameTick, isNeedClear);
            clone.CopyTo(_entities);
        }

        protected override void _DoCleanUselessSnapshot(int checkedTick){
            _entitiesBackuper.CleanUselessSnapshot(checkedTick,(es)=>es.Free());
        }
    #endregion
    #region Lifecycle
        private FuncOnEntityCreated<BoidSpawner> funcOnCreateEntityBoidSpawner;
        private FuncOnEntityCreated<BoidSpawner> funcResetEntityBoidSpawner;
        private FuncOnEntityCreated<BoidCell> funcOnCreateEntityBoidCell;
        private FuncOnEntityCreated<BoidCell> funcResetEntityBoidCell;
        private FuncOnEntityCreated<Boid> funcOnCreateEntityBoid;
        private FuncOnEntityCreated<Boid> funcResetEntityBoid;
        private FuncOnEntityCreated<BoidTarget> funcOnCreateEntityBoidTarget;
        private FuncOnEntityCreated<BoidTarget> funcResetEntityBoidTarget;
        private FuncOnEntityCreated<BoidObstacle> funcOnCreateEntityBoidObstacle;
        private FuncOnEntityCreated<BoidObstacle> funcResetEntityBoidObstacle; 
 

        protected override void _DoAwake(IServiceContainer services){            
            RegisterSystemFunctions();
            TempFields = new TempFields(this);
            OnInit(this,services);
            _entities.Alloc();
            _services = services;
            // reduce gc
            funcOnCreateEntityBoidSpawner = OnEntityCreatedBoidSpawner;
            funcResetEntityBoidSpawner = ResetEntityBoidSpawner;
            funcOnCreateEntityBoidCell = OnEntityCreatedBoidCell;
            funcResetEntityBoidCell = ResetEntityBoidCell;
            funcOnCreateEntityBoid = OnEntityCreatedBoid;
            funcResetEntityBoid = ResetEntityBoid;
            funcOnCreateEntityBoidTarget = OnEntityCreatedBoidTarget;
            funcResetEntityBoidTarget = ResetEntityBoidTarget;
            funcOnCreateEntityBoidObstacle = OnEntityCreatedBoidObstacle;
            funcResetEntityBoidObstacle = ResetEntityBoidObstacle; 
        }
        protected override void _DoDestroy(){
            TempFields.OnDestroy();
            OnDestroy();
            _entities.Free();
        }
        protected override void _BeforeSchedule(){
            TempFields.FramePrepare();
        }
        protected override void _AfterSchedule(){
            TempFields.FrameClearUp();
        }


        protected override void _DoDestroyEntity(EntityRef entityRef){
            DestroyEntityInternal(GetEntity(entityRef));
        }

        public Entity* GetEntity(EntityRef entityRef){
            switch (entityRef._type) {
                case EntityIds.BoidSpawner: return (Entity*) GetBoidSpawner(entityRef);
                case EntityIds.BoidCell: return (Entity*) GetBoidCell(entityRef);
                case EntityIds.Boid: return (Entity*) GetBoid(entityRef);
                case EntityIds.BoidTarget: return (Entity*) GetBoidTarget(entityRef);
                case EntityIds.BoidObstacle: return (Entity*) GetBoidObstacle(entityRef); 
            }
            return null;
        }

        private void DestroyEntityInternal(Entity* entity){
            if (entity == null) {
                return;
            }

            if (entity->_active == false) {
                return;
            }

            switch (entity->_ref._type) {
                case EntityIds.BoidSpawner:
                    DestroyBoidSpawnerInternal((BoidSpawner*) entity);
                    break;
                case EntityIds.BoidCell:
                    DestroyBoidCellInternal((BoidCell*) entity);
                    break;
                case EntityIds.Boid:
                    DestroyBoidInternal((Boid*) entity);
                    break;
                case EntityIds.BoidTarget:
                    DestroyBoidTargetInternal((BoidTarget*) entity);
                    break;
                case EntityIds.BoidObstacle:
                    DestroyBoidObstacleInternal((BoidObstacle*) entity);
                    break; 
            }
        }
  
        private unsafe void PostUpdateCreateBoidSpawner(){
            _entities._BoidSpawnerAry.PostUpdateCreate(funcOnCreateEntityBoidSpawner,funcResetEntityBoidSpawner);
        }
        private unsafe void PostUpdateCreateBoidCell(){
            _entities._BoidCellAry.PostUpdateCreate(funcOnCreateEntityBoidCell,funcResetEntityBoidCell);
        }
        private unsafe void PostUpdateCreateBoid(){
            _entities._BoidAry.PostUpdateCreate(funcOnCreateEntityBoid,funcResetEntityBoid);
        }
        private unsafe void PostUpdateCreateBoidTarget(){
            _entities._BoidTargetAry.PostUpdateCreate(funcOnCreateEntityBoidTarget,funcResetEntityBoidTarget);
        }
        private unsafe void PostUpdateCreateBoidObstacle(){
            _entities._BoidObstacleAry.PostUpdateCreate(funcOnCreateEntityBoidObstacle,funcResetEntityBoidObstacle);
        } 

    #endregion
    #region Entity BoidSpawner
        private void OnEntityCreatedBoidSpawner(BoidSpawner* dstPtr){
            _EntityCreated(&dstPtr->_entity);
            _entityService.OnEntityCreated(this, (Entity*) dstPtr);
            _entityService.OnBoidSpawnerCreated(this, dstPtr);
        }

        private void ResetEntityBoidSpawner(BoidSpawner* dstPtr){
            *dstPtr = _DefaultDefine.BoidSpawner;
        }
        public Boolean BoidSpawnerExists(EntityRef entityRef){
            return GetBoidSpawner(entityRef) != null;
        }

        public BoidSpawner* PostCmdCreateBoidSpawner(){
            return _entities.CreateTempBoidSpawner(this);
        }

        private void DestroyBoidSpawnerInternal(BoidSpawner* ptr){
            _entities._BoidSpawnerAry.ReleaseEntity((Entity*)ptr);
            _entityService.OnBoidSpawnerDestroy(this, ptr);
            _entityService.OnEntityDestroy(this, &ptr->_entity);
            var copy = ptr->_entity;
            *ptr = _DefaultDefine.BoidSpawner;
            ptr->_entity = copy;
            _EntityDestroy(&ptr->_entity);
        }

        public void DestroyBoidSpawner(BoidSpawner* ptr){
            if (ptr == null) {
                return;
            }

            if (ptr->_entity._active == false) {
                return;
            }

            _destroy.Enqueue(ptr->EntityRef);
        }

        public void DestroyBoidSpawner(EntityRef entityRef){
            _destroy.Enqueue(entityRef);
        }
    #endregion
    #region Entity BoidCell
        private void OnEntityCreatedBoidCell(BoidCell* dstPtr){
            _EntityCreated(&dstPtr->_entity);
            _entityService.OnEntityCreated(this, (Entity*) dstPtr);
            _entityService.OnBoidCellCreated(this, dstPtr);
        }

        private void ResetEntityBoidCell(BoidCell* dstPtr){
            *dstPtr = _DefaultDefine.BoidCell;
        }
        public Boolean BoidCellExists(EntityRef entityRef){
            return GetBoidCell(entityRef) != null;
        }

        public BoidCell* PostCmdCreateBoidCell(){
            return _entities.CreateTempBoidCell(this);
        }

        private void DestroyBoidCellInternal(BoidCell* ptr){
            _entities._BoidCellAry.ReleaseEntity((Entity*)ptr);
            _entityService.OnBoidCellDestroy(this, ptr);
            _entityService.OnEntityDestroy(this, &ptr->_entity);
            var copy = ptr->_entity;
            *ptr = _DefaultDefine.BoidCell;
            ptr->_entity = copy;
            _EntityDestroy(&ptr->_entity);
        }

        public void DestroyBoidCell(BoidCell* ptr){
            if (ptr == null) {
                return;
            }

            if (ptr->_entity._active == false) {
                return;
            }

            _destroy.Enqueue(ptr->EntityRef);
        }

        public void DestroyBoidCell(EntityRef entityRef){
            _destroy.Enqueue(entityRef);
        }
    #endregion
    #region Entity Boid
        private void OnEntityCreatedBoid(Boid* dstPtr){
            _EntityCreated(&dstPtr->_entity);
            _entityService.OnEntityCreated(this, (Entity*) dstPtr);
            _entityService.OnBoidCreated(this, dstPtr);
        }

        private void ResetEntityBoid(Boid* dstPtr){
            *dstPtr = _DefaultDefine.Boid;
        }
        public Boolean BoidExists(EntityRef entityRef){
            return GetBoid(entityRef) != null;
        }

        public Boid* PostCmdCreateBoid(){
            return _entities.CreateTempBoid(this);
        }

        private void DestroyBoidInternal(Boid* ptr){
            _entities._BoidAry.ReleaseEntity((Entity*)ptr);
            _entityService.OnBoidDestroy(this, ptr);
            _entityService.OnEntityDestroy(this, &ptr->_entity);
            var copy = ptr->_entity;
            *ptr = _DefaultDefine.Boid;
            ptr->_entity = copy;
            _EntityDestroy(&ptr->_entity);
        }

        public void DestroyBoid(Boid* ptr){
            if (ptr == null) {
                return;
            }

            if (ptr->_entity._active == false) {
                return;
            }

            _destroy.Enqueue(ptr->EntityRef);
        }

        public void DestroyBoid(EntityRef entityRef){
            _destroy.Enqueue(entityRef);
        }
    #endregion
    #region Entity BoidTarget
        private void OnEntityCreatedBoidTarget(BoidTarget* dstPtr){
            _EntityCreated(&dstPtr->_entity);
            _entityService.OnEntityCreated(this, (Entity*) dstPtr);
            _entityService.OnBoidTargetCreated(this, dstPtr);
        }

        private void ResetEntityBoidTarget(BoidTarget* dstPtr){
            *dstPtr = _DefaultDefine.BoidTarget;
        }
        public Boolean BoidTargetExists(EntityRef entityRef){
            return GetBoidTarget(entityRef) != null;
        }

        public BoidTarget* PostCmdCreateBoidTarget(){
            return _entities.CreateTempBoidTarget(this);
        }

        private void DestroyBoidTargetInternal(BoidTarget* ptr){
            _entities._BoidTargetAry.ReleaseEntity((Entity*)ptr);
            _entityService.OnBoidTargetDestroy(this, ptr);
            _entityService.OnEntityDestroy(this, &ptr->_entity);
            var copy = ptr->_entity;
            *ptr = _DefaultDefine.BoidTarget;
            ptr->_entity = copy;
            _EntityDestroy(&ptr->_entity);
        }

        public void DestroyBoidTarget(BoidTarget* ptr){
            if (ptr == null) {
                return;
            }

            if (ptr->_entity._active == false) {
                return;
            }

            _destroy.Enqueue(ptr->EntityRef);
        }

        public void DestroyBoidTarget(EntityRef entityRef){
            _destroy.Enqueue(entityRef);
        }
    #endregion
    #region Entity BoidObstacle
        private void OnEntityCreatedBoidObstacle(BoidObstacle* dstPtr){
            _EntityCreated(&dstPtr->_entity);
            _entityService.OnEntityCreated(this, (Entity*) dstPtr);
            _entityService.OnBoidObstacleCreated(this, dstPtr);
        }

        private void ResetEntityBoidObstacle(BoidObstacle* dstPtr){
            *dstPtr = _DefaultDefine.BoidObstacle;
        }
        public Boolean BoidObstacleExists(EntityRef entityRef){
            return GetBoidObstacle(entityRef) != null;
        }

        public BoidObstacle* PostCmdCreateBoidObstacle(){
            return _entities.CreateTempBoidObstacle(this);
        }

        private void DestroyBoidObstacleInternal(BoidObstacle* ptr){
            _entities._BoidObstacleAry.ReleaseEntity((Entity*)ptr);
            _entityService.OnBoidObstacleDestroy(this, ptr);
            _entityService.OnEntityDestroy(this, &ptr->_entity);
            var copy = ptr->_entity;
            *ptr = _DefaultDefine.BoidObstacle;
            ptr->_entity = copy;
            _EntityDestroy(&ptr->_entity);
        }

        public void DestroyBoidObstacle(BoidObstacle* ptr){
            if (ptr == null) {
                return;
            }

            if (ptr->_entity._active == false) {
                return;
            }

            _destroy.Enqueue(ptr->EntityRef);
        }

        public void DestroyBoidObstacle(EntityRef entityRef){
            _destroy.Enqueue(entityRef);
        }
    #endregion 

    #region GetEntity
        private BoidSpawnerIterator GetAllBoidSpawner(){
            return new BoidSpawnerIterator(_entities.GetBoidSpawner(0),_entities.MaxBoidSpawnerIndex + 1);
        }
        private BoidCellIterator GetAllBoidCell(){
            return new BoidCellIterator(_entities.GetBoidCell(0),_entities.MaxBoidCellIndex + 1);
        }
        private BoidIterator GetAllBoid(){
            return new BoidIterator(_entities.GetBoid(0),_entities.MaxBoidIndex + 1);
        }
        private BoidTargetIterator GetAllBoidTarget(){
            return new BoidTargetIterator(_entities.GetBoidTarget(0),_entities.MaxBoidTargetIndex + 1);
        }
        private BoidObstacleIterator GetAllBoidObstacle(){
            return new BoidObstacleIterator(_entities.GetBoidObstacle(0),_entities.MaxBoidObstacleIndex + 1);
        } 

        private EntityFilter[] GetAllEntities(){
            var all = new EntityFilter[_entities.CurTotalEntityCount];
            var count = 0;
            {
                var ptr = _entities.GetBoidSpawner(0);
                var len = _entities._BoidSpawnerAry.Length;
                for (var i = 0; i < len; ++i, ++ptr) {
                    all[count++].Entity = &ptr->_entity;
                }
            }
            {
                var ptr = _entities.GetBoidCell(0);
                var len = _entities._BoidCellAry.Length;
                for (var i = 0; i < len; ++i, ++ptr) {
                    all[count++].Entity = &ptr->_entity;
                }
            }
            {
                var ptr = _entities.GetBoid(0);
                var len = _entities._BoidAry.Length;
                for (var i = 0; i < len; ++i, ++ptr) {
                    all[count++].Entity = &ptr->_entity;
                }
            }
            {
                var ptr = _entities.GetBoidTarget(0);
                var len = _entities._BoidTargetAry.Length;
                for (var i = 0; i < len; ++i, ++ptr) {
                    all[count++].Entity = &ptr->_entity;
                }
            }
            {
                var ptr = _entities.GetBoidObstacle(0);
                var len = _entities._BoidObstacleAry.Length;
                for (var i = 0; i < len; ++i, ++ptr) {
                    all[count++].Entity = &ptr->_entity;
                }
            } 
            return all;
        }
    #endregion

    #region GetBuildInComponet
        public unsafe Buffer<AnimatorFilter> GetAllAnimator()
        {
            Buffer<AnimatorFilter> buffer = Buffer<AnimatorFilter>.Alloc(_entities.CurTotalEntityCount);
 
            return buffer;
        }
        public unsafe Buffer<CollisionAgentFilter> GetAllCollisionAgent()
        {
            Buffer<CollisionAgentFilter> buffer = Buffer<CollisionAgentFilter>.Alloc(_entities.CurTotalEntityCount);
 
            return buffer;
        }
        public unsafe Buffer<NavMeshAgentFilter> GetAllNavMeshAgent()
        {
            Buffer<NavMeshAgentFilter> buffer = Buffer<NavMeshAgentFilter>.Alloc(_entities.CurTotalEntityCount);
 
            return buffer;
        }
        public unsafe Buffer<PrefabFilter> GetAllPrefab()
        {
            Buffer<PrefabFilter> buffer = Buffer<PrefabFilter>.Alloc(_entities.CurTotalEntityCount);
            BoidSpawner* BoidSpawnerPtr = this._entities.GetBoidSpawner(0);
            var idxBoidSpawner = 2;
            while (idxBoidSpawner >= 0)
            {
                if (BoidSpawnerPtr->_entity._active)
                {
                  buffer.Items[buffer.Count].Entity = &BoidSpawnerPtr->_entity;
                  buffer.Items[buffer.Count].Prefab = &BoidSpawnerPtr->Prefab;
                  ++buffer.Count;
                }
                --idxBoidSpawner;
                ++BoidSpawnerPtr;
            }
            Boid* BoidPtr = this._entities.GetBoid(0);
            var idxBoid = 2000;
            while (idxBoid >= 0)
            {
                if (BoidPtr->_entity._active)
                {
                  buffer.Items[buffer.Count].Entity = &BoidPtr->_entity;
                  buffer.Items[buffer.Count].Prefab = &BoidPtr->Prefab;
                  ++buffer.Count;
                }
                --idxBoid;
                ++BoidPtr;
            }
            BoidTarget* BoidTargetPtr = this._entities.GetBoidTarget(0);
            var idxBoidTarget = 2;
            while (idxBoidTarget >= 0)
            {
                if (BoidTargetPtr->_entity._active)
                {
                  buffer.Items[buffer.Count].Entity = &BoidTargetPtr->_entity;
                  buffer.Items[buffer.Count].Prefab = &BoidTargetPtr->Prefab;
                  ++buffer.Count;
                }
                --idxBoidTarget;
                ++BoidTargetPtr;
            }
            BoidObstacle* BoidObstaclePtr = this._entities.GetBoidObstacle(0);
            var idxBoidObstacle = 2;
            while (idxBoidObstacle >= 0)
            {
                if (BoidObstaclePtr->_entity._active)
                {
                  buffer.Items[buffer.Count].Entity = &BoidObstaclePtr->_entity;
                  buffer.Items[buffer.Count].Prefab = &BoidObstaclePtr->Prefab;
                  ++buffer.Count;
                }
                --idxBoidObstacle;
                ++BoidObstaclePtr;
            } 
            return buffer;
        }
        public unsafe Buffer<Transform2DFilter> GetAllTransform2D()
        {
            Buffer<Transform2DFilter> buffer = Buffer<Transform2DFilter>.Alloc(_entities.CurTotalEntityCount);
 
            return buffer;
        }
        public unsafe Buffer<Transform3DFilter> GetAllTransform3D()
        {
            Buffer<Transform3DFilter> buffer = Buffer<Transform3DFilter>.Alloc(_entities.CurTotalEntityCount);
 
            return buffer;
        } 
    #endregion

    }
}                                                                                
                                                                                         