using System.Reflection;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class BoolGui: FieldGui<bool> {
    public BoolGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((Toggle)Element).SetValueWithoutNotify((bool)value);
    }
    
    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new Toggle(fieldInfo.Name) {
        value = (bool)fieldInfo.GetValue(item)
      };
    }
  }
}