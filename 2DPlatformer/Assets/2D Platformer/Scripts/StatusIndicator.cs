using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour 
{

    [SerializeField]
    private RectTransform _healthBarRect;
    [SerializeField]
    private Text _helthText;

    void Start()
    {
        if(_healthBarRect == null)
            Debug.LogError("STATUS INDICATOR: Нет связи с объектом полоски количества жизни");
        if (_helthText == null)
            Debug.LogError("STATUS INDICATOR: Нет связи с объектом текста количества жизни");
    }

    public void SetHealth(int cur, int max)
    {
        var value = (float)cur/max;

        _healthBarRect.localScale = new Vector3(value, _healthBarRect.localScale.y, _healthBarRect.localScale.z);
        _helthText.text = cur + "/" + max + " HP";
    }
}
