using System;

namespace MightyAttributes
{
    public class EulerAttribute : BasePropertyDrawerAttribute
    {
        /// <summary>
        /// Uses a Vector3 representation to defines the euler angles of a Quaternion.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        [Obsolete("Suffers from Gimbal Lock.")]
        public EulerAttribute(FieldOption options = FieldOption.Nothing) : base(options)
        {
        }
    }
}