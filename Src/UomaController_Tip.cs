using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace UomaWeb
{
    public partial class UomaController
    {
        private GameObject _tipCanvas;
        private GameObject _tipPrefab;
        private Queue<GameObject> _tipPool = new Queue<GameObject>();
        private List<GameObject> _activeTips = new List<GameObject>();
        private const int MaxPoolSize = 5;
        private const float DefaultTipDuration = 2f;
        private const float TipFadeDuration = 0.3f;

        private static readonly Dictionary<string, string> ErrorMessageMap = new Dictionary<string, string>
        {
            { "USER_ORDER_VIRTUAL_CURRENCY_BALANCE_INSUFFICIENT", "Insufficient balance" },
            { "USER_ORDER_GAME_ITEM_INSUFFICIENT", "Insufficient items" },
            { "USER_ORDER_GAME_ITEM_NOT_FOUND", "Item not found" },
            { "USER_ORDER_GAME_NOT_FOUND", "Game not found" },
            { "USER_ORDER_USER_NOT_FOUND", "User not found" },
            { "USER_ORDER_INVALID_REQUEST", "Invalid request" },
            { "USER_ORDER_INTERNAL_ERROR", "Server error, please try again" },
        };

        private static readonly Dictionary<int, string> ErrorCodeMessageMap = new Dictionary<int, string>
        {
            { 400, "Request error" },
            { 401, "Unauthorized, please login" },
            { 403, "Access denied" },
            { 404, "Resource not found" },
            { 500, "Server error" },
            { 502, "Server error" },
            { 503, "Service unavailable" },
        };

        public void ShowTip(string message, float duration = DefaultTipDuration)
        {
            if (string.IsNullOrEmpty(message))
            {
                Debug.LogWarning("[UomaTip] ShowTip called with empty message");
                return;
            }

            Debug.Log($"[UomaTip] Showing tip: {message}");
            StartCoroutine(ShowTipCoroutine(message, duration));
        }

        public void ShowErrorTip(int errorCode, string reason = null)
        {
            Debug.Log($"[UomaTip] ShowErrorTip called: errorCode={errorCode}, reason={reason}");
            string message = GetErrorMessage(errorCode, reason);
            ShowTip(message);
        }

        public void ShowErrorTip(string reason)
        {
            Debug.Log($"[UomaTip] ShowErrorTip called with reason: {reason}");
            string message = GetErrorMessageFromReason(reason);
            ShowTip(message);
        }

        private string GetErrorMessage(int errorCode, string reason)
        {
            if (!string.IsNullOrEmpty(reason))
            {
                string reasonMessage = GetErrorMessageFromReason(reason);
                if (!string.IsNullOrEmpty(reasonMessage))
                    return reasonMessage;
            }

            if (ErrorCodeMessageMap.TryGetValue(errorCode, out string errorMsg))
                return errorMsg;

            return $"Error: {errorCode}";
        }

        private string GetErrorMessageFromReason(string reason)
        {
            if (string.IsNullOrEmpty(reason))
                return null;

            if (ErrorMessageMap.TryGetValue(reason, out string message))
                return message;

            string readableReason = reason.Replace("_", " ").ToLower();
            return char.ToUpper(readableReason[0]) + readableReason.Substring(1);
        }

        private IEnumerator ShowTipCoroutine(string message, float duration)
        {
            EnsureTipCanvasExists();

            GameObject tipObject = GetTipFromPool();
            SetupTipObject(tipObject, message);

            _activeTips.Add(tipObject);

            Debug.Log($"[UomaTip] Tip object added, active tips count: {_activeTips.Count}");

            yield return StartCoroutine(AnimateTipIn(tipObject));

            yield return new WaitForSeconds(duration);

            yield return StartCoroutine(AnimateTipFloatUpAndFade(tipObject));

            _activeTips.Remove(tipObject);
            ReturnTipToPool(tipObject);

            Debug.Log($"[UomaTip] Tip object returned to pool, active tips count: {_activeTips.Count}");
        }

        private void EnsureTipCanvasExists()
        {
            if (_tipCanvas != null)
                return;

            Debug.Log("[UomaTip] Creating TipCanvas");

            _tipCanvas = new GameObject("UomaTipCanvas");
            _tipCanvas.transform.SetParent(transform);

            Canvas canvas = _tipCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 9999;

            CanvasScaler scaler = _tipCanvas.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            _tipCanvas.AddComponent<GraphicRaycaster>();

            CreateTipPrefab();

            Debug.Log("[UomaTip] TipCanvas created successfully");
        }

        private void CreateTipPrefab()
        {
            _tipPrefab = new GameObject("TipTemplate");
            _tipPrefab.transform.SetParent(_tipCanvas.transform);

            RectTransform rectTransform = _tipPrefab.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = new Vector2(1000, 200);

            Image bgImage = _tipPrefab.AddComponent<Image>();
            bgImage.color = new Color(0f, 0f, 0f, 0.85f);
            bgImage.raycastTarget = false;

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(_tipPrefab.transform);

            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(50, 30);
            textRect.offsetMax = new Vector2(-50, -30);

            Text text = textObj.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 60;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;

            _tipPrefab.SetActive(false);
        }

        private GameObject GetTipFromPool()
        {
            GameObject tip;

            if (_tipPool.Count > 0)
            {
                tip = _tipPool.Dequeue();
                Debug.Log($"[UomaTip] Got tip from pool, pool size: {_tipPool.Count}");
            }
            else
            {
                tip = Object.Instantiate(_tipPrefab, _tipCanvas.transform);
                Debug.Log($"[UomaTip] Created new tip instance");
            }

            tip.SetActive(true);
            return tip;
        }

        private void SetupTipObject(GameObject tipObject, string message)
        {
            Text textComponent = tipObject.GetComponentInChildren<Text>();
            if (textComponent != null)
            {
                textComponent.text = message;
            }

            RectTransform rectTransform = tipObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.sizeDelta = new Vector2(1000, 200);
            }

            CanvasGroup canvasGroup = tipObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = tipObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0f;
        }

        private void ReturnTipToPool(GameObject tipObject)
        {
            RectTransform rectTransform = tipObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = Vector2.zero;
            }

            tipObject.SetActive(false);

            if (_tipPool.Count < MaxPoolSize)
            {
                _tipPool.Enqueue(tipObject);
            }
            else
            {
                Object.Destroy(tipObject);
            }
        }

        private IEnumerator AnimateTipIn(GameObject tipObject)
        {
            CanvasGroup canvasGroup = tipObject.GetComponent<CanvasGroup>();
            RectTransform rectTransform = tipObject.GetComponent<RectTransform>();
            if (canvasGroup == null || rectTransform == null)
                yield break;

            Vector2 originalSize = rectTransform.sizeDelta;
            Vector2 startSize = originalSize * 0.8f;

            float elapsed = 0f;
            while (elapsed < TipFadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / TipFadeDuration);
                float easeT = 1f - (1f - t) * (1f - t);

                canvasGroup.alpha = t;
                rectTransform.sizeDelta = Vector2.Lerp(startSize, originalSize, easeT);
                yield return null;
            }

            canvasGroup.alpha = 1f;
            rectTransform.sizeDelta = originalSize;
        }

        private IEnumerator AnimateTipFloatUpAndFade(GameObject tipObject)
        {
            CanvasGroup canvasGroup = tipObject.GetComponent<CanvasGroup>();
            RectTransform rectTransform = tipObject.GetComponent<RectTransform>();
            if (canvasGroup == null || rectTransform == null)
                yield break;

            Vector2 startPos = rectTransform.anchoredPosition;
            Vector2 endPos = startPos + new Vector2(0, 150f);
            float floatDuration = 0.6f;

            float elapsed = 0f;
            float startAlpha = canvasGroup.alpha;

            while (elapsed < floatDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / floatDuration);
                float easeT = t * t;

                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, easeT);
                rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                yield return null;
            }

            canvasGroup.alpha = 0f;
            rectTransform.anchoredPosition = endPos;
        }

        private void OnDestroy()
        {
            if (_tipCanvas != null)
            {
                Object.Destroy(_tipCanvas);
            }
        }
    }
}
