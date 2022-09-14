using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Nomnom.EcsLiteDebugger {
#if DEBUG || LEOECSLITE_WORLD_EVENTS
  public class WorldDebugSystem: IEcsPreInitSystem, IEcsRunSystem, IEcsDestroySystem, IEcsWorldEventListener {
#else
  public class WorldDebugSystem: IEcsPreInitSystem, IEcsRunSystem, IEcsDestroySystem {
#endif
    private readonly string _name;
    private WorldDebugView _view;
    private List<(int, WorldDebugView.ChangeType)> _dirtyEntities;

    public WorldDebugSystem(string name) {
#if UNITY_EDITOR
      _name = name;
      _dirtyEntities = new List<(int, WorldDebugView.ChangeType)>();
      _view = new WorldDebugView();
#endif
    }
    
    public void PreInit(EcsSystems systems) {
#if UNITY_EDITOR
      EcsWorld world = systems.GetWorld();
      world.AddEventListener(this);
      
      _view.Init(world, _name);
      
      int[] entities = null;
      int entityCount = world.GetAllEntities(ref entities);

      for (int i = 0; i < entityCount; i++) {
        OnEntityCreated(entities[i]);
      }
#endif
    }

    public void Run(EcsSystems systems) {
#if UNITY_EDITOR
      if (_dirtyEntities.Count <= 0 || _view == null) {
        return;
      }

      foreach ((int entity, WorldDebugView.ChangeType changeType) in _dirtyEntities) {
        _view.UpdateEntity(entity, changeType);
      }

      _view.IsDirty = true;
      
      _dirtyEntities.Clear();
#endif
    }

    public void Destroy(EcsSystems systems) {
#if UNITY_EDITOR
      _view?.Destroy();
#endif
    }
    
    public void OnEntityCreated(int entity) {
#if UNITY_EDITOR
      _dirtyEntities.Add((entity, WorldDebugView.ChangeType.New));
#endif
    }

    public void OnEntityChanged(int entity) { 
#if UNITY_EDITOR
      _dirtyEntities.Add((entity, WorldDebugView.ChangeType.Modified));
#endif
    }

    public void OnEntityDestroyed(int entity) { 
#if UNITY_EDITOR
      _dirtyEntities.Add((entity, WorldDebugView.ChangeType.Del));
#endif
    }

    public void OnFilterCreated(EcsFilter filter) { }

    public void OnWorldResized(int newSize) {
#if UNITY_EDITOR
      _view.Repaint();
#endif
    }

    public void OnWorldDestroyed(EcsWorld world) {
#if UNITY_EDITOR
      world.RemoveEventListener(this);

      if (_view == null) {
        return;
      }

      WorldDebugView.Views.Remove(_view);
      _view.Repaint();
      _view.Destroy();
      _view = null;
#endif
    }
  }
}