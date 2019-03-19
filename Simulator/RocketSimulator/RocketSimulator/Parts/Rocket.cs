﻿using ALPHASim.SimMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketSimulator.Parts
{
    public class Rocket
    {
        public RocketConfiguration Config;
        public List<Surface> Surfaces { get; private set; }

        private Vector3D<double> SurfaceArea;
        private Vector3D<double> DragCoefficients;

        public struct RocketConfiguration
        {
            public Vector3D<double> CenterOfMass { get; set; }
            public Vector3D<double> CenterOfThrust { get; set; }
            public Vector3D<double> CenterOfPressure { get; set; }
        }

        public Rocket(RocketConfiguration config)
        {
            this.Config = config;
            this.Surfaces = new List<Surface>();
        }

        public Rocket(RocketConfiguration config, List<Surface> surfaces) : this (config) 
        { 
            this.Surfaces = surfaces; 
            GetDragCoefficients();
        }

        public void AddSurface(Surface newSurface) 
        { 
            Surfaces.Add(newSurface); 
            DragCoefficients.Add(newSurface.DragCoefficient);
        }

        public List<Surface> GetVisibleSurfacesFromOrientation(Vector3D<double> orientation)
        {
            throw new NotImplementedException();
        }

        public double GetVisibleSurfaceAreaFromOrientation(Vector3D<double> orientation)
        {
            throw new NotImplementedException();
        }

        public Vector3D<double> GetSurfaceAreas()
        {
            if (SurfaceArea == null)
            {
                Vector3D<double> surfaceArea = new Vector3D<double>();
                foreach(Surface surface in Surfaces) { surfaceArea.Add(surface.SurfaceArea); }
                SurfaceArea = surfaceArea;
                return surfaceArea;
            } else { return SurfaceArea; }
        }

        public Vector3D<double> GetDragCoefficients() 
        {
            if (DragCoefficients == null) 
            {
                Vector3D<double> drag = new Vector3D<double>();
                foreach (Surface surface in Surfaces) { drag.Add(surface.DragCoefficient); }
                DragCoefficients = drag;
                return drag;
            }
            else { return DragCoefficients; }
        }
        
    }
}
