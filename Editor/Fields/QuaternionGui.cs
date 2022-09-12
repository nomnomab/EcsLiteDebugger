using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class QuaternionGui: FieldGui<Quaternion> {
    public QuaternionGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((Vector4Field)Element).SetValueWithoutNotify(ToVector((Quaternion)value));
    }
    
    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new Vector4Field(fieldInfo.Name) {
        value = ToVector((Quaternion)fieldInfo.GetValue(item))
      };
    }

    private Vector4 ToVector(in Quaternion quaternion) {
      return new Vector4(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
    }
  }
}