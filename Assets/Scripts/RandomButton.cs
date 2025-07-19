using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class RandomButton : MonoBehaviour
{
    [SerializeField]
    private Popup popup;
    
    [SerializeField]
    private Button button;

    [SerializeField]
    private Image carImage;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private const float SHAKE_STRENGTH = 6;
    private readonly Vector3 SCALE_TARGET = new(10, 10, 10);

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            button.interactable = false;
            if (popup.gameObject.activeSelf == true) popup.Hide();
            PlayClickAnimation(() => popup.Show(CarManager.Instance.GetRandomCarData()));
        });
    }


    private void PlayClickAnimation(Action OnComplete)
    {
        AudioManager.Instance.Play(AudioClipEnum.CarIgnitionSound);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(carImage.transform.DORotate(new Vector3(0f, 0f, -SHAKE_STRENGTH), 0.125f).SetEase(Ease.Linear));
        sequence.Append(carImage.transform.DORotate(new Vector3(0f, 0f, SHAKE_STRENGTH), 0.25f).SetEase(Ease.Linear));
        sequence.Append(carImage.transform.DORotate(Vector3.zero, 0.125f).SetEase(Ease.Linear));

        sequence.AppendInterval(0.7f);

        sequence.Append(carImage.transform.DOScale(SCALE_TARGET, 1).SetEase(Ease.Linear));
        sequence.Join(canvasGroup.DOFade(0, 1).SetEase(Ease.Linear));

        sequence.AppendInterval(0.5f);

        sequence.OnComplete(() =>
        {
            OnComplete?.Invoke();
            carImage.transform.localScale = Vector3.one;
            carImage.transform.localRotation = Quaternion.identity;
            canvasGroup.alpha = 1;
            button.interactable = true;
        });
    }
}
