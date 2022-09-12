using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  internal class ObjectGui: FieldGui<Object> {
    public ObjectGui(ComponentField field) : base(field) { }

    public override void UpdateValue(object value) {
      ((ObjectField)Element).SetValueWithoutNotify((Object)value);

      // if (value == null || !(Object)value) {
      //   return;
      // } 
      //
      // ((InspectorElement)Element).Bind(new SerializedObject((Object)value));
    }
    
    protected override VisualElement CreateInternal(object item, FieldInfo fieldInfo) {
      return new ObjectField(fieldInfo.Name) {
        value = (Object)fieldInfo.GetValue(item),
        objectType = fieldInfo.FieldType
      };
      // return new InspectorElement((Object)fieldInfo.GetValue(item));
    }
  }
}