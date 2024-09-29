﻿using Grasshopper;
using Grasshopper.Kernel;
using Newtonsoft.Json;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows.Forms;
using TapirGrasshopperPlugin.Components;
using TapirGrasshopperPlugin.Data;
using TapirGrasshopperPlugin.Utilities;

namespace Tapir.Components
{
    public class GetSelectedElementsComponent : ArchicadAccessorComponent
    {
        public GetSelectedElementsComponent ()
          : base (
                "Get selected Elements",
                "SelectedElems",
                "Get currently selected elements.",
                "Selection"
            )
        {
        }

        protected override void RegisterInputParams (GH_InputParamManager pManager)
        {

        }

        protected override void RegisterOutputParams (GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter ("ElementIds", "ElementIds", "Currently selected element ids.", GH_ParamAccess.list);
        }

        protected override void SolveInstance (IGH_DataAccess DA)
        {
            CommandResponse response = SendArchicadAddOnCommand ("TapirCommand", "GetSelectedElements", null);
            if (!response.Succeeded) {
                AddRuntimeMessage (GH_RuntimeMessageLevel.Error, response.GetErrorMessage ());
                return;
            }
            ElementsObj elements = response.Result.ToObject<ElementsObj> ();
            DA.SetDataList (0, elements.Elements);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.TapirLogo;

        public override Guid ComponentGuid => new Guid ("1949E4B5-4E37-4F35-8C5C-BEA7575AC1C6");
    }
}
