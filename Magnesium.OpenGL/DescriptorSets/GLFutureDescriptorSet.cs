namespace Magnesium.OpenGL
{
    public class GLFutureDescriptorSet : IGLFutureDescriptorSet
    {
        public uint Key { get; private set; }
        public IGLFutureDescriptorPool Parent { get; private set; }
        public GLDescriptorPoolResourceInfo[] Resources { get; private set; }

        public GLFutureDescriptorSet(uint key, IGLFutureDescriptorPool parent)
        {
            Key = key;
            Parent = parent;
            IsValidDescriptorSet = false;
        }

        public void Initialize(GLDescriptorPoolResourceInfo[] resources)
        {
            Resources = resources;
            IsValidDescriptorSet = true;
        }

        public bool IsValidDescriptorSet { get; internal set; }

        public void Invalidate()
        {
            Parent = null;
            Resources = null;
            IsValidDescriptorSet = false;
        }

        #region IEquatable implementation

        public bool Equals(IGLFutureDescriptorSet other)
        {
            if (other == null)
                return false;

            if (!ReferenceEquals(Parent, other.Parent))
                return false;

            return Key == other.Key;
        }

        #endregion

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                //hash = hash * 23 + Pool.GetHashCode();
                hash = hash * 23 + Key.GetHashCode();
                return hash;
            }
        }
    }
}
