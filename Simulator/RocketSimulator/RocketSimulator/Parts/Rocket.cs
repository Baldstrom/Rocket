using ALPHASim.SimMath;
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
        public List<Surface> ExteriorSurfaces { get; private set; }

        private Vector3D<float> DragCoefficients;

        public struct RocketConfiguration
        {
            public Vector3D<float> CenterOfMass { get; set; }
            public Vector3D<float> CenterOfThrust { get; set; }
            public Vector3D<float> CenterOfPressure { get; set; }
        }

        public Rocket(RocketConfiguration config)
        {
            this.Config = config;
            this.Surfaces = new List<Surface>();
            this.ExteriorSurfaces = new List<Surface>();
        }

        public Rocket(RocketConfiguration config, List<Surface> surfaces) : this (config) 
        { 
            this.Surfaces = surfaces; 
            foreach (Surface surface in this.Surfaces)
            {
                if (surface.IsExterior)
                {
                    ExteriorSurfaces.Add(surface);
                }
            }
            GetDragCoefficients();
        }

        public void AddSurface(Surface newSurface) 
        { 
            Surfaces.Add(newSurface); 
            if (newSurface.IsExterior) { ExteriorSurfaces.Add(newSurface); }
            DragCoefficients.Add(newSurface.DragCoefficient);
        }

        public Vector3D<float> GetDragCoefficients() 
        {
            if (DragCoefficients == null) 
            {
                Vector3D<float> drag = new Vector3D<float>();
                foreach (Surface surface in Surfaces) { drag.Add(surface.DragCoefficient); }
                DragCoefficients = drag;
                return drag;
            }
            else { return DragCoefficients; }
        }
        
    }
}
