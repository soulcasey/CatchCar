using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CarBrand
{
    Hyundai,
    Kia,
    Chevrolet,
    Toyota,
    Honda,
    Ford,
    Volkswagen,
    Audi,
    BMW,
    MercedesBenz,
    Tesla,
    Lexus,
    Genesis,
    Porsche,
    LandRover,
    Maserati,
    Ferrari,
    Lamborghini,
}

[Serializable]
public class CarData
{
    public string name;
    public string brand;
    public string description;
    public int price;
    public Sprite image;
    public CarBrand brandEnum;
    public bool IsPremium
    {
        get
        {
            return brandEnum >= CarBrand.Audi;
        }
    }
}

[Serializable]
public class CarDataList
{
    public List<CarData> cars;
}

public class CarManager : SingletonBase<CarManager>
{
    [SerializeField]
    private List<CarData> carDataList = new();

    private void Start()
    {
        LoadJsonData();
    }

    private void LoadJsonData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Cars");
        if (jsonFile == null)
        {
            Debug.LogError("Cars.json not found in Resources folder.");
            return;
        }

        carDataList.Clear();

        CarDataList newDataList = JsonUtility.FromJson<CarDataList>("{\"cars\":" + jsonFile.text + "}");

        // Load images from Resources
        foreach (CarData carData in newDataList.cars)
        {
            carData.image = Resources.Load<Sprite>("CarImages/" + carData.name);
            if (carData.image == null)
            {
                Debug.LogWarning($"Image not found: {carData.name}");
                continue;
            }

            if (Enum.TryParse(carData.brand.Replace(" ", "").Replace("-", ""), true, out CarBrand carBrand) == false)
            {
                Debug.LogWarning($"Invalid Brand: {carData.name}");
                continue;
            }
            carData.brandEnum = carBrand;

            carDataList.Add(carData);
        }
    }

    public CarData GetRandomCarData()
    {
        return carDataList[UnityEngine.Random.Range(0, carDataList.Count)];
    }
}
