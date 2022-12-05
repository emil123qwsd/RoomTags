using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomTags
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements) 
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            //List<FamilyInstance> fInstance = new FilteredElementCollector(doc, doc.ActiveView.Id)
            //    .OfCategory(BuiltInCategory.OST_Rooms)
            //    .WhereElementIsNotElementType()
            //    .Cast<FamilyInstance>()
            //    .ToList();
            //TaskDialog.Show("Rooms count", fInstance.Count.ToString());
            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Element, "Выберите комнаты");

            Transaction ts = new Transaction(doc);
            ts.Start();
            foreach (var selectedElement in selectedElementRefList)
            { 
                Room tmpRoom = doc.GetElement(selectedElement) as Room;
                if (tmpRoom is Room)
                {
                    LocationPoint locationPoint = tmpRoom.Location as LocationPoint;
                    UV point = new UV(locationPoint.Point.X, locationPoint.Point.Y);
                    RoomTag newTag = doc.Create.NewRoomTag(new LinkElementId(tmpRoom.Id), point, null);
                }  
            }
            ts.Commit();

            return Result.Succeeded;
            
        }
        
        
    }
    
}
