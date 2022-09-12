using System;
using System.Collections.Generic;
using System.Linq;
using DuoVia.FuzzyStrings;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor {
  internal class EcsLiteHierarchyWindow: EditorWindow {
    [MenuItem("Tools/Nomnom/ecsLite Hierarchy")]
    private static void Open() {
      GetWindow<EcsLiteHierarchyWindow>("ecsLite Hierarchy").Show();
    }

    private DropdownField _worldMenu;
    private Toolbar _toolbar;
    private ToolbarSearchField _searchField;
    private ScrollView _hierarchy;
    
    private readonly List<string> _defaultOption = new List<string>(1) { "None" };

    private void CreateGUI() {
      EditorApplication.playModeStateChanged -= PlayModeStateChanged;
      EditorApplication.playModeStateChanged += PlayModeStateChanged;
      
      VisualTreeAsset baseWindow = Resources.Load<VisualTreeAsset>("ecsLite/HierarchyWindow");

      baseWindow.CloneTree(rootVisualElement);

      _worldMenu = new DropdownField(_defaultOption, 0);

      VisualElement toolbarBtn = _worldMenu.ElementAt(0);
      toolbarBtn.AddToClassList("unity-toolbar-button");
      toolbarBtn.style.borderBottomLeftRadius = 0;
      toolbarBtn.style.borderBottomRightRadius = 0;
      toolbarBtn.style.borderTopLeftRadius = 0;
      toolbarBtn.style.borderTopRightRadius = 0;
      
      _worldMenu.SetEnabled(false);
      _worldMenu.RegisterValueChangedCallback(e =>
      {
        EcsLiteInspectorWindow.SetEntity(default, default, true);
        _hierarchy.Clear();
        
        // fake some mutations to propagate the list
        if (!(WorldDebugView.Views == null || WorldDebugView.Views.Count == 0)) {
          WorldDebugView.Views[_worldMenu.index].CreateDummyList();
        }

        Refresh();
      });

      _toolbar = rootVisualElement.Q<Toolbar>("toolbar");
      _toolbar.Add(_worldMenu);
      
      _worldMenu.SendToBack();

      _searchField = rootVisualElement.Q("search").ElementAt(0) as ToolbarSearchField;
      _searchField.RegisterValueChangedCallback(e =>
      {
        FilterEntities();
      });
      
      _hierarchy = rootVisualElement.Q<ScrollView>("hierarchy");

      rootVisualElement.Q<Button>("reload").clicked += Refresh;

      Refresh();
    }

    private void Update() {
      if (WorldDebugView.Views != null) {
        if (WorldDebugView.Views.Count != _worldMenu.choices.Count) {
          Refresh();
        }
        
        for (int i = 0; i < WorldDebugView.Views.Count; i++) {
          WorldDebugView view = WorldDebugView.Views[i];
          
          if (!view.IsDirty || i != _worldMenu.index) {
            continue;
          }

          view.IsDirty = false;
          Refresh();
          break;
        }
      }
      
      EcsLiteInspectorWindow.Update();
    }

    private void PlayModeStateChanged(PlayModeStateChange e) {
      _hierarchy.Clear();
      Refresh();
    }

    private void Refresh() {
      var views = WorldDebugView.Views;
      _worldMenu.choices = views.Count == 0 ? _defaultOption : views.Select(v => v.Name).ToList();
      
      int index = Mathf.Max(0, Mathf.Min(_worldMenu.index, _worldMenu.choices.Count - 1));
      _worldMenu.index = index;
      
      _worldMenu.SetEnabled(views.Count > 0);

      UpdateHierarchy();
      FilterEntities();
    }

    private void UpdateHierarchy() {
      var views = WorldDebugView.Views;
      if (views.Count == 0) {
        return;
      }

      WorldDebugView view = views[_worldMenu.index];
      int maxId = view.GetWorld().GetAllocatedEntitiesCount();

      if (maxId == 0) {
        return;
      }
      
      var mutations = view.GetMutations();

      int start = _hierarchy.Children().Count();
      for (int i = start; i < maxId; i++) {
        _hierarchy.Add(CreateEmptyRow(i, view));
      }

      var children = _hierarchy.Children();
      
      foreach (WorldDebugView.Mutation mutation in mutations) {
        if (mutation.entity.id == -1) {
          continue;
        }
        
        VisualElement child = children.ElementAt(mutation.entity.id);
        switch (mutation.changeType) {
          case WorldDebugView.ChangeType.None:
            break;
          case WorldDebugView.ChangeType.New:
          case WorldDebugView.ChangeType.Modified:
            child.Q<Label>("name").text = mutation.entity.GetPrettyName();
            break;
          case WorldDebugView.ChangeType.Del:
            child.Q<Label>("name").text = null;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    private VisualElement CreateEmptyRow(int id, WorldDebugView view) {
      Button row = new Button();
      VisualElement left = new VisualElement();
      Label index = new Label(id.ToString());
      Label name = new Label(null) {
        name = "name"
      };

      left.Add(index);
        
      row.Add(left);
      row.Add(name);

      int tmpId = id;
      WorldDebugView.DebugEntity GetEntity() => view.GetEntities()[tmpId];
      row.clicked += () =>
      {
        EcsLiteInspectorWindow.SetEntity(view, GetEntity());
      };

      left.style.minWidth = new StyleLength(new Length(30, LengthUnit.Pixel));
      left.style.backgroundColor = new StyleColor(new Color32(45, 45, 45, 255));
        
      index.style.SetPadding(new StyleLength(new Length(2, LengthUnit.Pixel)));

      row.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
      row.style.flexWrap = new StyleEnum<Wrap>(Wrap.NoWrap);
      row.style.SetPadding(0);
      row.style.SetMargin(0);
      row.style.SetBorderRadius(0);
      row.style.backgroundImage = null;

      name.style.flexWrap = new StyleEnum<Wrap>(Wrap.NoWrap);
      name.style.paddingBottom = new StyleLength(new Length(2, LengthUnit.Pixel));
      name.style.paddingTop = new StyleLength(new Length(2, LengthUnit.Pixel));
      name.style.paddingLeft = new StyleLength(new Length(5, LengthUnit.Pixel));
      name.style.paddingRight = new StyleLength(new Length(5, LengthUnit.Pixel));
      name.style.textOverflow = new StyleEnum<TextOverflow>(TextOverflow.Ellipsis);
      name.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleLeft);

      return row;
    }

    private void FilterEntities() {
      string filter = _searchField.value.ToLower();
      int i = 0;

      foreach (VisualElement child in _hierarchy.Children()) {
        Label name = child.ElementAt(1) as Label;
        string text = name.text.ToLower();
        bool show = string.IsNullOrEmpty(filter) ||
                    !string.IsNullOrEmpty(text) &&
                    (filter.DiceCoefficient(text) >= 0.33 || 
                    filter.LevenshteinDistance(text) < 3 ||
                    text.Contains(filter));
        
        child.style.height = show ? StyleKeyword.Auto : 0;
        child.style.display = new StyleEnum<DisplayStyle>(show ? DisplayStyle.Flex : DisplayStyle.None);

        if (show) {
          Color color = i % 2 == 0 ? new Color32(56, 56, 56, 255) : new Color32(88, 88, 88, 255);
          child.style.backgroundColor = new StyleColor(color);
          i++;
        }
      }
      
      _hierarchy.MarkDirtyRepaint();
    }
  }
}