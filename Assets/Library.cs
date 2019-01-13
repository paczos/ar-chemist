using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Library : MonoBehaviour
{
    public GameObject ScrollView;
    public GameObject Preview;
    public GameObject ScrollViewContent;
    public GameObject ListItemTemplate;
    public GameObject FitToScanOverlay;
    bool IsVisible = true;
    List<string> MoleculeLibrary = new List<string>();

    public void LibItemButtonClicked(BaseEventData data)
    {
        var buttonText = data.selectedObject.GetComponentInChildren<Text>().text;
        LoadMoleculePreview(buttonText);
    }
    void Start()
    {
        string saved = PlayerPrefs.GetString("molecules");
        Debug.Log("saved molecules:" + saved);
        if (!string.IsNullOrEmpty(saved))
        {
            MoleculeLibrary = new List<string>(JsonUtility.FromJson<string[]>(saved));
        }

        if(MoleculeLibrary.Count==0)
        {
            ToggleVisibility();
            //AddMoleculeToLibrary("H2SO4");
             AddMoleculeToLibrary("HNO3");
            AddMoleculeToLibrary("H2O");
            //AddMoleculeToLibrary("H2SO3");
            //AddMoleculeToLibrary("HCN");
            // AddMoleculeToLibrary("H3BO3");
            //AddMoleculeToLibrary("CO2");
            //AddMoleculeToLibrary("CO");// not supported type
            //AddMoleculeToLibrary("NO2");
            //AddMoleculeToLibrary("SO2");
            //AddMoleculeToLibrary("SO3");
            //AddMoleculeToLibrary("H2S");
            //AddMoleculeToLibrary("HClO4");
            //AddMoleculeToLibrary("HClO4");
        }
    }
    public void ToggleVisibility()
    {
        IsVisible = !IsVisible;
        ScrollView.gameObject.SetActive(IsVisible);
        Preview.gameObject.SetActive(IsVisible);
        FitToScanOverlay.GetComponent<RawImage>().enabled = !IsVisible;
    }

    void LoadMoleculePreview(string formula)
    {
        foreach (var child in Preview.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject != Preview)
            {
                Destroy(child.gameObject);
            }
        }
        var toLib = VseprTester.InstantiateMolecule(formula);
        toLib.transform.localScale = new Vector3(.5f, .5f, .5f);
        var children = Preview.gameObject.GetComponentsInChildren<Transform>();

        toLib.transform.SetParent(Preview.transform);
        toLib.transform.position = Preview.transform.position;
    }
    public void AddMoleculeToLibrary(string formula)
    {
        if (MoleculeLibrary.Contains(formula))
        {
            Debug.Log("This molecule is already present in library");
            return;
        }

        MoleculeLibrary.Add(formula);
        //list item
        var button = Object.Instantiate(ListItemTemplate);
        button.GetComponentInChildren<Text>().text = formula;
        button.transform.SetParent(ScrollViewContent.transform, false);
        button.gameObject.SetActive(true);
        Debug.Log("to save: " + JsonUtility.ToJson(new { molecules = MoleculeLibrary.ToArray() }));
        PlayerPrefs.SetString("molecules", JsonUtility.ToJson(MoleculeLibrary.ToArray()));
    }

    void Update()
    {

    }
}
