using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class Vector2Gui: FieldGui<Vector2> {
    public Vector2Gui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((Vector2Field)Element).SetValueWithoutNotify((Vector2)value);
    }

    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new Vector2Field(fieldInfo.Name) {
        value = (Vector2)fieldInfo.GetValue(item)
      };
    }
  }
}