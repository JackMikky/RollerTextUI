using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class TextEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    enum TextSituation
    {
        AnimationActived = 0,
        AnimationInactivated = 1,
        AnimateAlways = -1
    }
    enum MoveDirection
    {
        Right = 1,
        Left = 0
    }
    TMP_Text _Text;
    string textCash;
    string oringeText;
    [SerializeField] TextSituation situation = TextSituation.AnimationInactivated;
    [SerializeField] MoveDirection direction = MoveDirection.Left;
    [Tooltip("When pointer exit UI reset text position")]
    [SerializeField] bool resetText = true;
    [Tooltip("Text move speed (perChar/second)")]
    [Range(0.01f, 1)]
    [SerializeField] float moveTimer = 0.25f;
    float timer;
    void Awake()
    {
        _Text = GetComponent<TMP_Text>();
        oringeText = _Text.text;
        textCash = _Text.text + "            ";
    }
    void FixedUpdate()
    {
        if (situation is 0 or (TextSituation)(-1))
        {
            timer += Time.fixedDeltaTime;
            MoveText();
        }
    }
    void MoveText()
    {
        if (timer >= moveTimer)
        {
            char textTemp;
            string newText;
            switch (direction)
            {
                case MoveDirection.Right:
                    newText = textCash[..(textCash.Length - 1)];
                    textTemp = textCash[^1];
                    textCash = textTemp + newText;
                    _Text.text = textCash;
                    break;
                case MoveDirection.Left:
                    newText = textCash[1..];
                    textTemp = textCash[0];
                    textCash = newText + textTemp;
                    _Text.text = textCash;
                    break;
            }
            timer = 0;
        }
    }

    /// <summary>
    /// index 0> AnimationActived 1> AnimationInactivated
    /// </summary>
    /// <param name="situationIndex"></param>
    void SwitchSituation(byte situationIndex)
    {
        situation = (TextSituation)situationIndex;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (situation is (TextSituation)(-1))
        {
            return;
        }
        SwitchSituation(0);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (situation is (TextSituation)(-1))
        {
            return;
        }
        SwitchSituation(1);
        if (resetText)
        {
            textCash = oringeText + "            ";
        }
        _Text.text = oringeText;
        timer = 0;
    }
}
