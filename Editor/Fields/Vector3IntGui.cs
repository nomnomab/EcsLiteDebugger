using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class Vector3IntGui: FieldGui<Vector3Int> {
    public Vector3IntGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((Vector3IntField)Element).SetValueWithoutNotify((Vector3Int)value);
    }

    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new Vector3IntField(fieldInfo.Name) {
        value = (Vector3Int)fieldInfo.GetValue(item)
      };
    }
  }
}