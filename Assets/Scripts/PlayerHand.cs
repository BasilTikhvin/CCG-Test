using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CCGTestTask
{
    public class PlayerHand : MonoBehaviour
    {
        [SerializeField] private TextureLoadHelper _textureLoadHelper;
        [SerializeField] private GameObject _cardPrefab;
        [SerializeField] private Button _updateButton;

        [Space]
        [SerializeField] private Vector2 _cardsAmountRange;
        [SerializeField] private Vector2 _randomizeCardStatsRange;
        [SerializeField] private List<Card> _cards;

        private int _cardsAmount;

        private void Start()
        {
            _updateButton.interactable = false;

            _cardsAmount = Random.Range((int)_cardsAmountRange.x, (int)_cardsAmountRange.y);

            for (int i = 0; i < _cardsAmount; i++)
            {
                Card card = Instantiate(_cardPrefab, transform.position, Quaternion.identity, transform).GetComponent<Card>();
                RawImage cardArt = card.GetComponentInChildren<RawImage>();

                StartCoroutine(_textureLoadHelper.LoadCardArtIMage(cardArt));

                _cards.Add(card);
            }

            CallUpdate();
        }

        public void UpdateHandCards()
        {
            _updateButton.interactable = false;

            StartCoroutine(nameof(ForEachCard));
        }

        private IEnumerator ForEachCard()
        {
            foreach (Card card in _cards.ToList())
            {
                card.RandomizeCardStats(_randomizeCardStatsRange);
                yield return new WaitForSeconds(0.5f);

                if (card.HealthValue <= 0)
                {
                    _cards.Remove(card);
                    Destroy(card.gameObject);
                }
            }

            CallUpdate();
        }

        private const float OFFSET_X = 0.5f;
        private const float OFFSET_Y = -3;
        private Vector3 GetCardNewPosition(int cardsAmount, int cardIndex)
        {
            return new Vector3(-cardsAmount / 2f + cardIndex + OFFSET_X, OFFSET_Y);
        }

        private const float TICK_TIME = 0.01f;
        private IEnumerator UpdateCardPositionAndRotation(Card card, Vector3 moveTo, float time)
        {
            card.Moving = true;

            var distance = (moveTo - card.transform.localPosition).magnitude;
            var direction = (moveTo - card.transform.localPosition).normalized;
            var speed = distance / time;
            var speedForTick = speed * TICK_TIME;

            while (distance >= 0.01f && card != null && card.Moving)
            {
                distance = (moveTo - card.transform.localPosition).magnitude;
                card.transform.position += direction * speedForTick;
                card.transform.rotation = Quaternion.FromToRotation(card.transform.position, new Vector3(0, OFFSET_Y, 0));
                yield return new WaitForSeconds(TICK_TIME);
            }
            card.transform.position = moveTo;

            card.Moving = false;
            _updateButton.interactable = true;
        }

        public void RemoveCard(Card cardToRemove)
        {
            _updateButton.interactable = false;

            _cards.Remove(cardToRemove);

            CallUpdate();
        }

        private void CallUpdate()
        {
            foreach (Card card in _cards)
            {
                StartCoroutine(UpdateCardPositionAndRotation(card, GetCardNewPosition(_cards.Count, _cards.IndexOf(card)), 1));
            }
        }
    }
}