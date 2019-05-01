

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corel.Interop.VGCore;

namespace CGS
{

    public class SelectByWay : ToolState
    {
        PointRange pointRange;
        OnScreenCurve ui;                                         
        bool isDrawing = false;                           
        ToolStateAttributes currentAttribs;     
        Application Application;                        
        Rect virtualRectangle;
        Curve screenCurve;
        
       
        public SelectByWay(Application app)
        {
            Application = app;  
        }

    
        public void OnStartState(ToolStateAttributes stateAttributes)
        {
         
            currentAttribs = stateAttributes;
            currentAttribs.SetCursor(cdrCursorShape.cdrCursorSmallcrosshair);
            currentAttribs.AllowTempPickState = false;
            currentAttribs.PropertyBarGuid = "dd680f8d-ef7d-4062-b778-ed1ae2e585e4";
            ui = Application.CreateOnScreenCurve(); 
            pointRange = Application.Math.CreatePointRange();
        }

        public void OnExitState()
        {
            Reset(); 
        }

        public bool IsDrawing
        {
            get { return isDrawing; }
        }

  
        public void OnAbort()
        {
            Reset(); 
        }

        public void OnCommit(Point pt)
        {
            if (isDrawing)
            {
                SelectShapes();
                Reset(); 
            }
        }
        public void SelectShapes()
        {
            if (Application.ActiveDocument == null || virtualRectangle == null)
                return;
            ShapeRange shapeRange = new ShapeRange();

            Shapes shapes = Application.ActiveDocument.ActivePage.SelectShapesFromRectangle(virtualRectangle.x,virtualRectangle.y,virtualRectangle.Right,virtualRectangle.Top,true).Shapes;
            for (int i = 1; i <= shapes.Count; i++)
            {
                if(shapes[i].DisplayCurve.IntersectsWith(screenCurve))
                    shapeRange.Add(shapes[i]);
            }
            
            shapeRange.CreateSelection();
            
        }
        private void createCurve()
        {
            if (pointRange.Count > 1)
            {
                screenCurve = Application.CreateCurve();
                screenCurve.AppendSubpathFitToPoints(pointRange);
                double x = 0, y = 0, w = 0, h = 0;
                screenCurve.GetBoundingBox(out x, out y, out w, out h);
                virtualRectangle = new Rect()
                {
                    x = x,
                    y = y,
                    Width = w,
                    Height = h
                };
            }
           
        }
      
        public void OnLButtonDownLeaveGrace(Point pt)
        {
            pointRange.AddPoint(pt);
            isDrawing = true;
        }

        public void OnLButtonUp(Point pt)
        {
            OnCommit(pt);
        }

        public void OnMouseMove(Point pt)
        {

            if (isDrawing)
            {
                pointRange.AddPoint(pt);
                createCurve();
                if (screenCurve != null)
                {
                    ui.SetCurve(screenCurve);
                    ui.Show();
                }
            }
            //else
            //{
            //    if (virtualRectangle != null)
            //    {
            //        if (virtualRectangle.IsPointInside(pt.x, pt.y))
            //            currentAttribs.SetCursor(cdrCursorShape.cdrCursorExtPick);
            //    }
            //}
        }

      
        public void OnSnapMouse(Point pt, ref bool handled)
        {
            handled = isDrawing; 
            if (isDrawing)
            {

            }
        }

        public void OnDelete(ref bool handled) { }

        public void OnKeyDown(int windowsKeyCode, ref bool handled) { }

        public void OnKeyUp(int windowsKeyCode, ref bool handled) { }


        public void OnLButtonDblClick(Point pt) { }

        public void OnLButtonDown(Point pt) { }

        public void OnClick(Point pt, ref bool handled) { }

        public void OnRButtonDown(Point pt, ref bool handled) { }

        public void OnRButtonUp(Point pt, ref bool handled) { }


        public void OnTimer(int timerId, int timeEllapsed) { }

        private void Reset()
        {
            ui.Hide();
            isDrawing = false;
            pointRange.RemoveAll();
            screenCurve = null;
        }


    }
}
