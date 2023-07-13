using UnityEngine;

public class PlateContainedIconsUI : WorldSpaceUI
{
    [SerializeField] private PlateKitchenObject plate;
    [SerializeField] private Transform iconTemplate;

    protected override void Awake()
    {
        base.Awake();
        plate.OnAddKitchenObject += OnAddKitchenObject;
    }

    private void OnAddKitchenObject(KitchenObjectSO kitchenObjectSO)
    {
        Transform iconTemplateTransform = Instantiate(iconTemplate, transform);
        iconTemplateTransform.localPosition = Vector3.zero;
        iconTemplateTransform.gameObject.SetActive(true);
        iconTemplateTransform.GetComponent<IconTemplate>().SetIconSprite(kitchenObjectSO);
    }
}
