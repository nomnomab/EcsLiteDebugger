using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class Vector2IntGui: FieldGui<Vector2Int> {
    public Vector2IntGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((Vector2IntField)Element).SetValueWithoutNotify((Vector2Int)value);
    }

    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new Vector2IntField(fieldInfo.Name) {
        value = (Vector2Int)fieldInfo.GetValue(item)
      };
    }
  }
}