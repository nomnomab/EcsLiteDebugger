using System.Reflection;
using UnityEngine.UIElements;

namespace Nomnom.EcsLiteDebugger.Editor.Fields {
  public abstract class FieldGui<T>: IFieldGui {
    public bool IsEditing { get; set; }

    protected VisualElement Element;

    private ComponentField _field;

    protected FieldGui(ComponentField field) {
      _field = field;
    }

    public VisualElement Create(object item, FieldInfo fieldInfo) {
      VisualElement element = CreateInternal(item, fieldInfo);
      Element = element;
      
      CreateFocusEvent(element);

      if (element is INotifyValueChanged<T> castElement) {
        CreateChangeEvent(fieldInfo, castElement); 
      }

      return element;
    }

    public abstract void UpdateValue(object value);

    protected abstract VisualElement CreateInternal(object item, FieldInfo fieldInfo);
    
    private void CreateFocusEvent(CallbackEventHandler element) {
      element.RegisterCallback<FocusInEvent>(v =>
      {
        IsEditing = true;
      });
      element.RegisterCallback<FocusOutEvent>(v =>
      {
        IsEditing = false;
      });
    }

    private void CreateChangeEvent<T>(FieldInfo fieldInfo, INotifyValueChanged<T> element) {
      element.RegisterValueChangedCallback(v => _field.UpdateReference(fieldInfo, v.newValue));
    }
  }

  internal interface IFieldGui {
    VisualElement Create(object item, FieldInfo fieldInfo);
    void UpdateValue(object value);
    bool IsEditing { get; set; }
  }
}