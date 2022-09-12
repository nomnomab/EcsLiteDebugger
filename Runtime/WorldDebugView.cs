using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;

namespace Nomnom.EcsLiteDebugger {
  public class WorldDebugView {
    public static List<WorldDebugView> Views { get; } = new List<WorldDebugView>();

    public string Name { get; private set; }
    public bool IsDirty;

    private EcsWorld _world;
    private List<DebugEntity> _entities;
    private List<Mutation> _mutations;

    public void Init(EcsWorld world, string name) {
      _world = world;
      Name = name;
      _entities = new List<DebugEntity>();
      _mutations = new List<Mutation>();
      Views.Add(this);
    }

    public void Destroy() {
      Views.Remove(this);
    }

    public void UpdateEntity(int id, ChangeType changeType) {
      while (_entities.Count <= id) {
        _entities.Add(default);
      }
      
      DebugEntity e = _entities[id];
      Type[] types = GetEntityTypes(id)?.ToArray();

      e.name = types == null ? string.Join(", ", types.Select(PrettyName)) : null;
      e.id = id;
      e.world = _world;
      e.types = types;

      _entities[id] = e;
      
      _mutations.Add(new Mutation {
        entity = e,
        changeType = changeType
      });
    }

    public void CreateDummyList() {
      for (int i = 0; i < _world.GetAllocatedEntitiesCount(); i++) {
        _mutations.Add(new Mutation {
          entity = _entities[i],
          changeType = ChangeType.New
        });
      }

      IsDirty = true;
    }
    
    private IEnumerable<Type> GetEntityTypes(int entity) {
      if (_world.GetEntityGen(entity) <= 0) {
        yield break;
      }

      DebugEntity e = _entities[entity];
      Type[] types = e.types;
      int count = _world.GetComponentTypes(entity, ref types);

      for (int i = 0; i < count; i++) {
        yield return types[i];
      }
    }

    public void Repaint() {
      IsDirty = true;
    }

    public IReadOnlyList<Mutation> GetMutations() {
      return _mutations;
    }

    public void ClearMutations() {
      _mutations.Clear();
      IsDirty = false;
    }

    public IReadOnlyList<DebugEntity> GetEntities() {
      return _entities;
    }
    
    public DebugEntity GetEntity(int entity) {
      return _entities[entity];
    }
    
    public EcsWorld GetWorld() {
      return _world;
    }

    private static string PrettyName(Type type) {
      if (type.IsGenericType) {
        Type[] generics = type.GenericTypeArguments;
        return type.Name.Replace($"`{generics.Length}", $"<{string.Join(", ", generics.Select(PrettyName))}>");
      }
      
      return type.Name;
    }

    public struct DebugEntity {
      public string name;
      public int id;
      public EcsWorld world;
      public Type[] types;

      public string GetPrettyName() {
        return string.Join(", ", types.Select(PrettyName));
      }
      
      public string GetPrettyName(int index) {
        return PrettyName(types[index]);
      }
    }

    public struct Mutation {
      public DebugEntity entity;
      public ChangeType changeType;
    }

    public enum ChangeType {
      None,
      New,
      Del,
      Modified
    }
  }
}