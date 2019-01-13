using System.Collections.Generic;
using System.Linq;

public class VSEPR
{
    public readonly string Formula;
    public List<Atom> atoms = new List<Atom>();
    int electricCharge = 0;

    public struct AtomInfo
    {
        public readonly int electrones;
        public readonly string color;
        public AtomInfo(int electrones, string color = "#ff1493")
        {
            this.electrones = electrones;
            this.color = color;
        }
    }

    public string Info()
    {
        return (atoms.Select(a => a.ToString()).ToList()).ToString();
    }

    //CPK coloring for atoms https://en.wikipedia.org/wiki/CPK_coloring
    public static Dictionary<string, AtomInfo> valencyElectrones = new Dictionary<string, AtomInfo>()
    {
		{ "OH", new AtomInfo(0)},
        

		{ "H", new AtomInfo(1, "#ffffff")},

		{ "B", new AtomInfo(3, "#00ff00")},
        { "Al", new AtomInfo(3, "#808090")},
        { "Ga", new AtomInfo(3)},
        { "In", new AtomInfo(3)},
        { "Tl", new AtomInfo(3)},

        { "C", new AtomInfo(4, "#000000")},
        { "Si", new AtomInfo(4, "#daa520")},
        { "Ge", new AtomInfo(4)},
        { "Sn", new AtomInfo(4)},
        { "Pb", new AtomInfo(4)},

        { "N", new AtomInfo(5, "#9c7ac7")},
        { "P", new AtomInfo(5, "#ffaa00")},
        { "As", new AtomInfo(5)},
        { "Sb", new AtomInfo(5)},
        { "Bi", new AtomInfo(5)},

        { "O", new AtomInfo(6, "#f00000")},
        { "S", new AtomInfo(6, "#ffff00")},
        { "Se", new AtomInfo(6)},
        { "Te", new AtomInfo(6)},
        { "Po", new AtomInfo(6)},

        { "F", new AtomInfo(7, "#daa520")},
        { "Cl", new AtomInfo(7, "#00ff00")},
        { "Br", new AtomInfo(7, "#802828")},
        { "I", new AtomInfo(7, "#a020f0")},
        { "At", new AtomInfo(7)},

        { "Ne", new AtomInfo(8)},
        { "Ar", new AtomInfo(8)},
        { "Kr", new AtomInfo(8)},
        { "Xe", new AtomInfo(8)},
        { "Rn", new AtomInfo(8)},
    };

   public struct Atom
    {
        public readonly string symbol;
        public readonly int count;
        public int valencySingle
        {
            get
            {
                return valencyElectrones[symbol].electrones;
            }
        }
		public AtomInfo info;

        public int valencyTotal
        {
            get { return valencySingle * count; }
        }
        public Atom(string symbol, int count = 1)
        {
            this.symbol = symbol;
            this.count = count;
			this.info=valencyElectrones[this.symbol];
        }
        public override string ToString()
        {
            return symbol + " x" + count;
        }
    }

	public AtomInfo CentralAtomInfo { get; set;}
    public VSEPR(string chemicalFormula)
    {
        Formula = chemicalFormula.Trim();
        if (Formula.Length == 0)
        {
            throw new System.FormatException("Formula cannot be an empty string");
        }
        ParseFormula(Formula);
		CentralAtomInfo = valencyElectrones [CentralAtom.symbol];

    }

    public bool isAcid
    {
        get
        {

            if (Formula[0] == 'H' && Formula != "H2O")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void ParseFormula(string formula)
    {

        string currentAtom = "";
        for (int i = 0; i < formula.Length; i++)
        {
            char c = formula[i];
            if (char.IsUpper(c))
            {
                if (currentAtom.Length > 0)
                {
                    AddAtom(currentAtom, 1);
                }
                currentAtom = "";
                currentAtom += c;
            }
            else if (char.IsLower(c))
            {
                currentAtom += c;
                if ((i + 1 == formula.Length) || (i + 1 < formula.Length && !char.IsDigit(formula[i + 1])))
                {
                    AddAtom(currentAtom, 1);
                    currentAtom = "";
                }
            }
            else if (char.IsDigit(c))
            {
                int count = int.Parse(c.ToString());
                AddAtom(currentAtom, count);
                currentAtom = "";
            }

        }
        if (currentAtom.Length > 0)
        {
            AddAtom(currentAtom, 1);
        }
    }
    void AddAtom(string symbol, int count)
    {
        AtomInfo info;
        if (!valencyElectrones.TryGetValue(symbol, out info))
        {
            throw new System.FormatException("The chemical element " + symbol + " not recognized");
        }
        atoms.Add(new Atom(symbol, count));

    }
    public int LoneElectronPairsCount
    {
        get
        {
            if (isOxygenAcid)
            {
                return ValencyElectronesAndCharge / 2 - 4 * OHGroupsCount - 4 * (ATypeLigandsCount - OHGroupsCount);
            }
            else
            {
                return ValencyElectronesAndCharge / 2 - 4 * ATypeLigandsCount - HTypeLigandsCount;
            }

        }
    }
    public int SigmaBondsCount()
    {
        return ATypeLigandsCount + HTypeLigandsCount;
    }
    public int ValencyElectronesAndCharge
    {
        get
        {
            int valencySum = atoms.Aggregate(0, (acc, atom) => (acc + atom.valencyTotal));
            return valencySum - electricCharge;
        }
    }

    public int OHGroupsCount
    {
        get
        {
            return System.Math.Min(hydrogenCount, oxygenCount);
        }
    }
    bool isOxygenAcid
    {
        get { return isAcid && hasOxygen; }
    }
    public int ATypeLigandsCount
    {
        get
        {
            int ligands = atoms.Where(n => n.symbol != "H").Select(m => m.count).Sum() - 1;
            return System.Math.Max(0, ligands);
        }
    }
    public int HTypeLigandsCount
    {
        get
        {
            return atoms.Where(a => a.symbol == "H").Select(at => at.count).Sum();
        }
    }
    public int StericNumber
    {
        get
        {
            if (isOxygenAcid)
            {
                return LoneElectronPairsCount + ATypeLigandsCount;
            }
            else
            {


                return SigmaBondsCount() + LoneElectronPairsCount;
            }
        }
    }
    public Atom CentralAtom { get { return atoms.Where(at => at.symbol != "H").First(); } }

    public struct Ligand
    {
        public enum LigandType
        {
            A, X, E
        }
        public readonly string Symbol;
        public readonly LigandType Type;
        public Ligand(string symbol, LigandType type)
        {
            this.Symbol = symbol;
            Type = type;
        }
        public override string ToString()
        {
            return Symbol;
        }

    }
    bool hasOxygen
    {
        get { return atoms.Exists(at => at.symbol == "O" && at.count > 0); }
    }
    int hydrogenCount
    {
        get
        {
            if (!atoms.Exists(at => at.symbol == "H"))
            {
                return 0;
            }
            else { return atoms.Single(at => at.symbol == "H").count; }

        }
    }
    int oxygenCount
    {
        get
        {
            if (hasOxygen)
            {
                var o = atoms.Single(at => at.symbol == "O");
                return o.count;
            }
            else { return 0; }
        }
    }
    public List<Ligand> Ligands
    {
        get
        {
            var ligands = new List<Ligand>();

            //first ligand is the central atom
            ligands.Add(new Ligand(CentralAtom.symbol, Ligand.LigandType.A));

            //count OH groups
            if (isAcid)
            {
                int remainingOxygen = System.Math.Max(0, oxygenCount - OHGroupsCount);
                int remainingHydrogen = System.Math.Max(0, hydrogenCount - OHGroupsCount);
                //add OH ligands
                if (OHGroupsCount > 0)
                    ligands.AddRange(Enumerable.Repeat(new Ligand("OH", Ligand.LigandType.X), OHGroupsCount));
                // add remaining H ligands
                if (remainingHydrogen > 0)
                    ligands.AddRange(Enumerable.Repeat(new Ligand("H", Ligand.LigandType.X), remainingHydrogen));
                // add remaining O ligands
                if (remainingOxygen > 0)
                    ligands.AddRange(Enumerable.Repeat(new Ligand("O", Ligand.LigandType.X), remainingOxygen));
            }
            else
            {
                foreach (var at in atoms)
                {
                    if (at.symbol == CentralAtom.symbol )
                    {
                        ligands.AddRange(Enumerable.Repeat(new Ligand(at.symbol, Ligand.LigandType.X), at.count - 1));//we are adding it count-1 times because central atom has already been added
                    }
                    else
                    {

                        ligands.AddRange(Enumerable.Repeat(new Ligand(at.symbol, Ligand.LigandType.X), at.count));
                    }

                }
            }
            //add lone pairs as ligands
            if (LoneElectronPairsCount > 0)
                ligands.AddRange(Enumerable.Repeat(new Ligand("El", Ligand.LigandType.E), LoneElectronPairsCount));

            //if (StericNumber != ligands.Count)
            //{
            //    throw new System.FormatException("Steric number and ligands count do not match");
            //}

            return ligands;
        }
    }

    public string LigandString
    {
        get
        {
            string str = "";
            foreach (var l in Ligands)
            {
                str += "|" + l.Symbol;
            }
            return str;
        }
    }

    public string AtomsString
    {
        get
        {
            string str = "steric num:" + StericNumber + "| Lone Pairs: " + LoneElectronPairsCount + " |ATypeLigands: " + ATypeLigandsCount + " |HTypeigands " + HTypeLigandsCount + " |OH groups " + OHGroupsCount + " |oxygen" + oxygenCount + " |hydrogen: " + hydrogenCount;
            foreach (var l in atoms)
            {
                str += "|" + l.symbol + ' ' + l.count;
            }
            return str;
        }
    }
}

