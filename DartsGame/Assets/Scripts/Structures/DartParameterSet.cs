namespace Assets.Scripts.Structures
{
    public class DartParameterSet
    {
        public float DistanceFromBoard { get; set; }
        public float ForwardForce { get; set; }
        public float RightForceMin { get; set; }
        public float RightForceMax { get; set; }
        public float UpForceMin { get; set; }
        public float UpForceMax { get; set; }

        public float RightForceCenter
        {
            get
            {
                return (RightForceMin + RightForceMax) / 2;
            }
        }

        public float UpForceCenter
        {
            get
            {
                return (UpForceMin + UpForceMax) / 2;
            }
        }

        public static DartParameterSet Defaults
        {
            get
            {
                return new DartParameterSet()
                {
                    DistanceFromBoard = 50.0f,
                    ForwardForce = 900.0f,
                    RightForceMin = -1100.0f,
                    RightForceMax = 1100.0f,
                    UpForceMin = 200.0f,
                    UpForceMax = 1300.0f
                };
            }
        }
    }
}
