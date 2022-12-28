using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CCGTestTask
{
    public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Text _manacostValueText;
        [SerializeField] private Text _attackValueText;
        [SerializeField] private Text _healthValueText;

        private int _manacostValue;
        private int _attackValue;
        private int _healthValue;
        public int HealthValue => _healthValue;

        private Canvas _canvas;
        private Vector2 _startPosioton;
        private Quaternion _startRotation;

        public bool Moving { get; set; }

        private void Start()
        {
            _canvas = transform.GetComponentInChildren<Canvas>();
        }

        public void RandomizeCardStats(Vector2 range)
        {
            _manacostValue = Random.Range((int)range.x, (int)range.y);
            _attackValue = Random.Range((int)range.x, (int)range.y);
            _healthValue = Random.Range((int)range.x, (int)range.y);

            UpdateText();
        }

        private void UpdateText()
        {
            _manacostValueText.text = _manacostValue.ToString();
            _attackValueText.text = _attackValue.ToString();
            _healthValueText.text = _healthValue.ToString();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Moving == false)
            {
                _startPosioton = transform.position;
                _startRotation = transform.rotation;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Moving == false)
            {
                transform.SetPositionAndRotation(new Vector3(Camera.main.ScreenToWorldPoint(eventData.position).x, Camera.main.ScreenToWorldPoint(eventData.position).y), new Quaternion());
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (Moving == false)
            {
                CircleArea circle = FindObjectOfType<CircleArea>();

                if (circle.Radius > Vector2.Distance(circle.transform.position, transform.position))
                {
                    transform.position = circle.transform.position;
                    transform.root.GetComponent<PlayerHand>().RemoveCard(this);
                }
                else
                {
                    transform.SetPositionAndRotation(_startPosioton, _startRotation);
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _canvas.sortingOrder = 1;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _canvas.sortingOrder = 0;
        }
    }
}