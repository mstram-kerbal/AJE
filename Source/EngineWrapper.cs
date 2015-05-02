﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP;
//using Firespitter.engine;

namespace AJE
{
    public class EngineWrapper
    {
        public enum EngineType
        {
            NONE,
            ModuleEngine,
            ModuleEngineFX,
            FSengine
        }
        public EngineType type = EngineType.NONE;
        public ModuleEngines engine;
        public ModuleEnginesFX engineFX;
//        public FSengine fsengine;
        public float idle, IspMultiplier, ThrustUpperLimit;

        public EngineWrapper(Part part)
        {
            engine = (ModuleEngines)part.Modules["ModuleEngines"];
            if (engine != null)
            {
                type = EngineType.ModuleEngine;
            }
            else
            {
                engineFX = (ModuleEnginesFX)part.Modules["ModuleEnginesFX"];
                if (engineFX != null)
                {
                    type = EngineType.ModuleEngineFX;
                }
                else
                {
 //                   fsengine = (Firespitter.engine.FSengine)part.Modules["FSengine"];
 //                   if (fsengine != null)
                    {
 //                       type = EngineType.FSengine;
                    }
                }
            }
            Debug.Log("EngineWrapper: engine type is " + type.ToString());
        }

        public void SetEngineParams(float fuelflow, float isp)
        {
            if (isp > 0)
            {
                FloatCurve resultcurve = new FloatCurve();
                this.realIsp = isp * IspMultiplier;
                resultcurve.Add(0, isp * IspMultiplier, 0, 0);

                switch (type)
                {
                    case EngineType.ModuleEngine:
                        engine.atmosphereCurve = resultcurve;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.atmosphereCurve = resultcurve;
                        break;
                    case EngineType.FSengine:
                        //                   fsengine.fuelConsumption = "0,0.0001;1," + (fsengine.maxThrust * 1000f / 9.801f / isp).ToString();
                        break;
                }
            }

            if (fuelflow > 0)
            {
                this.maxFuelFlow = fuelflow;
            }
            else
            {
                this.maxFuelFlow = 0.00000001f;
            }

        }
        public void SetIsp(float isp)
        {
            if (isp > 0)
            {
                FloatCurve resultcurve = new FloatCurve();
                this.realIsp = isp * IspMultiplier;
                resultcurve.Add(0, isp * IspMultiplier, 0, 0);

                switch (type)
                {
                    case EngineType.ModuleEngine: 
                        engine.atmosphereCurve = resultcurve;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.atmosphereCurve = resultcurve;
                        break;
                    case EngineType.FSengine:
                        //                   fsengine.fuelConsumption = "0,0.0001;1," + (fsengine.maxThrust * 1000f / 9.801f / isp).ToString();
                        break;
                }



            }
        }

        public void SetThrust(float t)
        {
            if (t > 0)
            {
                if (t > ThrustUpperLimit)
                    this.maxThrust = ThrustUpperLimit + (t - ThrustUpperLimit) / 10;
                else
                    this.maxThrust = t;
            }
            else
            {
                this.maxThrust = 0.0001f;
            }
 //           this.finalThrust = this.maxThrust * idle;
            this.maxThrust = this.maxThrust / this.currentThrottle;
            
        }


        public float maxFuelFlow
        {
            get
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        return engine.maxFuelFlow;
                    case EngineType.ModuleEngineFX:
                        return engineFX.maxFuelFlow;
                    case EngineType.FSengine:
                    //                     return fsengine.maxFuelFlow;
                    default:
                        return 0f;
                }
            }
            set
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        engine.maxFuelFlow = value;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.maxFuelFlow = value;
                        break;
                    case EngineType.FSengine:
                        //                     fsengine.maxFuelFlow = value;
                        break;
                }
            }
        }


        public float maxThrust
        {
            get
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        return engine.maxThrust;
                    case EngineType.ModuleEngineFX:
                        return engineFX.maxThrust;
                    case EngineType.FSengine:
   //                     return fsengine.maxThrust;
                    default:
                        return 0f;
                }
            }
            set
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        engine.maxThrust = value;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.maxThrust = value;
                        break;
                    case EngineType.FSengine:
   //                     fsengine.maxThrust = value;
                        break;
                }
            }
        }

        public float minThrust
        {

            set
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        engine.minThrust = value;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.minThrust = value;
                        break;
                    case EngineType.FSengine:
                        break;
                }
            }
        }
        public float machLimit
        {

            set
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        engine.machLimit = value;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.machLimit = value;
                        break;
                    case EngineType.FSengine:
                        break;
                }
            }
        }
        public float machHeatMult
        {

            set
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        engine.machHeatMult = value;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.machHeatMult = value;
                        break;
                    case EngineType.FSengine:
                        break;
                }
            }
        }


        public float finalThrust
        {
            set
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        engine.finalThrust = value;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.finalThrust = value;
                        break;
                    case EngineType.FSengine:
                        break;
                }
            }
        }

        public float realIsp
        {
            get
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        return engine.realIsp;
                    case EngineType.ModuleEngineFX:
                        return engineFX.realIsp;
                    case EngineType.FSengine:
                        return 0f;
                    default:
                        return 0f;
                }
            }
            set
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        engine.realIsp = value;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.realIsp = value;
                        break;
                    case EngineType.FSengine:
                        break;
                }
            }
        }

        public float currentThrottle
        {
            get
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        return engine.currentThrottle;
                    case EngineType.ModuleEngineFX:
                        return engineFX.currentThrottle;
                    case EngineType.FSengine:
                        return 0f;
                    default:
                        return 0f;
                }
            }
            set
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        engine.currentThrottle = value;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.currentThrottle = value;
                        break;
                    case EngineType.FSengine:
                        break;
                }
            }
        }

        public bool EngineIgnited
        {
            get
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        return engine.EngineIgnited;
                    case EngineType.ModuleEngineFX:
                        return engineFX.EngineIgnited;
                    case EngineType.FSengine:
  //                      return fsengine.EngineIgnited;
                    default:
                        return false;
                }
            }
            set
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        engine.EngineIgnited = value;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.EngineIgnited = value;
                        break;
                    case EngineType.FSengine:
  //                      fsengine.EngineIgnited = value;
                        break;
                }
            }
        }

        public float thrustPercentage
        {
            get
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        return engine.thrustPercentage;
                    case EngineType.ModuleEngineFX:
                        return engineFX.thrustPercentage;
                    case EngineType.FSengine:
                        return 1f;
                    default:
                        return 1f;
                }
            }
        }

        public string thrustVectorTransformName
        {
            get
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        return engine.thrustVectorTransformName;
                    case EngineType.ModuleEngineFX:
                        return engineFX.thrustVectorTransformName;
                    case EngineType.FSengine:
  //                      return fsengine.thrustTransformName;
                    default:
                        return null;
                }
            }
        }
        public List<Transform> thrustTransforms
        {
            get
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        return engine.thrustTransforms;
                    case EngineType.ModuleEngineFX:
                        return engineFX.thrustTransforms;
                    case EngineType.FSengine:
                    //                      return fsengine.thrustTransformName;
                    default:
                        return null;
                }
            }
        }
        public bool useEngineResponseTime
        {
            set
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        engine.useEngineResponseTime = value;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.useEngineResponseTime = value;
                        break;
                    case EngineType.FSengine:
                        break;
                }
            }
        }

        public bool atmChangeFlow
        {
            set
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        engine.atmChangeFlow = value;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.atmChangeFlow = value;
                        break;
                    case EngineType.FSengine:
                        break;
                }
            }
        }
        public bool useVelCurve
        {
            set
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        engine.useVelCurve = value;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.useVelCurve = value;
                        break;
                    case EngineType.FSengine:
                        break;
                }
            }
        }
        public bool useAtmCurve
        {
            set
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        engine.useAtmCurve = value;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.useAtmCurve = value;
                        break;
                    case EngineType.FSengine:
                        break;
                }
            }
        }

        public float engineAccelerationSpeed
        {
            set
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        engine.engineAccelerationSpeed = value;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.engineAccelerationSpeed = value;
                        break;
                    case EngineType.FSengine:
                        break;
                }
            }
        }
        public float engineDecelerationSpeed
        {
            set
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        engine.engineDecelerationSpeed = value;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.engineDecelerationSpeed = value;
                        break;
                    case EngineType.FSengine:
                        break;
                }
            }
        }
        public float heatProduction
        {
            set
            {
                switch (type)
                {
                    case EngineType.ModuleEngine:
                        engine.heatProduction = value;
                        break;
                    case EngineType.ModuleEngineFX:
                        engineFX.heatProduction = value;
                        break;
                    case EngineType.FSengine:
                        break;
                }
            }
        }
    }
}
