using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace Nomnom.EcsLiteDebugger {
  public class WorldDebugSystem: IEcsPreInitSystem, IEcsRunSystem, IEcsDestroySystem, IEcsWorldEventListener {
    private readonly string _name;
    private WorldDebugView _view;
    private List<(int, WorldDebugView.ChangeType)> _dirtyEntities;

    public WorldDebugSystem(string name) {
      _name = name;
      _dirtyEntities = new List<(int, WorldDebugView.ChangeType)>();
      _view = new WorldDebugView();
    }
    
    public void PreInit(EcsSystems systems) {
      EcsWorld world = systems.GetWorld();
      world.AddEventListener(this);
      
      _view.Init(world, _name);
      
      int[] entities = null;
      int entityCount = world.GetAllEntities(ref entities);

      for (int i = 0; i < entityCount; i++) {
        OnEntityCreated(entities[i]);
      }
    }

    public void Run(EcsSystems systems) {
      if (_dirtyEntities.Count <= 0 || _view == null) {
        return;
      }

      foreach ((int entity, WorldDebugView.ChangeType changeType) in _dirtyEntities) {
        _view.UpdateEntity(entity, changeType);
      }

      _view.IsDirty = true;
      
      _dirtyEntities.Clear();
    }

    public void Destroy(EcsSystems systems) {
      _view?.Destroy();
    }
    
    public void OnEntityCreated(int entity) {
      _dirtyEntities.Add((entity, WorldDebugView.ChangeType.New));
    }

    public void OnEntityChanged(int entity) { 
      _dirtyEntities.Add((entity, WorldDebugView.ChangeType.Modified));
    }

    public void OnEntityDestroyed(int entity) { 
      _dirtyEntities.Add((entity, WorldDebugView.ChangeType.Del));
    }

    public void OnFilterCreated(EcsFilter filter) { }

    public void OnWorldResized(int newSize) {
      _view.Repaint();
    }

    public void OnWorldDestroyed(EcsWorld world) {
      world.RemoveEventListener(this);

      if (_view == null) {
        return;
      }

      WorldDebugView.Views.Remove(_view);
      _view.Repaint();
      _view.Destroy();
      _view = null;
    }
  }
}