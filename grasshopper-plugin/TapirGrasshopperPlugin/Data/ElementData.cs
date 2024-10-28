using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Eto.Forms;
using Grasshopper.Kernel.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TapirGrasshopperPlugin.Data
{
    public static class GuidHelper
    {

        public static bool HasGuidProperty (object obj)
        {
            PropertyInfo guidProperty = obj?.GetType ().GetProperty ("Guid");
            return guidProperty != null;
        }

        public static string GetGuidProperty (object obj)
        {
            PropertyInfo guidProperty = obj?.GetType ().GetProperty ("Guid");
            return guidProperty.GetValue (obj)?.ToString ();
        }
    }


    public class ElementIdObj
    {

        public ElementIdObj () {}
        public ElementIdObj (GH_String guid)
        {
            if (System.Guid.TryParse (guid.Value, out _)) 
            {
                Guid = guid.Value;
            } 
            else 
            {
                throw new ArgumentException ("Invalid GUID format.");
            }
        }

        public ElementIdObj (string guid)
        {
            if (System.Guid.TryParse (guid, out _)) {
                Guid = guid;
            } 
            else 
            {
                throw new ArgumentException ("Invalid GUID format.");
            }
        }

        public override string ToString ()
        {
            return Guid;
        }

        [JsonProperty ("guid")]
        public string Guid;
    }

    public class ElementIdItemObj
    {
        public override string ToString ()
        {
            return ElementId.ToString ();
        }

        [JsonProperty ("elementId")]
        public ElementIdObj ElementId;

        public ElementIdItemObj () { }

        public ElementIdItemObj(GH_ObjectWrapper guidSource)
        {

            if (guidSource.Value is ElementIdItemObj element) {
                ElementId = element.ElementId;
            } 
            else if (guidSource.Value is GH_String guidString) {
                ElementId = new ElementIdObj (guidString);
            } else if (GuidHelper.HasGuidProperty (guidSource.Value)) {
                ElementId = new ElementIdObj (GuidHelper.GetGuidProperty (guidSource.Value));
            } else {
                throw new ArgumentException ("Unsupported item type in collection.");
            }
        }

    }

    public class ElementsObj
    {
        [JsonProperty ("elements")]
        public List<ElementIdItemObj> Elements = new List<ElementIdItemObj>();

        public ElementsObj () { }

        public ElementsObj (List<GH_ObjectWrapper> guidSources)
        {
            foreach (var guidSource in guidSources) {
                System.Diagnostics.Debug.WriteLine ($"Type: {guidSource.Value?.GetType ().Name}, Value: {guidSource}");
                if (guidSource.Value is ElementIdItemObj element) {
                    Elements.Add (element);
                } 
                else if (guidSource.Value is GH_String guidString) {
                    Elements.Add (new ElementIdItemObj { ElementId = new ElementIdObj (guidString) });
                } 
                else if (GuidHelper.HasGuidProperty(guidSource.Value)) {
                    Elements.Add (new ElementIdItemObj { ElementId = new ElementIdObj (GuidHelper.GetGuidProperty(guidSource.Value)) });
                } 
                else {
                    throw new ArgumentException ("Unsupported item type in collection.");
                }
            }
        }

    }

    public class ElementPropertyValueObj
    {
        [JsonProperty ("elementId")]
        public ElementIdObj ElementId;

        [JsonProperty ("propertyId")]
        public PropertyIdObj PropertyId;

        [JsonProperty ("propertyValue")]
        public PropertyValueObj PropertyValue;
    }

    public class ElementPropertyValuesObj
    {
        [JsonProperty ("elementPropertyValues")]
        public List<ElementPropertyValueObj> ElementPropertyValues;
    }

    public class DetailsOfElementObj
    {
        [JsonProperty ("type")]
        public string Type;

        [JsonProperty ("floorIndex")]
        public int FloorIndex;

        [JsonProperty ("layerIndex")]
        public int LayerIndex;

        [JsonProperty ("drawIndex")]
        public int DrawIndex;

        [JsonProperty ("details")]
        public JObject Details;
    }

    public class DetailsOfElementsObj
    {
        [JsonProperty ("detailsOfElements")]
        public List<DetailsOfElementObj> DetailsOfElements;
    }

    public class GDLParameterDetailsObj
    {
        [JsonProperty ("name")]
        public string Name;

        [JsonProperty ("index")]
        public string Index;

        [JsonProperty ("type")]
        public string Type;

        [JsonProperty ("dimension1")]
        public int Dimension1;

        [JsonProperty ("dimension2")]
        public int Dimension2;

        [JsonProperty ("value")]
        public Object Value;
    }

    public class GDLParameterListObj
    {
        public override string ToString ()
        {
            List<string> paramNames = new List<string> ();
            foreach (GDLParameterDetailsObj param in Parameters) {
                paramNames.Add (param.Name);
            }
            return String.Join (", ", paramNames);
        }

        [JsonProperty ("parameters")]
        public List<GDLParameterDetailsObj> Parameters;
    }

    public class Box3DObj
    {
        [JsonProperty ("xMin")]
        public double XMin;

        [JsonProperty ("yMin")]
        public double YMin;

        [JsonProperty ("zMin")]
        public double ZMin;

        [JsonProperty ("xMax")]
        public double XMax;

        [JsonProperty ("yMax")]
        public double YMax;

        [JsonProperty ("zMax")]
        public double ZMax;
    }

    public class BoundingBox3DObj
    {
        [JsonProperty ("boundingBox3D")]
        public Box3DObj BoundingBox3D;
    }

    public class BoundingBoxes3DObj
    {
        [JsonProperty ("boundingBoxes3D")]
        public List<BoundingBox3DObj> BoundingBoxes3D;
    }
}
