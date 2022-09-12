using System.Linq;
using Leopotam.EcsLite;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor {
  internal class EcsLiteInspectorWindow: EditorWindow {
    [MenuItem("Tools/Nomnom/ecsLite Inspector")]
    public static void Open() {
      GetWindow<EcsLiteInspectorWindow>("ecsLite Inspector").Show();
    }

    private const int TICK_RATE_DEFAULT = 60 * 2;
    private const string TICK_KEY = "Nomnom_ecsLite_Interval";
    private static EcsLiteInspectorWindow _instance;

    private static EcsPackedEntityWithWorld? _entity;
    private static WorldDebugView _view;
    
    private Label _idLabel;
    private ScrollView _components;
    private Button _reload;
    private IntegerField _intervalAmount;
    
    private int _ticks;
    private UQueryBuilder<ComponentGUI> _childQuery;

    public static bool TryGetEntity(out int id) {
      id = -1;
      bool valid = _instance && _instance.IsEntityValid(out _, out id);
      return valid;
    }
    
    public static void SetEntity(WorldDebugView view, WorldDebugView.DebugEntity entity, bool ignoreInstance = false) {
      _view = view;
      _entity = view != null ? entity.world.PackEntityWithWorld(entity.id) : null;

      if (ignoreInstance) {
        return;
      }
      
      if (_instance == null) {
        _instance = GetWindow<EcsLiteInspectorWindow>("ecsLite Inspector");
      }
      
      _instance.Refresh();
    }

    private void OnEnable() {
      _instance = this;
    }

    private void CreateGUI() {
      EditorApplication.playModeStateChanged -= PlayModeStateChanged;
      EditorApplication.playModeStateChanged += PlayModeStateChanged;
      
      VisualTreeAsset baseWindow = Resources.Load<VisualTreeAsset>("ecsLite/InspectorWindow");

      baseWindow.CloneTree(rootVisualElement);

      GetReferences();

      Refresh();
    }

    private void GetReferences() {
      _idLabel = rootVisualElement.Q<Label>("id");
      _components = rootVisualElement.Q<ScrollView>("components");
      _reload = rootVisualElement.Q<Button>("reload");
      
      _reload.clicked -= Refresh;
      _reload.clicked += Refresh;

      var interval = rootVisualElement.Q("interval");

      if (interval.childCount == 3) {
        _intervalAmount.RemoveFromHierarchy();
      }
      
      _intervalAmount = new IntegerField {
        value = EditorPrefs.GetInt(TICK_KEY, TICK_RATE_DEFAULT)
      };

      _intervalAmount.RegisterValueChangedCallback(v => EditorPrefs.SetInt(TICK_KEY, v.newValue));
      interval.Insert(1, _intervalAmount);
      
      _childQuery = rootVisualElement.Query<ComponentGUI>();
    }

    private void PlayModeStateChanged(PlayModeStateChange e) {
      Reset();
      GetReferences();
      Refresh();
    }

    public void Reset() {
      _entity = default;
      _view = null;
    }

    public void CustomUpdate() {
      if (_intervalAmount == null) {
        return;
      }
      
      if (_ticks++ < _intervalAmount.value) {
        return;
      }

      _ticks = 0;

      if (_view == null || !IsEntityValid(out _, out int e)) {
        Reset();
        Refresh();
        return;
      }

      UQueryState<ComponentGUI> query = _childQuery.Build();
      bool isValid = IsEntityValid(out _, out int id);

      if (!isValid) {
        Reset();
        Refresh();
        return;
      }

      WorldDebugView.DebugEntity entity = _view.GetEntity(id);
      if (entity.types.Length != query.Count()) {
        Refresh();
      }
      
      foreach (ComponentGUI componentGUI in query) {
        componentGUI.Refresh();
      }
    }

    private void Refresh() {
      bool validEntity = IsEntityValid(out var world, out var entity);

      if (_idLabel != null) {
        _idLabel.text = validEntity ? $"Entity {entity}" : "Nothing selected";
      }

      _components?.Clear();

      if (_view == null || !validEntity) {
        _reload.SetEnabled(false);
        return;
      }
      
      var debugEntity = _view.GetEntity(entity);
      
      _reload.SetEnabled(true);

      for (int i = 0; i < debugEntity.types.Length; i++) {
        Foldout foldout = new Foldout();
        IStyle toggleStyle = foldout.Q<Toggle>().style;

        toggleStyle.backgroundColor = new StyleColor(new Color32(62, 62, 62, 255));
        toggleStyle.borderBottomWidth = 1;
        toggleStyle.borderBottomColor = new StyleColor(new Color32(48, 48, 48, 255));
        toggleStyle.SetMargin(0);

        foldout.text = debugEntity.GetPrettyName(i);

        ComponentGUI gui = new ComponentGUI(debugEntity.types[i], entity, world);
        foldout.Add(gui);

        _components.Add(foldout);
      }
    }

    private bool IsEntityValid(out EcsWorld world, out int entity) {
      world = default;
      entity = default;
      
      return !(_entity == null || !_entity.Value.Unpack(out world, out entity) || entity == -1);
    }

    public static void Update() {
      if (_instance) {
        _instance.CustomUpdate();
      }

      _view?.ClearMutations();
    }
  }
}