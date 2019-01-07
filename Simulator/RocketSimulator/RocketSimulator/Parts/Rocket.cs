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

        public struct RocketConfiguration
        {
            public Vector3D<double> CenterOfMass { get; private set; }
            public Vector3D<double> CenterOfThrust { get; private set; }
            public Vector3D<double> CenterOfPressure { get; private set; }
        }

        public Rocket(RocketConfiguration config)
        {
            this.Config = config;
            this.Surfaces = new List<Surface>();
        }

        public Rocket(RocketConfiguration config, List<Surface> surfaces) : this (config) { this.Surfaces = surfaces; }

        public void AddSurface(Surface newSurface) { Surfaces.Add(newSurface); }
        
    }
}
