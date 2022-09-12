using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class IntegerGui: FieldGui<int> {
    public IntegerGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((IntegerField)Element).SetValueWithoutNotify((int)value);
    }
    
    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new IntegerField(fieldInfo.Name) {
        value = (int)fieldInfo.GetValue(item)
      };
    }
  }
}