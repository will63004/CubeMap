namespace GenerateMap
{
    internal struct NoiseDetail
    {
        public float refinement;
        public float offset;

        public NoiseDetail(float refinement, float offset) : this()
        {
            this.refinement = refinement;
            this.offset = offset;
        }
    }
}