using GoogleARCore.Examples.AugmentedImage;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class VseprTester : MonoBehaviour
{
    public Button VisualizeButton;
    public GameObject Tracking;
    private readonly GameObject moleculeModel;
    public GameObject MoleculeSpawn;
    public ParticleSystem SpawnParticles;
    public AudioSource LabSound;
    public GameObject MoleculeLibrary;

    // Use this for initialization
    void Start()
    {
        //PlaceMoleculeForFormula("H2SO4");
        // --  printDebug("HNO3");
        //	printDebug("NH3");
        //	printDebug("CO2");
        //	printDebug("CO");
        //  printDebug("BCl3");
        //  printDebug("H2SO4");
        // AddMoleculeToLibrary("H2SO4");

    }

    public void VisualizeMolecule()
    {
        Debug.Log("clicked");
        var TrackingController = Tracking.GetComponent<AugmentedImageExampleController>();
        var atoms = TrackingController.Visualizers.Values;
        if (atoms.Count == 0)
        {
            Debug.LogError("no atoms scanned, you tried to render empty molecule");
        }

        var formula = string.Join(string.Empty, atoms.Select(m => m.Image.Name).ToArray());
        var first = atoms.First();
        var last = atoms.Last();
        var x = (last.Image.CenterPose.position.x - first.Image.CenterPose.position.x) / 2;
        var pose = last.Image.CreateAnchor(last.Image.CenterPose);
        //new Vector3(x,
        //    first.Image.CenterPose.position.y,
        //    first.Image.CenterPose.position.z), 
        //    first.Image.CenterPose.rotation
        SpawnParticles.Play(true);
        var audioData = GetComponent<AudioSource>();
        audioData.Play();
        PlaceMoleculeForFormula(formula, pose.transform.position, Quaternion.identity);
    }


    static string PrefabNameVSEPR(VSEPR vs)
    {
        int X = vs.Ligands.Count(l => l.Type == VSEPR.Ligand.LigandType.X);
        int E = vs.Ligands.Count(l => l.Type == VSEPR.Ligand.LigandType.E);
        string prefabName = "A" + "X" + X + "E" + E;
        Debug.Log(prefabName);
        return prefabName;
    }
    public static GameObject InstantiateMolecule(string formula)
    {
        var vs = new VSEPR(formula);
        var name = PrefabNameVSEPR(vs);
        var res = Resources.Load(name);
        var model = (GameObject)Instantiate(res);
        model.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        model.GetComponent<Molecule>().vsepr = vs;
        return model;
    }
    void PlaceMoleculeForFormula(string formula, Vector3 position, Quaternion rotation)
    {
        var res = InstantiateMolecule(formula);
        res.transform.SetPositionAndRotation(position, rotation);
        MoleculeLibrary.GetComponent<Library>().AddMoleculeToLibrary(formula);
    }
}
