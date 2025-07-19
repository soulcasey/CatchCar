using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Popup : MonoBehaviour
{
    [SerializeField]
    private Button closeButton;

    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI brand;

    [SerializeField]
    private TextMeshProUGUI carName;

    [SerializeField]
    private TextMeshProUGUI price;

    [SerializeField]
    private TextMeshProUGUI description;

    [SerializeField]
    private CanvasGroup textCanvasGroup;

    [SerializeField]
    private CanvasGroup buttonCanvasGroup;


    private readonly Vector3 START_SCALE = new Vector3(8, 8, 8);
    private const float IMAGE_Y = 120;

    private readonly string ANIMATION_ID = "POPUP_ANIMATION";

    private void Start()
    {
        closeButton.onClick.AddListener(Hide);
    }

    public void Show(CarData carData, bool isAnimation = true)
    {
        image.sprite = carData.image;
        brand.text = carData.brand;
        carName.text = carData.name;
        price.text = "$" + carData.price.ToString();
        description.text = carData.description;

        if (isAnimation == true)
        {
            PlayShowAnimation(carData);
        }
        else
        {
            FadeAnimation();
        }
    }

    private void FadeAnimation()
    {
        closeButton.interactable = false;

        buttonCanvasGroup.alpha = 0;

        buttonCanvasGroup.DOFade(1, 0.4f).SetEase(Ease.Linear).SetId(ANIMATION_ID)
        .OnComplete(() =>
        {
            closeButton.interactable = true;
        });
    }


    private void PlayShowAnimation(CarData carData)
    {
        DOTween.Kill(ANIMATION_ID);

        image.transform.localScale = START_SCALE;
        image.transform.localPosition = Vector3.zero;
        Common.SetAlpha(image, 0);

        textCanvasGroup.alpha = 0;

        closeButton.gameObject.SetActive(false);

        gameObject.SetActive(true);

        Sequence sequence = DOTween.Sequence().SetId(ANIMATION_ID);

        sequence.Append(image.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.Linear));
        sequence.Join(image.DOFade(1, 0.5f).SetEase(Ease.Linear));

        sequence.Append(image.transform.DOShakeRotation(
            0.35f,
            new Vector3(0, 0, carData.IsPremium ? 1 : 0),
            30,
            fadeOut: false,
            randomnessMode: ShakeRandomnessMode.Harmonic)
            .OnStart(() =>
            {
                AudioManager.Instance.Play(carData.IsPremium ? AudioClipEnum.PremiumCarGetSound : AudioClipEnum.CarGetSound);
            })
            .OnComplete(() =>
            {
                image.transform.localRotation = Quaternion.identity;
            }));

        sequence.AppendInterval(1f);

        sequence.Append(image.transform.DOLocalMoveY(IMAGE_Y, 1f).SetEase(Ease.Linear));
        sequence.Join(textCanvasGroup.DOFade(1, 1f).SetEase(Ease.Linear));
        sequence.AppendInterval(1f);

        sequence.OnComplete(() =>
        {
            closeButton.gameObject.SetActive(true);
        });
    }

    public void Hide()
    {
        DOTween.Kill(ANIMATION_ID);

        buttonCanvasGroup.DOFade(0, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            gameObject.SetActive(false);
            buttonCanvasGroup.alpha = 1;
        });
    }
}
