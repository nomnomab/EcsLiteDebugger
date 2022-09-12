using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class LongGui: FieldGui<long> {
    public LongGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((LongField)Element).SetValueWithoutNotify((long)value);
    }
    
    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new LongField(fieldInfo.Name) {
        value = (long)fieldInfo.GetValue(item)
      };
    }
  }
}