using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class Vector4Gui: FieldGui<Vector4> {
    public Vector4Gui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((Vector4Field)Element).SetValueWithoutNotify((Vector4)value);
    }

    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new Vector4Field(fieldInfo.Name) {
        value = (Vector4)fieldInfo.GetValue(item)
      };
    }
  }
}